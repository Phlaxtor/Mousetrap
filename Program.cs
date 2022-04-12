using System.Runtime.InteropServices;
using System.Drawing;

TimeSpan period = TimeSpan.FromSeconds(5);
using PeriodicTimer timer = new PeriodicTimer(period);

[DllImport("user32.dll", SetLastError = true)]
[return: MarshalAs(UnmanagedType.Bool)]
static extern bool GetCursorPos(out POINT lpPoint);

[DllImport("user32.dll")]
static extern bool SetCursorPos(int X, int Y);

while (true)
{
    await timer.WaitForNextTickAsync();
    int oldX = 0;
    int oldY = 0;
    if (GetCursorPos(out POINT p))
    {
        oldX = p.X;
        oldY = p.Y;
        int newX = oldX > 1 ? (oldX / 2) : 100;
        int newY = oldY > 1 ? (oldY / 2) : 100;

        SetCursorPos(newX, newY);
        await Task.Delay(1000);
    }

    SetCursorPos(oldX, oldY);
}

[StructLayout(LayoutKind.Sequential)]
public struct POINT
{
    public int X;
    public int Y;

    public POINT(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static implicit operator Point(POINT p) => new Point(p.X, p.Y);
    public static implicit operator POINT(Point p) => new POINT(p.X, p.Y);
}