using System.Windows;

namespace CatchingFire.Area.Message.View
{
    /// <summary>
    /// Message.xaml 的交互逻辑
    /// </summary>
    public partial class Message
    {
        public Message()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
                {
                    Left = (Application.Current.MainWindow.ActualWidth - 200 - ActualWidth) / 2 + 200;
                    Top = (Application.Current.MainWindow.ActualHeight - ActualHeight) / 2;
                }
                else
                {
                    Left = Application.Current.MainWindow.Left + (Application.Current.MainWindow.ActualWidth - 200 - ActualWidth) / 2 + 200;
                    Top = Application.Current.MainWindow.Top + (Application.Current.MainWindow.ActualHeight - ActualHeight) / 2;
                }

            };
        }
    }
}
