using System;
using System.Windows;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PassSecure
{
    using System.Windows.Forms;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x101;
        private LowLevelKeyboardProc proc = HookCallback;
        private static IntPtr hookID = IntPtr.Zero;
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static TimeSpan buttonDownTime;
        private static TimeSpan buttonUpTime;
        private static Keys keyUp;
        private static Keys keyDown;

        public MainWindow()
        {
            InitializeComponent();
            IntPtr handle = GetConsoleWindow();
            ShowWindow(handle, 5);  // to hide the running application
            hookID = SetHook(proc);
            this.Closing += this.MainWindowClosing;
            PasswordBox.Focus();
            CaptchaControl.Hide();
        }

        protected void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UnhookWindowsHookEx(hookID); 
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
           
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                buttonDownTime = DateTime.Now.TimeOfDay;
                int vkCode = Marshal.ReadInt32(lParam);
                keyDown = (Keys) vkCode;
                //Debug.WriteLine(DateTime.Now.TimeOfDay + ": " + (Keys)vkCode + " Key Down");
            }
            else if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)
            {
                buttonUpTime = DateTime.Now.TimeOfDay;
                int vkCode = Marshal.ReadInt32(lParam);
                keyUp = (Keys) vkCode;
                //Debug.WriteLine(DateTime.Now.TimeOfDay + ": " + (Keys)vkCode + " Key Up");
                Debug.WriteLine(keyUp + ": " + (buttonUpTime.TotalMilliseconds - buttonDownTime.TotalMilliseconds));
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
    }
}
