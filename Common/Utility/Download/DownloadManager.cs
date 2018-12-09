
using System.Collections.Generic;
using System.Linq;

namespace Utility.Download
{
    public class DownloadManager
    {
        
        private static readonly Dictionary<string,DownloadInfo> _downloadInfos = new Dictionary<string, DownloadInfo>();

        private static readonly int _maxDownloadTask = 10;

        private static readonly DownloadManagerListener _listener = new DownloadManagerListener();

        public static bool CanDownload(DownloadInfo downloadInfo,out string msg)
        {
            msg = "";
            if (string.IsNullOrEmpty(downloadInfo.Url))
            {
                msg = "下载地址不能为空";
                return false;
            }
            if (_downloadInfos.Values.Count(o => o.Url == downloadInfo.Url && downloadInfo.DownloadStatus != DownloadStatus.DownloadPause) > 0)
            {
                Logger.LogHelper.Info($"当前文件{downloadInfo.Name}({downloadInfo.Name})正在下载");
                msg = "文件已经在下载中，请不要重复下载";
                return false;
            }
            return true;
        }

        public static void Start(DownloadInfo downloadInfo)
        {
            string msg;
            if (!CanDownload(downloadInfo,out msg))
            {
                return;
            }
            if (_downloadInfos.Values.Count(o => o.DownloadStatus == DownloadStatus.Downloading) >= _maxDownloadTask)
            {
                downloadInfo.DownloadStatus = DownloadStatus.DownloadWait;
                _downloadInfos.Add(downloadInfo.Key,downloadInfo);
                return;
            }
            _downloadInfos.Add(downloadInfo.Key,downloadInfo);
            if (downloadInfo.DownloadWay == DownloadWay.Http)
            {
                if (downloadInfo.UseMultiThread)
                {
                    DownloadByHttp.DownloadWithMultiThreed(downloadInfo, _listener);
                }
                else if (downloadInfo.FileType == FileType.Image)
                {
                    DownloadByHttp.DownloadWithSingleThreed(downloadInfo, _listener);
                }
                else
                {
                    DownloadByHttp.Start(downloadInfo, _listener);
                }
            }
        }

        public static void Pause(DownloadInfo downloadInfo)
        {

        }

        public static void Cancel(DownloadInfo downloadInfo)
        {
            if (_downloadInfos.ContainsKey(downloadInfo.Key))
            {
                _downloadInfos.Remove(downloadInfo.Key);
            }
        }

        public static void StartAll()
        {

        }

        public static void PauseAll()
        {

        }

        public static void CancelAll()
        {

        }

        public static void StartWaitTask()
        {
            var count = _downloadInfos.Values.Count(o => o.DownloadStatus == DownloadStatus.Downloading);
            foreach (var info in _downloadInfos.Values.Where(o=>o.DownloadStatus== DownloadStatus.DownloadWait))
            {
                if (count >= _maxDownloadTask)
                {
                    break;
                }
                Start(info);
                count++;
            }
        }

        public class DownloadManagerListener : IDownloadManagerListener
        {
            public void OnDownloadCompleted(DownloadInfo downloadInfo)
            {
                if (_downloadInfos.ContainsKey(downloadInfo.Key))
                {
                    _downloadInfos[downloadInfo.Key] = downloadInfo;
                }
                StartWaitTask();
            }

            public void OnDownloadFailed(DownloadInfo downloadInfo)
            {
                if (_downloadInfos.ContainsKey(downloadInfo.Key))
                {
                    _downloadInfos[downloadInfo.Key] = downloadInfo;
                }
                StartWaitTask();
            }
        }

        
    }
    
}
