using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ApiService.Owin
{
    public class Token
    {

        private const string _clientId = "YeJianbiao";
        private const string _clientSecret = "kfwgisfi";

        public static async Task<HttpResponseMessage> Get(string userName, string password)
        {
            
            var parameters = new Dictionary<string, string> {{"grant_type", "password"}};

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                parameters.Add("username", userName);
                parameters.Add("password", password);
            }

            Http.HttpClientUtil.Instance.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes(_clientId + ":" + _clientSecret)));
            var response = await Http.HttpClientUtil.Instance.PostAsync("/token", new FormUrlEncodedContent(parameters));
            return response;
        }

        public static async Task<HttpResponseMessage> Refresh(string refreshToken)
        {
            var parameters = new Dictionary<string, string> { { "grant_type", "refreshToken" }, { "refresh_token",refreshToken } };

            var response = await Http.HttpClientUtil.Instance.PostAsync("/token", new FormUrlEncodedContent(parameters));
            return response;
        }

    }
}
