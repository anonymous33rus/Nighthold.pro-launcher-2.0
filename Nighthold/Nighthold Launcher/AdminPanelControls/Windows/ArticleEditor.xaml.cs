using Nighthold_Launcher.Nighthold;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Nighthold_Launcher.AdminPanelControls.Windows
{
    /// <summary>
    /// Interaction logic for ArticleEditor.xaml
    /// </summary>
    public partial class ArticleEditor : Window
    {
        AdminPanel pAdminPanel;
        public string pArticleImageUrl;
        public string pArticleTitle;
        public string pArticleUrl;
        public long pArticleId;
        public long pExpansionId;

        public ArticleEditor(AdminPanel _adminPanel, string _imageUrl = null, string _articleTitle = null, string _articleUrl = null, long _articleId = 0, long _expansionId = 0)
        {
            InitializeComponent();
            pAdminPanel = _adminPanel;
            pArticleImageUrl = _imageUrl;
            pArticleTitle = _articleTitle;
            pArticleUrl = _articleUrl;
            pArticleId = _articleId;
            pExpansionId = _expansionId;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AnimHandler.FadeIn(pAdminPanel.OverlayBlur, 300);

            try
            {
                if (pArticleImageUrl != null)
                {
                    ToolHandler.SetImageSource(ArticleImage, pArticleImageUrl, UriKind.Absolute);
                    ImageUrl.Text = pArticleImageUrl;
                }

                if (pArticleTitle != null)
                {
                    ArticleTitle.Text = pArticleTitle;
                }

                if (pArticleUrl != null)
                {
                    ArticleUrl.Text = pArticleUrl;
                }
            }
            catch
            {

            }  
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            AnimHandler.FadeOut(pAdminPanel.OverlayBlur, 300);
        }

        private void ImageUrl_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Image Url")
                textBox.Text = string.Empty;
        }

        private void ImageUrl_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
                textBox.Text = "Image Url";
        }

        private async void BtnImagePreview_Click(object sender, RoutedEventArgs e)
        {
            var oFColor = BtnImagePreview.Foreground;
            BtnImagePreview.Foreground = ToolHandler.GetColorFromHex("#FFFF8B00");
            BtnImagePreview.Content = "Checking";

            try
            {
                if (await ToolHandler.ImageExistsAtUrl(ImageUrl.Text))
                {
                    ToolHandler.SetImageSource(ArticleImage, ImageUrl.Text, UriKind.Absolute);
                    BtnImagePreview.Foreground = ToolHandler.GetColorFromHex("#FF00FF00");
                    BtnImagePreview.Content = "Valid";
                    await Task.Delay(1500);
                }
                else
                {
                    BtnImagePreview.Foreground = ToolHandler.GetColorFromHex("#FFFF0000");
                    BtnImagePreview.Content = "Invalid";
                    await Task.Delay(1500);
                }
            }
            catch
            {
                BtnImagePreview.Foreground = ToolHandler.GetColorFromHex("#FFFF0000");
                BtnImagePreview.Content = "Invalid";
                await Task.Delay(1500);
            }
            finally
            {
                BtnImagePreview.Content = "Preview";
                BtnImagePreview.Foreground = oFColor;
            }
        }

        private void ArticleTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Title")
                textBox.Text = string.Empty;
        }

        private void ArticleTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
                textBox.Text = "Title";
        }

        private void ArticleUrl_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Redirect Url")
                textBox.Text = string.Empty;
        }

        private void ArticleUrl_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
                textBox.Text = "Redirect Url";
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
