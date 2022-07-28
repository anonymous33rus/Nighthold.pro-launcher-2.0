using Nighthold_Launcher.Nighthold;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Nighthold_Launcher.FrontPages.MainPageControls.Childs
{
    /// <summary>
    /// Interaction logic for PlayOrDownload.xaml
    /// </summary>
    public partial class PlayOrDownload : UserControl
    {
        public int GAME_STATE;

        public enum STATE_ENUM
        {
            READY,
            NEEDS_UPDATE,
            INVALID_PATH,
            UPDATING
        };

        public int ExpansionID;

        public PlayOrDownload(int _expansionID)
        {
            InitializeComponent();
            DataContext = this;
            ExpansionID = _expansionID;
        }

        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //DeleteOldPatches();
                DownloadGrid.Visibility = Visibility.Hidden;

                 //check if path to wow folder is set
                if (!ClientHandler.IsValidExpansionPath(ExpansionID, ClientHandler.GetExpansionPath(ExpansionID)))
                {
                    PlayOrDownloadButtonSettings.IsEnabled = true;
                    PlayOrDownloadButton.IsEnabled = false;
                    PlayOrDownloadButton.Content = "Папка не выбрана";
                    InfoBlock.Foreground = ToolHandler.GetColorFromHex("#FFFD8383");
                    InfoBlock.Text = "Выберите папку с игрой!";
                    GAME_STATE = (int)STATE_ENUM.INVALID_PATH;

                    return; // skip anything below
                }
                NeedsUpdate();

                // check if local and remote update versions match
                /*if (ClientHandler.GetLocalUpdateVersion(ExpansionID) == ClientHandler.GetRemoteUpdateVersion(ExpansionID))
                {
                    PlayOrDownloadButtonSettings.IsEnabled = true;
                    PlayOrDownloadButton.IsEnabled = true;
                    PlayOrDownloadButton.Content = "ИГРАТЬ";
                    InfoBlock.Foreground = ToolHandler.GetColorFromHex("#FFA4A4A4");
                    InfoBlock.Text = "Клиент игры обновлен, можно играть!";
                    GAME_STATE = (int)STATE_ENUM.READY;

                    return; // skip anything below
                }


                
                // if local and remote update versions are different
                PlayOrDownloadButtonSettings.IsEnabled = true;
                PlayOrDownloadButton.IsEnabled = true;
                PlayOrDownloadButton.Content = "ОБНОВИТЬ";
                GAME_STATE = (int)STATE_ENUM.NEEDS_UPDATE;
                InfoBlock.Foreground = ToolHandler.GetColorFromHex("#FFFFFFFF");
                InfoBlock.Text = "Доступны новые обновления!";*/
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        public void PlayBtn()
        {
            PlayOrDownloadButtonSettings.IsEnabled = true;
            PlayOrDownloadButton.IsEnabled = true;
            PlayOrDownloadButton.Content = "ИГРАТЬ";
            InfoBlock.Foreground = ToolHandler.GetColorFromHex("#FFA4A4A4");
            InfoBlock.Text = "Клиент игры обновлен, можно играть!";
            GAME_STATE = (int)STATE_ENUM.READY;
        }
        public async void DeleteOldPatches()
        {
            var listToDelete = CharClass.DeleteOldPatchesList.FromJson(await CharClass.GetDeletePatchListJson());
            
            if (listToDelete != null)
            {
                foreach (var patch in listToDelete)
                {
                    string a = patch.ToString();
                    string path = SetPath(a);
                    
                    try
                    {
                        System.IO.File.Delete(path);
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        public string SetPath(string item)
        {
            string path = null;
            string gPath = ClientHandler.GetExpansionPath(ExpansionID);
            if (ExpansionID == 3)
            {
                if (item.ToLower().Contains(".mpq") && item.Contains("-ruRU-"))
                    path = String.Format(@"{0}\Data\ruRU\{1}", gPath, item);
                else
                    path = String.Format(@"{0}\Data\{1}", gPath, item);
                if (item.ToLower().Contains(".exe"))
                    path = String.Format(@"{0}\{1}", gPath, item);
            }
            return path;
        }

        private async void PlayOrDownloadButton_Click(object sender, RoutedEventArgs e)
        {
              try
            {
                switch (GAME_STATE)
                {
                    case (int)STATE_ENUM.READY:
                        {
                            ClientHandler.StartWoWClient(ExpansionID);

                            switch (Properties.Settings.Default.PlayButtonSettingId)
                            {
                                case 0:
                                    // keep launcher window open
                                    break;
                                case 1: // minimize to taskbar
                                    SystemTray.nightholdLauncher.WindowState = WindowState.Minimized;
                                    break;
                                case 2: // minimize to system tray
                                    SystemTray.nightholdLauncher.Hide();
                                    SystemTray.notifier.Visible = true;
                                    break;
                                case 3: // shutdown launcher
                                    AppHandler.Shutdown();
                                    break;
                                default:
                                    break;
                            }
                            break;
                        }
                    case (int)STATE_ENUM.NEEDS_UPDATE:
                        {
                            // start update when press the button
                            PlayOrDownloadButtonSettings.IsEnabled = false;
                            PlayOrDownloadButton.IsEnabled = false;
                            InfoBlock.Text = string.Empty;
                            PlayOrDownloadButton.Content = "ОБНОВЛЕНИЕ";

                            Downloader downloader = new Downloader(this);
                            await downloader.CreateDownloadList();
                            downloader.DownloadUpdates();
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private async void NeedsUpdate()
        {
            PlayOrDownloadButtonSettings.IsEnabled = false;
            PlayOrDownloadButton.IsEnabled = false;
            InfoBlock.Text = string.Empty;
            PlayOrDownloadButton.Content = "ОБНОВЛЕНИЕ";

            Downloader downloader = new Downloader(this);
            await downloader.CreateDownloadList();
            downloader.DownloadUpdates();
        }

        private void PlayOrDownloadButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    fbd.SelectedPath = ClientHandler.GetExpansionPath(ExpansionID);

                    fbd.Description = $"Выберите папку с игрой: { ClientHandler.GetExpansionName(ExpansionID).ToUpper() }";

                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                    if (result.ToString() == "OK" && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        ClientHandler.SaveExpansionPath(ExpansionID, fbd.SelectedPath);
                        ClientHandler.SetLocalUpdateVersion(ExpansionID, 0);
                        UserControl_Loaded(sender, e);
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }
    }
}
