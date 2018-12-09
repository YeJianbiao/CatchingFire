/**
* 命名空间: Utility.Helper 
* 类    名： WinApiManager
* 创 建 人：lenovo
* 创建时间：2018/8/14 20:43:24
* 用    途：
* 
* 
* Copyright (c) . All rights reserved. 
* 版权所有：　　　　　　　　　　　　　　              
*/

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Utility.Helper
{
    public class WinApiManager
    {


        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public static Process RunningInstance(string pro)
        {
            var current = Process.GetCurrentProcess();
            //遍历与当前进程名称相同的进程列表   
            return Process.GetProcesses().Where(process => process.Id != current.Id).FirstOrDefault(process => process.ProcessName.ToLower() == pro.ToLower());
        }

        public static void HandleRunningInstance(Process instance)
        {
            ShowWindowAsync(instance.MainWindowHandle, 1); //调用api函数，正常显示窗口   
            SetForegroundWindow(instance.MainWindowHandle); //将窗口放置最前端。 
        }

    }
}
