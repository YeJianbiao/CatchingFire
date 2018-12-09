/**
* 命名空间: CatchingFire.Area.Sys.ViewModel 
* 类    名： UcDownloadInfoViewModel
* 创 建 人：lenovo
* 创建时间：2018/9/9 0:46:43
* 用    途：
* 
* 
* Copyright (c) . All rights reserved. 
* 版权所有：　　　　　　　　　　　　　　              
*/

using System;
using System.IO;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Utility.Download;

namespace CatchingFire.Area.Sys.ViewModel
{
    public class UcDownloadInfoViewModel : ViewModelBase, IDownloadListener
    {
        private readonly DownloadInfo _downloadInfo;
        public Action<DownloadInfo> CancelAction;

        public UcDownloadInfoViewModel(DownloadInfo info)
        {
            info.Listener = this;
            _downloadInfo = info;
            FileName = info.Name;
            Status = info.DownloadStatus;
            DownloadManager.Start(info);
        }

        public string FileName { get; set; }

        public string FileSize { get; set; }

        public DownloadStatus Status { get; set; }

        public int Progress { get; set; }

        public string Msg { get; set; } = "正在计算文件大小";


        public void OnDownloadCompleted(DownloadInfo downloadInfo)
        {
            if (string.IsNullOrEmpty(FileName))
            {
                FileName = downloadInfo.Name;
            }
            Status = DownloadStatus.Downloaded;
        }

        public void OnDownloadStart(DownloadInfo downloadInfo)
        {
            Msg = "";
            FileSize = GetFileSize(downloadInfo.FileSize);
            if (string.IsNullOrEmpty(FileName))
            {
                FileName = downloadInfo.Name;
            }
        }

        public void OnDownloadFailed(DownloadInfo downloadInfo)
        {
            Status = DownloadStatus.DownloadFailed;
            Msg = downloadInfo.Message;
        }

        public void OnDownloadCancled(DownloadInfo downloadInfo)
        {
            CancelAction?.Invoke(downloadInfo);
            DownloadManager.Cancel(downloadInfo);
        }

        public void OnDownloadProgress(DownloadInfo downloadInfo)
        {
            if (string.IsNullOrEmpty(FileName))
            {
                FileName = downloadInfo.Name;
            }
            if (downloadInfo.FileSize <= 0)
            {
                Msg = "获取文件大小失败";
                return;
            }
            if (string.IsNullOrEmpty(FileSize))
            {
                FileSize = GetFileSize(downloadInfo.FileSize);
            }
            Progress = (int)((downloadInfo.DownloadSize / downloadInfo.FileSize) * 100);
        }

        private string GetFileSize(long length)
        {
            if (length < 1024)
            {
                return "1KB";
            }
            if (length < 1024 * 1024)
            {
                return $"{length / 1024}KB";
            }
            if (length < 1024 * 1024 * 1024)
            {
                return $"{length / 1024 / 1024}M";
            }
            return $"{length / 1024 / 1024 / 1024}G";
        }

        private void Delete()
        {
            if (Status == DownloadStatus.Downloading)
            {
                _downloadInfo.CancellationTokenSource.Cancel();
            }
            else
            {
                CancelAction?.Invoke(_downloadInfo);
                DownloadManager.Cancel(_downloadInfo);
            }
        }

        private void OpenFile()
        {
            var path = Path.Combine(_downloadInfo.SavePath, _downloadInfo.Name);
            System.Diagnostics.Process.Start(path);
        }

        private void OpenFolder()
        {
            var path = Path.Combine(_downloadInfo.SavePath, _downloadInfo.Name);
            System.Diagnostics.Process.Start("explorer", "/select,\"" + path + "\"");
        }

        public ICommand DeleteCommand => new RelayCommand(Delete);

        public ICommand OpenFileCommand => new RelayCommand(OpenFile);

        public ICommand OpenFolderCommand => new RelayCommand(OpenFolder);
    }
}
