/**
* 命名空间: ApiService.Utils 
* 类    名： HttpConvert
* 创 建 人：lenovo
* 创建时间：2018/8/12 18:54:42
* 用    途：
* 
* 
* Copyright (c) . All rights reserved. 
* 版权所有：　　　　　　　　　　　　　　              
*/

using System.Net;
using System.Net.Http;
using System.Web.Http;
using Entity.Base;
using Entity.Sys;
using Newtonsoft.Json;

namespace ApiService.Utils
{
    public class HttpConvert<T> where T:class 
    {
        public static BaseResult<T> ConvertHttpResponse(HttpResponseMessage response,string successMsg,string errorMsg)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content.ReadAsAsync<T>().Result;
                //var json = JsonConvert.SerializeObject(content);
                //var menus = JsonConvert.DeserializeObject<T>(json);
                return new BaseResult<T>(content, true, successMsg, null);
            }

            var error = response.Content.ReadAsAsync<HttpError>().Result;
            return new BaseResult<T>(null, false, errorMsg, error);
        }

    }
}
