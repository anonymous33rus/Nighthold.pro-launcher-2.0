using Nighthold_Launcher.Nighthold;
using System.Windows;
using System.Windows.Controls;

namespace Nighthold_Launcher.FrontPages.TopPvPPageControls.Childs
{
    /// <summary>
    /// Interaction logic for TopPvPRow.xaml
    /// </summary>
    public partial class TopPvPRow : UserControl
    {
        public string pPlayerName;
        public string pArenaPoints;
        public string pHonorPoints;
        public string pTotalKills;
        public string pRealmName;

        public TopPvPRow(string _playerName, string _arenaPoints, string _honorPoints, string _totalKills, string _realmName)
        {
            InitializeComponent();
            pPlayerName = _playerName;
            pArenaPoints = _arenaPoints;
            pHonorPoints = _honorPoints;
            pTotalKills = _totalKills;
            pRealmName = _realmName;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PlayerName.Content = pPlayerName;
            ArenaPoints.Text = pArenaPoints;
            HonorPoints.Text = pHonorPoints;
            TotalKills.Text = pTotalKills;
            RealmName.Text = pRealmName;

        }

        private void PlayerName_Click(object sender, RoutedEventArgs e)
        {
            ToolHandler.CopyButtonTextToClipboard(sender as Button);
        }
    }
}
