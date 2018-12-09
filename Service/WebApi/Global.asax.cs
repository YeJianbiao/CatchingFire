
//using System;
//using System.IO;
//using Utility.Logger;
//using WebApi.EF;
//using WebApi.Socket;

using System;
using System.IO;
using System.Web.Http;
using Swashbuckle.Application;
using Utility.Logger;
using WebApi.EF;
using WebApi.Socket;

namespace WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 
        /// </summary>
        protected void Application_Start()
        {
            //var configuration = GlobalConfiguration.Configuration;
            //WebApiConfig.Register(configuration);
            //configuration.EnableSwagger(x => x.SingleApiVersion("v1", "YeSystem Api")).EnableSwaggerUi();
            

            //DataContext.Connection();
            //LoggerUdpAppender.Start();
            //LogHelper.SetConfig(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config"));
            //LogHelper.Info("WebApi 开启...");
        }

        //protected  void GlobalConfiguration.Configure
    }
}
