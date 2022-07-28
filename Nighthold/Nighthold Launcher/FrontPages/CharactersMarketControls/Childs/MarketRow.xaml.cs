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

namespace Nighthold_Launcher.FrontPages.CharactersMarketControls.Childs
{
    /// <summary>
    /// Interaction logic for MarketRow.xaml
    /// </summary>
    public partial class MarketRow : UserControl
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

        public MarketRow(long _marketID, long _guid, string _charName, long _class, long _race, long _gender, long _level, long _priceDB, long _realmID, string _realmName)
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

        private void BtnArmory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start($"http://mop-brasil.com/character/{pRealmID}/{pCharName}");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void BtnBuy_Click(object sender, RoutedEventArgs e)
        {
            BuyPopup buyPopup = new BuyPopup(pMarketID, pGuid, pCharName, pClass, pRace, pGender, pLevel, pPriceDP, pRealmID, pRealmName);
            buyPopup.Owner = SystemTray.nightholdLauncher;
            buyPopup.ShowDialog();
        }
    }
}
