using System;
using System.Runtime.InteropServices;

namespace Utility.Sys
{
    public static class Win32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int X;
            public int Y;
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out Point pt);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern Int32 GetWindowLong(IntPtr hWnd, Int32 nIndex);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern Int32 SetWindowLong(IntPtr hWnd, Int32 nIndex, Int32 newVal);

        /// <summary>
        /// 获取窗口句柄
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 设置窗体的显示与隐藏
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nCmdShow"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        public delegate void WinEventDelegate(
            IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread,
            uint dwmsEventTime);
        private static IntPtr _winEventHook = IntPtr.Zero;
        private static GCHandle _winEventDelegateHandle { get; set; }

        private static int _refCount = 0;

        public delegate void ForgroundWindowChangDelegate(
            IntPtr forGroundHwnd, string forGroundWinTitle, string forgroundProcName);

        public static ForgroundWindowChangDelegate ForgroundWindowChangEventHandler;

        public static void StartForgroundEventHook()
        {
            if (_winEventDelegateHandle == default(GCHandle))
            {
                //Logger.LogHelper.Info("Start ForgroundWindow WinEventHook");
                //WinEventDelegate hookProc = HandleWindowEvent;
                //_winEventDelegateHandle = GCHandle.Alloc(hookProc);
                //_winEventHook = SetWinHook(Event)
            }
        }
       

        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);
        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public static bool IsOpen(string path)
        {
            IntPtr vHandle = _lopen(path, OF_READWRITE | OF_SHARE_DENY_NONE);
            if (vHandle == new IntPtr(-1))
            {
                return true;
            }
            CloseHandle(vHandle);
            return false;
        }
    }
}
