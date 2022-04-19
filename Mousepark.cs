namespace Mousetrap
{
    public partial class Mousepark : Form
    {
        private readonly TimeSpan _period;
        private readonly double _startOpacity;
        private readonly double _stopOpacity;
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
            this.parkLabel.MouseHover += Start;
            this.parkLabel.MouseLeave += Stop;
            this.parkLabel.MouseDoubleClick += Exit;
            this.parkLabel.MouseDown += StartMove;
            this.parkLabel.MouseUp += StopMove;
        }

        private void Cancel()
        {
            if (_cancellation == null) return;
            Thread.EndCriticalRegion();
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

        private Point GetPosition(int no)
        {
            var dist = 5;
            var p = Cursor.Position;
            switch (no)
            {
                case 1: return new Point(p.X + dist, p.Y);
                case 2: return new Point(p.X, p.Y + dist);
                case 3: return new Point(p.X - dist, p.Y);
                case 4: return new Point(p.X, p.Y - dist);
                default: throw new ApplicationException();
            }
        }

        private bool IsSamePosition(Point first, Point second, int accuracyX = 10, int accuracyY = 10)
        {
            if (Math.Abs(first.X - second.X) > accuracyX) return false;
            if (Math.Abs(first.Y - second.Y) > accuracyY) return false;
            return true;
        }

        private async Task MoveMouse(int no, CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested) return;
            Cursor.Position = GetPosition(no);
            await Task.Delay(_period);
        }

        private Point SetDefaultPosition()
        {
            var position = GetDefaultPosition();
            this.Location = position;
            return position;
        }

        private async void Start(object? sender, EventArgs e)
        {
            await Start();
        }

        private async Task Start()
        {
            this.Opacity = _startOpacity;
            this.parkLabel.Text = DateTime.Now.ToString("HH:mm");
            _cancellation = new CancellationTokenSource();
            Thread.BeginCriticalRegion();
            await StartPeriodicMove(_cancellation.Token);
        }

        private void StartMove(object? sender, MouseEventArgs e)
        {
            _isMovingForm = true;
            _lastPosition = Cursor.Position;
        }

        private async Task StartPeriodicMove(CancellationToken cancellation)
        {
            var i = 0;
            while (cancellation.IsCancellationRequested == false)
            {
                i = i++ % 4 + 1;
                await MoveMouse(i, cancellation);
            }
        }

        private void Stop(object? sender, EventArgs e)
        {
            Stop();
        }

        private void Stop()
        {
            this.Opacity = _stopOpacity;
            this.parkLabel.Text = string.Empty;
            Cancel();
        }

        private void StopMove(object? sender, MouseEventArgs e)
        {
            if (_isMovingForm == false) return;
            _isMovingForm = false;
            this.Location = GetPosition();
        }
    }
}