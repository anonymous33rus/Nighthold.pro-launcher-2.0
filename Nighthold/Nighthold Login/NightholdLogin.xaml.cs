using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Nighthold_Login.Nighthold;
using System.Windows.Media.Animation;
using WebHandler;
using System;

namespace Nighthold_Login
{
    /// <summary>
    /// Interaction logic for NightholdLogin.xaml
    /// </summary>
    public partial class NightholdLogin : Window
    {
        public static bool LoggedOut;

        public NightholdLogin()
        {
            if (AnotherInstanceExists())
            {
                //MessageBox.Show("You cannot run more than one instance of this application.");
                Application.Current.Shutdown();
            }

            InitializeComponent();
        }

        public static bool AnotherInstanceExists()
        {
            Process currentRunningProcess = Process.GetCurrentProcess();
            Process[] listOfProcs = Process.GetProcessesByName(currentRunningProcess.ProcessName);

            foreach (Process proc in listOfProcs)
            {
                if ((proc.MainModule.FileName == currentRunningProcess.MainModule.FileName) && (proc.Id != currentRunningProcess.Id))
                    return true;
            }
            return false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoginUsernameBox.Focus();

            if (LoggedOut)
            {
                LogoutSavedLogin();
            }
            else
            {
                if (bool.Parse(XMLHelper.GetSettingValue("remember_me")))
                {
                    if (!string.IsNullOrEmpty(XMLHelper.GetSettingValue("login_user")) && !string.IsNullOrEmpty(XMLHelper.GetSettingValue("login_pass")))
                    {
                        LoginUsernameBox.Text = XMLHelper.GetSettingValue("login_user");
                        LoginPasswordBox.Password = XMLHelper.GetSettingValue("login_pass");
                        CheckBoxSaveLogin.IsChecked = true;
                        AttemptToLogin();
                    }
                }
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void LoginUsernameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LoginUsernameBox.Text == "Логин")
                LoginUsernameBox.Text = "";
        }

        private void LoginUsernameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginUsernameBox.Text))
                LoginUsernameBox.Text = "Логин";
        }

        private void LoginPasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LoginPasswordBox.Password == "Password")
                LoginPasswordBox.Password = "";
        }

        private void LoginPasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginPasswordBox.Password))
                LoginPasswordBox.Password = "Password";
        }

        private void CheckBoxSaveLogin_Checked(object sender, RoutedEventArgs e) => SaveLoginInfo();

        private void CheckBoxSaveLogin_Unchecked(object sender, RoutedEventArgs e) => ResetLoginInfo();

        private void SaveLoginInfo()
        {
            if (!string.IsNullOrEmpty(LoginUsernameBox.Text) && !string.IsNullOrEmpty(LoginPasswordBox.Password))
            {
                XMLHelper.UpdateSettingValue("login_user", LoginUsernameBox.Text);
                XMLHelper.UpdateSettingValue("login_pass", LoginPasswordBox.Password);
                XMLHelper.UpdateSettingValue("remember_me", "true");
            }
            else
                ResetLoginInfo();
        }

        private void ResetLoginInfo()
        {
            XMLHelper.UpdateSettingValue("login_user", "");
            XMLHelper.UpdateSettingValue("login_pass", "");
            XMLHelper.UpdateSettingValue("remember_me", "false");
        }

        private void LogoutSavedLogin()
        {
            ResetLoginInfo();
        }

        private void Button_Click(object sender, RoutedEventArgs e) => AttemptToLogin();

        private async void AttemptToLogin()
        {
            StartLoginAnimation();

            try
            {
                var loginResponse = AuthClass.LoginResponse.FromJson(await AuthClass.GetLoginReponseJson(LoginUsernameBox.Text, LoginPasswordBox.Password));
                if (loginResponse != null)
                {
                    if (!string.IsNullOrEmpty(loginResponse.Username) && loginResponse.Logged)
                    {
                        Process.Start($"Nighthold Launcher.exe", $"\"{ LoginUsernameBox.Text }\" \"{ LoginPasswordBox.Password }\"");

                        if (CheckBoxSaveLogin.IsChecked ?? true)
                            SaveLoginInfo();

                        Application.Current.Shutdown();
                    }
                    else
                        ErrorBlock.Text = loginResponse.Response;
                }
                else
                    ErrorBlock.Text = "Что-то мешает мне подключится к интернету!";
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }

            StopLoginAnimation();
        }

        Storyboard NightholdLogoSB;

        private void StartLoginAnimation()
        {
            NightholdLogoSB = AnimHandler.SpinForever(NightholdLogo);

            LoginUsernameBox.IsEnabled = false;
            LoginPasswordBox.IsEnabled = false;
            CheckBoxSaveLogin.IsEnabled = false;
            LoginButton.IsEnabled = false;

            ErrorBlock.Text = "";
        }

        private void StopLoginAnimation()
        {
            NightholdLogoSB.Stop();

            LoginUsernameBox.IsEnabled = true;
            LoginPasswordBox.IsEnabled = true;
            CheckBoxSaveLogin.IsEnabled = true;
            LoginButton.IsEnabled = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (NightholdLogoSB != null)
                {
                    if (NightholdLogoSB.GetCurrentState() != ClockState.Active)
                        AttemptToLogin();
                }
                else
                    AttemptToLogin();
            }
        }

        private void BtnNewAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(Properties.Settings.Default.RegisterAccountUrl);
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private void BtnResetPassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(Properties.Settings.Default.ResetPasswordUrl);
            }
            catch (Exception ex)
            {
                ExceptionHandler.AskToReport(ex, new StackTrace(true).GetFrame(0).GetFileName(), new StackTrace(ex, true).GetFrame(0).GetFileLineNumber());
            }
        }

        private void LoginPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckBoxSaveLogin.IsChecked = false;
        }
    }
}
