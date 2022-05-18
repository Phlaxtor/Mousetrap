namespace Mousetrap
{
    internal static class Program
    {
        private static readonly Mutex _mutex = new Mutex(true, "{C4B75A0E-9202-43C3-BB32-02D5A6F87CC9}");

        [STAThread]
        private static void Main()
        {
            if (_mutex.WaitOne(TimeSpan.Zero, true))
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new Mousepark());
                _mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("Mousetrap is already running");
            }
        }
    }
}