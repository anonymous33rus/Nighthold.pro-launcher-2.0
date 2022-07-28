using Nighthold_Launcher.FrontPages.MainPageControls.Childs;
using Nighthold_Launcher.Nighthold;
using Nighthold_Launcher.OtherWindows;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace Nighthold_Launcher.FrontPages.MainPageControls
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await XMLHandler.LoadXMLRemoteConfigAsync();

            if (Documents.RemoteConfig.HasChildNodes)
            {
                try
                {
                    foreach (XmlNode node in Documents.RemoteConfig.SelectNodes("NightholdLauncher/Expansions/Expansion"))
                    {
                        int.TryParse(node.Attributes["id"].Value, out int _expansionID);

                        // add navbar expansion buttons
                        NavbarPanel.Children.Add(new NavbarButton(this, _expansionID));

                        // add PlayOrDownload button for each expansion to a collection
                        var PoD = new PlayOrDownload(_expansionID) { Visibility = Visibility.Hidden };
                        playOrDownloadGrid.Children.Add(PoD);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
                }

                NavbarPanel.Children.OfType<NavbarButton>().FirstOrDefault().OnExpansionSelected();
            }
        }

        public void SetExpansionBackground(int _expansionID)
        {
            AnimHandler.FadeIn(ExpansionBackground, 1000);
            switch (_expansionID)
            {
                case 1: // vanilla
                    ToolHandler.SetImageSource(ExpansionBackground, "/Nighthold Launcher;component/Assets/Expansion Backgrounds/expansion_bg_classic.png", UriKind.Relative);
                    break;
                case 2: // tbc
                    ToolHandler.SetImageSource(ExpansionBackground, "/Nighthold Launcher;component/Assets/Expansion Backgrounds/expansion_bg_tbc.png", UriKind.Relative);
                    break;
                case 3: // wotlk
                    ToolHandler.SetImageSource(ExpansionBackground, "/Nighthold Launcher;component/Assets/Expansion Backgrounds/expansion_bg_wotlk.png", UriKind.Relative);
                    break;
                case 4: // cata
                    ToolHandler.SetImageSource(ExpansionBackground, "/Nighthold Launcher;component/Assets/Expansion Backgrounds/expansion_bg_cata.png", UriKind.Relative);
                    break;
                case 5: // mop
                    ToolHandler.SetImageSource(ExpansionBackground, "/Nighthold Launcher;component/Assets/Expansion Backgrounds/expansion_bg_mop.png", UriKind.Relative);
                    break;
                case 6: // wod
                    ToolHandler.SetImageSource(ExpansionBackground, "/Nighthold Launcher;component/Assets/Expansion Backgrounds/expansion_bg_wod.png", UriKind.Relative);
                    break;
                case 7: // legion
                    ToolHandler.SetImageSource(ExpansionBackground, "/Nighthold Launcher;component/Assets/Expansion Backgrounds/expansion_bg_legion.png", UriKind.Relative);
                    break;
                case 8: // bfa
                    ToolHandler.SetImageSource(ExpansionBackground, "/Nighthold Launcher;component/Assets/Expansion Backgrounds/expansion_bg_bfa.png", UriKind.Relative);
                    break;
                case 9: // shadowlands
                    ToolHandler.SetImageSource(ExpansionBackground, "/Nighthold Launcher;component/Assets/Expansion Backgrounds/expansion_bg_shadowlands.png", UriKind.Relative);
                    break;
                default:
                    ToolHandler.SetImageSource(ExpansionBackground, "/Nighthold Launcher;component/Assets/Expansion Backgrounds/expansion_bg_classic.png", UriKind.Relative);
                    break;
            }
        }

        private void BtnWebsite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(Documents.RemoteConfig.SelectSingleNode("NightholdLauncher/Links/WebsiteLink").InnerText);
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private void BtnDiscord_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(Documents.RemoteConfig.SelectSingleNode("NightholdLauncher/Links/DiscordLink").InnerText);
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private void ExpansionsButton_Click(object sender, RoutedEventArgs e)
        {
            ExpansionsButton.IsEnabled = false;

            ExpansionsPopup popup = new ExpansionsPopup(this)
            {
                Margin = new Thickness(ExpansionsButton.Margin.Left, ExpansionsButton.Margin.Top + ExpansionsButton.Height + 10, 0, 0)
            };

            mainPageGrid.Children.Add(popup);

            AnimHandler.MoveUpAndFadeIn300Ms(popup);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow();
            settings.Owner = SystemTray.nightholdLauncher;
            settings.ShowDialog();
        }

        private void BtnShop_Click(object sender, RoutedEventArgs e)
        {
            //SystemTray.nightholdLauncher.shopPage.LoadShopPage();
        }

        private void BtnVote_Click(object sender, RoutedEventArgs e)
        {
            //SystemTray.nightholdLauncher.votePage.LoadVotePage();
        }

        private void BtnTopPvP_Click(object sender, RoutedEventArgs e)
        {
            //SystemTray.nightholdLauncher.pvpPage.LoadPvPPage();
        }

        private void BtnOnlinePlayers_Click(object sender, RoutedEventArgs e)
        {
            //SystemTray.nightholdLauncher.onlinePage.LoadPvPPage();
        }

        private void BtnCharactersMarket_Click(object sender, RoutedEventArgs e)
        {
            //SystemTray.nightholdLauncher.marketPage.LoadMarketPage();
        }
    }
}
