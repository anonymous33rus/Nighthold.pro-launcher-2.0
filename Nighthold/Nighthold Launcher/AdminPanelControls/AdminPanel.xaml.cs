
using Nighthold_Launcher.AdminPanelControls.Pages;
using Nighthold_Launcher.Nighthold;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Nighthold_Launcher.AdminPanelControls
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        public string SecKey;

        public AdminPanel()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AnimHandler.FadeIn(SystemTray.nightholdLauncher.OverlayBlur, 300);
            AnimHandler.FadeIn(this, 1000);
            SecurityKeyForm.Visibility = Visibility.Visible;
            SecurityKey.Focus();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AnimHandler.FadeOut(SystemTray.nightholdLauncher.OverlayBlur, 300);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnAdmin_Click(object sender, RoutedEventArgs e)
        {
            ShowAdminPage();
            ShowActionMessage("Showing admin panel..");
        }

        private void BtnNewsManager_Click(object sender, RoutedEventArgs e)
        {
            ShowNewsManagerPage();
            ShowActionMessage("Showing news manager..");
        }

        private void BtnNotificationsManager_Click(object sender, RoutedEventArgs e)
        {
            ShowNotificationsManagerPage();
            ShowActionMessage("Showing notifications manager..");
        }

        private void BtnSoapLogs_Click(object sender, RoutedEventArgs e)
        {
            ShowSoapLogsPage();
            ShowActionMessage("Showing soap logs..");
        }

        private void BtnActiveSessions_Click(object sender, RoutedEventArgs e)
        {
            ShowActiveSessionsPage();
            ShowActionMessage("Showing active sessions..");
        }

        public void ShowAdminPage()
        {
            PanelGrid.Children.Clear();
            PanelGrid.Children.Add(new Admin(this));
        }

        public void ShowNewsManagerPage()
        {
            PanelGrid.Children.Clear();
            PanelGrid.Children.Add(new NewsManager(this));
        }

        public void ShowNotificationsManagerPage()
        {
            PanelGrid.Children.Clear();
            PanelGrid.Children.Add(new NotificationsManager(this));
        }

        public void ShowSoapLogsPage()
        {
            PanelGrid.Children.Clear();
            PanelGrid.Children.Add(new SoapLogs(this));
        }

        public void ShowActiveSessionsPage()
        {
            PanelGrid.Children.Clear();
            PanelGrid.Children.Add(new ActiveSessions(this));
        }

        public async void ShowActionMessage(string message)
        {
            try
            {
                SPActionSentMessage.Children.Clear();
                TextBlock labelMessage = new TextBlock()
                {
                    Text = message,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(10, 0, 0, 0),
                    Background = null,
                    Foreground = ToolHandler.GetColorFromHex("#FFD3D697"),
                    FontWeight = FontWeights.Bold,
                    FontFamily = new System.Windows.Media.FontFamily("Open Sans"),
                    TextTrimming = TextTrimming.CharacterEllipsis,
                };
                SPActionSentMessage.Children.Add(labelMessage);
                await AnimHandler.MoveUpAndFadeInThenFadeOut(labelMessage, 3500);
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private async void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            SecurityErrorBlock.Visibility = Visibility.Collapsed;
            LoadingSpinner.Visibility = Visibility.Visible;
            SecurityErrorBlock.Text = string.Empty;
            SecurityKey.IsEnabled = false;

            try
            {
                var secPa = AuthClass.SecPaResponse.FromJson(await AuthClass.GetSecPResponseJson(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword, ToolHandler.StringToMD5(SecurityKey.Password)));
                SecurityKey.IsEnabled = true;
                LoadingSpinner.Visibility = Visibility.Collapsed;
                
                if (secPa != null)
                {
                    if (secPa.Response)
                    {
                        SecKey = ToolHandler.StringToMD5(SecurityKey.Password);
                        SecurityKeyForm.Visibility = Visibility.Hidden;

                        PanelGrid.Children.Clear();
                        PanelGrid.Children.Add(new NewsManager(this));

                        ShowActionMessage($"Welcome {NightholdLauncher.LoginUsername} buddy, you are a part of the admin staff. What are you up to?");
                    }
                    else
                    {
                        SecurityErrorBlock.Visibility = Visibility.Visible;
                        SecurityErrorBlock.Text = "Invalid security key";
                        SecurityKey.Focus();
                    }

                    SecurityKey.Password = string.Empty;
                }
                else
                {
                    SecurityErrorBlock.Visibility = Visibility.Visible;
                    SecurityErrorBlock.Text = "Invalid security key";
                    SecurityKey.Focus();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private void SecurityKey_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                BtnYes_Click(sender, e);
        }
    }
}
