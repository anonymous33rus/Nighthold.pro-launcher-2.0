using Nighthold_Launcher.Nighthold;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Nighthold_Launcher.FrontPages.MainPageControls.Childs
{
    /// <summary>
    /// Interaction logic for ExpansionMenuRow.xaml
    /// </summary>
    public partial class ExpansionMenuRealmRow : UserControl
    {
        private string RealmName;
        private string Realmlist;
        private int Port;

        public ExpansionMenuRealmRow(string _realmName, string _realmlist, int _port)
        {
            InitializeComponent();
            RealmName = _realmName;
            Realmlist = _realmlist;
            Port = _port;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            RealmNameTextBlock.Text = RealmName;

            var pendingAnim = AnimHandler.SpinForever(RealmStatusIcon);

            SetRealmstatusIcon();

            pendingAnim.Stop();
        }

        private async void SetRealmstatusIcon()
        {
            if (await Task.Run(() => RealmHandler.GetRealmStatus(Realmlist, Port, 2500)))
                ToolHandler.SetImageSource(RealmStatusIcon, "/Nighthold Launcher;component/Assets/Menu Icons/realm_up.png", UriKind.Relative);
            else
                ToolHandler.SetImageSource(RealmStatusIcon, "/Nighthold Launcher;component/Assets/Menu Icons/realm_down.png", UriKind.Relative);
        }
    }
}
