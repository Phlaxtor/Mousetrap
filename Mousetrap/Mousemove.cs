using System.Runtime.InteropServices;

namespace Mousetrap
{
    public sealed class Mousemove
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out MousePoint lpPoint);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        public Mousemove(TimeSpan period)
        {
            Period = period;
        }

        public TimeSpan Period { get; }

        public int X { get; private set; }

        public int Y { get; private set; }

        public async Task Start(CancellationToken cancellation)
        {
            if (GetCursorPos(out MousePoint p) == false) return;
            X = p.X;
            Y = p.Y;
            using var timer = new PeriodicTimer(Period);
            while (cancellation.IsCancellationRequested == false)
            {
                await timer.WaitForNextTickAsync();
                await Move(cancellation);
            }
        }

        private async ValueTask Move(CancellationToken cancellation)
        {
            if (cancellation.IsCancellationRequested) return;
            int x = X;
            int y = Y - 1;
            SetCursorPos(x, y);
            await Task.Delay(100);
            SetCursorPos(X, Y);
        }
    }
}
