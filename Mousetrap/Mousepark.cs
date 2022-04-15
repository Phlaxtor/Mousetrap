namespace Mousetrap
{
    public partial class Mousepark : Form
    {
        private readonly double _stopOpacity;
        private readonly double _startOpacity;
        private readonly Mousemove _move;
        private bool _isMovingForm = false;
        private Point _lastPosition;
        private CancellationTokenSource? _cancellation;

        public Mousepark(TimeSpan period, double opacity)
        {
            _startOpacity = 1;
            _stopOpacity = opacity;
            _move = new Mousemove(period);
            this.Opacity = _stopOpacity;
            InitializeComponent();
            _lastPosition = SetDefaultPosition();
            this.parkLabel.MouseHover += Start;
            this.parkLabel.MouseLeave += Stop;
            this.parkLabel.MouseDoubleClick += Exit;
            this.parkLabel.MouseDown += StartMove;
            this.parkLabel.MouseUp += StopMove;
        }

        private Point SetDefaultPosition()
        {
            var position = GetDefaultPosition();
            this.Location = position;
            return position;
        }

        private Point GetDefaultPosition()
        {
            var screenWidth = Screen.PrimaryScreen.WorkingArea.Size.Width;
            var startX = screenWidth - this.Width;
            return new Point(startX, 0);
        }

        private async void Start(object? sender, EventArgs e)
        {
            await Start();
        }

        private void Stop(object? sender, EventArgs e)
        {
            Stop();
        }

        private void Exit(object? sender, MouseEventArgs e)
        {
            Cancel();
            Environment.Exit(0);
        }

        private void StartMove(object? sender, MouseEventArgs e)
        {
            _isMovingForm = true;
            _lastPosition = _move.GetCursorPosition();
        }

        private void StopMove(object? sender, MouseEventArgs e)
        {
            if (_isMovingForm == false) return;
            _isMovingForm = false;
            var defaultPosition = GetDefaultPosition();
            var newPosition = _move.GetCursorPosition();
            if (newPosition.X > defaultPosition.X && newPosition.Y < 50)
            {
                _lastPosition = defaultPosition;
            }
            else if (newPosition.X < 100 && newPosition.Y < 50)
            {
                _lastPosition = new Point(0, 0);
            }
            else if (Math.Abs(newPosition.X - _lastPosition.X) > 10 && Math.Abs(newPosition.Y - _lastPosition.Y) > 10)
            {
                _lastPosition = newPosition;
            }
            else
            {
                _lastPosition = this.Location;
            }
            this.Location = _lastPosition;
        }

        private Point GetPosition()
        {
            Point lastPosition = _lastPosition;
            Point curentPosition = this.Location;
            Point defaultPosition = GetDefaultPosition();
            Point newPosition = _move.GetCursorPosition();
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

        private async ValueTask Start()
        {
            this.Opacity = _startOpacity;
            this.parkLabel.Text = DateTime.Now.ToString("HH:mm");
            _cancellation = new CancellationTokenSource();
            await _move.Start(_cancellation.Token);
        }

        private void Stop()
        {
            this.Opacity = _stopOpacity;
            this.parkLabel.Text = string.Empty;
            Cancel();
        }

        private void Cancel()
        {
            if (_cancellation == null) return;
            _cancellation.Cancel(false);
            _cancellation.Dispose();
            _cancellation = null;
        }
    }
}