using System.Runtime.InteropServices;

namespace Mousetrap
{
    public partial class Mousepark : Form
    {
        private const EXECUTION_STATE AwakeState = EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS;
        private readonly double _startOpacity = 1;
        private readonly double _stopOpacity = 0.1;
        private bool _alwaysAwakeModeOn = false;
        private bool _isMovingForm = false;
        private Point _lastPosition;

        internal Mousepark()
        {
            InitializeComponent();
            SetStopLabel();
            SetDefaultPosition();
            this.parkLabel.MouseClick += ToggleAwakeMode;
            this.parkLabel.MouseDoubleClick += Exit;
            this.parkLabel.MouseDown += StartMove;
            this.parkLabel.MouseHover += Start;
            this.parkLabel.MouseLeave += Stop;
            this.parkLabel.MouseUp += StopMove;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        private void Exit(object? sender, MouseEventArgs e)
        {
            Environment.Exit(0);
        }

        private Point GetCurrentPosition()
        {
            var screenSize = Screen.PrimaryScreen.WorkingArea.Size;
            var curentPosition = Cursor.Position;
            var x = curentPosition.X;
            var y = curentPosition.Y;
            if (x < 50) x = 0;
            if (x > screenSize.Width - 50) y = screenSize.Width - 200;
            if (y < 50) x = 0;
            if (y > screenSize.Height - 50) y = screenSize.Height - 50;
            return new Point(x, y);
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
            Point curentPosition = GetCurrentPosition();
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

        private void SetDefaultPosition()
        {
            var position = GetDefaultPosition();
            this.Location = position;
            _lastPosition = position;
        }

        private void SetStartLabel()
        {
            this.Opacity = _startOpacity;
            this.BackColor = _alwaysAwakeModeOn ? Color.LightGray : Color.LightGray;
            this.parkLabel.BackColor = _alwaysAwakeModeOn ? Color.LightGray : Color.LightGray;
            this.parkLabel.ForeColor = _alwaysAwakeModeOn ? Color.Green : Color.Black;
            this.parkLabel.Text = DateTime.Now.ToString("HH:mm");
        }

        private void SetStopLabel()
        {
            if (_alwaysAwakeModeOn == false)
            {
                this.Opacity = _stopOpacity;
                this.BackColor = Color.LightGray;
                this.parkLabel.BackColor = Color.LightGray;
                this.parkLabel.ForeColor = Color.Black;
                this.parkLabel.Text = string.Empty;
            }
        }

        private void Start(object? sender, EventArgs e)
        {
            SetStartLabel();
            SetThreadExecutionState(AwakeState);
        }

        private void StartMove(object? sender, MouseEventArgs e)
        {
            _isMovingForm = true;
            _lastPosition = Cursor.Position;
        }

        private void Stop(object? sender, EventArgs e)
        {
            SetStopLabel();
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
                SetStartLabel();
                SetThreadExecutionState(AwakeState);
            }
            else
            {
                SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
            }
        }
    }
}