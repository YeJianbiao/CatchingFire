
using CatchingFire.Area.Sys.ViewModel;

namespace CatchingFire.Area.Sys.View
{
    /// <summary>
    /// LogView.xaml 的交互逻辑
    /// </summary>
    public partial class LogPreview 
    {
        public LogPreview()
        {
            InitializeComponent();
            DataContext = new LogPreviewViewModel();
        }
    }
}
