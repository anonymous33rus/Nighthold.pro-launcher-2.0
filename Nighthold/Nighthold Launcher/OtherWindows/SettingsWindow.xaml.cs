using Nighthold_Launcher.Nighthold;
using System.Windows;
using System.Windows.Input;

namespace Nighthold_Launcher.OtherWindows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CheckBox1_a_Checked(object sender, RoutedEventArgs e)
        {
            AppHandler.AddApplicationToStartup();
        }

        private void CheckBox1_a_Unchecked(object sender, RoutedEventArgs e)
        {
            AppHandler.RemoveApplicationFromStartup();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AnimHandler.FadeIn(SystemTray.nightholdLauncher.OverlayBlur, 300);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AnimHandler.FadeOut(SystemTray.nightholdLauncher.OverlayBlur, 300);
        }
    }
}
