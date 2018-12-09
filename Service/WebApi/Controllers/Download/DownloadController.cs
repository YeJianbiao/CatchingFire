using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using WebApi.Business.Download;
using WebApi.Controllers.Base;

namespace WebApi.Controllers.Download
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("download")]
    public class DownloadController : BaseApiController
    {
        private const int BufferSize = 80 * 1024;

        private const string MimeType = "application/octet-stream";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [HttpGet, Route("getFile/{filePath}")]
        public HttpResponseMessage GetFileResponse(string filePath)
        {
            var bytes = HttpServerUtility.UrlTokenDecode(filePath);
            var path = HttpUtility.UrlDecode(bytes, Encoding.UTF8);
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            var fileName = Path.GetFileName(path);
            var fielStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            long fileLength = fielStream.Length;
            var fileInfo = GetFileByRequest(Request, fileLength);

            //获取剩余部分文件流
            var stream = new PartialContentFileStream(fielStream, fileInfo.From, fileInfo.To);
            var response = new HttpResponseMessage { Content = new StreamContent(stream, BufferSize) };
            SetResponseHeaders(response, fileInfo, fileLength, fileName);
            return response;
        }

        private FileInfo GetFileByRequest(HttpRequestMessage request, long entityLength)
        {
            var fileInfo = new FileInfo
            {
                From = 0,
                To = entityLength - 1,
                IsPartial = false,
                Length = entityLength
            };
            
            var rangeHeader = request.Headers.Range;
            if (rangeHeader != null && rangeHeader.Ranges.Count != 0)
            {
                if (rangeHeader.Ranges.Count > 1)
                {
                    throw new HttpResponseException(HttpStatusCode.RequestedRangeNotSatisfiable);
                }
                var range = rangeHeader.Ranges.First();
                if ((range.From.HasValue && range.From < 0) || (range.To.HasValue && range.To > entityLength - 1))
                {
                    throw new HttpResponseException(HttpStatusCode.RequestedRangeNotSatisfiable);
                }

                fileInfo.From = range.From ?? 0;
                fileInfo.To = range.To ?? entityLength - 1;
                fileInfo.IsPartial = true;
                fileInfo.Length = entityLength;
                if (range.From.HasValue && range.To.HasValue)
                {
                    fileInfo.Length = range.To.Value - range.From.Value + 1;
                }
                else if (range.From.HasValue)
                {
                    fileInfo.Length = entityLength - range.From.Value + 1;
                }
                else if (range.To.HasValue)
                {
                    fileInfo.Length = range.To.Value + 1;
                }
            }

            return fileInfo;
        }

        /// <summary>
        /// 设置响应头信息
        /// </summary>
        /// <param name="response"></param>
        /// <param name="fileInfo"></param>
        /// <param name="fileLength"></param>
        /// <param name="fileName"></param>
        private void SetResponseHeaders(HttpResponseMessage response, FileInfo fileInfo,
                                      long fileLength, string fileName)
        {
            response.Headers.AcceptRanges.Add("bytes");
            response.StatusCode = fileInfo.IsPartial ? HttpStatusCode.PartialContent
                                      : HttpStatusCode.OK;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeType);
            response.Content.Headers.ContentLength = fileInfo.Length;
            if (fileInfo.IsPartial)
            {
                response.Content.Headers.ContentRange
                    = new ContentRangeHeaderValue(fileInfo.From, fileInfo.To, fileLength);
            }
        }

        public class FileInfo
        {
            public long From;
            public long To;
            public bool IsPartial;
            public long Length;
        }
    }
}
