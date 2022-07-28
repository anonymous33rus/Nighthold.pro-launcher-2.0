using Nighthold_Launcher.Nighthold;
using Nighthold_Launcher.OtherWindows;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Nighthold_Launcher.AdminPanelControls.Pages
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : UserControl
    {
        private AdminPanel pAdminPanel;

        public Admin(AdminPanel _adminPanel)
        {
            InitializeComponent();
            pAdminPanel = _adminPanel;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadingOverlay.Visibility = Visibility.Visible;

            try
            {
                CBGMLevelRealm.Items.Clear();
                CBRawCommandRealm.Items.Clear();
                CBGMLevelRank.Items.Clear();
                int realmsCount = 0;

                CBGMLevelRealm.Items.Add(new ComboBoxItem()
                {
                    Content = "All Realms",
                    Tag = "-1"
                });

                // realms
                foreach (var realm in GameMasterClass.RealmsList.FromJson(await GameMasterClass.GetRealmsListJson(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword)))
                {
                    // "set gm level" realms combobox
                    CBGMLevelRealm.Items.Add(new ComboBoxItem()
                    {
                        Content = realm.RealmName,
                        Tag = realm.RealmId
                    });

                    // "send raw command" realms combobox
                    CBRawCommandRealm.Items.Add(new ComboBoxItem()
                    {
                        Content = realm.RealmName,
                        Tag = realm.RealmId
                    });
                    realmsCount++;
                }

                // gm ranks
                foreach (var gmRank in GameMasterClass.GMRanksList.FromJson(await GameMasterClass.GetGMRanksListJson(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword)))
                {
                    CBGMLevelRank.Items.Add(new ComboBoxItem()
                    {
                        Content = gmRank.Name,
                        Tag = gmRank.Rank
                    });
                    realmsCount++;
                    CBGMLevelRank.SelectedIndex = 0;
                }

                // delete last two highest ranks from the list ("owner", "console")
                if (CBGMLevelRank.Items.Count > 2)
                {
                    CBGMLevelRank.Items.RemoveAt(CBGMLevelRank.Items.Count - 1);
                    CBGMLevelRank.Items.RemoveAt(CBGMLevelRank.Items.Count - 1);
                }

                if (realmsCount == 0)
                    CBGMLevelRealm.Items.Add(new ComboBoxItem() { Content = "None", Tag = 0 });
                else
                {
                    CBGMLevelRealm.SelectedIndex = 0;
                    CBRawCommandRealm.SelectedIndex = 0;
                }

                LoadingOverlay.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private async void BtnSetGMLevel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cbSelectedRealm = ((ComboBoxItem)CBGMLevelRealm.SelectedItem).Content.ToString();
                string cbSelectedRealmId = ((ComboBoxItem)CBGMLevelRealm.SelectedItem).Tag.ToString();
                string cbSelectedRankId = ((ComboBoxItem)CBGMLevelRank.SelectedItem).Tag.ToString();

                ConfirmationWindow confirmation = new ConfirmationWindow("Set Account GM Level",
                    $"Are you sure about setting account [{GMLevelAccountName.Text}] to GM Level [{cbSelectedRankId}] on realm [{cbSelectedRealm}]", false, false, null, pAdminPanel);
                confirmation.Owner = SystemTray.nightholdLauncher;
                if (confirmation.ShowDialog() == true)
                {
                    pAdminPanel.ShowActionMessage($"Setting account [{GMLevelAccountName.Text}] to GM level to {cbSelectedRankId} on realm [{cbSelectedRealm}].");

                    pAdminPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                    (
                        await GameMasterClass.GetSetGMLevelJson
                        (
                            NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword, pAdminPanel.SecKey, GMLevelAccountName.Text, cbSelectedRankId, cbSelectedRealmId, "1")
                        ).ResponseMsg
                    );
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private async void BtnPasswordChange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfirmationWindow confirmation = new ConfirmationWindow("Change Account Password",
                    $"Are you sure about changing account [{PasswordChangeAccountName.Text}]'s password?", false, false, null, pAdminPanel);
                confirmation.Owner = SystemTray.nightholdLauncher;
                if (confirmation.ShowDialog() == true)
                {
                    pAdminPanel.ShowActionMessage($"Changing password for account [{PasswordChangeAccountName.Text}].");

                    pAdminPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                    (
                        await GameMasterClass.GetSetAccountPasswordJson
                        (
                            NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword, pAdminPanel.SecKey, PasswordChangeAccountName.Text, PasswordChangePassword.Password, "1")
                        ).ResponseMsg
                    );
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private async void BtnSendRawCommand_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cbSelectedRealm = ((ComboBoxItem)CBRawCommandRealm.SelectedItem).Content.ToString();
                string cbSelectedRealmId = ((ComboBoxItem)CBRawCommandRealm.SelectedItem).Tag.ToString();

                ConfirmationWindow confirmation = new ConfirmationWindow("Send a console command",
                    $"Are you sure about sending command [{RawCommand.Text}] to the console on realm [{cbSelectedRealm}]?", false, false, null, pAdminPanel);
                confirmation.Owner = SystemTray.nightholdLauncher;
                if (confirmation.ShowDialog() == true)
                {
                    pAdminPanel.ShowActionMessage($"Sending raw command to console on realm [{cbSelectedRealm}].");

                    pAdminPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                    (
                        await GameMasterClass.GetSendRawCommandJson
                        (
                            NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword, pAdminPanel.SecKey, RawCommand.Text, cbSelectedRealmId)
                        ).ResponseMsg
                    );
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }
    }
}
