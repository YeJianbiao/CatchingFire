using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Utility.Sys;

namespace Utility.Download
{
    public class DownloadByHttp
    {
        #region

        //private static readonly object obj = new object();
        private static readonly TimeSpan _timeOut = TimeSpan.FromSeconds(15);

        private static readonly int _bytesLength = 80 * 1024;

        private static readonly int _maxThread = 8;

        /// <summary>
        /// 文件大小达到多少时启用多线程
        /// </summary>
        private static readonly long _multiThreadMinSize = 30 * 1024 * 1024;

        /// <summary>
        /// 每个线程下载多少数据
        /// </summary>
        private static readonly long _oneThreadSize = 10 * 1024 * 1024;

        private static string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.87 Safari/537.36";
        private static string _acceptLanguage = "zh-CN,zh;q=0.8";
        private static string _accept = "text/html, application/xhtml+xml, */*";
        private static string _acceptEncoding = "gzip, deflate";
        private static string _charset = "utf-8";

        private static readonly Dictionary<string, FileInfo> _downloadingFiles = new Dictionary<string, FileInfo>();

        #endregion

        public static async void Start(DownloadInfo info, IDownloadManagerListener managerListener)
        {
            try
            {
                var file = await GetFileSize(info);
                info.FileSize = file.Item2;
                if (string.IsNullOrEmpty(info.Name))
                {
                    info.Name = file.Item1;
                }
                info.Listener.OnDownloadStart(info);
                if (info.FileSize > _multiThreadMinSize)
                {
                    Task.Run(() => DownloadWithMultiThreed(info, managerListener));
                }
                else if (info.FileSize > 0)
                {
                    Task.Run(() => DownloadWithSingleThreed(info, managerListener));
                }
                else
                {
                    DownloadUnKownSizeFile(info, managerListener);
                }

            }
            catch (Exception ex)
            {
                Logger.LogHelper.Error("下载文件出错", ex);
                info.Exception = ex;
                info.Message = ex.Message;
                info.Listener.OnDownloadFailed(info);
            }
        }

        /// <summary>
        /// 单线程下载
        /// </summary>
        /// <param name="info"></param>
        /// <param name="managerListener"></param>
        public static async void DownloadWithSingleThreed(DownloadInfo info, IDownloadManagerListener managerListener)
        {
            try
            {
                var contentLength = info.FileSize;
                if (info.FileSize <= 0 || string.IsNullOrEmpty(info.Name))
                {
                    var file = await GetFileSize(info);
                    if (string.IsNullOrEmpty(info.Name))
                    {
                        info.Name = file.Item1;
                    }
                    if (file.Item2 <= 0)
                    {
                        DownloadUnKownSizeFile(info, managerListener);
                        return;
                    }
                }
                if (string.IsNullOrEmpty(info.Name))
                {
                    info.Name = Guid.NewGuid().ToString();
                }
                info.Listener.OnDownloadStart(info);
                using (var httpClient = CreateHttpClient())
                {
                    if (!Directory.Exists(info.SavePath))
                    {
                        Directory.CreateDirectory(info.SavePath);
                    }
                    using (var fileStream = new FileStream(Path.Combine(info.SavePath, info.Name), FileMode.Append, FileAccess.Write))
                    {
                        var bytes = new byte[_bytesLength];
                        var beginSecond = DateTime.Now.Second;
                        if (fileStream.Length >= contentLength)
                        {
                            info.Listener.OnDownloadCompleted(info);
                            managerListener.OnDownloadCompleted(info);
                            return;
                        }
                        httpClient.DefaultRequestHeaders.Add("Range", "bytes = " + fileStream.Length + " - " + (contentLength - 1));
                        contentLength -= fileStream.Length;
                        var response = await httpClient.GetAsync(info.Url, info.CancellationTokenSource.Token);
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        {
                            int writeLength;
                            var readLength = _bytesLength;
                            if (readLength != -1 && contentLength < readLength)
                            {
                                readLength = (int)contentLength;
                            }
                            while ((writeLength = (stream.Read(bytes, 0, readLength))) > 0)
                            {
                                if (info.CancellationTokenSource.Token.IsCancellationRequested)
                                {
                                    info.Listener.OnDownloadCancled(info);
                                }

                                fileStream.Write(bytes, 0, writeLength);

                                info.DownloadSize += writeLength;
                                info.Progress = info.DownloadSize / contentLength;

                                if (beginSecond != DateTime.Now.Second)
                                {
                                    info.Speed = writeLength / (DateTime.Now.Second - beginSecond) / 1024;
                                }
                                info.Listener.OnDownloadProgress(info);
                                contentLength -= writeLength;
                                if (contentLength <= 0)
                                {
                                    info.DownloadStatus = DownloadStatus.Downloaded;
                                    info.Listener.OnDownloadCompleted(info);
                                    managerListener.OnDownloadCompleted(info);
                                }
                            }
                            stream.Flush();
                            stream.Close();
                        }
                        fileStream.Flush();
                        fileStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogHelper.Error("下载文件出错", ex);
                info.Message = ex.Message;
                info.Exception = ex;
                info.DownloadStatus = DownloadStatus.DownloadFailed;
                info.Listener.OnDownloadFailed(info);
                managerListener.OnDownloadFailed(info);
            }

        }

        public static async void DownloadWithMultiThreed(DownloadInfo info, IDownloadManagerListener managerListener)
        {
            try
            {
                var contentLength = info.FileSize;
                if (info.FileSize <= 0 || string.IsNullOrEmpty(info.Name))
                {
                    var file = await GetFileSize(info);
                    if (string.IsNullOrEmpty(info.Name))
                    {
                        info.Name = file.Item1;
                    }
                    if (file.Item2 <= 0)
                    {
                        DownloadUnKownSizeFile(info, managerListener);
                        return;
                    }
                }
                if (string.IsNullOrEmpty(info.Name))
                {
                    info.Name = Guid.NewGuid().ToString();
                }
                info.Listener.OnDownloadStart(info);
                var fileInfo = new FileInfo { FileLength = contentLength };
                if (_downloadingFiles.ContainsKey(info.Url))
                {
                    _downloadingFiles.Remove(info.Url);
                }
                _downloadingFiles.Add(info.Url, fileInfo);

                int taskCount = (int)(_downloadingFiles[info.Url].FileLength / _oneThreadSize);
                var tasks = new List<Task>();
                for (int i = 0; i < _maxThread; i++)
                {
                    if (taskCount <= 0)
                    {
                        break;
                    }
                    taskCount--;
                    var t = Task.Run(async () => { await DownloadChunk(info); });
                    tasks.Add(t);
                }
                Task.WaitAll(tasks.ToArray());
                if (info.DownloadStatus == DownloadStatus.Downloading)
                {
                    MergeFile(info);
                    info.DownloadStatus = DownloadStatus.Downloaded;
                    info.Listener.OnDownloadCompleted(info);
                    managerListener.OnDownloadCompleted(info);
                }
            }
            catch (Exception ex)
            {
                Logger.LogHelper.Error("下载文件出错", ex);
                if (info.Message == null)
                {
                    info.Exception = ex;
                    info.Message = ex.Message;
                }
                info.DownloadStatus = DownloadStatus.DownloadFailed;
                info.Listener.OnDownloadFailed(info);
                managerListener.OnDownloadFailed(info);
            }
        }

        private static async Task DownloadChunk(DownloadInfo info)
        {
            try
            {
                var sequence = 1;
                var path = Path.Combine(info.SavePath, info.Name.Replace(".", ""));
                
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                while (_downloadingFiles[info.Url].DownloadingFiles.Contains($"{info.Name.Split('.')[0]}_{sequence}.temp"))
                {
                    sequence++;
                }
                if (_downloadingFiles[info.Url].FileLength < (sequence - 1) * _oneThreadSize)
                {
                    return;
                }
                string fileName = $"{info.Name.Split('.')[0]}_{sequence}.temp";
                _downloadingFiles[info.Url].DownloadingFiles.Add(fileName);

                using (var httpClient = CreateHttpClient())
                {
                    long from = (sequence - 1) * _oneThreadSize;
                    long to = Math.Min(sequence * _oneThreadSize, _downloadingFiles[info.Url].FileLength);
                    using (var fileStream = File.Open(Path.Combine(path, fileName), FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    {
                        from += fileStream.Length;
                        long downloadSize = to - from;
                        if (from < to - 1)
                        {
                            httpClient.DefaultRequestHeaders.Add("Range", $"bytes={from}-{to - 1}");
                            var response = await httpClient.GetAsync(info.Url, info.CancellationTokenSource.Token);

                            using (var stream = await response.Content.ReadAsStreamAsync())
                            {
                                var bytes = new byte[_bytesLength];
                                int writeLength;
                                int readLength = _bytesLength;
                                if (downloadSize != 0 && downloadSize < readLength)
                                {
                                    readLength = (int)downloadSize;
                                }
                                while ((writeLength = (stream.Read(bytes, 0, readLength))) > 0)
                                {
                                    if (info.CancellationTokenSource.Token.IsCancellationRequested)
                                    {
                                        info.Listener.OnDownloadCancled(info);
                                    }

                                    fileStream.Write(bytes, 0, writeLength);
                                    downloadSize -= writeLength;
                                    info.DownloadSize += writeLength;

                                    info.Listener.OnDownloadProgress(info);
                                    if (downloadSize != 0 && downloadSize < readLength)
                                    {
                                        readLength = (int)downloadSize;
                                    }
                                    if (downloadSize <= 0)
                                    {
                                        break;
                                    }
                                }
                                stream.Flush();
                                stream.Close();
                            }
                        }
                        fileStream.Flush();
                        fileStream.Close();
                    }
                }
                var fileCount = _downloadingFiles.Count();
                if (_downloadingFiles[info.Url].FileLength < fileCount * _oneThreadSize)
                {
                    return;
                }
                await DownloadChunk(info);
            }
            catch (Exception ex)
            {
                Logger.LogHelper.Error("下载文件出错", ex);
                info.Exception = ex;
                info.Message = ex.Message;
                info.DownloadStatus = DownloadStatus.DownloadFailed;
                info.Listener.OnDownloadFailed(info);
                info.CancellationTokenSource.Cancel();
            }

        }

        private static async void DownloadUnKownSizeFile(DownloadInfo info, IDownloadManagerListener managerListener)
        {
            try
            {
                using (var httpClient = CreateHttpClient())
                {
                    var response =
                        await
                            httpClient.GetAsync(info.Url, HttpCompletionOption.ResponseHeadersRead,
                                info.CancellationTokenSource.Token);
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        var bytes = new byte[_bytesLength];
                        using (
                            var fileStream = new FileStream(Path.Combine(info.Url, info.Name), FileMode.Append,
                                FileAccess.Write))
                        {
                            int writeLength;
                            stream.Position = fileStream.Length;
                            while ((writeLength = (stream.Read(bytes, 0, _bytesLength))) > 0)
                            {
                                if (info.CancellationTokenSource.Token.IsCancellationRequested)
                                {
                                    info.DownloadStatus = DownloadStatus.UnDownload;
                                    info.Listener.OnDownloadCancled(info);
                                    break;
                                }

                                fileStream.Write(bytes, 0, writeLength);
                                info.DownloadSize += writeLength;
                                info.Listener.OnDownloadProgress(info);
                            }
                            fileStream.Flush();
                            fileStream.Close();
                            stream.Flush();
                            stream.Close();
                        }
                        if (info.DownloadStatus == DownloadStatus.Downloading)
                        {
                            info.DownloadStatus = DownloadStatus.Downloaded;
                            info.Listener.OnDownloadCompleted(info);
                            managerListener.OnDownloadCompleted(info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogHelper.Error("下载文件出错", ex);
                info.Exception = ex;
                info.Message = ex.Message;
                info.DownloadStatus = DownloadStatus.DownloadFailed;
                info.Listener.OnDownloadFailed(info);
            }
        }

        private static async Task<Tuple<string, long>> GetFileSize(DownloadInfo info)
        {
            using (var httpClient = CreateHttpClient())
            {
                httpClient.Timeout = _timeOut;
                var response =
                    await
                        httpClient.GetAsync(info.Url, HttpCompletionOption.ResponseHeadersRead,
                            info.CancellationTokenSource.Token);
                var contentLength = response.Content.Headers.ContentLength;
                if (contentLength == null)
                {
                    Logger.LogHelper.Info($"下载文件:{info.Name}({info.Url});无法获取文件长度");
                    return new Tuple<string, long>(response.Content.Headers.ContentDisposition?.FileName, 0);
                }
                return new Tuple<string, long>(response.Content.Headers.ContentDisposition?.FileName, contentLength.Value); ;
            }
        }

        private static void MergeFile(DownloadInfo info)
        {
            var path = Path.Combine(info.SavePath, info.Name.Replace(".", ""));
            //这里排序一定要正确，转成数字后排序（字符串会按1 10 11排序，默认10比2小）
            foreach (var filePath in Directory.GetFiles(path).OrderBy(Path.GetFileNameWithoutExtension))
            {
                var file = Directory.GetParent(path).FullName + @"\" + info.Name;

                using (var fs = File.Open(file, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    //int count = 0;
                    //while (count < 20 && Win32.IsOpen(filePath))
                    //{
                    //    await Task.Delay(200);
                    //    count++;
                    //}
                    var bytes = File.ReadAllBytes(filePath);//读取文件到字节数组
                    fs.Write(bytes, 0, bytes.Length);//写入文件
                    fs.Flush();
                    fs.Close();
                }
                //int sequence = 0;
                //while (sequence < 20 && Win32.IsOpen(filePath))
                //{
                //    await Task.Delay(200);
                //    sequence++;
                //}
                if (!Win32.IsOpen(filePath))
                {
                    File.Delete(filePath);
                }
            }
            if (!Directory.GetFiles(path).Any())
            {
                Directory.Delete(path);
            }
        }

        private static HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler()
            {
                //CookieContainer = cookieContainer,
                UseCookies = true,
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.GZip
                //UseProxy = true,
                //Proxy = new WebProxy("218.64.147.211", 9000)
            };
            var _httpClient = new HttpClient(handler) { Timeout = _timeOut };
            _httpClient.DefaultRequestHeaders.Add("Accept", _accept);
            _httpClient.DefaultRequestHeaders.Add("Accept-Language", _acceptLanguage);
            _httpClient.DefaultRequestHeaders.Add("Accept-Encoding", _acceptEncoding);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", _userAgent);
            _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            _httpClient.DefaultRequestHeaders.Add("Accept-Charset", _charset);

            return _httpClient;
        }

        public class FileInfo
        {
            public List<string> DownloadingFiles = new List<string>();

            public long FileLength;

            public long DownloadedLength;
        }
    }
}
