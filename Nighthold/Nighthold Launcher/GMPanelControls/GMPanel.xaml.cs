using Nighthold_Launcher.GMPanelControls.Pages;
using Nighthold_Launcher.Nighthold;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Nighthold_Launcher.GMPanelControls
{
    /// <summary>
    /// Interaction logic for GMPanel.xaml
    /// </summary>
    public partial class GMPanel : Window
    {
        public GMPanel()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AnimHandler.FadeIn(SystemTray.nightholdLauncher.OverlayBlur, 300);
            AnimHandler.FadeIn(this, 1000);

            PanelGrid.Children.Clear();
            PanelGrid.Children.Add(new TicketsManager(this));
            ShowActionMessage($"Добро пожаловать {NightholdLauncher.LoginUsername}, вы являетесь частью коллектива. Что ты задумал?");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AnimHandler.FadeOut(SystemTray.nightholdLauncher.OverlayBlur, 300);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnTickets_Click(object sender, RoutedEventArgs e)
        {
            ShowTicketsPage();
            ShowActionMessage("Смотрим список тикетов..");
        }

        private void BtnBans_Click(object sender, RoutedEventArgs e)
        {
            ShowBansPage();
            ShowActionMessage("Смотрим список банов..");
        }

        private void BtnMuteLogs_Click(object sender, RoutedEventArgs e)
        {
            ShowMutesPage();
            ShowActionMessage("Показываю логи мутов..");
        }

        private void BtnPInfo_Click(object sender, RoutedEventArgs e)
        {
            ShowPlayerinfoPage();
            ShowActionMessage("Отображение вкладки с информацией об игроке..");
        }

        public void ShowTicketsPage()
        {
            PanelGrid.Children.Clear();
            PanelGrid.Children.Add(new TicketsManager(this));
        }

        public void ShowBansPage()
        {
            PanelGrid.Children.Clear();
            PanelGrid.Children.Add(new BansManager(this));
        }

        public void ShowMutesPage()
        {
            PanelGrid.Children.Clear();
            PanelGrid.Children.Add(new Pages.MuteLogs(this));
        }

        public void ShowPlayerinfoPage()
        {
            PanelGrid.Children.Clear();
            PanelGrid.Children.Add(new Pages.PlayerInfo(this));
        }

        public async void ShowActionMessage(string message)
        {
            try
            {
                SPActionSentMessage.Children.Clear();
                TextBlock labelMessage = new TextBlock()
                {
                    Text = message,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(10, 0, 0, 0),
                    Background = null,
                    Foreground = ToolHandler.GetColorFromHex("#FFD3D697"),
                    FontWeight = FontWeights.Bold,
                    FontFamily = new System.Windows.Media.FontFamily("Open Sans"),
                    TextTrimming = TextTrimming.CharacterEllipsis,
                };
                SPActionSentMessage.Children.Add(labelMessage);
                await AnimHandler.MoveUpAndFadeInThenFadeOut(labelMessage, 3500);
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }
    }
}
