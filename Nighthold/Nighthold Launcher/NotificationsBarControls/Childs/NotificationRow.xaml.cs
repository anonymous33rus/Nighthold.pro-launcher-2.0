using Nighthold_Launcher.Nighthold;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Nighthold_Launcher.NotificationsBarControls.Childs
{
    /// <summary>
    /// Interaction logic for NotificationRow.xaml
    /// </summary>
    public partial class NotificationRow : UserControl
    {
        public long pID;
        public string pSubject;
        public string pMessage;
        public string pRedirectUrl;
        public bool pIsMarkedAsRead;

        public NotificationRow(long _id, string _subject, string _message, string _redirectUrl, bool _markedAsRead)
        {
            InitializeComponent();
            pID = _id;
            pSubject = _subject;
            pMessage = _message;
            pRedirectUrl = _redirectUrl;
            pIsMarkedAsRead = _markedAsRead;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Subject.Text = pSubject;
            Message.Text = pMessage;

            if (pIsMarkedAsRead)
            {
                BtnMarkAsRead_Click(sender, e);
            }

            if (pRedirectUrl == null || string.IsNullOrEmpty(pRedirectUrl) || string.IsNullOrWhiteSpace(pRedirectUrl) ||
                (!pRedirectUrl.StartsWith("http://") && !pRedirectUrl.StartsWith("https://")))
            {
                BtnLink.IsEnabled = false;
            }
        }

        private void BtnLink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(pRedirectUrl);
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private async void BtnMarkAsRead_Click(object sender, RoutedEventArgs e)
        {
            Opacity = 0;
            await NotificationsClass.MarkNotificationAsRead(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword, pID.ToString());
            SetVisualMarkAsRead(); // don't reload notifications after marking notification as read
            AnimHandler.MoveUpAndFadeIn(this);
        }

        public void SetVisualMarkAsRead()
        {
            TitleBar.Background = ToolHandler.GetColorFromHex("#FF303030");
            Subject.Foreground = ToolHandler.GetColorFromHex("#FF616161");
            Message.Foreground = ToolHandler.GetColorFromHex("#FF616161");
            TagNew.Visibility = Visibility.Collapsed;
            BtnMarkAsRead.IsEnabled = false;
            Opacity = 0.7;
        }
    }
}
