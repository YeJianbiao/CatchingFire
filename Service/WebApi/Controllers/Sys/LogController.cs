using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApi.Business.Sys;
using WebApi.Controllers.Base;

namespace WebApi.Controllers.Sys
{
    [RoutePrefix("log")]
    public class LogController:BaseApiController
    {
        // GET: api/User
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<dynamic> Get()
        {
            var result = await new LogManager().GetAll();
            return result;
        }

        [Route("File")]
        [HttpGet]
        public async Task<dynamic> GetFileLog()
        {
            var result = await new LogManager().GetFileLogs();
            return result;
        }

    }
}