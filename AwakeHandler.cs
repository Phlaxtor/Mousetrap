namespace Mousetrap
{
    public delegate void AwakeEventHandler(object? sender, AwakeEventArgs e);

    public delegate void AwakePingHandler(object? sender, AwakeEventArgs e);

    public sealed class AwakeHandler : IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly SemaphoreSlim _semaphore;
        private bool _isAwakeState;
        private bool _isLocked;

        public AwakeHandler()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _semaphore = new SemaphoreSlim(1);
        }

        public event AwakePingHandler? OnPing;

        public event AwakeEventHandler? OnUpdate;

        public bool IsAwakeState => _isAwakeState;

        public bool IsLocked => _isLocked;

        public bool IsLockedAwake => _isLocked && _isAwakeState;

        public void Dispose()
        {
            _cancellationTokenSource.Cancel(false);
            _cancellationTokenSource.Dispose();
        }

        public void Lock()
        {
            _isLocked = true;
        }

        public void Refresh()
        {
            CallOnUpdate();
        }

        public bool Start()
        {
            return SetStart(false, false);
        }

        public async Task Start(TimeSpan period, CancellationToken cancellationToken)
        {
            Start();
            using PeriodicTimer timer = new PeriodicTimer(period);
            await timer.WaitForNextTickAsync(cancellationToken);
            Stop();
        }

        public void StartLocked()
        {
            SetStart(true, true);
        }

        public async Task StartLocked(TimeSpan period, CancellationToken cancellationToken)
        {
            StartLocked();
            using PeriodicTimer timer = new PeriodicTimer(period);
            await timer.WaitForNextTickAsync(cancellationToken);
            StopLocked();
        }

        public bool Stop()
        {
            return SetStop(false, false);
        }

        public void StopLocked()
        {
            SetStop(false, true);
        }

        public void Unlock()
        {
            _isLocked = false;
        }

        private void CallOnUpdate()
        {
            if (OnUpdate == null) return;
            OnUpdate(this, new AwakeEventArgs(IsAwakeState, IsLocked));
        }

        private async Task Ping(TimeSpan period, CancellationToken cancellationToken)
        {
            if (OnPing == null || _isAwakeState == false) return;
            using PeriodicTimer timer = new PeriodicTimer(period);
            while (_isAwakeState)
            {
                await timer.WaitForNextTickAsync(cancellationToken);
                if (_isAwakeState == false) break;
                OnPing(this, new AwakeEventArgs(IsAwakeState, IsLocked));
            }
        }

        private bool SetStart(bool lockValue, bool forceUpdate)
        {
            if (_isLocked == true && forceUpdate == false) return false;
            _isLocked = lockValue;
            _isAwakeState = true;
            InteropFunctions.SetThreadExecutionState(InteropFunctions.ES_ALWAYS_AWAKE);
            CallOnUpdate();
            Task.Run(StartPingAsync, _cancellationTokenSource.Token);
            return true;
        }

        private bool SetStop(bool lockValue, bool forceUpdate)
        {
            if (_isLocked == true && forceUpdate == false) return false;
            _isLocked = lockValue;
            _isAwakeState = false;
            InteropFunctions.SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
            CallOnUpdate();
            return true;
        }

        private async Task StartPingAsync()
        {
            if (OnPing == null || _isAwakeState == false) return;
            if (await _semaphore.WaitAsync(TimeSpan.Zero, _cancellationTokenSource.Token) == false) return;
            try
            {
                await Ping(TimeSpan.FromMinutes(1), _cancellationTokenSource.Token);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}