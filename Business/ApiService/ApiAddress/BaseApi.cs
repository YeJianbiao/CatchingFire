

namespace ApiService.ApiAddress
{
    public class BaseApi
    {

        private const string BaseUrl = "http://localhost:1680";

        public static string GetDomain()
        {
            return BaseUrl;
        }

    }
}
