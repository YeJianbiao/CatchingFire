using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Filters;

namespace WebApi.Filter
{
    /// <summary>
    /// 异常拦截器
    /// </summary>
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private HttpResponseMessage GetResponse(string message)
        {

            return new HttpResponseMessage()
            {
                Content = new ObjectContent<string>(
                    message,
                    new JsonMediaTypeFormatter(),
                    "application/json"
                    )
            };
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var message = actionExecutedContext.Exception.Message; 

            
            if (actionExecutedContext.Response == null)
            {
                actionExecutedContext.Response = GetResponse( message);
            }

            //记录错误日志
            //LogUtils.ErrorLog(SecurityHelper.GetUserIp(), actionExecutedContext.Exception);

            base.OnException(actionExecutedContext);
        }
    }
}