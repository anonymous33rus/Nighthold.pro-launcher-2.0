using Nighthold_Launcher.AdminPanelControls.Childs;
using Nighthold_Launcher.Nighthold;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Nighthold_Launcher.AdminPanelControls.Pages
{
    /// <summary>
    /// Interaction logic for ActiveSessions.xaml
    /// </summary>
    public partial class ActiveSessions : UserControl
    {
        AdminPanel pAdminPanel;

        public ActiveSessions(AdminPanel _adminPanel)
        {
            InitializeComponent();
            pAdminPanel = _adminPanel;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var activeSessionsCollection = CiSessionsClass.ActiveSessionsList.FromJson(await CiSessionsClass.GetActiveSessionsListJson(NightholdLauncher.LoginUsername,
                    NightholdLauncher.LoginPassword, pAdminPanel.SecKey));
                SPActiveSessions.Children.Clear();
                if (activeSessionsCollection != null)
                {
                    foreach (var activeSession in activeSessionsCollection)
                    {
                        var activeSessionRow = new ActiveSessionRow(pAdminPanel,
                            activeSession.AvatarUrl,
                            activeSession.AccountId,
                            activeSession.AccountName,
                            activeSession.LastIp);

                        SPActiveSessions.Children.Add(activeSessionRow);
                    }
                }

                AnimHandler.MoveUpAndFadeIn(SPActiveSessions);
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }

            if (SPActiveSessions.Children.Count == 0)
            {
                SPActiveSessions.Children.Add(new Label() { 
                    Content = "No active sessions..",
                    Foreground = ToolHandler.GetColorFromHex("#FFFF0000"),
                    FontSize = 14,
                    FontWeight = FontWeights.SemiBold,
                    FontFamily = new System.Windows.Media.FontFamily("Open Sans"),
                    Margin = new Thickness(0, 0, 0, 0)
                });
            }
        }

        private void SPActiveSessions_LayoutUpdated(object sender, EventArgs e)
        {
            int count = 0;
            foreach (ActiveSessionRow activeSessionRow in SPActiveSessions.Children.OfType<ActiveSessionRow>())
                if (activeSessionRow.Visibility == Visibility.Visible)
                    count++;

            ActiveSessionsCount.Text = $"{count} active sessions displayed";
        }
    }
}
