using Nighthold_Launcher.GMPanelControls.Childs;
using Nighthold_Launcher.Nighthold;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Nighthold_Launcher.GMPanelControls.Pages
{
    /// <summary>
    /// Interaction logic for MuteLogs.xaml
    /// </summary>
    public partial class PlayerInfo : UserControl
    {
        GMPanel pGmPanel;

        public PlayerInfo(GMPanel _gmPanel)
        {
            InitializeComponent();
            pGmPanel = _gmPanel;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadingOverlay.Visibility = Visibility.Visible;

            try
            {
                CBRealms.Items.Clear();
                int realmsCount = 0;
                foreach (var realm in GameMasterClass.RealmsList.FromJson(await GameMasterClass.GetRealmsListJson(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword)))
                {
                    CBRealms.Items.Add(new ComboBoxItem()
                    {
                        Content = realm.RealmName,
                        Tag = realm.RealmId
                    });
                    realmsCount++;
                }

                if (realmsCount == 0)
                    CBRealms.Items.Add(new ComboBoxItem() { Content = "None", Tag = 0 });
                else
                    CBRealms.SelectedIndex = 0;

                LoadingOverlay.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private async void BtnResetSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(SearchBox.Text) && !string.IsNullOrWhiteSpace(SearchBox.Text))
                {
                    string cbSelectedRealm = ((ComboBoxItem)CBRealms.SelectedItem).Content.ToString();

                    LoadingOverlay.Visibility = Visibility.Visible;

                    LoadingOverlayPlaceholder.Content = $"Поиск игрока \"{SearchBox.Text}\" в мире: {cbSelectedRealm}";

                    pGmPanel.ShowActionMessage($"Поиск игрока {SearchBox.Text.ToUpper()} в мире: {cbSelectedRealm}");

                    SPPlayerInfo.Children.Clear();

                    var label1 = new Label()
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(0, 20, 0, 0),
                        FontFamily = new System.Windows.Media.FontFamily("Open Sans"),
                        Foreground = ToolHandler.GetColorFromHex("#FFFF8700"),
                        FontSize = 14,
                        Content = "Подождите, пока будет получена информация об игроке.."
                    };

                    SPPlayerInfo.Children.Add(label1);

                    AnimHandler.MoveUpAndFadeIn(label1);

                    var pInfoCollection = GameMasterClass.PlayerInfo.FromJson(await GameMasterClass.GetPlayerInfoJson(
                        NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword, SearchBox.Text, ((ComboBoxItem)CBRealms.SelectedItem).Tag.ToString()));

                    SPPlayerInfo.Children.Clear();

                    foreach (var pInfo in pInfoCollection)
                    {
                        var pinfoRow = new PlayerInfoRow(pGmPanel,
                            pInfo.AccountName,
                            pInfo.AccountId,
                            pInfo.AccountRankColor,
                            pInfo.AccountRankName,
                            pInfo.PlayerName,
                            pInfo.PlayerRace,
                            pInfo.PlayerClass,
                            pInfo.PlayerGender,
                            pInfo.PlayerLevel,
                            pInfo.GuildName,
                            pInfo.RealmName,
                            pInfo.RealmId,
                            pInfo.TotalPlayedTime,
                            pInfo.Online,
                            pInfo.LastLogout,
                            pInfo.Money,
                            pInfo.ArenaPoints,
                            pInfo.TotalHonorPoints,
                            pInfo.TotalKills,
                            pInfo.TodayKills,
                            pInfo.YesterdayKills,
                            pInfo.AccBanLogs,
                            pInfo.CharBanLogs,
                            pInfo.MuteLogs,
                            pInfo.VpDp);

                        SPPlayerInfo.Children.Add(pinfoRow);
                    }

                    LoadingOverlay.Visibility = Visibility.Hidden;

                    if (SPPlayerInfo.Children.Count == 0)
                    {
                        var label2 = new Label()
                        {
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Margin = new Thickness(0, 20, 0, 0),
                            FontFamily = new System.Windows.Media.FontFamily("Open Sans"),
                            Foreground = ToolHandler.GetColorFromHex("#FFFF0000"),
                            FontSize = 14,
                            Content = $"Игрок [{SearchBox.Text}] не найден в мире: {cbSelectedRealm}!"
                        };

                        SPPlayerInfo.Children.Add(label2);

                        AnimHandler.MoveUpAndFadeIn(label2);
                    }

                    AnimHandler.MoveUpAndFadeIn(SPPlayerInfo);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private void SearchBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                BtnResetSearch_Click(sender, e);
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Имя игрока")
                textBox.Text = string.Empty;
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
                textBox.Text = "Имя игрока";
        }
    }
}
