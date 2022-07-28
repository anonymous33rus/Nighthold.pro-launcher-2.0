using System.Windows;
using System.Windows.Controls;

namespace Nighthold_Launcher.AccountStandingBarControls.Childs
{
    /// <summary>
    /// Interaction logic for SinHistoryRow.xaml
    /// </summary>
    public partial class SinHistoryRow : UserControl
    {
        private int pSinType;
        private string pCharacterName;
        private string pDateFirst;
        private string pDuration;
        private string pDateSecond;
        private string pRealmName;
        private string pReason;

        public SinHistoryRow(int _sinType, string _characterName, string _dateFirst, string _duration, string _dateSecond, string _realmName, string _reason)
        {
            InitializeComponent();

            pSinType = _sinType;
            pCharacterName = _characterName;
            pDateFirst = _dateFirst;
            pDuration = _duration;
            pDateSecond = _dateSecond;
            pRealmName = _realmName;
            pReason = _reason;

            SPAccountBan.Visibility = Visibility.Collapsed;
            SPCharacterBan.Visibility = Visibility.Collapsed;
            SPMute.Visibility = Visibility.Collapsed;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            switch (pSinType)
            {
                case 0: // ACCOUNT BAN
                    {
                        SPAccountBan.Visibility = Visibility.Visible;

                        AccBanDate.Text = pDateFirst;
                        AccBanDuration.Text = pDuration;
                        AccUnbanDate.Text = pDateSecond;
                        AccBanReason.Text = pReason;
                        break;
                    }
                case 1: // CHARACTER BAN
                    {
                        SPCharacterBan.Visibility = Visibility.Visible;

                        CharName.Text = pCharacterName;
                        CharBanDate.Text = pDateFirst;
                        CharBanDuration.Text = pDuration;
                        CharUnbanDate.Text = pDateSecond;
                        CharRealmName.Text = pRealmName;
                        CharBanReason.Text = pReason;
                        break;
                    }
                case 2: // MUTE
                    {
                        SPMute.Visibility = Visibility.Visible;

                        MuteDate.Text = pDateFirst;
                        MuteDuration.Text = pDuration;
                        UnmuteDate.Text = pDateSecond;
                        MuteReason.Text = pReason;
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
