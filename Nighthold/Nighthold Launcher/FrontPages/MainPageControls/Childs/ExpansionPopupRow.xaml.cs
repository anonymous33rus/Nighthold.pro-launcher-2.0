using Nighthold_Launcher.Nighthold;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nighthold_Launcher.FrontPages.MainPageControls.Childs
{
    /// <summary>
    /// Interaction logic for ExpansionPopupRow.xaml
    /// </summary>
    public partial class ExpansionPopupRow : UserControl
    {
        private MainPage mainPage;
        private int ExpansionID;

        public ExpansionPopupRow(MainPage _mainPage, int _expansionID)
        {
            InitializeComponent();

            mainPage = _mainPage;
            ExpansionID = _expansionID;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            switch (ExpansionID)
            {
                case 1: // vanilla
                    ExpansionName.Text = "Classic";
                    break;
                case 2: // tbc
                    ExpansionName.Text = "Burning Crusade";
                    break;
                case 3: // wotlk
                    ExpansionName.Text = "Wrath of the Lich King";
                    break;
                case 4: // cata
                    ExpansionName.Text = "Cataclysm";
                    break;
                case 5: // mop
                    ExpansionName.Text = "Mists of Pandaria";
                    break;
                case 6: // wod
                    ExpansionName.Text = "Warlords of Draenor";
                    break;
                case 7: // legion
                    ExpansionName.Text = "Legion";
                    break;
                case 8: // bfa
                    ExpansionName.Text = "Battle for Azeroth";
                    break;
                case 9: // shadowlands
                    ExpansionName.Text = "Shadowlands";
                    break;
                default:
                    ExpansionName.Text = "Classic";
                    break;
            }
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            RowBorder.Background = ToolHandler.GetColorFromHex("#FF2e3139");
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            RowBorder.Background = null;
        }

        private async void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            foreach (var btn in mainPage.NavbarPanel.Children.OfType<NavbarButton>())
            {
                if (btn.pExpansionID == ExpansionID)
                    btn.OnExpansionSelected();
            }

            ExpansionsPopup tempElement = null;

            foreach (var element in mainPage.mainPageGrid.Children.OfType<ExpansionsPopup>())
            {

                AnimHandler.FadeOut(element, 500);
                tempElement = element;
            }

            mainPage.ExpansionsButton.IsEnabled = true;

            await Task.Delay(1000);

            mainPage.mainPageGrid.Children.Remove(tempElement);
        }
    }
}
