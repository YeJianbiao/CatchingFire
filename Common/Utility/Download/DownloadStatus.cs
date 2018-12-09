
namespace Utility.Download
{
    public enum DownloadStatus
    {
        //已取消或暂停
        UnDownload,

        Downloading,

        Downloaded,

        DownloadFailed,

        DownloadErrorUrl,

        DownloadPause,

        DownloadWait

    }
}
