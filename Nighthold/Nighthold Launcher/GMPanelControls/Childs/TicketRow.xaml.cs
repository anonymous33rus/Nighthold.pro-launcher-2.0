using Nighthold_Launcher.Nighthold;
using Nighthold_Launcher.OtherWindows;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Nighthold_Launcher.GMPanelControls.Childs
{
    /// <summary>
    /// Interaction logic for TicketRow.xaml
    /// </summary>
    public partial class TicketRow : UserControl
    {
        private GMPanel pGMPanel;
        public long pTicketId;
        public string pTicketName;
        public long pTicketOnline;
        public long pTicketPlayerRace;
        public long pTicketPlayerClass;
        public long pTicketPlayerGender;
        public long pTicketCreateTime;
        public long pTicketLastModified;
        public string pTicketAsignedTo;
        public string pTicketMessage;
        public string pTicketRealmName;
        public long pTicketRealmId;

        public TicketRow(GMPanel _gmPanel, long _ticketId, string _ticketName, long _ticketOnline,
            long _ticketPlayerRace, long _ticketPlayerClass, long _ticketPlayerGender, string _ticketMessage,
            long _ticketCreateTime, long _ticketLastModified, string _ticketAssignedTo, 
            string _ticketRealmName, long _ticketRealmId)
        {
            InitializeComponent();
            pGMPanel = _gmPanel;
            pTicketId = _ticketId;
            pTicketName = _ticketName;
            pTicketOnline = _ticketOnline;
            pTicketPlayerRace = _ticketPlayerRace;
            pTicketPlayerClass = _ticketPlayerClass;
            pTicketPlayerGender = _ticketPlayerGender;
            pTicketMessage = _ticketMessage;
            pTicketCreateTime = _ticketCreateTime;
            pTicketLastModified = _ticketLastModified;
            pTicketAsignedTo = _ticketAssignedTo;
            pTicketRealmName = _ticketRealmName;
            pTicketRealmId = _ticketRealmId;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TicketName.Text = pTicketName;

            if (pTicketOnline == 0)
            {
                TicketOnline.Text = "Не в игре";
                TicketOnline.Foreground = ToolHandler.GetColorFromHex("#FFFF0000");
            }
            else
            {
                TicketOnline.Text = "В игре";
                TicketOnline.Foreground = ToolHandler.GetColorFromHex("#FF17FF00");
            }

            ToolHandler.SetRaceGenderImage(TicketCharRace, pTicketPlayerRace, pTicketPlayerGender);
            ToolHandler.SetClassImage(TicketCharClass, pTicketPlayerClass);

            TicketMessage.Text = pTicketMessage;

            var tCreateTime = ToolHandler.UnixTimeStampToDateTime(pTicketCreateTime);
            var tLastModified = ToolHandler.UnixTimeStampToDateTime(pTicketLastModified);
            TicketCreateTime.Text = tCreateTime.ToString("dd MMMM yyyy");
            TicketLastModified.Text = tLastModified.ToString("dd MMMM yyyy");

            //TicketAsignedTo.Text = pTicketAsignedTo;
            //TicketRealmName.Text = $"[{pTicketRealmId}] {pTicketRealmName}";
        }

        private async void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfirmationWindow confirmation = new ConfirmationWindow("Закрыть тикет", $"Вы уверены что хотите закрыть тикет игрока {pTicketName}?", false, false, pGMPanel);
                confirmation.Owner = SystemTray.nightholdLauncher;
                if (confirmation.ShowDialog() == true)
                {
                    pGMPanel.ShowActionMessage($"Закрываем тикет игрока [{pTicketName}], ID {pTicketId}.");

                    pGMPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                    (
                        await GameMasterClass.GetTicketCloseJson
                        (
                            NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword, pTicketId.ToString(), pTicketRealmId.ToString())
                        ).ResponseMsg
                    );

                    pGMPanel.ShowTicketsPage();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfirmationWindow confirmation = new ConfirmationWindow("Удалить тикет", $"Вы уверены что хотите удалить тикет игрока {pTicketName}?", false, false, pGMPanel);
                confirmation.Owner = SystemTray.nightholdLauncher;
                if (confirmation.ShowDialog() == true)
                {
                    pGMPanel.ShowActionMessage($"Закрываем и удаляем тикет игрока [{pTicketName}], ID {pTicketId}.");

                    pGMPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                    (
                        await GameMasterClass.GetTicketCloseJson
                        (
                            NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword, pTicketId.ToString(), pTicketRealmId.ToString())
                        ).ResponseMsg
                    );

                    pGMPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                    (
                        await GameMasterClass.GetTicketDeleteJson
                        (
                            NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword, pTicketId.ToString(), pTicketRealmId.ToString())
                        ).ResponseMsg
                    );

                    pGMPanel.ShowTicketsPage();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private async void BtnAssign_Click(object sender, RoutedEventArgs e)
        {
            /*try
            {
                ConfirmationWindow confirmation = new ConfirmationWindow("Assign Ticket", $"Do you really want to assign {pTicketName}'s ticket? " +
                $"Type the GM's character name you would like to be assigned to.", true, false, pGMPanel);
                confirmation.Owner = SystemTray.nightholdLauncher;
                if (confirmation.ShowDialog() == true)
                {
                    pGMPanel.ShowActionMessage($"Assigning player [{pTicketName}]'s ticket with ID {pTicketId} to GM [{confirmation.TextInserted}].");

                    pGMPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                    (
                        await GameMasterClass.GetTicketAssignJson
                        (
                            NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword, pTicketId.ToString(), confirmation.TextInserted, pTicketRealmId.ToString())
                        ).ResponseMsg
                    );

                    pGMPanel.ShowTicketsPage();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }*/
        }

        private async void BtnUnAssign_Click(object sender, RoutedEventArgs e)
        {
            /*try
            {
                ConfirmationWindow confirmation = new ConfirmationWindow("Unassign Ticket", $"Do you really want to unassign {pTicketName}'s ticket?", false, false, pGMPanel);
                confirmation.Owner = SystemTray.nightholdLauncher;
                if (confirmation.ShowDialog() == true)
                {
                    pGMPanel.ShowActionMessage($"Unassigning player [{pTicketName}]'s ticket with ID {pTicketId}.");

                    pGMPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                    (
                        await GameMasterClass.GetTicketUnassignJson
                        (
                            NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword, pTicketId.ToString(), pTicketRealmId.ToString())
                        ).ResponseMsg
                    );

                    pGMPanel.ShowTicketsPage();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }*/
        }

        private async void BtnReply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfirmationWindow confirmation = new ConfirmationWindow("Ответить на тикет", $"Хотите ответить на тикет игрока {pTicketName}" +
                $"?", true, false, pGMPanel);
                confirmation.Owner = SystemTray.nightholdLauncher;
                if (confirmation.ShowDialog() == true)
                {
                    pGMPanel.ShowActionMessage($"Отвечаем игроку [{pTicketName}], ID тикета: {pTicketId}.");

                    pGMPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                    (
                        await GameMasterClass.GetTicketReplyJson
                        (
                            NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword, confirmation.TextInserted, pTicketId.ToString(), pTicketRealmId.ToString())
                        ).ResponseMsg
                    );

                    pGMPanel.ShowActionMessage(GameMasterClass.SoapResponse.FromJson
                    (
                        await GameMasterClass.GetTicketCompleteJson
                        (
                            NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword, pTicketId.ToString(), pTicketRealmId.ToString())
                        ).ResponseMsg
                    );

                    pGMPanel.ShowTicketsPage();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }
    }
}
