namespace Mousetrap
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Mousepark(TimeSpan.FromSeconds(1), 0.2));
        }
    }
}