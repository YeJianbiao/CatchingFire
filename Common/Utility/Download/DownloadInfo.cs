using System;
using System.Threading;

namespace Utility.Download
{
    public class DownloadInfo
    {

        public string Key { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string SavePath { get; set; }

        public DownloadWay DownloadWay { get; set; }

        public long FileSize { get; set; }

        public FileType FileType { get; set; }

        public DownloadStatus DownloadStatus { get; set; }

        public Exception Exception { get; set; }

        public string Message { get; set; }

        public double ImageHeight { get; set; }

        public double ImageWidth { get; set; }

        public decimal DownloadSize { get; set; }

        public decimal Progress { get; set; }

        public decimal Speed { get; set; }

        public IDownloadListener Listener { get; set; }

        public CancellationTokenSource CancellationTokenSource { get; set; }

        public bool UseMultiThread { get; set; }


        public DownloadInfo()
        {
            Key = Guid.NewGuid().ToString();
            UseMultiThread = false;
            CancellationTokenSource = new CancellationTokenSource();
            DownloadWay = DownloadWay.Http;
            DownloadStatus = DownloadStatus.Downloading;
        }

    }
}