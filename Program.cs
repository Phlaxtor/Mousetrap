using System.Runtime.InteropServices;

namespace Mousetrap
{
    internal static class Program
    {
        private const int HWND_BROADCAST = 0xffff;
        private static readonly Mutex _mutex = new Mutex(true, "{C4B75A0E-9202-43C3-BB32-02D5A6F87CC9}");
        private static readonly int _showForm = RegisterWindowMessage("WM_SHOWME");

        [STAThread]
        private static void Main()
        {
            if (_mutex.WaitOne(TimeSpan.Zero, true))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new Mousepark(_showForm));
                _mutex.ReleaseMutex();
            }
            else
            {
                PostMessage((IntPtr)HWND_BROADCAST, _showForm, IntPtr.Zero, IntPtr.Zero);
            }
        }

        [DllImport("user32")]
        private static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32")]
        private static extern int RegisterWindowMessage(string message);
    }
}