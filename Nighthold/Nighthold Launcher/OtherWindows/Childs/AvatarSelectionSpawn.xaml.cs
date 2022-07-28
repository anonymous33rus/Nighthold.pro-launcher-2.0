using Nighthold_Launcher.Nighthold;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nighthold_Launcher.OtherWindows.Childs
{
    /// <summary>
    /// Interaction logic for AvatarSelectionSpawn.xaml
    /// </summary>
    public partial class AvatarSelectionSpawn : UserControl
    {
        AvatarSelector pAvatarSelector;
        string pAvatarPath;
        bool pIsDBAvatar;

        public AvatarSelectionSpawn(AvatarSelector _avatarSelector, string _path, bool _isDBAvatar = false)
        {
            InitializeComponent();
            pAvatarSelector = _avatarSelector;
            pAvatarPath = _path;
            pIsDBAvatar = _isDBAvatar;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!pIsDBAvatar)
                ToolHandler.SetImageSource(imgAvatar, $"/Nighthold Launcher;component/{pAvatarPath}", UriKind.Relative);
            else
                ToolHandler.SetImageSource(imgAvatar, pAvatarPath, UriKind.Absolute);
        }

        private void imgAvatar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            foreach (AvatarSelectionSpawn avatar in pAvatarSelector.WPAvatars.Children.OfType<AvatarSelectionSpawn>())
            {
                avatar.chkAvatar.IsChecked = false;
            }

            chkAvatar.IsChecked = !chkAvatar.IsChecked;

            pAvatarSelector.pAvatarPath = pIsDBAvatar ? pAvatarPath : $"/Nighthold Launcher;component/{pAvatarPath}";
            pAvatarSelector.pAvatarUriKind = pIsDBAvatar ? UriKind.Absolute : UriKind.Relative;
        }

        private void chkAvatar_Checked(object sender, RoutedEventArgs e)
        {
            BorderThickness = new Thickness(2);
        }

        private void chkAvatar_Unchecked(object sender, RoutedEventArgs e)
        {
            BorderThickness = new Thickness(0);
        }
    }
}
