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

namespace Nighthold_Launcher.FrontPages.ShopPageControls.Windows
{
    /// <summary>
    /// Interaction logic for BuyPopup.xaml
    /// </summary>
    public partial class BuyPopup : Window
    {
        private long pId;
        private string pTitle;
        private string pDescription;
        private string pImgUrl;
        private long pPriceDP;
        private long pPriceVP;
        private long pCategory;
        private long pRealmId;

        public BuyPopup(long _id, string _title, string _description, string _imgUrl, long _priceDP, long _priceVP, long _category, long _realmID)
        {
            InitializeComponent();
            pId = _id;
            pTitle = _title;
            pDescription = _description;
            pImgUrl = _imgUrl;
            pPriceDP = _priceDP;
            pPriceVP = _priceVP;
            pCategory = _category;
            pRealmId = _realmID;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
                Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AnimHandler.FadeIn(SystemTray.nightholdLauncher.OverlayBlur, 300);

            switch (pCategory)
            {
                case 1:
                    CategoryName.Text = "Service";
                    break;
                case 2:
                    CategoryName.Text = "Bundle";
                    break;
                case 3:
                    CategoryName.Text = "Item";
                    break;
                case 4:
                    CategoryName.Text = "Mount";
                    break;
                case 5:
                    CategoryName.Text = "Pet";
                    break;
                default:
                    CategoryName.Text = "Unknown Category";
                    break;
            }

            ServiceOrItemName.Text = pTitle;

            ServiceOrItemDescription.Text = pDescription;

            ToolHandler.SetImageSource(ShopImage, pImgUrl, UriKind.Absolute);

            if (pPriceDP != 0)
            {
                PriceDP.Text = pPriceDP.ToString();
            }
            else
            {
                SPGoldCoin.Visibility = Visibility.Collapsed;
            }

            if (pPriceVP != 0)
            {
                PriceVP.Text = pPriceVP.ToString();
            }
            else
            {
                SPSilverCoin.Visibility = Visibility.Collapsed;
            }

            // get characters list if necessary based on the category type
            try
            {
                var realmsCollection = CharClass.RealmCharacterData.FromJson(await CharClass.GetRealmCharactersListJson(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword, pRealmId.ToString()));

                ComboBox1_ac.Items.Clear();

                int cCount = 0;

                if (realmsCollection != null)
                {
                    foreach (var realm in realmsCollection)
                    {
                        foreach (var character in realm)
                        {
                            ComboBox1_ac.Items.Add(new ComboBoxItem()
                            {
                                Content = character.Name,
                                Tag = character.Name
                            });

                            cCount++;
                        }
                    }
                }

                if (cCount == 0)
                {
                    ComboBox1_ac.IsEnabled = false;
                    ComboBox1_ac.Items.Add(new ComboBoxItem()
                    {
                        Content = "No characters"
                    });

                    BtnConfirm.IsEnabled = false;
                }
                else
                {
                    ComboBox1_ac.SelectedIndex = 0;
                    BtnConfirm.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());

                ComboBox1_ac.IsEnabled = false;
                ComboBox1_ac.Items.Add(new ComboBoxItem()
                {
                    Content = "No characters"
                });

                BtnConfirm.IsEnabled = false;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AnimHandler.FadeOut(SystemTray.nightholdLauncher.OverlayBlur, 300);
        }

        private void SPGoldCoin_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RBtnDP.IsChecked = true;
        }

        private void SPSilverCoin_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RBtnVP.IsChecked = true;
        }

        private async void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            BtnConfirm.IsEnabled = false;

            if (RBtnDP.IsChecked == false && RBtnVP.IsChecked == false)
            {
                ResponseBlock.Foreground = Brushes.Red;
                ResponseBlock.Text = "Currency not selected!";
                BtnConfirm.IsEnabled = true;
                return;
            }

            string currencyType = "0";

            if (RBtnVP.IsChecked == true)
            {
                currencyType = "1";
            }

            string playerName = ((ComboBoxItem)ComboBox1_ac.SelectedItem).Tag.ToString();
            string accountName = NightholdLauncher.LoginUsername;

            ResponseBlock.Foreground = Brushes.Orange;
            ResponseBlock.Text = "Processing your request..";

            await Task.Delay(2000);

            var response = AuthClass.ShopPurchaseResponse.FromJson(await AuthClass.GetShopPurchaseResponse(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword,
                pId.ToString(), currencyType, playerName, accountName));

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
                // closing window in 7 seconds
            }
        }
    }
}
