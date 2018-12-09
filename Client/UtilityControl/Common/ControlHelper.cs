using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace UtilityControl.Common
{
    public static class ControlHelper
    {
        //从Handle中获取Window对象
        private static Window GetWindowFromHwnd(IntPtr hwnd)
        {
            return (Window)HwndSource.FromHwnd(hwnd).RootVisual;
        }

        //GetForegroundWindow API
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        /////调用GetForegroundWindow然后调用GetWindowFromHwnd

        /// <summary>
        /// 获取当前顶级窗体，若获取失败则返回主窗体
        /// </summary>
        public static Window GetTopWindow()
        {
            var hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero)
                return Application.Current.MainWindow;

            return GetWindowFromHwnd(hwnd);
        }

        public static T GetChild<T>(DependencyObject obj, string childName = "") where T : class
        {
            if (obj == null) { return default(T); }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && (child.GetValue(FrameworkElement.NameProperty) + "" == childName || string.IsNullOrEmpty(childName)))
                {
                    return child as T;
                }
                var grandChild = GetChild<T>(child, childName);
                if (grandChild != null)
                {
                    return grandChild;
                }

            }
            return default(T);
        }
    }
}
