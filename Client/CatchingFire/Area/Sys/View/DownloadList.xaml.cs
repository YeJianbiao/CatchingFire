
using System.Windows;


namespace CatchingFire.Area.Sys.View
{
    /// <summary>
    /// DownloadList.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadList 
    {
        public DownloadList()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                Left = SystemParameters.WorkArea.Width - ActualWidth ;
                Top = SystemParameters.WorkArea.Height - ActualHeight;
            };
        }
    }
}
