using Nighthold_Launcher.AccountStandingBarControls.Childs;
using Nighthold_Launcher.Nighthold;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WebHandler;

namespace Nighthold_Launcher.AccountStandingBarControls.Windows
{
    /// <summary>
    /// Interaction logic for SinsHistory.xaml
    /// </summary>
    public partial class SinsHistory : Window
    {
        public SinsHistory()
        {
            InitializeComponent();

            LoadSinsHistory();
        }

        public async void LoadSinsHistory()
        {
            try
            {
                var sinsHistoryCollection = SinsHistoryClass.SinsHistoryList.FromJson(
                    await SinsHistoryClass.GetSinsHistoryListJson(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword));

                SPSinsHistory.Children.Clear();

                if (sinsHistoryCollection != null)
                {
                    foreach (var sin in sinsHistoryCollection)
                    {
                        // account bans history
                        foreach (var item in sin.AccBanLogs)
                        {
                            SinHistoryRow sinRow = new SinHistoryRow(0, string.Empty, item.BanDate, item.Duration, item.UnbanDate, string.Empty, item.Reason);
                            SPSinsHistory.Children.Add(sinRow);
                        }

                        // character bans history
                        foreach (var item in sin.CharBanLogs)
                        {
                            SinHistoryRow sinRow = new SinHistoryRow(1, item.CharName, item.BanDate, item.Duration, item.UnbanDate, item.Realm, item.Reason);
                            SPSinsHistory.Children.Add(sinRow);
                        }
                    }
                }
                AnimHandler.MoveUpAndFadeIn300Ms(SPSinsHistory);
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
            finally
            {
                if (SPSinsHistory.Children.Count <= 0)
                {
                    var label = new Label()
                    {
                        Content = "You are in a good standing!",
                        Foreground = Brushes.Lime,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Background = null,
                        Margin = new Thickness(0, 10, 0, 0),
                        FontFamily = new FontFamily("#Open Sans"),
                        FontSize = 14
                    };

                    SPSinsHistory.Children.Add(label);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AnimHandler.FadeIn(SystemTray.nightholdLauncher.OverlayBlur, 300);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AnimHandler.FadeOut(SystemTray.nightholdLauncher.OverlayBlur, 300);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
