using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ApiService.ApiAddress;
using ApiService.Http;
using ApiService.Owin;
using Entity.Base;

namespace ApiService.User
{
    public class UserManager
    {

        public async Task<BaseResult<Entity.Sys.User>> Login(string name,string pwd)
        {
            var response = await Token.Get(name, pwd);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var token= await response.Content.ReadAsAsync<TokenResponse>();
                HttpClientUtil.SetToken(token.AccessToken);
                return await Get(name);
            }
            var error = await response.Content.ReadAsAsync<HttpError>();
            return new BaseResult<Entity.Sys.User>(null,false,"用户验证失败",error);
        }

        public async Task<BaseResult<Entity.Sys.User>> Get(string name)
        {
            var url = $"{UserApi.GetUserApi}{name}";
            var response = await HttpClientUtil.Instance.GetAsync(url);
            return Utils.HttpConvert<Entity.Sys.User>.ConvertHttpResponse(response, "Success", "用户名或密码错误");
            
        }



    }
}
