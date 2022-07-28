using Nighthold_Launcher.Nighthold;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Nighthold_Launcher.GMPanelControls.Childs
{
    /// <summary>
    /// Interaction logic for BanRow.xaml
    /// </summary>
    public partial class MuteLogRow : UserControl
    {
        private GMPanel pGMPanel;
        public string pUsername;
        public long pMuteDate;
        public long pUnmuteDate;
        public long pMuteTimeMinutes;
        public string pMutedBy;
        public string pMuteReason;

        public MuteLogRow(GMPanel _gmPanel, string _username, long _muteDate, long _unmuteDate, long _pMuteTimeMinutes, string _mutedBy, string _muteReason)
        {
            InitializeComponent();
            pGMPanel = _gmPanel;
            pUsername = _username;
            pMuteDate = _muteDate;
            pUnmuteDate = _unmuteDate;
            pMuteTimeMinutes = _pMuteTimeMinutes;
            pMutedBy = _mutedBy;
            pMuteReason = _muteReason;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                AccName.Text = pUsername;

                int.TryParse(pMuteDate.ToString(), out int iMuteDate);
                var tMuteDate = ToolHandler.UnixTimeStampToDateTime(iMuteDate);
                MuteDate.Text = tMuteDate.ToString("dd MMMM yyyy");

                int.TryParse(pUnmuteDate.ToString(), out int iUnmuteDate);
                var tUnmuteDate = ToolHandler.UnixTimeStampToDateTime(iUnmuteDate);
                //UnmuteDate.Text = tUnmuteDate.ToString("dd MMMM yyyy");

                MuteTime.Text = $"{pMuteTimeMinutes} минут";

                MutedBy.Text = pMutedBy;

                MuteReason.Text = pMuteReason;
            }
            catch (System.Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }
    }
}
