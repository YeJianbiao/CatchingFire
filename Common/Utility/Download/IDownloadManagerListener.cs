/**
* 命名空间: Utility.Download 
* 类    名： IDownloadManagerListener
* 创 建 人：lenovo
* 创建时间：2018/9/9 4:49:19
* 用    途：
* 
* 
* Copyright (c) . All rights reserved. 
* 版权所有：　　　　　　　　　　　　　　              
*/



namespace Utility.Download
{
    public interface IDownloadManagerListener
    {

        void OnDownloadCompleted(DownloadInfo downloadInfo);

        void OnDownloadFailed(DownloadInfo downloadInfo);

    }
}
