using Nighthold_Launcher.NotificationsBarControls.Childs;
using Nighthold_Launcher.Nighthold;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using WebHandler;

namespace Nighthold_Launcher.NotificationsBarControls.Windows
{
    /// <summary>
    /// Interaction logic for NotificationsWindow.xaml
    /// </summary>
    public partial class NotificationsWindow : Window
    {
        public NotificationsWindow()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AnimHandler.FadeIn(SystemTray.nightholdLauncher.OverlayBlur, 300);
            LoadNotifications();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SystemTray.nightholdLauncher.notificationsBar.UpdateVisualNotificationsCount();
            AnimHandler.FadeOut(SystemTray.nightholdLauncher.OverlayBlur, 300);
        }

        public async void LoadNotifications()
        {
            try
            {
                var notificationsCollection = NotificationsClass.NotificationsList.FromJson(await NotificationsClass.GetNotificationsListJson(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword));
                SPNotifications.Children.Clear();
                if (notificationsCollection != null)
                {
                    foreach (var notification in notificationsCollection)
                    {
                        SPNotifications.Children.Add(new NotificationRow(notification.Id, notification.Subject, notification.Message, notification.RedirectUrl, notification.IsMarkedAsRead));
                    }
                    AnimHandler.MoveUpAndFadeIn300Ms(SPNotifications);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }
    }
}
