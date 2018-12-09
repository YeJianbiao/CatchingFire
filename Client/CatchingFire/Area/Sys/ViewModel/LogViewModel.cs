/**
* 命名空间: CatchingFire.Area.Sys.ViewModel 
* 类    名： LogViewModel
* 创 建 人：lenovo
* 创建时间：2018/8/19 3:04:24
* 用    途：
* 
* 
* Copyright (c) . All rights reserved. 
* 版权所有：　　　　　　　　　　　　　　              
*/

using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace CatchingFire.Area.Sys.ViewModel
{
    public class LogViewModel : ViewModelBase
    {

        public LogViewModel()
        {
            Init();
        }

        private async void Init()
        {
            await Task.Run(() =>
            {
                FileLogVM = new FileLogViewModel();
            });
        }

        public FileLogViewModel FileLogVM { get; set; }

        public LocalLogViewModel LocalLogVM { get; set; }

        private void SelectionChanged(int selectedIndex)
        {
            if (LocalLogVM == null && selectedIndex == 1)
            {
                LocalLogVM = new LocalLogViewModel();
            }
        }

        public ICommand SelectionChangedCommand => new RelayCommand<int>(SelectionChanged);

    }
}
