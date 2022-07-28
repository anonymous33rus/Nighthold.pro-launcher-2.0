using System.Windows;

namespace Nighthold_Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            NightholdLauncher WindowParent = new NightholdLauncher();
            WindowParent.SetArguments(e.Args);
        }
    }
}
