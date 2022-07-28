using Nighthold_Launcher.Nighthold;
using System;
using System.Diagnostics;
using System.Windows.Controls;

namespace Nighthold_Launcher.FrontPages.MainPageControls.Childs
{
    /// <summary>
    /// Interaction logic for ExpansionMenuRow.xaml
    /// </summary>
    public partial class ExpansionMenuRow : UserControl
    {
        private string MenuItemIcon;
        private string MenuItemText;
        private string URL;

        public ExpansionMenuRow(string _menuIconName, string _menuItemText, string _url)
        {
            InitializeComponent();
            MenuItemIcon = _menuIconName;
            MenuItemText = _menuItemText;
            URL = _url;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ToolHandler.SetImageSource(RowIcon, $"/Nighthold Launcher;component/Assets/Menu Icons/{ MenuItemIcon }.png", UriKind.Relative);
            RowTextButton.Content = MenuItemText;
        }

        private void RowTextButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start(URL);
        }
    }
}
