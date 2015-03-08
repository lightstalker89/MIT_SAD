// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// <author>Mario Murrent</author>
// --------------------------------------------------------------------------------------------------------------------

namespace PassSecure.Views
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Forms;

    using PassSecure.Data;
    using PassSecure.Models;
    using PassSecure.Service;

    #endregion

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// </summary>
        private static MainWindow Instance { get; set; }

        /// <summary>
        /// </summary>
        private const int WH_KEYBOARD_LL = 13;

        /// <summary>
        /// </summary>
        private const int WM_KEYDOWN = 0x0100;

        /// <summary>
        /// </summary>
        private const int WM_KEYUP = 0x101;

        /// <summary>
        /// </summary>
        private LowLevelKeyboardProc proc = HookCallback;

        /// <summary>
        /// </summary>
        private static IntPtr hookID = IntPtr.Zero;

        /// <summary>
        /// </summary>
        /// <param name="nCode">
        /// </param>
        /// <param name="wParam">
        /// </param>
        /// <param name="lParam">
        /// </param>
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// </summary>
        private static TimeSpan keyDownTime;

        /// <summary>
        /// </summary>
        private static TimeSpan keyUpTime;

        /// <summary>
        /// </summary>
        private static Keys keyUp;

        // private static Keys keyDown;
        /// <summary>
        /// </summary>
        private static readonly List<KeyStroke> KeyStrokes = new List<KeyStroke>();

        private DataStore dataStore = null;

        /// <summary>
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Bootstrapper bootstrapper = new Bootstrapper();
            dataStore = SimpleContainer.Resolve<DataStore>();
            IntPtr handle = GetConsoleWindow();
            ShowWindow(handle, 5); // to hide the running application
            hookID = SetHook(this.proc);
            this.Closing += this.MainWindowClosing;
            this.Password.Focus();
            Instance = this;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        protected void MainWindowClosing(object sender, CancelEventArgs e)
        {
            UnhookWindowsHookEx(hookID);
        }

        /// <summary>
        /// </summary>
        /// <param name="nCode">
        /// </param>
        /// <param name="wParam">
        /// </param>
        /// <param name="lParam">
        /// </param>
        /// <returns>
        /// </returns>
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (ApplicationIsActivated() && Instance.Password.IsFocused)
            {
                if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
                {
                    keyDownTime = DateTime.Now.TimeOfDay;
                }
                else if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)
                {
                    keyUpTime = DateTime.Now.TimeOfDay;
                    int vkCode = Marshal.ReadInt32(lParam);
                    keyUp = (Keys)vkCode;
                    if (keyUp == Keys.Enter)
                    {

                    }
                    else
                    {
                        Debug.WriteLine(
                            DateTime.Now.TimeOfDay + " - " + keyUp + "(" + vkCode + "): "
                            + (keyUpTime.TotalMilliseconds - keyDownTime.TotalMilliseconds));
                        KeyStrokes.Add(new KeyStroke(keyUp) { KeyDownTime = keyDownTime, KeyUpTime = keyUpTime });
                    }
                }
            }

            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }

        /// <summary>
        /// </summary>
        /// <param name="proc">
        /// </param>
        /// <returns>
        /// </returns>
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        #region DLL Imports

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// </summary>
        /// <param name="handle">
        /// </param>
        /// <param name="processId">
        /// </param>
        /// <returns>
        /// </returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        /// <summary>
        /// </summary>
        /// <param name="idHook">
        /// </param>
        /// <param name="lpfn">
        /// </param>
        /// <param name="hMod">
        /// </param>
        /// <param name="dwThreadId">
        /// </param>
        /// <returns>
        /// </returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(
            int idHook, 
            LowLevelKeyboardProc lpfn, 
            IntPtr hMod, 
            uint dwThreadId);

        /// <summary>
        /// </summary>
        /// <param name="hhk">
        /// </param>
        /// <returns>
        /// </returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        /// <summary>
        /// </summary>
        /// <param name="hhk">
        /// </param>
        /// <param name="nCode">
        /// </param>
        /// <param name="wParam">
        /// </param>
        /// <param name="lParam">
        /// </param>
        /// <returns>
        /// </returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// </summary>
        /// <param name="lpModuleName">
        /// </param>
        /// <returns>
        /// </returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        /// <summary>
        /// </summary>
        /// <param name="hWnd">
        /// </param>
        /// <param name="nCmdShow">
        /// </param>
        /// <returns>
        /// </returns>
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        #endregion

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static bool ApplicationIsActivated()
        {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                return false;
            }

            var procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);

            return activeProcId == procId;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void MenuItemModeTrainChecked(object sender, RoutedEventArgs e)
        {
            MenuItemModeNormal.IsChecked = !MenuItemModeTrain.IsChecked;
            CheckMode();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void MenuItemModeNormalChecked(object sender, RoutedEventArgs e)
        {
            MenuItemModeTrain.IsChecked = !MenuItemModeNormal.IsChecked;
            CheckMode();
        }

        /// <summary>
        /// </summary>
        private void CheckMode()
        {
            if (ModeText != null)
            {
                ModeText.Text = MenuItemModeNormal.IsChecked
                                    ? MenuItemModeNormal.Header.ToString()
                                    : MenuItemModeTrain.Header.ToString();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void AddUserButtonClick(object sender, RoutedEventArgs e)
        {
            AddUserWindow addUserWindowWindow = new AddUserWindow();
            addUserWindowWindow.ShowDialog();
            if (addUserWindowWindow.DialogResult.HasValue && addUserWindowWindow.DialogResult.Value)
            {
                dataStore.AddUserTraining(
                    new UserTraining()
                        {
                            UserName = addUserWindowWindow.UserName.Text, 
                            Password = addUserWindowWindow.Password.Text
                        });
            }
        }
    }
}
