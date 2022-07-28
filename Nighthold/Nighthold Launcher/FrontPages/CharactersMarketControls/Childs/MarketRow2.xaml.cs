using Nighthold_Launcher.FrontPages.CharactersMarketControls.Windows;
using Nighthold_Launcher.Nighthold;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebHandler;

namespace Nighthold_Launcher.FrontPages.CharactersMarketControls.Childs
{
    /// <summary>
    /// Interaction logic for MarketRow2.xaml
    /// </summary>
    public partial class MarketRow2 : UserControl
    {
        public long pMarketID;
        public long pGuid;
        public string pCharName;
        public long pClass;
        public long pRace;
        public long pGender;
        public long pLevel;
        public long pPriceDP;
        public long pRealmID;
        public string pRealmName;

        public MarketRow2(long _marketID, long _guid, string _charName, long _class, long _race, long _gender, long _level, long _priceDB, long _realmID, string _realmName)
        {
            InitializeComponent();
            pMarketID = _marketID;
            pGuid = _guid;
            pCharName = _charName;
            pClass = _class;
            pRace = _race;
            pGender = _gender;
            pLevel = _level;
            pPriceDP = _priceDB;
            pRealmID = _realmID;
            pRealmName = _realmName;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CharName.Text = pCharName;
            CharClass.Text = ToolHandler.ClassToName(pClass);
            CharClass.Foreground = ToolHandler.GetPlayerClassColorBrush(pClass);
            ToolHandler.SetClassImage(CharClassImg, pClass);
            ToolHandler.SetRaceGenderImage(CharRaceImg, pRace, pGender);
            CharLevel.Text = pLevel.ToString();
            CharPriceDP.Text = pPriceDP.ToString();
            RealmName.Text = pRealmName;
        }

        private async void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = CharactersMarketClass.MarketCancelSaleResponse.FromJson(
                    await CharactersMarketClass.GetMarketCancelSaleResponse(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword, pGuid.ToString(), pRealmID.ToString()));

                SystemTray.nightholdLauncher.marketOwnPage.Visibility = Visibility.Collapsed;
                AnimHandler.FadeIn(SystemTray.nightholdLauncher.marketPage, 300);

                if (!response.Response) // failed transaction, print error
                {
                }
                else
                {
                    SystemTray.nightholdLauncher.userPanel.UpdateCharactersList();
                }
            }
            catch
            {

            }
        }
    }
}
