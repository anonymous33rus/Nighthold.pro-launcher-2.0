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

namespace Nighthold_Launcher.FrontPages.CharactersMarketControls.Childs
{
    /// <summary>
    /// Interaction logic for CBoxCharacter.xaml
    /// </summary>
    public partial class CBoxCharacter : ComboBoxItem
    {
        public long pGuid;
        public string pCharName;
        public long pClass;
        public long pRace;
        public long pGender;
        public long pLevel;
        public long pRealmID;
        public string pRealmName;

        public CBoxCharacter(long _guid, string _charName, long _class, long _race, long _gender, long _level, long _realmID, string _realmName)
        {
            InitializeComponent();
            pGuid = _guid;
            pCharName = _charName;
            pClass = _class;
            pRace = _race;
            pGender = _gender;
            pLevel = _level;
            pRealmID = _realmID;
            pRealmName = _realmName;
        }

        private void ComboBoxItem_Loaded(object sender, RoutedEventArgs e)
        {
            ToolHandler.SetRaceGenderImage(RaceImg, pRace, pGender);
            ToolHandler.SetClassImage(ClassImg, pClass);
            CharName.Text = pCharName;
            CharName.Foreground = ToolHandler.GetPlayerClassColorBrush(pClass);
            CharLevel.Text = pLevel.ToString();
        }
    }
}
