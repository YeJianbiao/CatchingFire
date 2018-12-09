using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using ApiService.ApiAddress;

namespace Utility.Http
{
    public class HttpClientUtil
    {

        private static HttpClient _httpClient;
        private static readonly TimeSpan _timeOut = TimeSpan.FromSeconds(15);
        private static string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.87 Safari/537.36";
        private static string _acceptLanguage = "zh-CN,zh;q=0.8";
        private static string _accept = "text/html, application/xhtml+xml, */*";
        private static string _acceptEncoding = "gzip, deflate";
        private static string _charset = "utf-8";

        public string UserAgent { set { ChangeHeaders("User-Agent", value); } }
        public string AcceptLanguage { set { ChangeHeaders("Accept-Language", value); } }
        public string Accept { set { ChangeHeaders("Accept", value); } }
        public string Cookie { get; set; }
        public string AcceptEncoding { set { ChangeHeaders("Accept-Encoding", value); } }
        public string ContentType { get; set; } = "application/x-www-form-urlencoded";
        public string Charset { set { ChangeHeaders("Accept-Charset", value); } }

        public static HttpClient Instance => _httpClient ?? (_httpClient = new HttpClient());

        public HttpClientUtil()
        {
            if (_httpClient != null)
            {
                return;
            }
            _httpClient = CreateHttpClient();
        }

        public static void SetToken(string token)
        {
            Instance.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private void ChangeHeaders(string name, string newHeaders)
        {
            _httpClient.DefaultRequestHeaders.Remove(name);
            _httpClient.DefaultRequestHeaders.Add(name, newHeaders);
        }

        public static HttpClient CreateHttpClient()
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
            var httpClient = new HttpClient(handler) { BaseAddress = new Uri(BaseApi.GetDomain()),Timeout = _timeOut};
            httpClient.DefaultRequestHeaders.Add("Accept", _accept);
            httpClient.DefaultRequestHeaders.Add("Accept-Language", _acceptLanguage);
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", _acceptEncoding);
            httpClient.DefaultRequestHeaders.Add("User-Agent", _userAgent);
            httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            httpClient.DefaultRequestHeaders.Add("Accept-Charset", _charset);
            return httpClient;
        }

    }
}
