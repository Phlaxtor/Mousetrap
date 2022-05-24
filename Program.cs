using System.Runtime.InteropServices;

namespace Mousetrap
{
    internal static class Program
    {
        private const int HWND_BROADCAST = 0xffff;
        private static readonly Mutex _mutex = new Mutex(true, @"Global\Mousepark.Unique");
        private static readonly int _showMsg = RegisterWindowMessage("WM_SHOWAPPLICATION");

        [STAThread]
        private static void Main()
        {
            if (_mutex.WaitOne(TimeSpan.Zero, true))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new Mousepark(_showMsg));
                _mutex.ReleaseMutex();
            }
            else
            {
                PostMessage((IntPtr)HWND_BROADCAST, _showMsg, IntPtr.Zero, IntPtr.Zero);
            }
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int RegisterWindowMessage(string message);
    }
}