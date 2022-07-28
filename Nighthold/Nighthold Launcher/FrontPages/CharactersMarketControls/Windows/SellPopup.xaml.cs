using Nighthold_Launcher.FrontPages.CharactersMarketControls.Childs;
using Nighthold_Launcher.Nighthold;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for SellPopup.xaml
    /// </summary>
    public partial class SellPopup : Window
    {
        public SellPopup()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
                Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AnimHandler.FadeIn(SystemTray.nightholdLauncher.OverlayBlur, 300);

            UpdateCharactersList();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AnimHandler.FadeOut(SystemTray.nightholdLauncher.OverlayBlur, 300);
            SystemTray.nightholdLauncher.marketPage.LoadMarketPage();
        }

        public async void UpdateCharactersList()
        {
            try
            {
                var realmsCollection = CharactersMarketClass.OwnCharactersList.FromJson(await CharactersMarketClass.GetOwnCharactersListJson(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword));

                CBoxAllCharacters.Items.Clear();

                int cCount = 0;

                if (realmsCollection != null)
                {
                    foreach (var realm in realmsCollection)
                    {
                        var realmHeader = new ComboBoxItem()
                        {
                            IsEnabled = false
                        };

                        CBoxAllCharacters.Items.Add(realmHeader);

                        foreach (var character in realm)
                        {
                            realmHeader.Content = character.RealmName;
                            var characterRow = new CBoxCharacter(character.Guid, character.Name, character.Class, character.Race, character.Gender, character.Level, character.RealmId, character.RealmName);

                            CBoxAllCharacters.Items.Add(characterRow);
                            cCount++;
                        }
                    }
                }

                if (cCount == 0)
                {
                    CBoxAllCharacters.Items.Add(new ComboBoxItem
                    {
                        Content = "No characters..",
                        IsEnabled = false
                    });
                }
                else
                {
                    CBoxAllCharacters.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());

                CBoxAllCharacters.Items.Add(new ComboBoxItem
                {
                    Content = "No characters..",
                    IsEnabled = false
                });
            }
        }

        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool IsTextNumeric(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void PriceDP_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextNumeric(e.Text);
        }

        private void PriceDP_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextNumeric(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private async void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            BtnConfirm.IsEnabled = false;

            ResponseBlock.Foreground = Brushes.Orange;
            ResponseBlock.Text = "Processing your request..";

            await Task.Delay(1000);

            try
            {
                CBoxCharacter CBSelectedChar = (CBoxCharacter)CBoxAllCharacters.SelectedItem;

                int.TryParse(PriceDP.Text, out int priceDP);

                var response = CharactersMarketClass.MarketSellResponse.FromJson(await CharactersMarketClass.GetMarketSellResponse(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword,
                    CBSelectedChar.pGuid.ToString(), priceDP.ToString(), CBSelectedChar.pRealmID.ToString()));

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
            catch (Exception)
            {
                ResponseBlock.Foreground = Brushes.Red;
                ResponseBlock.Text = "Please select a valid character!";
                BtnConfirm.IsEnabled = true;
            }
        }
    }
}
