using System.Runtime.InteropServices;

namespace Mousetrap
{
    public partial class Mousepark : Form
    {
        public const EXECUTION_STATE AlwaysAwakeMode = AwakeState | EXECUTION_STATE.ES_CONTINUOUS;
        public const EXECUTION_STATE AwakeState = EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_SYSTEM_REQUIRED;
        private readonly TimeSpan _period;
        private readonly double _startOpacity;
        private readonly double _stopOpacity;
        private bool _alwaysAwakeModeOn = false;
        private CancellationTokenSource? _cancellation;
        private bool _isMovingForm = false;
        private Point _lastPosition;

        public Mousepark(TimeSpan period, double opacity)
        {
            _period = period;
            _startOpacity = 1;
            _stopOpacity = opacity;
            this.Opacity = _stopOpacity;
            InitializeComponent();
            _lastPosition = SetDefaultPosition();
            this.parkLabel.MouseClick += ToggleAwakeMode;
            this.parkLabel.MouseDoubleClick += Exit;
            this.parkLabel.MouseDown += StartMove;
            this.parkLabel.MouseHover += Start;
            this.parkLabel.MouseLeave += Stop;
            this.parkLabel.MouseUp += StopMove;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        private void Cancel()
        {
            if (_cancellation == null) return;
            _cancellation.Cancel(false);
            _cancellation.Dispose();
            _cancellation = null;
        }

        private void Exit(object? sender, MouseEventArgs e)
        {
            Cancel();
            Environment.Exit(0);
        }

        private Point GetDefaultPosition()
        {
            var screenWidth = Screen.PrimaryScreen.WorkingArea.Size.Width;
            var startX = screenWidth - this.Width;
            return new Point(startX, 0);
        }

        private Point GetPosition()
        {
            Point lastPosition = _lastPosition;
            Point curentPosition = this.Location;
            Point defaultPosition = GetDefaultPosition();
            Point newPosition = Cursor.Position;
            if (newPosition.X > defaultPosition.X && newPosition.Y < 50) return defaultPosition;
            if (newPosition.X < 100 && newPosition.Y < 50) return new Point(0, 0);
            if (IsSamePosition(newPosition, lastPosition) == false) return newPosition;
            return curentPosition;
        }

        private bool IsSamePosition(Point first, Point second, int accuracyX = 10, int accuracyY = 10)
        {
            if (Math.Abs(first.X - second.X) > accuracyX) return false;
            if (Math.Abs(first.Y - second.Y) > accuracyY) return false;
            return true;
        }

        private Point SetDefaultPosition()
        {
            var position = GetDefaultPosition();
            this.Location = position;
            return position;
        }

        private void SetLabel(bool start)
        {
            if (start)
            {
                this.Opacity = _startOpacity;
                this.BackColor = _alwaysAwakeModeOn ? Color.LightGray : Color.LightGray;
                this.parkLabel.BackColor = _alwaysAwakeModeOn ? Color.LightGray : Color.LightGray;
                this.parkLabel.ForeColor = _alwaysAwakeModeOn ? Color.Green : Color.Black;
                this.parkLabel.Text = DateTime.Now.ToString("HH:mm");
            }
            else if (_alwaysAwakeModeOn == false)
            {
                this.Opacity = _stopOpacity;
                this.BackColor = Color.LightGray;
                this.parkLabel.BackColor = Color.LightGray;
                this.parkLabel.ForeColor = Color.Black;
                this.parkLabel.Text = string.Empty;
            }
        }

        private async void Start(object? sender, EventArgs e)
        {
            await Start();
        }

        private async Task Start()
        {
            SetLabel(true);
            _cancellation = new CancellationTokenSource();
            await StartPeriodicMove(_cancellation.Token);
        }

        private void StartMove(object? sender, MouseEventArgs e)
        {
            _isMovingForm = true;
            _lastPosition = Cursor.Position;
        }

        private async Task StartPeriodicMove(CancellationToken cancellation)
        {
            while (cancellation.IsCancellationRequested == false)
            {
                SetThreadExecutionState(AwakeState);
                await Task.Delay(_period);
            }
        }

        private void Stop(object? sender, EventArgs e)
        {
            Stop();
        }

        private void Stop()
        {
            SetLabel(false);
            Cancel();
        }

        private void StopMove(object? sender, MouseEventArgs e)
        {
            if (_isMovingForm == false) return;
            _isMovingForm = false;
            this.Location = GetPosition();
        }

        private void ToggleAwakeMode(object? sender, MouseEventArgs e)
        {
            ToggleAwakeMode();
        }

        private void ToggleAwakeMode()
        {
            _alwaysAwakeModeOn = !_alwaysAwakeModeOn;
            if (_alwaysAwakeModeOn)
            {
                SetLabel(true);
                SetThreadExecutionState(AlwaysAwakeMode);
            }
            else
            {
                SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
            }
        }
    }
}