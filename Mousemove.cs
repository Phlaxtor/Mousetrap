using System.Runtime.InteropServices;

namespace Mousetrap
{
    public sealed class Mousemove
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out MousePoint lpPoint);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        public Mousemove(TimeSpan period)
        {
            Period = period;
        }

        public TimeSpan Period { get; }

        public int X { get; private set; }

        public int Y { get; private set; }

        public async Task Start(CancellationToken cancellation)
        {
            if (SaveCurrentCursorPosition() == false) return;
            await StartPeriodicMove(cancellation);
        }

        public Point GetCursorPosition()
        {
            if (TryGetCursorPosition(out int x, out int y)) return new Point(x, y);
            throw new InvalidOperationException("Not possible to get mouse position");
        }

        private bool SaveCurrentCursorPosition()
        {
            if (TryGetCursorPosition(out int x, out int y) == false) return false;
            X = x;
            Y = y;
            return true;
        }

        private bool TryGetCursorPosition(out int x, out int y)
        {
            if (GetCursorPos(out MousePoint p))
            {
                x = p.X;
                y = p.Y;
                return true;
            }
            x = 0;
            y = 0;
            return false;
        }

        private async Task StartPeriodicMove(CancellationToken cancellation)
        {
            using var timer = new PeriodicTimer(Period);
            while (cancellation.IsCancellationRequested == false)
            {
                await WaitForNextMove(timer, cancellation);
            }
        }

        private async Task WaitForNextMove(PeriodicTimer timer, CancellationToken cancellation)
        {
            try
            {
                await timer.WaitForNextTickAsync(cancellation);
                await Move(cancellation);
            }
            catch
            {
            }
        }

        private async ValueTask Move(CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested) return;
            var distance = 5;
            var wait = 100;
            SetCursorPos(X + distance, Y);
            await Task.Delay(wait);
            SetCursorPos(X, Y + distance);
            await Task.Delay(wait);
            SetCursorPos(X - distance, Y);
            await Task.Delay(wait);
            SetCursorPos(X, Y - distance);
            await Task.Delay(wait);
        }
    }
}
