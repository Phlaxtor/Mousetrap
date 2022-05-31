namespace Mousetrap
{
    public partial class Mousepark : Form
    {
        private readonly AwakeHandler _handler;
        private readonly uint _showMsg;
        private MouseparkAction _action;
        private CancellationTokenSource? _cancellationTokenSource;
        private Point _lastPosition;

        internal Mousepark(uint showMsg)
        {
            InitializeComponent();
            _handler = new AwakeHandler();
            _showMsg = showMsg;
            SetPosition(ParkPosition.UpperRight);
            KeyUp += MainKeyUp;
            MouseClick += MainMouseClick;
            MouseDoubleClick += MainMouseDoubleClick;
            MouseDown += MainMouseDown;
            MouseHover += MainMouseHover;
            MouseLeave += MainMouseLeave;
            MouseMove += MainMouseMove;
            MouseUp += MainMouseUp;
            _handler.OnUpdate += OnUpdate;
            _handler.Stop();
        }

        public override void Refresh()
        {
            base.Refresh();
            WindowState = FormWindowState.Normal;
            TopMost = true;
            _handler.Refresh();
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
            _handler.StopLocked();
            DoCancellation();
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

        private void InitAlwaysOn()
        {
            WindowState = FormWindowState.Normal;
            TopMost = true;
            Opacity = 0.4;
            BackColor = Color.Beige;
        }

        private void InitOff()
        {
            WindowState = FormWindowState.Normal;
            TopMost = true;
            Opacity = 0.1;
            BackColor = Color.LightGray;
        }

        private void InitOn()
        {
            WindowState = FormWindowState.Normal;
            TopMost = true;
            Opacity = 1;
            BackColor = Color.LightGray;
        }

        private void InitShow()
        {
            WindowState = FormWindowState.Normal;
            TopMost = true;
            Opacity = 0.6;
            BackColor = Color.GreenYellow;
        }

        private bool IsSamePosition(Point first, Point second, int accuracyX = 10, int accuracyY = 10)
        {
            if (Math.Abs(first.X - second.X) > accuracyX) return false;
            if (Math.Abs(first.Y - second.Y) > accuracyY) return false;
            return true;
        }

        private async void MainKeyUp(object? sender, KeyEventArgs e)
        {
            if (_action != MouseparkAction.Start) return;
            _action = MouseparkAction.None;

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
                    _handler.StopLocked();
                    break;

                case Keys.H:
                    HideApplication();
                    break;

                case Keys.M:
                    _action = MouseparkAction.Move;
                    break;

                case Keys.Q:
                    Exit();
                    break;

                case Keys.S:
                    _handler.StartLocked();
                    break;

                case Keys.T:
                    await StartTimer();
                    break;
            }
        }

        private void MainMouseClick(object? sender, MouseEventArgs e)
        {
            _action = MouseparkAction.Start;
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
            _handler.Start();
        }

        private void MainMouseLeave(object? sender, EventArgs e)
        {
            _handler.Stop();
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
            if (_action != MouseparkAction.Move) return;
            SetPosition(Cursor.Position);
        }

        private void OnUpdate(object? sender, AwakeEventArgs e)
        {
            if (e.IsLocked == true && e.IsAwakeState == true) InitAlwaysOn();
            if (e.IsLocked == false && e.IsAwakeState == true) InitOn();
            if (e.IsLocked == false && e.IsAwakeState == false) InitOff();
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
            InitShow();
            TimeSpan time = TimeSpan.FromSeconds(2);
            await ExecuteTimer(time, Refresh);
        }

        private async Task StartTimer()
        {
            try
            {
                TimeSpan period = GetTimerValue();
                if (period == TimeSpan.Zero) return;
                CancellationToken cancellationToken = GetCancellationToken();
                await _handler.StartLocked(period, cancellationToken);
            }
            catch
            {
            }
            finally
            {
                DoCancellation();
            }
        }
    }
}