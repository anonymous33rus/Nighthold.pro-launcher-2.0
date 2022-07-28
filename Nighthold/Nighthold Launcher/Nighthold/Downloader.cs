using Nighthold_Launcher.FrontPages.MainPageControls.Childs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using WebHandler;

namespace Nighthold_Launcher.Nighthold
{
    class Downloader
    {
        /// <summary>
        /// string: fileUrl
        /// string: localPath
        /// </summary>
        public static List<KeyValuePair<string, string>> DownloadList = new List<KeyValuePair<string, string>>();
        public static List<KeyValuePair<string, long>> ModifiedTimeList = new List<KeyValuePair<string, long>>();

        private WebClient client = new WebClient();
        private Stopwatch SWSpeed = new Stopwatch();
        private DateTime WstartTime;

        PlayOrDownload playOrDownload;

        public Downloader(PlayOrDownload _playOrDownload)
        {
            playOrDownload = _playOrDownload;
        }

        public static int CurrentFileIndex;
        public static string CurrentFileName;
        public static string CurrentFilePath;
        public static int DownloadListCount;

        private string  GetCompleteFileURL(string _gameFolderFilePath)
        {
            return $"{ Config.GameFolderUrl }/{ playOrDownload.ExpansionID }/{ _gameFolderFilePath }";
        }

        private string GetCompleteLocalFilePath(string _gameFolderFilePath)
        {
            return $"{ ClientHandler.GetExpansionPath(playOrDownload.ExpansionID) }/{ _gameFolderFilePath }";
        }

        private string GetFileNameFromPath(string _filePathFormat)
        {
            return _filePathFormat.Substring(_filePathFormat.LastIndexOf('/') + 1);
        }

        public async Task CreateDownloadList()
        {
            await Task.Delay(1000);
            DownloadList.Clear();

            ModifiedTimeList.Clear();

            CurrentFileIndex = 0;

            DownloadListCount = 0;

            playOrDownload.InfoBlock.Text = "Получение списка файлов..";

            playOrDownload.InfoBlock.Foreground = ToolHandler.GetColorFromHex("#FFA4A4A4");

            await Task.Delay(1000);

            var collection = FilesListClass.FilesList.FromJson(await FilesListClass.GetFilesListJson(playOrDownload.ExpansionID));

            foreach (var fileDataArray in collection)
            {
                foreach (var fileData in fileDataArray)
                {
                    if (ToolHandler.IsFileDifferent(fileData.FileSize, fileData.ModifiedTime, GetCompleteLocalFilePath(fileData.FilePath)))
                    {
                        DownloadList.Add(new KeyValuePair<string, string>(GetCompleteFileURL(fileData.FilePath), GetCompleteLocalFilePath(fileData.FilePath)));
                        ModifiedTimeList.Add(new KeyValuePair<string, long>(GetCompleteLocalFilePath(fileData.FilePath), fileData.ModifiedTime));
                    }
                }
            }
            DownloadListCount = DownloadList.Count;

            playOrDownload.InfoBlock.Text = "Подготовка к обновлению..";

            await Task.Delay(1000);
        }

        public void DownloadUpdates()
        {
            if (DownloadListCount != 0)
            {
                playOrDownload.InfoBlock.Text = string.Empty;

                foreach (var item in DownloadList)
                {
                    playOrDownload.DownloadGrid.Visibility = Visibility.Visible;

                    string localPath = item.Value;
                    string remoteUrl = item.Key;

                    CurrentFileIndex++;
                    CurrentFileName = GetFileNameFromPath(item.Key);
                    CurrentFilePath = localPath;

                    if (!string.IsNullOrEmpty(Path.GetDirectoryName(localPath)) && !string.IsNullOrWhiteSpace(Path.GetDirectoryName(localPath)))
                        if (!Directory.Exists(Path.GetDirectoryName(localPath)))
                            Directory.CreateDirectory(Path.GetDirectoryName(localPath));

                    DownloadFile(remoteUrl, localPath);
                    break; // stop after the first item
                }
            }
            else
            {
                playOrDownload.DownloadGrid.Visibility = Visibility.Hidden;
                ClientHandler.SetLocalUpdateVersion(playOrDownload.ExpansionID, ClientHandler.GetRemoteUpdateVersion(playOrDownload.ExpansionID));
                //playOrDownload.UserControl_Loaded(playOrDownload, null);
                playOrDownload.PlayBtn();
            }
        }

        private void DownloadFile(string urlAddress, string destination)
        {
            WstartTime = DateTime.Now;

            using (client = new WebClient())
            {
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed); // completed event

                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged); // progress change event

                Uri downloadURL = new Uri(urlAddress);

                SWSpeed.Start(); // Start the stopwatch which we will be using to calculate the download speed

                try
                {
                    client.DownloadFileAsync(downloadURL, destination);
                }
                catch
                {
                    playOrDownload.PlayOrDownloadButton.Visibility = Visibility.Visible;
                }
            }
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            TimeSpan span = DateTime.Now - WstartTime;

            if (span.TotalMilliseconds >= 100)
            {
                WstartTime = DateTime.Now;

                playOrDownload.FileIndexFirst.Text = CurrentFileIndex.ToString();

                playOrDownload.FileIndexSecond.Text = DownloadListCount.ToString();

                playOrDownload.DownloadBar.Value = e.ProgressPercentage;

                playOrDownload.FileName.Text = CurrentFileName;

                playOrDownload.DownloadSpeed.Text = $"{e.BytesReceived / 1024d / 1024d / SWSpeed.Elapsed.TotalSeconds:0.00} MB/s";
            }
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            SWSpeed.Reset(); // timer used for download speed

            if (e.Cancelled == true)
            {
                playOrDownload.UserControl_Loaded(this, null);

                playOrDownload.DownloadGrid.Visibility = Visibility.Hidden;
            }
            else
            {
                foreach (var _item in ModifiedTimeList)
                {
                    try
                    {
                        if (_item.Key == CurrentFilePath)
                        {
                            File.SetLastWriteTime(CurrentFilePath, ToolHandler.UnixTimeStampToDateTime(_item.Value));

                            if (CurrentFileName.ToLower().Contains("wow.mfil"))
                                ToolHandler.FixWowMFILAttributes(CurrentFilePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                if (CurrentFileIndex >= DownloadListCount) // all downloads completed
                {
                    playOrDownload.DownloadGrid.Visibility = Visibility.Hidden;

                    ClientHandler.SetLocalUpdateVersion(playOrDownload.ExpansionID, ClientHandler.GetRemoteUpdateVersion(playOrDownload.ExpansionID));

                    playOrDownload.UserControl_Loaded(this, null);

                    playOrDownload.DownloadBar.Value = 100;

                    DownloadList.RemoveAt(0);
                }
                else // continue download list left files
                {
                    playOrDownload.DownloadBar.Value = 100;

                    DownloadList.RemoveAt(0);

                    DownloadUpdates(); // continue with the next file from the download list
                }
            }
        }
    }
}
