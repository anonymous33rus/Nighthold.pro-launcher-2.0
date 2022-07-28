using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Nighthold_Launcher.AdminPanelControls.Childs.Subchilds
{
    /// <summary>
    /// Interaction logic for PageNumber.xaml
    /// </summary>
    public partial class PageNumber : Button
    {
        private PageNavigator pPageNavigator;

        public PageNumber(PageNavigator _pageNavigator)
        {
            InitializeComponent();
            pPageNavigator = _pageNavigator;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetAsCurrentSelected();
        }

        private void ClearSelectedButtons()
        {
            foreach (PageNumber pageNumber in pPageNavigator.SPLeftNumbers.Children.OfType<PageNumber>())
            {
                pageNumber.IsEnabled = true;
            }
        }

        public void SetAsCurrentSelected()
        {
            ClearSelectedButtons();

            IsEnabled = false;

            int.TryParse(Content.ToString(), out int _pageNumber);

            pPageNavigator.pStackPanel.Children.Clear();

            if (pPageNavigator.pListSoapLogs != null)
            {
                if (pPageNavigator.pListSoapLogs.Any())
                {
                    foreach (var soapLog in pPageNavigator.pListSoapLogs.Skip(pPageNavigator.MaxResultsPerPage * (_pageNumber - 1)).Take(pPageNavigator.MaxResultsPerPage))
                    {
                        var soapLogRow = new SoapLogRow(soapLog.AccountName, soapLog.Date.UtcDateTime.ToString(), soapLog.RealmName ?? "unknown", soapLog.Command);
                        pPageNavigator.pStackPanel.Children.Add(soapLogRow);
                    }
                }
            }
            else
            {
                // ??
            }

            pPageNavigator.PageNumberBox.Text = Content.ToString();

            pPageNavigator.CurrentPageNumber = _pageNumber;
        }
    }
}
