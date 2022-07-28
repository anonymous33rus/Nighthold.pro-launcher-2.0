using Nighthold_Launcher.GMPanelControls.Childs;
using Nighthold_Launcher.Nighthold;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Nighthold_Launcher.GMPanelControls.Pages
{
    /// <summary>
    /// Interaction logic for MuteLogs.xaml
    /// </summary>
    public partial class MuteLogs : UserControl
    {
        GMPanel pGmPanel;

        public MuteLogs(GMPanel _gmPanel)
        {
            InitializeComponent();
            pGmPanel = _gmPanel;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var mutesCollection = GameMasterClass.MuteLogs.FromJson(await GameMasterClass.GetMuteLogsJson(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword));
                SPMuteLogs.Children.Clear();

                if (mutesCollection != null)
                {
                    foreach (var mute in mutesCollection)
                    {
                        SPMuteLogs.Children.Add(new MuteLogRow(
                            pGmPanel,
                            mute.Username,
                            mute.MuteDate,
                            mute.UnmuteDate,
                            mute.MuteTime,
                            mute.MutedBy,
                            mute.MuteReason));
                    }
                }

                AnimHandler.MoveUpAndFadeIn(SPMuteLogs);
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }

            if (SPMuteLogs.Children.Count == 0)
            {
                SPMuteLogs.Children.Add(new Label() { 
                    Content = "Нет мутов..",
                    Foreground = ToolHandler.GetColorFromHex("#FFFF0000"),
                    FontSize = 14,
                    FontWeight = FontWeights.SemiBold,
                    FontFamily = new System.Windows.Media.FontFamily("Open Sans"),
                    Margin = new Thickness(0, 0, 0, 0)
                });
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox.Text == "Поиск")
                return;

            try
            {
                foreach (MuteLogRow mute in SPMuteLogs.Children.OfType<MuteLogRow>())
                {
                    string searchText = SearchBox.Text.ToLower();

                    if (string.IsNullOrEmpty(searchText) || string.IsNullOrWhiteSpace(searchText))
                    {
                        mute.Visibility = Visibility.Visible;
                        continue;
                    }

                    switch (CBSearchOptions.SelectedIndex)
                    {
                        case 0: // account name
                            if (!mute.pUsername.ToLower().Contains(searchText))
                                mute.Visibility = Visibility.Collapsed;
                            break;
                        case 1: // mute reason
                            if (!mute.pMuteReason.ToLower().Contains(searchText))
                                mute.Visibility = Visibility.Collapsed;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private void BtnResetSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = string.Empty;
            CBSearchOptions.SelectedIndex = 0;
        }

        private void SPMuteLogs_LayoutUpdated(object sender, EventArgs e)
        {
            int count = 0;
            foreach (MuteLogRow muteLogRow in SPMuteLogs.Children.OfType<MuteLogRow>())
                if (muteLogRow.Visibility == Visibility.Visible)
                    count++;

            MuteLogsCount.Text = $"{count} Мутов";
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Поиск")
                textBox.Text = string.Empty;
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
                textBox.Text = "Поиск";
        }
    }
}
