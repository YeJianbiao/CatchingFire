using System;
using System.IO;
using System.Web.Http;
using Microsoft.Owin;
using Owin;
using Swashbuckle.Application;
using Utility.Logger;
using WebApi;
using WebApi.EF;
using WebApi.Socket;

[assembly: OwinStartup(typeof(Startup))]
namespace WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            var configuration = new HttpConfiguration();
            WebApiConfig.Register(configuration);
            configuration.EnableSwagger(x => x.SingleApiVersion("v1", "YeSystem Api")).EnableSwaggerUi();



            app.UseWebApi(configuration);

            DataContext.Connection();
            LoggerUdpAppender.Start();
            LogHelper.SetConfig(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,  "log4net.config"));
            LogHelper.Info("WebApi 开启...");
        }
    }
}