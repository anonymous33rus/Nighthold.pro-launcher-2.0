using Nighthold_Launcher.Nighthold;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Nighthold_Launcher.AdminPanelControls.Windows
{
    /// <summary>
    /// Interaction logic for NotificationEditor.xaml
    /// </summary>
    public partial class NotificationEditor : Window
    {
        AdminPanel pAdminPanel;

        public NotificationEditor(AdminPanel _adminPanel)
        {
            InitializeComponent();
            pAdminPanel = _adminPanel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AnimHandler.FadeIn(pAdminPanel.OverlayBlur, 300);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            AnimHandler.FadeOut(pAdminPanel.OverlayBlur, 300);
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void NotificationSubject_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Subject")
                textBox.Text = string.Empty;
        }

        private void NotificationSubject_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
                textBox.Text = "Subject";
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

        private void NotificationUrl_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Redirect Url")
                textBox.Text = string.Empty;
        }

        private void NotificationUrl_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
                textBox.Text = "Redirect Url";
        }

        private void NotificationMessage_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Message")
                textBox.Text = string.Empty;
        }

        private void NotificationMessage_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
                textBox.Text = "Message";
        }

        private void AccountID_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Account ID or 0 = Everyone")
                textBox.Text = string.Empty;
        }

        private void AccountID_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
                textBox.Text = "Account ID or 0 = Everyone";
        }
    }
}
