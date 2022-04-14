using System.Runtime.InteropServices;

namespace Mousetrap
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MousePoint
    {
        public int X;
        public int Y;

        public MousePoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Point(MousePoint p) => new Point(p.X, p.Y);
        public static implicit operator MousePoint(Point p) => new MousePoint(p.X, p.Y);
    }
}
