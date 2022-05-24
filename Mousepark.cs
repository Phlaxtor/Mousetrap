namespace Mousetrap
{
    public partial class Mousepark : Form
    {
        private readonly Color _backColor = Color.LightGray;
        private readonly Color _backColorAlwaysOn = Color.Beige;
        private readonly Color _backColorShow = Color.GreenYellow;
        private readonly double _opacityAlwaysOn = 0.4;
        private readonly double _opacityShow = 0.6;
        private readonly double _opacityStart = 1;
        private readonly double _opacityStop = 0.1;
        private readonly uint _showMsg;
        private bool _alwaysAwakeModeOn = false;
        private bool _isInitAction = false;
        private bool _isMovingForm = false;
        private Point _lastPosition;

        internal Mousepark(uint showMsg)
        {
            _showMsg = showMsg;
            InitializeComponent();
            TryStopAwakeState();
            SetDefaultPosition();
            KeyUp += PerformAction;
            MouseClick += ToggleAwakeMode;
            MouseDoubleClick += Exit;
            MouseDown += InitAction;
            MouseHover += Start;
            MouseLeave += Stop;
            MouseMove += StartMove;
            MouseUp += StopMove;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)_showMsg) MakeVisible();
            base.WndProc(ref m);
        }

        private void Exit(object? sender, MouseEventArgs e)
        {
            InteropFunctions.SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
            Application.Exit();
        }

        private Point GetDefaultPosition()
        {
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Size.Width;
            int startX = screenWidth - Width;
            return new Point(startX, 0);
        }

        private Point GetNewPosition(Point curentPosition)
        {
            Point thisSize = (Point)Size;
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

        private void MakeVisible()
        {
            WindowState = FormWindowState.Normal;
            TopMost = true;
            Opacity = _opacityShow;
            BackColor = _backColorShow;
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
            Point position = GetDefaultPosition();
            Location = position;
            _lastPosition = position;
        }

        private void SetPosition(Point position)
        {
            Point lastPosition = _lastPosition;
            Point newPosition = GetNewPosition(position);
            if (IsSamePosition(newPosition, lastPosition)) newPosition = lastPosition;
            Location = newPosition;
            _lastPosition = newPosition;
        }

        private void Start(object? sender, EventArgs e)
        {
            StartAwakeState();
        }

        private void StartAwakeState()
        {
            InteropFunctions.SetThreadExecutionState(InteropFunctions.ES_ALWAYS_AWAKE);
            Opacity = _alwaysAwakeModeOn ? _opacityAlwaysOn : _opacityStart;
            BackColor = _alwaysAwakeModeOn ? _backColorAlwaysOn : _backColor;
        }

        private void StartMove(object? sender, MouseEventArgs e)
        {
            if (_isInitAction) _isMovingForm = true;
            _isInitAction = false;
        }

        private void Stop(object? sender, EventArgs e)
        {
            TryStopAwakeState();
        }

        private void StopAlwaysOnAwakeState()
        {
            Opacity = _opacityStart;
            BackColor = _backColor;
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
                StartAwakeState();
            }
            else
            {
                StopAlwaysOnAwakeState();
            }
        }

        private void TryStopAwakeState()
        {
            if (_alwaysAwakeModeOn == false)
            {
                InteropFunctions.SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
                Opacity = _opacityStop;
                BackColor = _backColor;
            }
        }
    }
}