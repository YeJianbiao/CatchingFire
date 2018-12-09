using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Utility.Logger;

namespace WebApi.Socket
{
    /// <summary>
    /// 
    /// </summary>
    public class LoggerUdpAppender
    {

        /// <summary>
        /// 
        /// </summary>
        public static void Start()
        {
            Task.Factory.StartNew(() =>
            {
                var address = IPAddress.Parse("127.0.0.1");
                var remoteEndPoint = new IPEndPoint(address, 0);

                try
                {
                    var udpClient = new UdpClient(8082);

                    while (true)
                    {
                        var buffer = udpClient.Receive(ref remoteEndPoint);
                        var loggingEvent = Encoding.ASCII.GetString(buffer);
                        
                        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "Client", "Fatal");
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        var fileName = Path.Combine(filePath, $"{DateTime.Now.ToString("yyyyMMdd")}.txt");
                        File.AppendAllText(fileName, loggingEvent,Encoding.ASCII);
                    }
                }
                catch (Exception e)
                {
                    LogHelper.Fatal($"接收客户端日志出错：{e.Message}", e);
                }
            });

        }

    }
}