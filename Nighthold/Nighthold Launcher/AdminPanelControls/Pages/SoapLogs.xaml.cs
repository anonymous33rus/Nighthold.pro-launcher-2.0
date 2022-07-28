using Nighthold_Launcher.AdminPanelControls.Childs;
using Nighthold_Launcher.AdminPanelControls.Childs.Subchilds;
using Nighthold_Launcher.Nighthold;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Nighthold_Launcher.AdminPanelControls.Pages
{
    /// <summary>
    /// Interaction logic for SoapLogs.xaml
    /// </summary>
    public partial class SoapLogs : UserControl
    {
        private AdminPanel pAdminPanel;

        private PageNavigator pPageNavigator;

        List<GameMasterClass.SoapLogs> soapLogs;

        public SoapLogs(AdminPanel _adminPanel)
        {
            InitializeComponent();
            pAdminPanel = _adminPanel;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                soapLogs = GameMasterClass.SoapLogs.FromJson(await GameMasterClass.GetSoapLogsJson(NightholdLauncher.LoginUsername,
                    NightholdLauncher.LoginPassword, pAdminPanel.SecKey));

                SPSoapLogs.Children.Clear();

                foreach (var soapLog in soapLogs.Skip(0).Take(50))
                {
                    var soapLogRow = new SoapLogRow(soapLog.AccountName, soapLog.Date.ToString(), soapLog.RealmName, soapLog.Command);
                    SPSoapLogs.Children.Add(soapLogRow);
                }

                AnimHandler.MoveUpAndFadeIn(SPSoapLogs);

                pPageNavigator = new PageNavigator(soapLogs.Count, SPSoapLogs, null, soapLogs)
                {
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(4, 0, 0, 0)
                };

                CommandLogsGrid.Children.Add(pPageNavigator);
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }

            if (SPSoapLogs.Children.Count == 0)
            {
                SPSoapLogs.Children.Add(new Label() { 
                    Content = "No command logs..",
                    Foreground = ToolHandler.GetColorFromHex("#FFFF0000"),
                    FontSize = 14,
                    FontWeight = FontWeights.SemiBold,
                    FontFamily = new System.Windows.Media.FontFamily("Open Sans"),
                    Margin = new Thickness(0, 0, 0, 0)
                });
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == "Search By")
            {
                pPageNavigator.SetSelectedPageNumber(0);
                return;
            }

            string searchText = SearchBox.Text.ToLower();

            if (string.IsNullOrEmpty(searchText) || string.IsNullOrWhiteSpace(searchText))
            {
                pPageNavigator.SetSelectedPageNumber(0);
                return;
            }

            SPSoapLogs.Children.Clear();

            foreach (var soapLog in soapLogs)
            {

                switch (CBSearchIn.SelectedIndex)
                {
                    case 0: // account name
                        if (soapLog.AccountName.ToLower().Contains(searchText))
                            SPSoapLogs.Children.Add(new SoapLogRow(soapLog.AccountName, soapLog.Date.ToString(), soapLog.RealmName, soapLog.Command));
                        break;
                    case 1: // date and time column
                        if (soapLog.Date.ToString().ToLower().Contains(searchText))
                            SPSoapLogs.Children.Add(new SoapLogRow(soapLog.AccountName, soapLog.Date.ToString(), soapLog.RealmName, soapLog.Command));
                        break;
                    case 2: // realm column
                        if (soapLog.RealmName.ToLower().Contains(searchText))
                            SPSoapLogs.Children.Add(new SoapLogRow(soapLog.AccountName, soapLog.Date.ToString(), soapLog.RealmName, soapLog.Command));
                        break;
                    case 3: // command column
                        if (soapLog.Command.ToLower().Contains(searchText))
                            SPSoapLogs.Children.Add(new SoapLogRow(soapLog.AccountName, soapLog.Date.ToString(), soapLog.RealmName, soapLog.Command));
                        break;
                    default:
                        break;
                }
            }
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Search By")
                textBox.Text = string.Empty;
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
                textBox.Text = "Search By";
        }

        private void SPSoapLogs_LayoutUpdated(object sender, EventArgs e)
        {
            int count = 0;
            foreach (SoapLogRow soapLogRow in SPSoapLogs.Children.OfType<SoapLogRow>())
                if (soapLogRow.Visibility == Visibility.Visible)
                    count++;

            SoapLogsCount.Text = $"{count} results displayed";
        }
    }
}
