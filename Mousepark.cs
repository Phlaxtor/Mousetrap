using System.Runtime.InteropServices;

namespace Mousetrap
{
    public partial class Mousepark : Form
    {
        private const EXECUTION_STATE AwakeState = EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS;
        private readonly double _alwaysOnOpacity = 0.5;
        private readonly Color _backColor = Color.LightGray;
        private readonly Color _backColorAlwaysOn = Color.Beige;
        private readonly double _startOpacity = 1;
        private readonly double _stopOpacity = 0.1;
        private bool _alwaysAwakeModeOn = false;
        private bool _isInitAction = false;
        private bool _isMovingForm = false;
        private Point _lastPosition;

        internal Mousepark()
        {
            InitializeComponent();
            SetStopLabel();
            SetDefaultPosition();
            this.KeyUp += PerformAction;
            this.MouseClick += ToggleAwakeMode;
            this.MouseDoubleClick += Exit;
            this.MouseDown += InitAction;
            this.MouseHover += Start;
            this.MouseLeave += Stop;
            this.MouseMove += StartMove;
            this.MouseUp += StopMove;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        private void Exit(object? sender, MouseEventArgs e)
        {
            Environment.Exit(0);
        }

        private Point GetDefaultPosition()
        {
            var screenWidth = Screen.PrimaryScreen.WorkingArea.Size.Width;
            var startX = screenWidth - this.Width;
            return new Point(startX, 0);
        }

        private Point GetNewPosition(Point curentPosition)
        {
            Point thisSize = (Point)this.Size;
            Point screenSize = (Point)Screen.PrimaryScreen.WorkingArea.Size;
            int x, y;

            if (curentPosition.X < 50) x = 0;
            else if (curentPosition.X > screenSize.X - thisSize.X) x = screenSize.X - thisSize.X;
            else x = curentPosition.X;

            if (curentPosition.Y < 50) y = 0;
            else if (curentPosition.Y > screenSize.Y - thisSize.Y) y = screenSize.Y - thisSize.Y;
            else y = curentPosition.Y;

            return new Point(x, y);
        }

        private void InitAction(object? sender, MouseEventArgs e)
        {
            _isMovingForm = false;
            _isInitAction = true;
        }

        private bool IsSamePosition(Point first, Point second, int accuracyX = 10, int accuracyY = 10)
        {
            if (Math.Abs(first.X - second.X) > accuracyX) return false;
            if (Math.Abs(first.Y - second.Y) > accuracyY) return false;
            return true;
        }

        private void PerformAction(object? sender, KeyEventArgs e)
        {
            if (_isInitAction == false) return;
            _isInitAction = false;
            Point screenSize = (Point)Screen.PrimaryScreen.WorkingArea.Size;
            int maxX = screenSize.X;
            int maxY = screenSize.Y;
            int middleX = screenSize.X / 2;
            int middleY = screenSize.Y / 2;
            switch (e.KeyCode)
            {
                case Keys.Q:
                    SetPosition(new Point(0, 0));
                    break;

                case Keys.W:
                    SetPosition(new Point(middleX, 0));
                    break;

                case Keys.E:
                    SetPosition(new Point(maxX, 0));
                    break;

                case Keys.D:
                    SetPosition(new Point(maxX, middleY));
                    break;

                case Keys.C:
                    SetPosition(new Point(maxX, maxY));
                    break;

                case Keys.X:
                    SetPosition(new Point(middleX, maxY));
                    break;

                case Keys.Z:
                    SetPosition(new Point(0, maxY));
                    break;

                case Keys.A:
                    SetPosition(new Point(0, middleY));
                    break;
            }
        }

        private void SetDefaultPosition()
        {
            var position = GetDefaultPosition();
            this.Location = position;
            _lastPosition = position;
        }

        private void SetPosition(Point position)
        {
            Point lastPosition = _lastPosition;
            Point newPosition = GetNewPosition(position);
            if (IsSamePosition(newPosition, lastPosition)) newPosition = lastPosition;
            this.Location = newPosition;
            _lastPosition = newPosition;
        }

        private void SetStartLabel()
        {
            this.Opacity = _alwaysAwakeModeOn ? _alwaysOnOpacity : _startOpacity;
            this.BackColor = _alwaysAwakeModeOn ? _backColorAlwaysOn : _backColor;
        }

        private void SetStopAlwaysOnLabel()
        {
            this.Opacity = _startOpacity;
            this.BackColor = _backColor;
        }

        private void SetStopLabel()
        {
            if (_alwaysAwakeModeOn == false)
            {
                this.Opacity = _stopOpacity;
                this.BackColor = _backColor;
            }
        }

        private void Start(object? sender, EventArgs e)
        {
            SetStartLabel();
            SetThreadExecutionState(AwakeState);
        }

        private void StartMove(object? sender, MouseEventArgs e)
        {
            if (_isInitAction) _isMovingForm = true;
            _isInitAction = false;
        }

        private void Stop(object? sender, EventArgs e)
        {
            SetStopLabel();
        }

        private void StopMove(object? sender, MouseEventArgs e)
        {
            _isInitAction = false;
            if (_isMovingForm == false) return;
            _isMovingForm = false;
            SetPosition(Cursor.Position);
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