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
        private CancellationTokenSource? _cancellationTokenSource;
        private bool _isInAction = false;
        private bool _isMovingForm = false;
        private Point _lastPosition;

        internal Mousepark(uint showMsg)
        {
            _showMsg = showMsg;
            InitializeComponent();
            TryEndAwakeState();
            SetPosition(ParkPosition.UpperRight);
            KeyUp += MainKeyUp;
            MouseClick += MainMouseClick;
            MouseDoubleClick += MainMouseDoubleClick;
            MouseDown += MainMouseDown;
            MouseHover += MainMouseHover;
            MouseLeave += MainMouseLeave;
            MouseMove += MainMouseMove;
            MouseUp += MainMouseUp;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)_showMsg) ShowApplication();
            base.WndProc(ref m);
        }

        private void DoCancellation()
        {
            if (_cancellationTokenSource == null) return;
            _cancellationTokenSource.Cancel(false);
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }

        private void EndAlwaysAwakeState()
        {
            _alwaysAwakeModeOn = false;
            EndAwakeState();
            SetDefaultPresentation();
        }

        private void EndAwakeState()
        {
            InteropFunctions.SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
            Opacity = _alwaysAwakeModeOn ? _opacityAlwaysOn : _opacityStop;
            BackColor = _alwaysAwakeModeOn ? _backColorAlwaysOn : _backColor;
        }

        private async Task ExecuteTimer(TimeSpan period, Action action)
        {
            await ExecuteTimer(period);
            action();
        }

        private async Task ExecuteTimer(TimeSpan period)
        {
            try
            {
                CancellationToken cancellationToken = GetCancellationToken();
                using PeriodicTimer? timer = new PeriodicTimer(period);
                await timer.WaitForNextTickAsync(cancellationToken);
            }
            catch
            {
            }
            finally
            {
                DoCancellation();
            }
        }

        private void Exit()
        {
            InteropFunctions.SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
            Application.Exit();
        }

        private CancellationToken GetCancellationToken()
        {
            if (_cancellationTokenSource != null) return _cancellationTokenSource.Token;
            _cancellationTokenSource = new CancellationTokenSource();
            return _cancellationTokenSource.Token;
        }

        private Point GetNewPosition(Point curentPosition)
        {
            Point thisSize = (Point)Size;
            Point screenSize = (Point)Screen.PrimaryScreen.WorkingArea.Size;
            int x, y;

            if (curentPosition.X < 50) x = 0;
            else if (curentPosition.X > screenSize.X - thisSize.X) x = screenSize.X - thisSize.X;
            else x = curentPosition.X - (thisSize.X / 2);

            if (curentPosition.Y < 50) y = 0;
            else if (curentPosition.Y > screenSize.Y - thisSize.Y) y = screenSize.Y - thisSize.Y;
            else y = curentPosition.Y - (thisSize.Y / 2);

            return new Point(x, y);
        }

        private TimeSpan GetTimerValue()
        {
            using TimerDialog? dialog = new TimerDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK) return dialog.Value;
            return TimeSpan.Zero;
        }

        private void HideApplication()
        {
            WindowState = FormWindowState.Minimized;
            TopMost = false;
        }

        private bool IsCursorHovering()
        {
            if (Cursor.Position.X > _lastPosition.X + (Size.Width / 2)) return false;
            if (Cursor.Position.X < _lastPosition.X - (Size.Width / 2)) return false;
            if (Cursor.Position.Y > _lastPosition.Y + (Size.Height / 2)) return false;
            if (Cursor.Position.Y < _lastPosition.Y - (Size.Height / 2)) return false;
            return true;
        }

        private bool IsSamePosition(Point first, Point second, int accuracyX = 10, int accuracyY = 10)
        {
            if (Math.Abs(first.X - second.X) > accuracyX) return false;
            if (Math.Abs(first.Y - second.Y) > accuracyY) return false;
            return true;
        }

        private async void MainKeyUp(object? sender, KeyEventArgs e)
        {
            if (_isInAction == false) return;
            _isInAction = false;

            switch (e.KeyCode)
            {
                case Keys.D1:
                    SetPosition(ParkPosition.UpperLeft);
                    break;

                case Keys.D2:
                    SetPosition(ParkPosition.UpperMiddle);
                    break;

                case Keys.D3:
                    SetPosition(ParkPosition.UpperRight);
                    break;

                case Keys.D4:
                    SetPosition(ParkPosition.MiddleLeft);
                    break;

                case Keys.D5:
                    SetPosition(ParkPosition.MiddleMiddle);
                    break;

                case Keys.D6:
                    SetPosition(ParkPosition.MiddleRight);
                    break;

                case Keys.D7:
                    SetPosition(ParkPosition.LowerLeft);
                    break;

                case Keys.D8:
                    SetPosition(ParkPosition.LowerMiddle);
                    break;

                case Keys.D9:
                    SetPosition(ParkPosition.LowerRight);
                    break;

                case Keys.A:
                    await ShowApplicationAsync();
                    break;

                case Keys.C:
                    DoCancellation();
                    break;

                case Keys.E:
                    EndAlwaysAwakeState();
                    break;

                case Keys.H:
                    HideApplication();
                    break;

                case Keys.M:
                    _isMovingForm = true;
                    break;

                case Keys.Q:
                    Exit();
                    break;

                case Keys.S:
                    StartAlwaysAwakeState();
                    break;

                case Keys.T:
                    await StartTimer();
                    break;
            }
        }

        private void MainMouseClick(object? sender, MouseEventArgs e)
        {
            StartAction();
        }

        private void MainMouseDoubleClick(object? sender, MouseEventArgs e)
        {
            Exit();
        }

        private void MainMouseDown(object? sender, MouseEventArgs e)
        {
        }

        private void MainMouseHover(object? sender, EventArgs e)
        {
            StartAwakeState();
        }

        private void MainMouseLeave(object? sender, EventArgs e)
        {
            TryEndAwakeState();
        }

        private void MainMouseMove(object? sender, MouseEventArgs e)
        {
            MoveForm();
        }

        private void MainMouseUp(object? sender, MouseEventArgs e)
        {
        }

        private void MoveForm()
        {
            if (_isMovingForm == false) return;
            SetPosition(Cursor.Position);
        }

        private void SetDefaultPresentation()
        {
            WindowState = FormWindowState.Normal;
            TopMost = true;
            double defaultOpacity = IsCursorHovering() ? _opacityStart : _opacityStop;
            Opacity = _alwaysAwakeModeOn ? _opacityAlwaysOn : defaultOpacity;
            BackColor = _alwaysAwakeModeOn ? _backColorAlwaysOn : _backColor;
        }

        private void SetPosition(ParkPosition position)
        {
            Point screenSize = (Point)Screen.PrimaryScreen.WorkingArea.Size;
            int maxX = screenSize.X;
            int maxY = screenSize.Y;
            int middleX = screenSize.X / 2;
            int middleY = screenSize.Y / 2;
            switch (position)
            {
                case ParkPosition.UpperLeft:
                    SetPosition(new Point(0, 0));
                    break;

                case ParkPosition.UpperMiddle:
                    SetPosition(new Point(middleX, 0));
                    break;

                case ParkPosition.UpperRight:
                    SetPosition(new Point(maxX, 0));
                    break;

                case ParkPosition.MiddleLeft:
                    SetPosition(new Point(0, middleY));
                    break;

                case ParkPosition.MiddleMiddle:
                    SetPosition(new Point(middleX, middleY));
                    break;

                case ParkPosition.MiddleRight:
                    SetPosition(new Point(maxX, middleY));
                    break;

                case ParkPosition.LowerLeft:
                    SetPosition(new Point(0, maxY));
                    break;

                case ParkPosition.LowerMiddle:
                    SetPosition(new Point(middleX, maxY));
                    break;

                case ParkPosition.LowerRight:
                    SetPosition(new Point(maxX, maxY));
                    break;
            }
        }

        private void SetPosition(Point position)
        {
            Point lastPosition = _lastPosition;
            Point newPosition = GetNewPosition(position);
            if (IsSamePosition(newPosition, lastPosition)) newPosition = lastPosition;
            Location = newPosition;
            _lastPosition = newPosition;
        }

        private void ShowApplication()
        {
            TimeSpan time = TimeSpan.FromSeconds(3);
            ShowApplicationAsync().Wait(time);
        }

        private async Task ShowApplicationAsync()
        {
            WindowState = FormWindowState.Normal;
            TopMost = true;
            Opacity = _opacityShow;
            BackColor = _backColorShow;
            TimeSpan time = TimeSpan.FromSeconds(2);
            await ExecuteTimer(time, SetDefaultPresentation);
        }

        private void StartAction()
        {
            _isInAction = true;
            _isMovingForm = false;
        }

        private void StartAlwaysAwakeState()
        {
            _alwaysAwakeModeOn = true;
            StartAwakeState();
        }

        private void StartAwakeState()
        {
            InteropFunctions.SetThreadExecutionState(InteropFunctions.ES_ALWAYS_AWAKE);
            Opacity = _alwaysAwakeModeOn ? _opacityAlwaysOn : _opacityStart;
            BackColor = _alwaysAwakeModeOn ? _backColorAlwaysOn : _backColor;
        }

        private async Task StartTimer()
        {
            TimeSpan time = GetTimerValue();
            if (time == TimeSpan.Zero) return;
            StartAlwaysAwakeState();
            await ExecuteTimer(time, EndAlwaysAwakeState);
        }

        private bool TryEndAwakeState()
        {
            if (_alwaysAwakeModeOn == true) return false;
            EndAwakeState();
            return true;
        }
    }
}