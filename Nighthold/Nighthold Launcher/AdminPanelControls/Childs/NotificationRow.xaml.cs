using Nighthold_Launcher.AdminPanelControls.Pages;
using Nighthold_Launcher.Nighthold;
using Nighthold_Launcher.OtherWindows;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WebHandler;

namespace Nighthold_Launcher.AdminPanelControls.Childs
{
    /// <summary>
    /// Interaction logic for NotificationRow.xaml
    /// </summary>
    public partial class NotificationRow : UserControl
    {
        private AdminPanel pAdminPanel;
        private NotificationsManager pNotificationsManager;
        public long pID;
        public string pMention;
        public string pSubject;
        public string pMessage;

        public NotificationRow(AdminPanel _adminPanel, NotificationsManager _notificationsManager, long _id, string _mention, string _subject, string _message)
        {
            InitializeComponent();
            pAdminPanel = _adminPanel;
            pNotificationsManager = _notificationsManager;
            pID = _id;
            pMention = _mention;
            pSubject = _subject;
            pMessage = _message;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (pMention != null)
            {
                Mention.Text = pMention;
                Mention.Foreground = Brushes.Yellow;
            }

            Subject.Text = pSubject;
            Message.Text = pMessage;
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfirmationWindow confirmation = new ConfirmationWindow("Delete notification", $"Sure you want to delete this notification?\r\n<{pSubject}>", false, false, null, pAdminPanel);
                confirmation.Owner = SystemTray.nightholdLauncher;
                if (confirmation.ShowDialog() == true)
                {
                    pAdminPanel.ShowActionMessage($"Deleting notification \"{pSubject}\"");

                    var json = NotificationsClass.NotificationDelete.FromJson(await NotificationsClass.GetNotificationDeleteResponseJson(NightholdLauncher.LoginUsername,
                        NightholdLauncher.LoginPassword, pAdminPanel.SecKey, pID.ToString()));

                    pAdminPanel.ShowActionMessage(json.ResponseMsg);

                    pNotificationsManager.LoadNotifications();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }
    }
}
