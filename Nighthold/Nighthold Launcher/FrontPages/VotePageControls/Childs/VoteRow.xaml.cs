using Nighthold_Launcher.Nighthold;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WebHandler;

namespace Nighthold_Launcher.FrontPages.VotePageControls.Childs
{
    /// <summary>
    /// Interaction logic for VoteRow.xaml
    /// </summary>
    public partial class VoteRow : UserControl
    {
        public long pVoteSiteId;
        public string pVoteSiteName;
        public long pVoteCooldownSecLeft;
        public string pVoteImageUrl;
        public string pVoteLink;
        public long pVotePoints;

        private DispatcherTimer timer = new DispatcherTimer();

        public VoteRow(long _voteSiteId, string _voteSiteName, long _voteCooldownSecLeft, string _voteImageUrl, string _voteLink, long _votePoints)
        {
            InitializeComponent();
            pVoteSiteId = _voteSiteId;
            pVoteSiteName = _voteSiteName;
            pVoteCooldownSecLeft = _voteCooldownSecLeft;
            pVoteImageUrl = _voteImageUrl;
            pVoteLink = _voteLink;
            pVotePoints = _votePoints;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            VoteSiteName.Content = pVoteSiteName;

            StartRealTimeCooldown();

            if (pVoteCooldownSecLeft != 0)
            {
                VoteImage.Cursor = Cursors.Arrow;
                VoteImage.Opacity = 0.5;
            }

            ToolHandler.SetImageSource(VoteImage, pVoteImageUrl, UriKind.Absolute);

            VoteImage.Tag = pVoteLink;

            VotePointsReward.Text = pVotePoints.ToString();

        }

        private void StartRealTimeCooldown()
        {
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            VoteImageSpinner.Stop();

            if (pVoteCooldownSecLeft <= 0)
            {
                pVoteCooldownSecLeft = 0;

                VoteCooldown.Text = "READY";
                VoteCooldown.Foreground = ToolHandler.GetColorFromHex("#FF8F8F8F");

                VoteImage.IsEnabled = true;
                VoteImage.Opacity = 1;
            }
            else
            {
                pVoteCooldownSecLeft -= 1;

                TimeSpan time = TimeSpan.FromSeconds(pVoteCooldownSecLeft);

                VoteCooldown.Text = time.ToString(@"hh\:mm\:ss");
                VoteCooldown.Foreground = System.Windows.Media.Brushes.Red;

                VoteImage.IsEnabled = false;
                VoteImage.Opacity = 0.5;
            }
        }

        private void VoteSiteName_Click(object sender, RoutedEventArgs e)
        {
            ToolHandler.CopyButtonTextToClipboard(sender as Button);
        }

        private void VoteImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //try
            //{
            //    if (pVoteCooldownSecLeft <= 0)
            //    {
            //        Process.Start(VoteImage.Tag.ToString());

            //        var voteResponse =  AuthClass.VoteClickResponse.FromJson(await AuthClass.SelfVoteClick(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword, pVoteSiteId.ToString()));

            //        if (voteResponse.VoteRegistered)
            //        {
            //            SystemTray.nightholdLauncher.votePage.LoadVotePage();
            //            SystemTray.nightholdLauncher.userPanel.UpdateAccountBalance();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            //}
        }
    }
}
