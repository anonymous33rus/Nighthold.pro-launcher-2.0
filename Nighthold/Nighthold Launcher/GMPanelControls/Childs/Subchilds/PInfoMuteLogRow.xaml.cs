using Nighthold_Launcher.Nighthold;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Nighthold_Launcher.GMPanelControls.Subchilds
{
    /// <summary>
    /// Interaction logic for BanRow.xaml
    /// </summary>
    public partial class PInfoMuteLogRow : UserControl
    {
        private GMPanel pGMPanel;
        public string pMuteDate;
        public string pMuteDuration;
        public string pMutedBy;
        public string pMuteReason;

        public PInfoMuteLogRow(GMPanel _gmPanel, string _muteDate, string _muteDuration, string _mutedBy, string _muteReason)
        {
            InitializeComponent();
            pGMPanel = _gmPanel;
            pMuteDate = _muteDate;
            pMuteDuration = _muteDuration;
            pMutedBy = _mutedBy;
            pMuteReason = _muteReason;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                MuteDate.Text = pMuteDate;
                MuteDuration.Text = pMuteDuration;
                MutedBy.Text = pMutedBy;
                MuteReason.Text = pMuteReason;
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }
    }
}
