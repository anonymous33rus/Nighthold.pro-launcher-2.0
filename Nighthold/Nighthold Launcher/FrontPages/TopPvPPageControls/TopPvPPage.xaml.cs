using Nighthold_Launcher.FrontPages.TopPvPPageControls.Childs;
using Nighthold_Launcher.Nighthold;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Nighthold_Launcher.FrontPages.TopPvPPageControls
{
    /// <summary>
    /// Interaction logic for TopPvPPage.xaml
    /// </summary>
    public partial class TopPvPPage : UserControl
    {

        public TopPvPPage()
        {
            InitializeComponent();
        }

        private void BtnReturnHome_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            AnimHandler.FadeIn(SystemTray.nightholdLauncher.mainPage, 300);
        }

        public async void LoadPvPPage()
        {
            SystemTray.nightholdLauncher.mainPage.Visibility = Visibility.Hidden;
            AnimHandler.FadeIn(this, 300);

            SPRows.Children.Clear();
            SPRows.Children.Add(new Spinners.BlueSpinnerTwo() { Width = 20, Height = 20, Margin = new Thickness(0, 50, 0, 0) });

            try
            {
                // realms
                var realmsCollection = GameMasterClass.RealmsList.FromJson(await GameMasterClass.GetRealmsListJson(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword));
                CBRealms.Items.Clear();
                CBRealms.Items.Add(new ComboBoxItem()
                {
                    Content = "All Realms",
                    Tag = 0
                });
                foreach (var realm in realmsCollection)
                {
                    CBRealms.Items.Add(new ComboBoxItem()
                    {
                        Content = realm.RealmName,
                        Tag = realm.RealmId
                    });
                }

                // players
                var topPvPCollection = CharClass.TopPvPList.FromJson(await CharClass.GetTopPvPListJson("10"));
                SPRows.Children.Clear();
                if (topPvPCollection != null)
                {
                    foreach (var subCollection in topPvPCollection)
                    {
                        if (subCollection != null)
                        {
                            foreach (var player in subCollection)
                            {
                                var pvpRow = new TopPvPRow(
                                    player.Name,
                                    player.ArenaPoints.ToString(),
                                    player.TotalHonorPoints.ToString(),
                                    player.TotalKills.ToString(),
                                    player.Realm);

                                SPRows.Children.Add(pvpRow);
                            }
                        }
                    }

                    AnimHandler.MoveUpAndFadeIn300Ms(SPRows);
                    CBRealms.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }

            if (SPRows.Children.Count == 0)
            {
                SPRows.Children.Add(new Label()
                {
                    Content = "No players..",
                    Foreground = ToolHandler.GetColorFromHex("#FFFF0000"),
                    FontSize = 14,
                    FontWeight = FontWeights.SemiBold,
                    FontFamily = new System.Windows.Media.FontFamily("Open Sans"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(0, 50, 0, 0)
                });
            }
        }

        private void CBRealms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (CBRealms.Items != null)
                {
                    var pvpRowCollection = SPRows.Children.OfType<TopPvPRow>();

                    if (pvpRowCollection != null)
                    {
                        string cbSelectedRealm = ((ComboBoxItem)CBRealms.SelectedItem).Content.ToString();

                        foreach (TopPvPRow pvpRow in pvpRowCollection)
                        {
                            pvpRow.Visibility = Visibility.Visible;

                            switch (CBRealms.SelectedIndex)
                            {
                                case 0: // show all
                                    pvpRow.Visibility = Visibility.Visible;
                                    break;
                                default: // realm name
                                    if (pvpRow.pRealmName != cbSelectedRealm)
                                        pvpRow.Visibility = Visibility.Collapsed;
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
                // no action is required
            }
        }

        private void SPRows_LayoutUpdated(object sender, EventArgs e)
        {
            int count = 0;
            foreach (TopPvPRow pvpRow in SPRows.Children.OfType<TopPvPRow>())
                if (pvpRow.Visibility == Visibility.Visible)
                    count++;

            PlayersCount.Text = $"{count} Players Displayed";
        }
    }
}
