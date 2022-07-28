 using Nighthold_Launcher.Nighthold;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using WinForms = System.Windows.Forms;

namespace Nighthold_Launcher
{
    /// <summary>
    /// Interaction logic for NightholdLauncher.xaml
    /// </summary>
    public partial class NightholdLauncher : Window
    {
        #region FIX WINDOW MAXIMIZE BORDERS
        private static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }
            return (IntPtr)0;
        }

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            if (monitor != IntPtr.Zero)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            }
            Marshal.StructureToPtr(mmi, lParam, true);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>x coordinate of point.</summary>
            public int x;
            /// <summary>y coordinate of point.</summary>
            public int y;
            /// <summary>Construct a point of coordinates (x,y).</summary>
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public static readonly RECT Empty = new RECT();
            public int Width { get { return Math.Abs(right - left); } }
            public int Height { get { return bottom - top; } }
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }
            public RECT(RECT rcSrc)
            {
                left = rcSrc.left;
                top = rcSrc.top;
                right = rcSrc.right;
                bottom = rcSrc.bottom;
            }
            public bool IsEmpty { get { return left >= right || top >= bottom; } }
            public override string ToString()
            {
                if (this == Empty) { return "RECT {Empty}"; }
                return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
            }
            public override bool Equals(object obj)
            {
                if (!(obj is Rect)) { return false; }
                return (this == (RECT)obj);
            }
            /// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
            public override int GetHashCode() => left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
            /// <summary> Determine if 2 RECT are equal (deep compare)</summary>
            public static bool operator ==(RECT rect1, RECT rect2) { return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom); }
            /// <summary> Determine if 2 RECT are different(deep compare)</summary>
            public static bool operator !=(RECT rect1, RECT rect2) { return !(rect1 == rect2); }
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
        #endregion

        public static string LoginUsername;

        public static string LoginPassword;

        public NightholdLauncher()
        {
            if (AnotherInstanceExists())
            {
                SystemTray.notifier.Dispose();
                AppHandler.Shutdown();
            }

            InitializeComponent();

            #region LAUNCHER VERSION
            AppVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            #endregion

            #region SYSTEM TRAY
            _ = new SystemTray(this);
            SystemTray.notifier.MouseDown += new WinForms.MouseEventHandler(notifier_MouseDown);
            SystemTray.notifier.DoubleClick += new EventHandler(notifier_DoubleClick);
            SystemTray.notifier.Icon = Properties.Resources.app_icon;
            SystemTray.notifier.Visible = true;
            #endregion

            #region LOAD SAVED WINDOW SIZE
            if (Properties.Settings.Default.WindowSizeWidth > 0 && Properties.Settings.Default.WindowSizeHeight > 0)
            {
                Width = Properties.Settings.Default.WindowSizeWidth;
                Height = Properties.Settings.Default.WindowSizeHeight;

                if (Properties.Settings.Default.WindowMaximized)
                    WindowState = WindowState.Maximized;
            }
            #endregion

            #region FIX WINDOW MAXIMIZE BORDERS
            SourceInitialized += (s, e) =>
            {
                IntPtr handle = (new WindowInteropHelper(this)).Handle;
                HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(WindowProc));
            };
            #endregion

            #region INITIALIZE WINDOW CONTROL BUTTONS
            MinimizeButton.Click += (s, e) => WindowState = WindowState.Minimized;
            MaximizeButton.Click += (s, e) => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            CloseButton.Click += (s, e) =>
            {
                Properties.Settings.Default.Save();
                switch (Properties.Settings.Default.CloseButtonSettingId)
                {
                    case 0:
                        {
                            SystemTray.notifier.Visible = true;
                            Hide();
                            break;
                        }
                    case 1:
                        AppHandler.Shutdown();
                        break;
                    default:
                        break;
                }
            };
            #endregion

            #region ADD LAUNCHER BORDER
            MainGrid.Children.Add(new Border()
            {
                BorderBrush = ToolHandler.GetColorFromHex("#FF292B33"),
                BorderThickness = new Thickness(1, 1, 1, 1)
            });
            #endregion
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
            if (Properties.Settings.Default.StartLauncherWithComputer)
            {
                if (Properties.Settings.Default.StartLauncherMinimizedToTray)
                {
                    SystemTray.nightholdLauncher.Hide();
                    SystemTray.notifier.Visible = true;
                }
            }
        }

        void notifier_MouseDown(object sender, WinForms.MouseEventArgs e)
        {
            if (e.Button == WinForms.MouseButtons.Right)
            {
                ContextMenu menu = (ContextMenu)FindResource("NotifierContextMenu");
                menu.IsOpen = true;
            }
        }

        private void notifier_DoubleClick(object sender, EventArgs e)
        {
            if (Visibility == Visibility.Hidden)
            {
                Show();
                SystemTray.notifier.Visible = false;
            }
        }

        private void notifier_Menu_Open(object sender, RoutedEventArgs e)
        {
            if (Visibility == Visibility.Hidden)
            {
                Show();
                SystemTray.notifier.Visible = false;
            }
        }

        private void notifier_Menu_Close(object sender, RoutedEventArgs e)
        {
            SystemTray.notifier.Visible = false;
            AppHandler.Shutdown();
        }

        public void SetArguments(string[] args)
        {
            LoginUsername = args[0];
            LoginPassword = args[1];
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AppHandler.SaveWindowSize(this);

            switch (Properties.Settings.Default.CloseButtonSettingId)
            {
                case 0:
                    {
                        SystemTray.notifier.Visible = true;
                        Hide();
                        e.Cancel = true;
                        break;
                    }
                case 1:
                    SystemTray.notifier.Visible = false;
                    SystemTray.notifier.Dispose();
                    AppHandler.Shutdown();
                    break;
                default:
                    break;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
    }
}
