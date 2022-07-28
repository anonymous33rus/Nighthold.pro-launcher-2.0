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
using System.Windows.Shapes;
using WebHandler;

namespace Nighthold_Launcher.FrontPages.CharactersMarketControls.Windows
{
    /// <summary>
    /// Interaction logic for BuyPopup.xaml
    /// </summary>
    public partial class BuyPopup : Window
    {
        private long pMarketID;
        private long pGuid;
        private string pCharName;
        private long pClass;
        private long pRace;
        private long pGender;
        private long pLevel;
        private long pPriceDP;
        private long pRealmId;
        private string pRealmName;

        public BuyPopup(long _marketID, long _guid, string _charName, long _class, long _race, long _gender, long _level, long _priceDB, long _realmID, string _realmName)
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
            pRealmId = _realmID;
            pRealmName = _realmName;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AnimHandler.FadeIn(SystemTray.nightholdLauncher.OverlayBlur, 300);

            CharName.Text = pCharName;

            RealmName.Text = pRealmName;

            ToolHandler.SetRaceGenderImage(RaceImg, pRace, pGender);

            CharLevel.Text = pLevel.ToString();

            CharClass.Text = ToolHandler.ClassToName(pClass);

            CharClass.Foreground = ToolHandler.GetPlayerClassColorBrush(pClass);

            CharRace.Text = ToolHandler.RaceToName(pRace);

            PriceDP.Text = pPriceDP.ToString();


            if (pPriceDP != 0)
                PriceDP.Text = pPriceDP.ToString();
            else
                SPGoldCoin.Visibility = Visibility.Collapsed;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AnimHandler.FadeOut(SystemTray.nightholdLauncher.OverlayBlur, 300);
            SystemTray.nightholdLauncher.marketPage.LoadMarketPage();
        }

        private async void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            BtnConfirm.IsEnabled = false;

            ResponseBlock.Foreground = Brushes.Orange;
            ResponseBlock.Text = "Processing your request..";

            await Task.Delay(1000);

            var response = CharactersMarketClass.MarketPurchaseResponse.FromJson(await CharactersMarketClass.GetMarketPurchaseResponse(pMarketID.ToString(), NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword));

            if (!response.Response) // failed transaction, print error
            {
                ResponseBlock.Foreground = Brushes.Red;
                ResponseBlock.Text = response.ResponseMsg;
                BtnConfirm.IsEnabled = true;
            }
            else
            {
                ResponseBlock.Foreground = Brushes.Lime;
                ResponseBlock.Text = response.ResponseMsg;
                BtnConfirm.IsEnabled = false;
                SystemTray.nightholdLauncher.userPanel.UpdateAccountBalance();
                SystemTray.nightholdLauncher.userPanel.UpdateCharactersList();
            }
        }
    }
}
