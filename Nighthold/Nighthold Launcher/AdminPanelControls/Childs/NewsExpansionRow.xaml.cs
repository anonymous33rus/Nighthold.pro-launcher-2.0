using Nighthold_Launcher.AdminPanelControls.Pages;
using Nighthold_Launcher.Nighthold;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Nighthold_Launcher.AdminPanelControls.Childs
{
    /// <summary>
    /// Interaction logic for NewsExpansionRow.xaml
    /// </summary>
    public partial class NewsExpansionRow : UserControl
    {
        private NewsManager pNewsManager;
        public int pExpansionId;

        public NewsExpansionRow(NewsManager _newsManager, int _expansionId)
        {
            InitializeComponent();
            pNewsManager = _newsManager;
            pExpansionId = _expansionId;
        }

        private void ExpansionName_Loaded(object sender, RoutedEventArgs e)
        {
            ExpansionName.Content = ToolHandler.ExpansionIdToName(pExpansionId);
            LoadArticlesForThisExpansionID(pExpansionId);
        }

        public async void LoadArticlesForThisExpansionID(long _expansionId)
        {
            try
            {
                var newsCollection = NewsClass.NewsList.FromJson(await NewsClass.GetNewsListJson(pExpansionId.ToString(), "10000"));
                if (newsCollection != null)
                {
                    pNewsManager.WPArticles.Children.Clear();
                    foreach (var article in newsCollection)
                    {
                        var newsExpansionArticle = new NewsExpansionArticle(
                            pNewsManager,
                            article.ImageUrl,
                            article.ArticleTitle,
                            article.ArticleDate.UtcDateTime.ToString(),
                            article.ArticleUrl,
                            article.ArticleId,
                            pExpansionId);

                        pNewsManager.WPArticles.Children.Add(newsExpansionArticle);
                        AnimHandler.MoveUpAndFadeIn300Ms(newsExpansionArticle);
                    }
                }

                pNewsManager.SelectedExpansion = pExpansionId;
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private void ExpansionName_Click(object sender, RoutedEventArgs e)
        {
            LoadArticlesForThisExpansionID(pExpansionId);
        }
    }
}
