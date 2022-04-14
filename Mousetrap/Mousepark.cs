namespace Mousetrap
{
    public partial class Mousepark : Form
    {
        private readonly double _opacity;
        private readonly Mousemove _move;
        private CancellationTokenSource? _cancellation;

        public Mousepark(TimeSpan period, double opacity)
        {
            _opacity = opacity;
            InitializeComponent();
            this.Opacity = _opacity;
            var screenWidth = Screen.PrimaryScreen.WorkingArea.Size.Width;
            var startX = screenWidth - this.Width;
            this.Location = new Point(startX, 0);
            _move = new Mousemove(period);
            this.parkLabel.MouseHover += Start;
            this.parkLabel.MouseLeave += Stop;
        }

        private async void Start(object? sender, EventArgs e)
        {
            this.Opacity = 1;
            this.parkLabel.Text = DateTime.Now.ToString("HH:mm");
            _cancellation = new CancellationTokenSource();
            await _move.Start(_cancellation.Token);
        }

        private void Stop(object? sender, EventArgs e)
        {
            this.Opacity = _opacity;
            this.parkLabel.Text = string.Empty;
            if (_cancellation == null) return;
            _cancellation.Cancel(false);
            _cancellation.Dispose();
            _cancellation = null;
        }
    }
}