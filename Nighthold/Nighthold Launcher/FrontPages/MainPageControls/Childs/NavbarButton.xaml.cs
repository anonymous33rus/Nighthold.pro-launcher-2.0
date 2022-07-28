using Nighthold_Launcher.Nighthold;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using WebHandler;

namespace Nighthold_Launcher.FrontPages.MainPageControls.Childs
{
    /// <summary>
    /// Interaction logic for NavbarButton.xaml
    /// </summary>
    public partial class NavbarButton : UserControl
    {
        public MainPage mainPage;
        public int pExpansionID = 1;

        public NavbarButton(MainPage _mainPage, int _expansionID)
        {
            InitializeComponent();

            mainPage = _mainPage;
            pExpansionID = _expansionID;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetToolTip();
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            OnExpansionSelected();
        }

        public async void OnExpansionSelected()
        {
            UnhoverSiblings();
            SetHoverState();
            SetWoWLogo();

            // set expansion background
            mainPage.SetExpansionBackground(pExpansionID);

            // clear panel
            mainPage.ExpansionMenuPanel.Children.Clear();

            try // menu items
            {
                foreach (XmlNode node in Documents.RemoteConfig.SelectNodes("NightholdLauncher/Expansions/Expansion"))
                {
                    if (int.Parse(node.Attributes["id"].Value) == pExpansionID)
                    {
                        foreach (XmlNode childnode in node.SelectNodes("Menu/Item"))
                        {
                            string menu_icon_name;
                            switch (int.Parse(childnode.Attributes["icon"].Value))
                            {
                                case 1:
                                    menu_icon_name = "chat_bubble";
                                    break;
                                case 2:
                                    menu_icon_name = "patch_notes";
                                    break;
                                case 3:
                                    menu_icon_name = "shopping_cart";
                                    break;
                                case 4:
                                    menu_icon_name = "download_icon";
                                    break;
                                case 5:
                                    menu_icon_name = "website_icon";
                                    break;
                                default:
                                    menu_icon_name = "chat_buble";
                                    break;
                            }

                            mainPage.ExpansionMenuPanel.Children.Add(new ExpansionMenuRow(menu_icon_name, childnode.InnerText, childnode.Attributes["url"].Value));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }

            try // menu realms
            {
                foreach (XmlNode node in Documents.RemoteConfig.SelectNodes("NightholdLauncher/Expansions/Expansion"))
                {
                    if (int.Parse(node.Attributes["id"].Value) == pExpansionID)
                    {
                        var realmlist = node.SelectSingleNode("Realms").Attributes["realmlist"].Value;
                        foreach (XmlNode childnode in node.SelectNodes("Realms/Realm"))
                        {
                            mainPage.ExpansionMenuPanel.Children.Add(new ExpansionMenuRealmRow(
                                childnode.InnerText,
                                realmlist, 
                                int.Parse(childnode.Attributes["port"].Value)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }

            AnimHandler.MoveUpAndFadeIn(mainPage.ExpansionMenuScrollViewer);
            mainPage.maintenanceNote.SetText(string.Empty);

            // maintenance announcement
            try
            {
                mainPage.maintenanceNote.Visibility = Visibility.Collapsed;

                foreach (XmlNode node in Documents.RemoteConfig.SelectNodes("NightholdLauncher/Expansions/Expansion"))
                {
                    if (int.Parse(node.Attributes["id"].Value) == pExpansionID)
                    {
                        var nodeM = node.SelectSingleNode("Maintenance");

                        bool.TryParse(nodeM.Attributes["enable"].Value, out bool mEnable);
                        if (mEnable)
                        {
                            mainPage.maintenanceNote.SetText(ToolHandler.StringBeautify(nodeM.InnerText));
                            mainPage.maintenanceNote.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }

            try // news
            {
                var newsCollection = NewsClass.NewsList.FromJson(await NewsClass.GetNewsListJson(pExpansionID.ToString(), "10"));
                mainPage.ArticlesPanel.Children.Clear();
                if (newsCollection != null)
                {
                    foreach (var news in newsCollection)
                    {
                        var article = new Article(
                            news.ImageUrl,
                            news.ArticleTitle,
                            news.ArticleDate.UtcDateTime.ToString(),
                            news.ArticleUrl);

                        mainPage.ArticlesPanel.Children.Add(article);

                        AnimHandler.MoveUpAndFadeIn300Ms(article);
                    }

                    if (mainPage.ArticlesPanel.Children.Count == 0)
                    {
                        mainPage.ArticlesPanel.Children.Add(new Label()
                        {
                            Content = "Нет новостей!",
                            Foreground = ToolHandler.GetColorFromHex("#FF919191"),
                            FontFamily = new System.Windows.Media.FontFamily("/Nighthold Launcher;component/Assets/Font/#Open Sans"),
                            FontSize = 14
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }

            // play or download buttons
            foreach (var item in mainPage.playOrDownloadGrid.Children.OfType<PlayOrDownload>())
            {
                if (item.ExpansionID == pExpansionID)
                {
                    item.Visibility = Visibility.Visible;
                    AnimHandler.FadeIn(item, 1000);
                }
                else
                {
                    item.Visibility = Visibility.Hidden;
                }
            }
        }

        private void UnhoverSiblings()
        {
            foreach (var btn in mainPage.NavbarPanel.Children.OfType<NavbarButton>())
                if (btn is FrameworkElement)
                    btn.NavButton.IsEnabled = true;
        }

        private void SetHoverState()
        {
            NavButton.IsEnabled = false;
        }

        private void SetToolTip()
        {
            switch (pExpansionID)
            {
                case 1:
                    ToolTip = "Classic";
                    break;
                case 2:
                    ToolTip = "The Burning Crusade";
                    break;
                case 3:
                    ToolTip = "Wrath of the Lich King";
                    break;
                case 4:
                    ToolTip = "Cataclysm";
                    break;
                case 5:
                    ToolTip = "Mists of Pandaria";
                    break;
                case 6:
                    ToolTip = "Warlords of Draenor";
                    break;
                case 7:
                    ToolTip = "Legion";
                    break;
                case 8:
                    ToolTip = "Battle for Azeroth";
                    break;
                case 9:
                    ToolTip = "Shadowlands";
                    break;
                default:
                    ToolTip = "World of Warcraft";
                    break;
            }
        }

        private void SetWoWLogo()
        {
            switch (pExpansionID)
            {
                case 1: // classic
                {
                    ToolHandler.SetImageSource(mainPage.WoWLogo, "/Nighthold Launcher;component/Assets/Logos/wow_classic_logo.png", UriKind.Relative);
                    break;
                }
                case 2: // tbc
                {
                    ToolHandler.SetImageSource(mainPage.WoWLogo, "/Nighthold Launcher;component/Assets/Logos/wow_tbc_logo.png", UriKind.Relative);
                    break;
                }
                case 3: // wotlk
                {
                    ToolHandler.SetImageSource(mainPage.WoWLogo, "/Nighthold Launcher;component/Assets/Logos/wow_wotlk_logo.png", UriKind.Relative);
                    break;
                }
                case 4: // cata
                {
                    ToolHandler.SetImageSource(mainPage.WoWLogo, "/Nighthold Launcher;component/Assets/Logos/wow_cata_logo.png", UriKind.Relative);
                    break;
                }
                case 5: // mop
                {
                    ToolHandler.SetImageSource(mainPage.WoWLogo, "/Nighthold Launcher;component/Assets/Logos/wow_mop_logo.png", UriKind.Relative);
                    break;
                }
                case 6: // wod
                {
                    ToolHandler.SetImageSource(mainPage.WoWLogo, "/Nighthold Launcher;component/Assets/Logos/wow_wod_logo.png", UriKind.Relative);
                    break;
                }
                case 7: // legion
                {
                    ToolHandler.SetImageSource(mainPage.WoWLogo, "/Nighthold Launcher;component/Assets/Logos/wow_legion_logo.png", UriKind.Relative);
                    break;
                }
                case 8: // bfa
                {
                    ToolHandler.SetImageSource(mainPage.WoWLogo, "/Nighthold Launcher;component/Assets/Logos/wow_bfa_logo.png", UriKind.Relative);
                    break;
                }
                case 9: // shadowlands
                {
                    ToolHandler.SetImageSource(mainPage.WoWLogo, "/Nighthold Launcher;component/Assets/Logos/wow_sl_logo.png", UriKind.Relative);
                    break;
                }
                default:
                    break;
            }

            AnimHandler.ScaleIn(mainPage.WoWLogo);
        }
    }
}
