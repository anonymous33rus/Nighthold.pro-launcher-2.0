using Nighthold_Launcher.FrontPages.VotePageControls.Childs;
using Nighthold_Launcher.Nighthold;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Nighthold_Launcher.FrontPages.VotePageControls
{
    /// <summary>
    /// Interaction logic for VotePage.xaml
    /// </summary>
    public partial class VotePage : UserControl
    {
        public VotePage()
        {
            InitializeComponent();
        }

        private void BtnReturnHome_Click(object sender, RoutedEventArgs e)
        {
            SPVoteRows.Children.Clear();
            Visibility = Visibility.Hidden;
            AnimHandler.FadeIn(SystemTray.nightholdLauncher.mainPage, 300);
        }

        public async void LoadVotePage()
        {
            SystemTray.nightholdLauncher.mainPage.Visibility = Visibility.Hidden;
            AnimHandler.FadeIn(this, 300);

            SPVoteRows.Children.Clear();

            try
            {
                var voteSitesCollection = AuthClass.VoteSitesList.FromJson(await AuthClass.GetVoteSitesListJson(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword));

                if (voteSitesCollection != null)
                {
                    foreach (var voteSite in voteSitesCollection)
                    {
                        SPVoteRows.Children.Add(new VoteRow(voteSite.SiteId, voteSite.SiteName, voteSite.CooldownSecLeft, voteSite.ImageUrl, voteSite.VoteUrl, voteSite.Points));
                    }
                    AnimHandler.MoveUpAndFadeIn300Ms(SPVoteRows);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }
    }
}
