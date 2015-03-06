namespace PassSecure.Views
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Forms;


    using PassSecure.Models;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private static MainWindow Instance { get; set; }
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x101;
        private LowLevelKeyboardProc proc = HookCallback;
        private static IntPtr hookID = IntPtr.Zero;
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static TimeSpan keyDownTime;
        private static TimeSpan keyUpTime;
        private static Keys keyUp;
        //private static Keys keyDown;
        private static readonly List<KeyStroke> keyStrokes = new List<KeyStroke>();

        public MainWindow()
        {
            InitializeComponent();
            IntPtr handle = GetConsoleWindow();
            ShowWindow(handle, 5);  // to hide the running application
            hookID = SetHook(this.proc);
            this.Closing += this.MainWindowClosing;
            this.Password.Focus();
            Instance = this;
        }

        protected void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UnhookWindowsHookEx(hookID);
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (Instance.IsFocused && Instance.Password.IsFocused)
            {
                if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
                {
                    keyDownTime = DateTime.Now.TimeOfDay;
                    int vkCode = Marshal.ReadInt32(lParam);
                    //keyDown = (Keys)vkCode;
                }
                else if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)
                {
                    keyUpTime = DateTime.Now.TimeOfDay;
                    int vkCode = Marshal.ReadInt32(lParam);
                    keyUp = (Keys)vkCode;
                    Debug.WriteLine(DateTime.Now.TimeOfDay + " - " + keyUp + "(" + vkCode + "): " + (keyUpTime.TotalMilliseconds - keyDownTime.TotalMilliseconds));
                    keyStrokes.Add(new KeyStroke(keyUp) { KeyDownTime = keyDownTime, KeyUpTime = keyUpTime });
                }
            }
            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        #region DLL Imports
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        #endregion

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemModeTrainChecked(object sender, RoutedEventArgs e)
        {
            MenuItemModeNormal.IsChecked = !MenuItemModeTrain.IsChecked;
            CheckMode();
        }

        private void MenuItemModeNormalChecked(object sender, RoutedEventArgs e)
        {
            MenuItemModeTrain.IsChecked = !MenuItemModeNormal.IsChecked;
            CheckMode();
        }

        private void CheckMode()
        {
            if (ModeText != null)
            {
                ModeText.Text = (MenuItemModeNormal.IsChecked)
                                   ? MenuItemModeNormal.Header.ToString()
                                   : MenuItemModeTrain.Header.ToString();
            }
        }
    }
}
