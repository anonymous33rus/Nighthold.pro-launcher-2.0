using Nighthold_Launcher.NotificationsBarControls.Windows;
using Nighthold_Launcher.Nighthold;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WebHandler;

namespace Nighthold_Launcher.NotificationsBarControls
{
    /// <summary>
    /// Interaction logic for NotificationsBar.xaml
    /// </summary>
    public partial class NotificationsBar : UserControl
    {
        public List<NotificationPopup> NotificationPopups = new List<NotificationPopup>();

        public int pNotificationsCount = 0;

        public NotificationsBar()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateVisualNotificationsCount();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(60);
            timer.Start();
            timer.Tick += (_s, _e) =>
            {
                timer.Stop();
                UserControl_Loaded(sender, e);
            };
        }

        private void BtnNotifications_Click(object sender, RoutedEventArgs e)
        {
            NotificationsWindow notificationsWindow = new NotificationsWindow();
            notificationsWindow.Owner = SystemTray.nightholdLauncher;
            notificationsWindow.ShowDialog();
        }

        private bool NotificationExists(long _notificationId)
        {
            foreach (var ePopup in NotificationPopups)
            {
                if (ePopup.pNotificationID == _notificationId)
                {
                    return true;
                }
            }

            return false;
        }

        public async void UpdateVisualNotificationsCount()
        {
            pNotificationsCount = 0;

            try
            {
                // get notifications collection from web api
                var notificationsCollection = NotificationsClass.NotificationsList.FromJson(await NotificationsClass.GetNotificationsListJson(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword));
                
                // clear list
                //NotificationPopups.Clear();

                // clear all notification popups
                foreach (var window in Application.Current.Windows)
                {
                    if (window is NotificationPopup)
                    {
                        NotificationPopup nPopup = window as NotificationPopup;
                        nPopup.Close(); // closes/disposes the window
                    }
                }

                // spawn popups & update unread notifications count
                if (notificationsCollection != null)
                {
                    foreach (var notification in notificationsCollection)
                    {
                        if (!notification.IsMarkedAsRead)
                        {
                            if (!NotificationExists(notification.Id))
                            {
                                var notificationPopup = new NotificationPopup(notification.Id, notification.ImageUrl, notification.Subject, notification.Message, notification.RedirectUrl);
                                if (Properties.Settings.Default.EnableNotificationPopups)
                                {
                                    NotificationPopups.Add(notificationPopup);
                                    notificationPopup.Show();
                                }
                            }
                            pNotificationsCount++;
                        }
                    }
                }

                if (pNotificationsCount > 0)
                {
                    BellOff.Visibility = Visibility.Collapsed;
                    BellOn.Visibility = Visibility.Visible;

                    BtnNotificationsOff.Visibility = Visibility.Collapsed;
                    BtnNotificationsOn.Visibility = Visibility.Visible;

                    BtnNotificationsOn.Content = $"{pNotificationsCount} notifications";
                }
                else
                {
                    BellOff.Visibility = Visibility.Visible;
                    BellOn.Visibility = Visibility.Collapsed;

                    BtnNotificationsOff.Visibility = Visibility.Visible;
                    BtnNotificationsOn.Visibility = Visibility.Collapsed;

                    BtnNotificationsOff.Content = $"{pNotificationsCount} notifications";
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }
    }
}
