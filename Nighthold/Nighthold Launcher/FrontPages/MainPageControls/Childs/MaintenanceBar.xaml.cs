using Nighthold_Launcher.Nighthold;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nighthold_Launcher.FrontPages.MainPageControls.Childs
{
    /// <summary>
    /// Interaction logic for MaintenanceNote.xaml
    /// </summary>
    public partial class MaintenanceBar : UserControl
    {
        public MaintenanceBar()
        {
            InitializeComponent();
        }

        public void SetText(string _text)
        {
            Note.Text = _text;
            Note.Height = 40;
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Note.Height != 40)
                AnimHandler.ToSpecificHeight(Note, 40);
            else
                AnimHandler.ToSpecificHeight(Note, 190);
        }
    }
}
