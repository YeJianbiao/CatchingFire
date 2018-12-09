/**
* 命名空间: CatchingFire.Area.Sys.ViewModel 
* 类    名： DownloadListViewModel
* 创 建 人：lenovo
* 创建时间：2018/8/19 23:06:28
* 用    途：
* 
* 
* Copyright (c) . All rights reserved. 
* 版权所有：　　　　　　　　　　　　　　              
*/


using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Utility.Download;
using MessageBox = UtilityControl.Control.MessageBox;

namespace CatchingFire.Area.Sys.ViewModel
{
    public class DownloadListViewModel:ViewModelBase
    {

        public DownloadListViewModel()
        {
            Init();
        }

        private async void Init()
        {
            await Task.Run(() =>
            {
                DownList = new ObservableCollection<UcDownloadInfoViewModel>();
            });
        }

        public ObservableCollection<UcDownloadInfoViewModel> DownList { get; set; }


        public void AddDownload(DownloadInfo info)
        {
            string msg;
            if (!DownloadManager.CanDownload(info,out msg))
            {
                MessageBox.Show("重复下载", msg);
                return;
            }
            var vm = new UcDownloadInfoViewModel(info);
            vm.CancelAction = o =>
            {
                DownList.Remove(vm);
            };
            DownList.Add(vm);
        }

        private void Create()
        {
            var downlistWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(o => o.GetType() == typeof(View.DownloadList));
            if (downlistWindow == null)
            {
                return;
            }
            var createDownload = Application.Current.Windows.Cast<Window>().FirstOrDefault(o => o.GetType() == typeof(View.CreateDownload));
            if (createDownload != null)
            {
                createDownload.Activate();
                return;
            }
            Task.Run(() =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Common.ControlHelper.OpenWindow("CatchingFire.Area.Sys.View.CreateDownload", downlistWindow);
                }));
            });
        }

        private void Pause()
        {
            
        }

        private void Start()
        {
            
        }

        private void Clear()
        {
            
        }

        public ICommand CreateCommand=>new RelayCommand(Create);
        public ICommand PauseCommand => new RelayCommand(Pause);
        public ICommand StartCommand => new RelayCommand(Start);
        public ICommand ClearCommand => new RelayCommand(Clear);

    }
}
