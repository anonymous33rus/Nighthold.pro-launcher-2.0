using System.Windows.Forms;

namespace Nighthold_Launcher.Nighthold
{
    class SystemTray
    {
        public static NotifyIcon notifier = new NotifyIcon();

        public static NightholdLauncher nightholdLauncher;

        public SystemTray(NightholdLauncher _nightholdLauncher)
        {
            nightholdLauncher = _nightholdLauncher;
        }
    }
}
