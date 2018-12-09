using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Utility.Helper
{
    public class Control
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="className"></param>
        /// <param name="owner"></param>
        /// <param name="showDialog"></param>
        /// <param name="isMainWindow"></param>
        public static void OpenWindow(string projectName, string className, Window owner = null, bool showDialog = true, bool isMainWindow = false)
        {
            var window = Assembly.Load(projectName).CreateInstance(className) as Window;
            if (window == null) { return; }

            window.Owner = owner;

            if (isMainWindow)
            {
                Application.Current.MainWindow = window;
            }

            if (showDialog)
            {
                window.ShowDialog();
            }
            else
            {
                window.Show();
            }
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="t">窗体类型（typeof(窗体)）</param>
        public static void CloseWindow(Type t)
        {
            foreach (Window win in Application.Current.Windows.Cast<Window>().Where(o => o.GetType() == t))
            {
                win.Close();
                break;
            }
        }

        /// <summary>
        /// 下一个焦点
        /// </summary>
        public static void MoveNextFoces()
        {
            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
            elementWithFocus?.MoveFocus(request);

            UIElement nextElement = Keyboard.FocusedElement as UIElement;
            var textBox = nextElement as TextBox;
            textBox?.Select(textBox.Text.Length, 0);
        }

        /// <summary>
        /// 上一个焦点
        /// </summary>
        public static void MovePreviousFoces()
        {
            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Previous);
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
            elementWithFocus?.MoveFocus(request);

            UIElement nextElement = Keyboard.FocusedElement as UIElement;
            var textBox = nextElement as TextBox;
            textBox?.Select(textBox.Text.Length, 0);
        }

        /// <summary>
        /// 第一个焦点
        /// </summary>
        public static void MoveFirstFoces()
        {
            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.First);
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
            elementWithFocus?.MoveFocus(request);

            UIElement nextElement = Keyboard.FocusedElement as UIElement;
            var textBox = nextElement as TextBox;
            textBox?.Select(textBox.Text.Length, 0);
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
