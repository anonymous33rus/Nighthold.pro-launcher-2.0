using Nighthold_Launcher.FrontPages.ShopPageControls.Windows;
using Nighthold_Launcher.Nighthold;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Nighthold_Launcher.FrontPages.ShopPageControls.Childs
{
    /// <summary>
    /// Interaction logic for ShopRow.xaml
    /// </summary>
    public partial class ShopRow : UserControl
    {
        public long pId;
        public string pTitle;
        public string pDescription;
        public string pImgUrl;
        public long pPriceDP;
        public long pPriceVP;
        public long pCategory;
        public long pRealmId;

        public ShopRow(long _id, string _title, string _description, string _imgUrl, long _priceDP, long _priceVP, long _category, long _realmID)
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
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
        }

        private void BtnBuyNow_Click(object sender, RoutedEventArgs e)
        {
            BuyPopup buyPopup = new BuyPopup(pId, pTitle, pDescription, pImgUrl, pPriceDP, pPriceVP, pCategory, pRealmId);
            buyPopup.Owner = SystemTray.nightholdLauncher;
            buyPopup.ShowDialog();
        }
    }
}
