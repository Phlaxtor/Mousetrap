namespace Mousetrap
{
    public delegate void AwakeEventHandler(object? sender, AwakeEventArgs e);

    public sealed class AwakeHandler
    {
        private bool _isAwakeState;

        private bool _isLocked;

        public event AwakeEventHandler? OnUpdate;

        public bool IsAwakeState => _isAwakeState;

        public bool IsLocked => _isLocked;

        public bool IsLockedAwake => _isLocked && _isAwakeState;

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

        public async Task Start(TimeSpan period, CancellationToken cancellationToken = default)
        {
            try
            {
                Start();
                CallOnUpdate(period);
                using PeriodicTimer timer = new PeriodicTimer(period);
                await timer.WaitForNextTickAsync(cancellationToken);
                Stop();
            }
            catch
            {
            }
        }

        public void StartLocked()
        {
            SetStart(true, true);
        }

        public async Task StartLocked(TimeSpan period, CancellationToken cancellationToken = default)
        {
            try
            {
                StartLocked();
                CallOnUpdate(period);
                using PeriodicTimer timer = new PeriodicTimer(period);
                await timer.WaitForNextTickAsync(cancellationToken);
                StopLocked();
            }
            catch
            {
            }
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

        private void CallOnUpdate(TimeSpan period = default)
        {
            if (OnUpdate == null) return;
            OnUpdate(this, new AwakeEventArgs(IsAwakeState, IsLocked, period));
        }

        private bool SetStart(bool lockValue, bool forceUpdate)
        {
            if (_isLocked == true && forceUpdate == false) return false;
            _isLocked = lockValue;
            _isAwakeState = true;
            InteropFunctions.SetThreadExecutionState(InteropFunctions.ES_ALWAYS_AWAKE);
            CallOnUpdate();
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
    }
}