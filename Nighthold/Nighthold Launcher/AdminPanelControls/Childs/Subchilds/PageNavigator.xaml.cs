using Nighthold_Launcher.Nighthold;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WebHandler;

namespace Nighthold_Launcher.AdminPanelControls.Childs.Subchilds
{
    /// <summary>
    /// Interaction logic for PageNavigator.xaml
    /// </summary>
    public partial class PageNavigator : UserControl
    {
        public List<GameMasterClass.CommandLogs> pListCommandLogs;
        public List<GameMasterClass.SoapLogs> pListSoapLogs;
        public StackPanel pStackPanel;

        public readonly int MaxResultsPerPage = 50;
        private readonly int MaxPageNumbers = 22;
        public int TotalPagesCount = 1;
        public int CurrentPageNumber = 1;

        public PageNavigator(int _totalPagesCount, StackPanel _stackPanel,
            List<GameMasterClass.CommandLogs> _listCommandLogs = null,
            List<GameMasterClass.SoapLogs> _listSoapLogs = null)
        {
            InitializeComponent();
            pListCommandLogs = _listCommandLogs;
            pListSoapLogs = _listSoapLogs;
            TotalPagesCount = _totalPagesCount;
            pStackPanel = _stackPanel;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SPLeftNumbers.Children.Clear();

                int numberOfPages = (int)Math.Ceiling((double)TotalPagesCount / MaxResultsPerPage);

                for (int i = 1; i <= numberOfPages; i++)
                {
                    var pageNumber = new PageNumber(this) { Content = i };

                    SPLeftNumbers.Children.Add(pageNumber);
                }

                LastPageNumber.Content = numberOfPages;

                if (numberOfPages > 0)
                    SPLeftNumbers.Children.OfType<PageNumber>().FirstOrDefault().SetAsCurrentSelected();
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private void SPLeftNumbers_LayoutUpdated(object sender, EventArgs e)
        {
            SPRightNumbers.Visibility = SPLeftNumbers.Children.Count >= MaxPageNumbers ? Visibility.Visible : Visibility.Collapsed;

            BtnPrevious.IsEnabled = CurrentPageNumber > 1;

            BtnNext.IsEnabled = CurrentPageNumber < SPLeftNumbers.Children.Count;
        }

        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedPageNumber((CurrentPageNumber - 1) - 1);
        }

        private void LastPageNumber_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedPageNumber(((int)Math.Ceiling((double)TotalPagesCount / MaxResultsPerPage)) - 1);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedPageNumber((CurrentPageNumber - 1) + 1);
        }

        private void BtnSkipTo_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(PageNumberBox.Text, out int _pageNumber);
            SetSelectedPageNumber(_pageNumber - 1);
        }

        public void SetSelectedPageNumber(int _pageNumber)
        {
            try
            {
                foreach (var item in SPLeftNumbers.Children.OfType<PageNumber>())
                {
                    int index = SPLeftNumbers.Children.IndexOf(item);
                    if (index == _pageNumber)
                    {
                        item.SetAsCurrentSelected();
                        SVLeftNumbers.ScrollToHorizontalOffset(item.TransformToVisual(SPLeftNumbers).TransformBounds(new Rect(0, 0, 1, 1)).Left);
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
