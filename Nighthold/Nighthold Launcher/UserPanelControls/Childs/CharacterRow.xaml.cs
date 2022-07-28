using Nighthold_Launcher.Nighthold;
using System.Windows;
using System.Windows.Controls;

namespace Nighthold_Launcher.UserPanelControls.Childs
{
    /// <summary>
    /// Interaction logic for CharRealmRow.xaml
    /// </summary>
    public partial class CharacterRow : UserControl
    {
        private long Level;
        private long RaceID;
        private long ClassID;
        private long Gender;

        public CharacterRow(string _name, long _level, long _raceID, long _classID, long _gender)
        {
            InitializeComponent();

            CharName.Text = _name;
            Level = _level;
            RaceID = _raceID;
            ClassID = _classID;
            Gender = _gender;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CharLevel.Text = Level.ToString();

            ToolHandler.SetRaceGenderImage(RaceIcon, RaceID, Gender);
            RaceName.Text = ToolHandler.RaceToName(RaceID);
            ClassName.Text = ToolHandler.ClassToName(ClassID);
        }
    }
}
