using Nighthold_Launcher.Nighthold;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WebHandler;

namespace Nighthold_Launcher.NotificationsBarControls.Windows
{
    /// <summary>
    /// Interaction logic for NotificationPopup.xaml
    /// </summary>
    public partial class NotificationPopup : Window
    {
        public long pNotificationID;
        public string pImageUrl;
        public string pSubject;
        public string pMessage;
        public string pRedirectUrl;

        public NotificationPopup(long _id, string _imageUrl, string _subject, string _message, string _redirectUrl)
        {
            InitializeComponent();
            pNotificationID = _id;
            pImageUrl = _imageUrl;
            pSubject = _subject;
            pMessage = _message;
            pRedirectUrl = _redirectUrl;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ToolHandler.SetImageSource(NotificationImg, pImageUrl, UriKind.Absolute);
                NotificationSubject.Text = pSubject;
                NotificationMessage.Text = pMessage;

                if (pRedirectUrl == null || string.IsNullOrEmpty(pRedirectUrl) || string.IsNullOrWhiteSpace(pRedirectUrl) ||
                (!pRedirectUrl.StartsWith("http://") && !pRedirectUrl.StartsWith("https://")))
                {
                    NotificationImg.Cursor = Cursors.Arrow;
                    NotificationSubject.Cursor = Cursors.Arrow;
                    NotificationMessage.Cursor = Cursors.Arrow;
                }
                RepositionNotifications();
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }


            if (Properties.Settings.Default.EnableNotificationPopups && Properties.Settings.Default.NotificationPopupsSettingId != 0)
            {
                int closeAfterXSeconds = 0;
                switch (Properties.Settings.Default.NotificationPopupsSettingId)
                {
                    case 1:
                        closeAfterXSeconds = 5;
                        break;
                    case 2:
                        closeAfterXSeconds = 10;
                        break;
                    case 3:
                        closeAfterXSeconds = 20;
                        break;
                    case 4:
                        closeAfterXSeconds = 30;
                        break;
                    case 5:
                        closeAfterXSeconds = 40;
                        break;
                    case 6:
                        closeAfterXSeconds = 50;
                        break;
                    case 7:
                        closeAfterXSeconds = 60;
                        break;
                    default:
                        break;
                }

                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(closeAfterXSeconds);
                timer.Start();
                timer.Tick += (_s, _e) =>
                {
                    timer.Stop();
                    Close();
                };
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();

            SystemTray.nightholdLauncher.notificationsBar.NotificationPopups.Remove(this);

            RepositionNotifications();

            await NotificationsClass.MarkNotificationAsRead(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword, pNotificationID.ToString());

            RecountNotifications();
        }

        public void RepositionNotifications()
        {
            int index = 0;

            foreach (NotificationPopup notification in SystemTray.nightholdLauncher.notificationsBar.NotificationPopups)
            {
                if (notification.IsVisible)
                {
                    var desktopWorkingArea = SystemParameters.WorkArea;
                    notification.Left = desktopWorkingArea.Right - notification.Width - 15;
                    notification.Top = desktopWorkingArea.Bottom - notification.Height - 15 - ((index * notification.Height) + (index * 15));
                    index++;
                }
            }
        }

        public void OpenLink(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (pRedirectUrl == null || string.IsNullOrEmpty(pRedirectUrl) || string.IsNullOrWhiteSpace(pRedirectUrl) ||
                (!pRedirectUrl.StartsWith("http://") && !pRedirectUrl.StartsWith("https://")))
                {
                    // do nothing if link is invalid
                }
                else
                {
                    Process.Start(pRedirectUrl);
                    Button_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private void RecountNotifications()
        {
            int pNotificationsCount = SystemTray.nightholdLauncher.notificationsBar.NotificationPopups.Count;

            if (pNotificationsCount > 0)
            {
                SystemTray.nightholdLauncher.notificationsBar.BellOff.Visibility = Visibility.Collapsed;
                SystemTray.nightholdLauncher.notificationsBar.BellOn.Visibility = Visibility.Visible;

                SystemTray.nightholdLauncher.notificationsBar.BtnNotificationsOff.Visibility = Visibility.Collapsed;
                SystemTray.nightholdLauncher.notificationsBar.BtnNotificationsOn.Visibility = Visibility.Visible;

                SystemTray.nightholdLauncher.notificationsBar.BtnNotificationsOn.Content = $"{pNotificationsCount} notifications";
            }
            else
            {
                SystemTray.nightholdLauncher.notificationsBar.BellOff.Visibility = Visibility.Visible;
                SystemTray.nightholdLauncher.notificationsBar.BellOn.Visibility = Visibility.Collapsed;

                SystemTray.nightholdLauncher.notificationsBar.BtnNotificationsOff.Visibility = Visibility.Visible;
                SystemTray.nightholdLauncher.notificationsBar.BtnNotificationsOn.Visibility = Visibility.Collapsed;

                SystemTray.nightholdLauncher.notificationsBar.BtnNotificationsOff.Content = $"{pNotificationsCount} notifications";
            }
        }
    }
}
