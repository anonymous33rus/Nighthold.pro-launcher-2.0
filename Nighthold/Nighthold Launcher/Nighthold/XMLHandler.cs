using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Nighthold_Launcher.Nighthold
{
    class XMLHandler
    {
        public static async Task LoadXMLRemoteConfigAsync()
        {
            try
            {
                await Task.Run(() => Documents.RemoteConfig.Load(Properties.Settings.Default.XMLDocumentUrl));
                PeriodicallyCheckLauncherVersion();
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private static async void PeriodicallyCheckLauncherVersion()
        {
            var LV = WebHandler.FilesListClass.LVersionResponse.FromJson(await WebHandler.FilesListClass.GetLauncherVersionResponseJson());

            if (LV.Version != System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())
            {
                AnimHandler.FadeIn(SystemTray.nightholdLauncher.NightholdUpdate, 300);
            }

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(60);
            timer.Start();
            timer.Tick += (_s, _e) =>
            {
                timer.Stop();
                PeriodicallyCheckLauncherVersion();
            };
        }
    }
}
