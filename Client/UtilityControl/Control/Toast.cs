using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UtilityControl.Control
{
    [TemplatePart(Name = "Message", Type = typeof(TextBlock))]
    public class Toast : WindowBase
    {
        private const int MAX_MILLISECOND = 10000;

        private const int DEFAULT_MILLISECOND = 5000;

        static Toast()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Toast), new FrameworkPropertyMetadata(typeof(Toast)));
        }

        private Toast()
        {
            WindowStartupLocation = WindowStartupLocation.Manual;
            Style = FindResource("DefaultToast") as Style;
            AllowsTransparency = true;
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            ShowInTaskbar = false;
            Topmost = true;
            Loaded += (sender, args) =>
            {
                if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
                {
                    Left = (Application.Current.MainWindow.ActualWidth - 200 - ActualWidth) / 2 + 200;
                    Top = (Application.Current.MainWindow.ActualHeight - ActualHeight) / 5 * 4;
                }
                else
                {
                    Left = Application.Current.MainWindow.Left + (Application.Current.MainWindow.ActualWidth - 200 - ActualWidth) / 2 + 200;
                    Top = Application.Current.MainWindow.Top + (Application.Current.MainWindow.ActualHeight - ActualHeight) / 5 * 4;
                }
            };
        }

        public static void Show(string message, ToastType type, int millisecond = DEFAULT_MILLISECOND)
        {
            var toast = new Toast { Message = message };
            switch (type)
            {
                case ToastType.Error:
                    toast.UIcon = "\ue644";
                    toast.UIconForeground = System.Windows.Media.Brushes.Red;
                    break;
                case ToastType.Info:
                    toast.UIcon = "\ue659";
                    break;
                case ToastType.Success:
                    toast.UIcon = "\ue646";
                    break;
                case ToastType.Warn:
                    toast.UIcon = "\ue60b";
                    break;
            }
            toast.Show();
            toast.Wait(Math.Min(millisecond, MAX_MILLISECOND));
        }

        private async void Wait(int millisecond)
        {
            await Task.Delay(millisecond);
            Close();
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
            "Message", typeof(string), typeof(Toast), new PropertyMetadata(default(string)));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

    }

    public enum ToastType
    {
        Info,
        Warn,
        Error,
        Success
    }
}
