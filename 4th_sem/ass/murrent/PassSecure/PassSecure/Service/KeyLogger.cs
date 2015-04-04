#region File Header
// <copyright file="KeyLogger.cs" company="">
// Copyright (c) 2015 Mario Murrent. All rights reserved.
// </copyright>
// <summary>
// </summary>
// <author>Mario Murrent</author>
#endregion
#region File Header
// <copyright file="KeyLogger.cs" company="">
// Copyright (c) 2015 Mario Murrent. All rights reserved.
// </copyright>
// <summary>
// </summary>
// <author>Mario Murrent</author>
#endregion
namespace PassSecure.Service
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Windows.Input;

    using PassSecure.Events;
    using PassSecure.Models;
    using PassSecure.Views;

    #endregion

    /// <summary>
    /// </summary>
    public class KeyLogger
    {
        private static readonly List<Key> IgnoredKeys = new List<Key> { Key.Back, Key.LeftCtrl, Key.RightCtrl, Key.RightAlt, Key.LeftAlt, Key.F1, Key.F2, Key.F3, Key.F4, Key.F5, Key.F6, Key.F7, Key.F8, Key.F9, Key.F10, Key.F11, Key.F12, Key.F13, Key.F14, Key.F15, Key.F16, Key.F18, Key.F19, Key.F20, Key.F21, Key.F22, Key.F23, Key.F24 };

        private static bool isCapsLockEnabled = false;

        private static bool isShiftKeyPressed = false;

        /// <summary>
        /// </summary>
        private static KeyLogger instance;

        /// <summary>
        /// </summary>
        private static MainWindow mainwindow = null;

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
        private static TimeSpan keyDownTime;

        /// <summary>
        /// </summary>
        private static TimeSpan keyUpTime;

        /// <summary>
        /// </summary>
        private static Key keyUp;

        private static Key keyDown;

        /// <summary>
        /// </summary>
        public event EventHandler<EventArgs> EnterPressed;

        /// <summary>
        /// </summary>
        public event EventHandler<KeyLogEventArgs> KeyLogPerformed;

        /// <summary>
        /// </summary>
        /// <param name="nCode">
        /// </param>
        /// <param name="wParam">
        /// </param>
        /// <param name="lParam">
        /// </param>
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

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
        public KeyLogger()
        {
            IntPtr handle = GetConsoleWindow();
            ShowWindow(handle, 5);
            hookID = SetHook(this.proc);
            instance = this;
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
            if (ApplicationIsActivated() && mainwindow.Password.IsFocused)
            {
                if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
                {
                    keyDownTime = DateTime.Now.TimeOfDay;
                    int vkCode = Marshal.ReadInt32(lParam);
                    //keyDown = (Keys)vkCode;
                    keyDown = KeyInterop.KeyFromVirtualKey(vkCode);
                    if (keyDown == Key.LeftShift || keyDown == Key.RightShift)
                    {
                        isShiftKeyPressed = true;
                    }
                }
                else if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)
                {
                    keyUpTime = DateTime.Now.TimeOfDay;
                    int vkCode = Marshal.ReadInt32(lParam);
                    //keyUp = (Keys)vkCode;
                    keyUp = KeyInterop.KeyFromVirtualKey(vkCode);
                    if (keyUp == Key.Enter)
                    {
                        instance.OnEnterPressed();
                    }
                    else if (keyUp == Key.LeftShift || keyUp == Key.RightShift)
                    {
                        isShiftKeyPressed = false;
                    }
                    else if (keyUp == Key.CapsLock)
                    {
                        isCapsLockEnabled = !isCapsLockEnabled;
                    }
                    else
                    {
                        if (!IgnoredKeys.Contains(keyUp))
                        {
                            KeyStroke keyStroke = new KeyStroke(keyUp)
                                                      {
                                                          KeyDownTime = keyDownTime,
                                                          KeyUpTime = keyUpTime
                                                      };
                            instance.OnKeyLogPerformend(new KeyLogEventArgs(keyStroke));
                        }
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
        private void OnEnterPressed()
        {
            if (EnterPressed != null)
            {
                EnterPressed(null, new EventArgs());
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        private void OnKeyLogPerformend(KeyLogEventArgs args)
        {
            if (KeyLogPerformed != null)
            {
                KeyLogPerformed(null, args);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="mainWindow">
        /// </param>
        public void SetMainWindow(MainWindow mainWindow)
        {
            mainwindow = mainWindow;
        }

        /// <summary>
        /// </summary>
        public void UnHook()
        {
            UnhookWindowsHookEx(hookID);
        }
    }
}
