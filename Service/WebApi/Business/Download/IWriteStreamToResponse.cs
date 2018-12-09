using System.IO;
using System.Net;
using System.Net.Http;

namespace WebApi.Business.Download
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWriteStreamToResponse<T>
    {
        /// <summary>
        /// 
        /// </summary>
        T Source { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputStream"></param>
        /// <param name="content"></param>
        /// <param name="context"></param>
        void WriteToStream(Stream outputStream, HttpContent content, TransportContext context);
    }
}
