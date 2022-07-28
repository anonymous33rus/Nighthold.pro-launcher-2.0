using Microsoft.Win32;
using Nighthold_Login;
using System;
using System.Windows;
using WebHandler;

namespace Nighthold_Launcher.Nighthold
{
    class AppHandler
    {
        public static void SaveWindowSize(Window window)
        {
            if (window.WindowState == WindowState.Maximized)
            {
                Properties.Settings.Default.WindowMaximized = true;
            }
            else
            {
                Properties.Settings.Default.WindowSizeWidth = window.Width;
                Properties.Settings.Default.WindowSizeHeight = window.Height;
                Properties.Settings.Default.WindowMaximized = false;
            }

            Properties.Settings.Default.Save();
        }

        public static void AddApplicationToStartup()
        {
            var LoginAssembly = typeof(NightholdLogin).Assembly;

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue(LoginAssembly.GetName().Name, $"\"{LoginAssembly.Location}\"");
            }
        }

        public static void RemoveApplicationFromStartup()
        {
            var LoginAssembly = typeof(NightholdLogin).Assembly;

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.DeleteValue(LoginAssembly.GetName().Name, false);
            }
        }

        public static void MinimizeToTaskBar(Window window) => Application.Current.MainWindow.WindowState = WindowState.Minimized;

        public static void Shutdown() => Environment.Exit(Environment.ExitCode);

        public static async void PingMeAlive()
        {
            await CiSessionsClass.PingMeAlive(NightholdLauncher.LoginUsername);
        }
    }
}
