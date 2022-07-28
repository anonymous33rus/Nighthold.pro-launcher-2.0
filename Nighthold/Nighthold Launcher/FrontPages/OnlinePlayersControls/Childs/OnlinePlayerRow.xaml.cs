using Nighthold_Launcher.Nighthold;
using System.Windows;
using System.Windows.Controls;

namespace Nighthold_Launcher.FrontPages.OnlinePlayersControls.Childs
{
    /// <summary>
    /// Interaction logic for OnlinePlayerRow.xaml
    /// </summary>
    public partial class OnlinePlayerRow : UserControl
    {
        public string pPlayerName;
        public long pLevel;
        public long pRace;
        public long pGender;
        public long pClass;
        public string pRealmName;

        public OnlinePlayerRow(string _playerName, long _playerLevel, long _playerRace, long _playerGender, long _playerClass, string _realmName)
        {
            InitializeComponent();
            pPlayerName = _playerName;
            pLevel = _playerLevel;
            pRace = _playerRace;
            pGender = _playerGender;
            pClass = _playerClass;
            pRealmName = _realmName;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PlayerName.Content = pPlayerName;
            PlayerLevel.Text = pLevel.ToString();
            ToolHandler.SetRaceGenderImage(PlayerRace, pRace, pGender);
            ToolHandler.SetClassImage(PlayerClass, pClass);
            RealmName.Text = pRealmName;

        }

        private void PlayerName_Click(object sender, RoutedEventArgs e)
        {
            ToolHandler.CopyButtonTextToClipboard(sender as Button);
        }
    }
}
