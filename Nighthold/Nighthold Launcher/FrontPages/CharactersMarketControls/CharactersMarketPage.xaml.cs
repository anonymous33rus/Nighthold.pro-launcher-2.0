using Nighthold_Launcher.FrontPages.CharactersMarketControls.Childs;
using Nighthold_Launcher.FrontPages.CharactersMarketControls.Windows;
using Nighthold_Launcher.Nighthold;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Nighthold_Launcher.FrontPages.CharactersMarketControls
{
    /// <summary>
    /// Interaction logic for CharactersMarketPage.xaml
    /// </summary>
    public partial class CharactersMarketPage : UserControl
    {
        public CharactersMarketPage()
        {
            InitializeComponent();
        }

        private void BtnReturnHome_Click(object sender, RoutedEventArgs e)
        {
            SPMarketRows.Children.Clear();
            Visibility = Visibility.Hidden;
            AnimHandler.FadeIn(SystemTray.nightholdLauncher.mainPage, 300);
        }

        public async void LoadMarketPage()
        {
            SystemTray.nightholdLauncher.mainPage.Visibility = Visibility.Hidden;
            AnimHandler.FadeIn(this, 300);

            try
            {
                // Retrieve realms list first
                var realms = GameMasterClass.RealmsList.FromJson(await GameMasterClass.GetRealmsListJson(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword));

                ComboBox1_ab.Items.Clear();

                foreach (GameMasterClass.RealmsList realm in realms)
                {
                    ComboBox1_ab.Items.Add(new ComboBoxItem()
                    {
                        Content = realm.RealmName,
                        Tag = realm.RealmId
                    });
                }

                if (ComboBox1_ab.Items.Count != 0)
                {
                    ComboBox1_ab.SelectedIndex = 0;
                    ComboBox1_ab.IsEnabled = true;
                }

                var marketList = CharactersMarketClass.CharactersMarketList.FromJson(await CharactersMarketClass.GetCharactersMarketListJson(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword));

                SPMarketRows.Children.Clear();

                if (marketList != null)
                {
                    foreach (var marketItem in marketList)
                    {
                        var marketRow = new MarketRow(marketItem.MarketId, marketItem.Guid, marketItem.Name, marketItem.Class, marketItem.Race, marketItem.Gender, marketItem.Level, marketItem.PriceDp, marketItem.RealmId, marketItem.RealmName);
                        SPMarketRows.Children.Add(marketRow);
                        AnimHandler.MoveUpAndFadeIn300Ms(marketRow);
                    }

                    SimulateRealmSelection();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Search")
                textBox.Text = string.Empty;
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
                textBox.Text = "Search";
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox.Text == "Search")
                return;

            try
            {
                string searchText = SearchBox.Text.ToLower();

                int.TryParse(((ComboBoxItem)ComboBox1_ab.SelectedItem).Tag.ToString(), out int _realmId);

                if (!string.IsNullOrEmpty(searchText) && !string.IsNullOrWhiteSpace(searchText))
                {
                    foreach (MarketRow marketRow in SPMarketRows.Children.OfType<MarketRow>())
                    {
                        if (!marketRow.pCharName.ToLower().Contains(searchText))
                        {
                            marketRow.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            if (marketRow.pRealmID != _realmId)
                                marketRow.Visibility = Visibility.Collapsed;
                            else
                                marketRow.Visibility = Visibility.Visible;
                        }
                    }
                }
                else
                {
                    foreach (MarketRow marketRow in SPMarketRows.Children.OfType<MarketRow>())
                    {
                        if (marketRow.pRealmID != _realmId)
                            marketRow.Visibility = Visibility.Collapsed;
                        else
                            marketRow.Visibility = Visibility.Visible;
                    }
                }
            }
            catch
            {

            }
        }

        private void ComboBox1_ab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SimulateRealmSelection();
        }

        private void SimulateRealmSelection()
        {
            try
            {
                foreach (MarketRow marketRow in SPMarketRows.Children.OfType<MarketRow>())
                    marketRow.Visibility = Visibility.Collapsed;

                foreach (MarketRow marketRow in SPMarketRows.Children.OfType<MarketRow>())
                {
                    int.TryParse(((ComboBoxItem)ComboBox1_ab.SelectedItem).Tag.ToString(), out int _realmId);

                    if (marketRow.pRealmID == _realmId)
                    {
                        marketRow.Visibility = Visibility.Visible;
                    }
                }
            }
            catch
            {

            }
        }

        private void SellCharacterBtn_Click(object sender, RoutedEventArgs e)
        {
            SellPopup sellPopup = new SellPopup();
            sellPopup.Owner = SystemTray.nightholdLauncher;
            sellPopup.ShowDialog();
        }

        private void BtnMyList_Click(object sender, RoutedEventArgs e)
        {
            SPMarketRows.Children.Clear();
            Visibility = Visibility.Hidden;
            AnimHandler.FadeIn(SystemTray.nightholdLauncher.mainPage, 300);
            SystemTray.nightholdLauncher.marketOwnPage.LoadMarketOwnPage();
        }
    }
}
