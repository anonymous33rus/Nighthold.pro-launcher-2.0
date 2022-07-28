using Nighthold_Launcher.Nighthold;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nighthold_Launcher.AdminPanelControls.Childs
{
    /// <summary>
    /// Interaction logic for ActiveSessionRow.xaml
    /// </summary>
    public partial class ActiveSessionRow : UserControl
    {
        AdminPanel pAdminPanel;
        public string pAvatarUrl;
        public long pAccountID;
        public string pAccountName;
        public string pIPAddress;

        public ActiveSessionRow(AdminPanel _adminPanel, string _avatarUrl, long _accountId, string _accountName, string _ipAddress)
        {
            InitializeComponent();
            pAdminPanel = _adminPanel;
            pAvatarUrl = _avatarUrl;
            pAccountID = _accountId;
            pAccountName = _accountName;
            pIPAddress = _ipAddress;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(pAvatarUrl) && !string.IsNullOrWhiteSpace(pAvatarUrl))
            {
                if (pAvatarUrl.StartsWith("http") || pAvatarUrl.StartsWith("https"))
                {
                    ToolHandler.SetImageSource(Avatar, pAvatarUrl, UriKind.Absolute);
                }
                else
                {
                    string pathToRelativeRes = pAvatarUrl.Replace("/Nighthold Launcher;component/", "");
                    pathToRelativeRes = pathToRelativeRes.Replace(" ", "%20");

                    if (ToolHandler.RelativeResourceExists(pathToRelativeRes))
                        ToolHandler.SetImageSource(Avatar, pAvatarUrl, UriKind.Relative);
                }
            }

            AccountID.Text = pAccountID.ToString();
            AccountName.Text = pAccountName;
            IPAddress.Text = pIPAddress;
        }
    }
}
