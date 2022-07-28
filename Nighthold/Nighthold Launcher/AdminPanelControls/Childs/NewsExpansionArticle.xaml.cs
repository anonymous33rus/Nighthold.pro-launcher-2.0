using Nighthold_Launcher.AdminPanelControls.Pages;
using Nighthold_Launcher.AdminPanelControls.Windows;
using Nighthold_Launcher.Nighthold;
using Nighthold_Launcher.OtherWindows;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Nighthold_Launcher.AdminPanelControls.Childs
{
    /// <summary>
    /// Interaction logic for NewsExpansionArticle.xaml
    /// </summary>
    public partial class NewsExpansionArticle : UserControl
    {
        NewsManager pNewsManager;
        string pArticleImageUrl;
        string pArticleTitle;
        string pArticleDate;
        string pArticleUrl;
        long pArticleId;
        long pExpansionId;

        public NewsExpansionArticle(NewsManager _newsManger, string _articleImageUrl, string _articleTitle, string _articleDate, string _articleUrl, long _articleId, long _expansionId)
        {
            InitializeComponent();
            pNewsManager = _newsManger;
            pArticleImageUrl = _articleImageUrl;
            pArticleTitle = _articleTitle;
            pArticleDate = _articleDate;
            pArticleUrl = _articleUrl;
            pArticleId = _articleId;
            pExpansionId = _expansionId;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ToolHandler.SetImageSource(ArticleImage, pArticleImageUrl, UriKind.Absolute);
                ArticleDate.Text = pArticleDate;
                ArticleTitle.Text = pArticleTitle;
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private async void BtnArticleEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ArticleEditor editor = new ArticleEditor(pNewsManager.pAdminPanel, pArticleImageUrl, pArticleTitle, pArticleUrl, pArticleId, pExpansionId);
                editor.Owner = pNewsManager.pAdminPanel;
                if (editor.ShowDialog() == true)
                {
                    pNewsManager.pAdminPanel.ShowActionMessage($"Editing article \"{editor.ArticleTitle.Text}\" for expansion \"{ToolHandler.ExpansionIdToName(pExpansionId)}\".");

                    var json = NewsClass.ArticleEdit.FromJson(await NewsClass.GetArticleEditResponseJson(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword, pNewsManager.pAdminPanel.SecKey,
                        pArticleId.ToString(), pExpansionId.ToString(), editor.ArticleTitle.Text, editor.ArticleUrl.Text, editor.ImageUrl.Text));

                    pNewsManager.pAdminPanel.ShowActionMessage(json.ResponseMsg);

                    pNewsManager.LoadArticles();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private async void BtnArticleDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfirmationWindow confirmation = new ConfirmationWindow("Delete article", $"Sure you want to delete this article?\r\n<{pArticleTitle}>", false, false, null, pNewsManager.pAdminPanel);
                confirmation.Owner = SystemTray.nightholdLauncher;
                if (confirmation.ShowDialog() == true)
                {
                    pNewsManager.pAdminPanel.ShowActionMessage($"Deleting article \"{pArticleTitle}\" from expansion \"{ToolHandler.ExpansionIdToName(pExpansionId)}\"");

                    var json = NewsClass.ArticleDelete.FromJson(await NewsClass.GetArticleDeleteResponseJson(NightholdLauncher.LoginUsername, NightholdLauncher.LoginPassword, pNewsManager.pAdminPanel.SecKey,
                        pArticleId.ToString(), pExpansionId.ToString()));

                    pNewsManager.pAdminPanel.ShowActionMessage(json.ResponseMsg);

                    pNewsManager.LoadArticles();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }
    }
}
