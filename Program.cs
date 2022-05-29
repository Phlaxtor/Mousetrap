namespace Mousetrap
{
    internal static class Program
    {
        private static readonly Mutex _mutex = new Mutex(true, @"Global\Mousepark.Unique");
        private static readonly uint _showMsg = InteropFunctions.RegisterWindowMessage("WM_SHOWAPPLICATION");

        [STAThread]
        private static void Main()
        {
            if (RunApplication() == false) ShowApplication();
        }

        private static bool RunApplication()
        {
            if (_mutex.WaitOne(TimeSpan.Zero, true) == false) return false;
            StartApplication();
            return true;
        }

        private static void ShowApplication()
        {
            bool r = InteropFunctions.SendNotifyMessageA((IntPtr)InteropFunctions.HWND_BROADCAST, _showMsg, IntPtr.Zero, IntPtr.Zero);
        }

        private static void StartApplication()
        {
            try
            {
                ApplicationConfiguration.Initialize();
                using Mousepark form = new Mousepark(_showMsg);
                Application.Run(form);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.GetType().Name);
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }
    }
}