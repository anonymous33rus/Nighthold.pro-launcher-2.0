using Nighthold_Launcher.Nighthold;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Nighthold_Launcher.GMPanelControls.Pages
{
    /// <summary>
    /// Interaction logic for NewBanWindow.xaml
    /// </summary>
    public partial class NewBan : Window
    {
        public int pBanType;
        public string pAccOrCharacterName;
        public string pBanTime;
        public string pBanReason;
        public long pRealmId;
        public string pRealmName;

        public NewBan()
        {
            InitializeComponent();
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                pBanType = CBBanType.SelectedIndex;
                pAccOrCharacterName = AccountOrCharacterName.Text;

                if (CheckBoxPermanent.IsChecked == false)
                    pBanTime = BanTimeBox.Text;
                else
                    pBanTime = "-1";

                pBanReason = BanReason.Text;
                int.TryParse(((ComboBoxItem)CBRealms.SelectedItem).Tag.ToString(), out int iRealmId);
                pRealmId = iRealmId;
                pRealmName = ((ComboBoxItem)CBRealms.SelectedItem).Content.ToString();
                DialogResult = true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CheckBoxPermanent_Checked(object sender, RoutedEventArgs e)
        {
            BanTimeBox.IsEnabled = false;
        }

        private void CheckBoxPermanent_Unchecked(object sender, RoutedEventArgs e)
        {
            BanTimeBox.IsEnabled = true;
        }

        private void CBBanType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //try
            //{
            //    await Task.Delay(300);
            //    if (CBBanType.SelectedIndex == 0)
            //        SPRealm.Visibility = Visibility.Collapsed;
            //    else
            //        SPRealm.Visibility = Visibility.Visible;
            //}
            //catch
            //{
                
            //}
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
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

        private void BanReason_TextChanged(object sender, TextChangedEventArgs e)
        {
            MsgCharCount.Text = BanReason.Text.Length.ToString();
        }

        private void AccountOrCharacterName_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Имя персонажа/аккаунта")
                textBox.Text = string.Empty;
        }

        private void AccountOrCharacterName_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
                textBox.Text = "Имя персонажа/аккаунта";
        }

        private void BanTimeBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Время")
                textBox.Text = string.Empty;
        }

        private void BanTimeBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
                textBox.Text = "Время";
        }
    }
}
