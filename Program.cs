namespace Mousetrap
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            var mousepark = new Mousepark(TimeSpan.FromSeconds(15), 0.2);
            Application.Run(mousepark);
        }
    }
}