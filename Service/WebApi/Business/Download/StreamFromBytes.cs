using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Utility.Logger;

namespace WebApi.Business.Download
{
    /// <summary>
    /// 
    /// </summary>
    public class StreamFromBytes : IWriteStreamToResponse<Stream>
    {
        
        public Stream Source { get; set; }

        public async void WriteToStream(Stream outputStream, HttpContent content, TransportContext context)
        {
            try
            {
                var buffer = new byte[1024 * 1024 * 2];
                using (var video = Source)
                {
                    var length = (int)video.Length;
                    var bytesRead = 1;

                    while (length > 0 && bytesRead > 0)
                    {
                        bytesRead = video.Read(buffer, 0, Math.Min(length, buffer.Length));
                        await outputStream.WriteAsync(buffer, 0, bytesRead);
                        length -= bytesRead;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message,ex);
            }
            finally
            {
                Source.Close();
                outputStream.Close();
            }
        }
    }
}