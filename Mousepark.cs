using System.Runtime.InteropServices;

namespace Mousetrap
{
    public partial class Mousepark : Form
    {
        private const EXECUTION_STATE AwakeState = EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS;
        private readonly double _alwaysOnOpacity = 0.5;
        private readonly Color _backColor = Color.LightGray;
        private readonly Color _backColorAlwaysOn = Color.Beige;
        private readonly Color _foreColor = Color.Black;
        private readonly Color _foreColorAlwaysOn = Color.LimeGreen;
        private readonly double _startOpacity = 1;
        private readonly double _stopOpacity = 0.1;
        private bool _alwaysAwakeModeOn = false;
        private bool _isInitMovingForm = false;
        private bool _isMovingForm = false;
        private Point _lastPosition;

        internal Mousepark()
        {
            InitializeComponent();
            SetStopLabel();
            SetDefaultPosition();
            this.parkLabel.MouseClick += ToggleAwakeMode;
            this.parkLabel.MouseDoubleClick += Exit;
            this.parkLabel.MouseDown += InitMove;
            this.parkLabel.MouseHover += Start;
            this.parkLabel.MouseLeave += Stop;
            this.parkLabel.MouseMove += StartMove;
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
            Point thisSize = (Point)this.Size;
            Point screenSize = (Point)Screen.PrimaryScreen.WorkingArea.Size;
            Point curentPosition = Cursor.Position;
            int x, y;

            if (curentPosition.X < 50) x = 0;
            else if (curentPosition.X > screenSize.X - thisSize.X) x = screenSize.X - thisSize.X;
            else x = curentPosition.X;

            if (curentPosition.Y < 50) y = 0;
            else if (curentPosition.Y > screenSize.Y - thisSize.Y) y = screenSize.Y - thisSize.Y;
            else y = curentPosition.Y;

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
            Point newPosition = GetCurrentPosition();
            if (IsSamePosition(newPosition, lastPosition)) return lastPosition;
            return newPosition;
        }

        private void InitMove(object? sender, MouseEventArgs e)
        {
            _isMovingForm = false;
            _isInitMovingForm = true;
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
            this.Opacity = _alwaysAwakeModeOn ? _alwaysOnOpacity : _startOpacity;
            this.BackColor = _alwaysAwakeModeOn ? _backColorAlwaysOn : _backColor;
            this.parkLabel.BackColor = _alwaysAwakeModeOn ? _backColorAlwaysOn : _backColor;
            this.parkLabel.ForeColor = _alwaysAwakeModeOn ? _foreColorAlwaysOn : _foreColor;
            this.parkLabel.Text = DateTime.Now.ToString("HH:mm");
        }

        private void SetStopAlwaysOnLabel()
        {
            this.Opacity = _startOpacity;
            this.BackColor = _backColor;
            this.parkLabel.BackColor = _backColor;
            this.parkLabel.ForeColor = _foreColor;
        }

        private void SetStopLabel()
        {
            if (_alwaysAwakeModeOn == false)
            {
                this.Opacity = _stopOpacity;
                this.BackColor = _backColor;
                this.parkLabel.BackColor = _backColor;
                this.parkLabel.ForeColor = _foreColor;
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
            if (_isInitMovingForm) _isMovingForm = true;
            _isInitMovingForm = false;
        }

        private void Stop(object? sender, EventArgs e)
        {
            SetStopLabel();
        }

        private void StopMove(object? sender, MouseEventArgs e)
        {
            _isInitMovingForm = false;
            if (_isMovingForm == false) return;
            _isMovingForm = false;
            var newPosition = GetPosition();
            this.Location = newPosition;
            _lastPosition = newPosition;
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
                SetStopAlwaysOnLabel();
                SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
            }
        }
    }
}