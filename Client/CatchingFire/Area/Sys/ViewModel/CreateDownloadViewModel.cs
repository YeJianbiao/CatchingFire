/**
* 命名空间: CatchingFire.Area.Sys.ViewModel 
* 类    名： CreateDownloadViewModel
* 创 建 人：lenovo
* 创建时间：2018/9/9 5:35:11
* 用    途：
* 
* 
* Copyright (c) . All rights reserved. 
* 版权所有：　　　　　　　　　　　　　　              
*/

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CatchingFire.Common;
using Entity.Sys;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using Utility.Download;

namespace CatchingFire.Area.Sys.ViewModel
{
    public class CreateDownloadViewModel:ViewModelBase
    {


        private void Start(string url)
        {
            var info = new DownloadInfo
            {
                Url = url,
                SavePath = GetDownloadFileSavePath()
            };
            
            ControlHelper.CloseWindow(typeof(View.CreateDownload));
            var message = Application.Current.Windows.Cast<Window>().FirstOrDefault(o => o.GetType() == typeof(View.DownloadList));
            if (message != null)
            {
                message.Activate();
                (SimpleIoc.Default.GetInstance<DownloadListViewModel>()).AddDownload(info);
                return;
            }
            Task.Run(() =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ControlHelper.OpenWindow("CatchingFire.Area.Sys.View.DownloadList", null, false);
                    (SimpleIoc.Default.GetInstance<DownloadListViewModel>()).AddDownload(info);
                }));
            });
        }

        private string GetDownloadFileSavePath()
        {
            string savePath;
            var dict =
                LocalService.SQLite.SQLiteHelper.DB.Table<Dict>().FirstOrDefault(o => o.Code == "download_file_savepath");
            if (dict == null)
            {
                savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Download");
                LocalService.SQLite.SQLiteHelper.DB.InsertOrReplace(new LocalService.Entity.Dict()
                {
                    Code = "download_file_savepath",
                    Value = savePath
                });
            }
            else
            {
                savePath = dict.Value;
            }
            return savePath;
        }

        public ICommand StartCommand =>new RelayCommand<string>(Start);

    }
}
