using Nighthold_Launcher.AdminPanelControls;
using Nighthold_Launcher.GMPanelControls;
using Nighthold_Launcher.Nighthold;
using System.Windows;

namespace Nighthold_Launcher.OtherWindows
{
    /// <summary>
    /// Interaction logic for ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow : Window
    {
        private GMPanel pGMPanel;
        private AdminPanel pAdminPanel;
        private bool LauncherOverlay;
        private bool UseMessage;
        public string TextInserted;

        public ConfirmationWindow(string _title, string _text, bool _useMessage, bool _launcherOverlay = true, GMPanel _gmPanel = null, AdminPanel _adminPanel = null)
        {
            InitializeComponent();

            WindowTitle.Text = _title;
            WindowText.Text = _text;
            LauncherOverlay = _launcherOverlay;
            UseMessage = _useMessage;
            pGMPanel = _gmPanel;
            pAdminPanel = _adminPanel;
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (LauncherOverlay)
                AnimHandler.FadeIn(SystemTray.nightholdLauncher.OverlayBlur, 300);

            if (!UseMessage)
                SPMessage.Visibility = Visibility.Collapsed;

            if (pGMPanel != null)
                AnimHandler.FadeIn(pGMPanel.OverlayBlur, 300);

            if (pAdminPanel != null)
                AnimHandler.FadeIn(pAdminPanel.OverlayBlur, 300);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (LauncherOverlay)
                AnimHandler.FadeOut(SystemTray.nightholdLauncher.OverlayBlur, 300);

            if (pGMPanel != null)
                AnimHandler.FadeOut(pGMPanel.OverlayBlur, 300);

            if (pAdminPanel != null)
                AnimHandler.FadeOut(pAdminPanel.OverlayBlur, 300);
        }

        private void UserTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            MsgCharCount.Text = UserTextBox.Text.Length.ToString();
            TextInserted = UserTextBox.Text;
        }
    }
}
