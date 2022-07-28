using System.Windows.Controls;

namespace Nighthold_Launcher.Spinners
{
    /// <summary>
    /// Interaction logic for BlueSpinnerOne.xaml
    /// </summary>
    public partial class BlueSpinnerOne : UserControl
    {
        public BlueSpinnerOne()
        {
            InitializeComponent();
        }

        public void Stop()
        {
            Spinner.Children.Clear();
        }
    }
}
