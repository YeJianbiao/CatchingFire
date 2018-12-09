
namespace Utility.Download
{
    public interface IDownloadListener
    {

        void OnDownloadCompleted(DownloadInfo downloadInfo);

        void OnDownloadStart(DownloadInfo downloadInfo);

        void OnDownloadFailed(DownloadInfo downloadInfo);

        void OnDownloadCancled(DownloadInfo downloadInfo);

        void OnDownloadProgress(DownloadInfo downloadInfo);

    }
}
