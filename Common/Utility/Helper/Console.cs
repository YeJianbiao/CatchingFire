using System;

namespace Utility.Helper
{
    /// <summary>
    /// 控制台帮助类
    /// </summary>
    public static class Console
    {


        /// <summary>
        /// 隐藏控制台
        /// </summary>
        /// <param name="consoleTitle">控制台标题(可为空,为空则取默认值)</param>
        public static void Hide(string consoleTitle = "")
        {
            consoleTitle = string.IsNullOrEmpty(consoleTitle) ? System.Console.Title : consoleTitle;
            IntPtr hWnd = Sys.Win32.FindWindow("ConsoleWindowClass", consoleTitle);
            if (hWnd != IntPtr.Zero)
            {
                Sys.Win32.ShowWindow(hWnd, 0);
            }
        }

        /// <summary>
        /// 显示控制台
        /// </summary>
        /// <param name="consoleTitle">控制台标题(可为空,为空则去默认值)</param>
        public static void Show(string consoleTitle = "")
        {
            consoleTitle = string.IsNullOrEmpty(consoleTitle) ? System.Console.Title : consoleTitle;
            IntPtr hWnd = Sys.Win32.FindWindow("ConsoleWindowClass", consoleTitle);
            if (hWnd != IntPtr.Zero)
            {
                Sys.Win32.ShowWindow(hWnd, 1);
            }
        }
    }
}
