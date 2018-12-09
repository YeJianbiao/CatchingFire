using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Business.Sys;

namespace WebApi.Controllers.Sys
{
    [Authorize]
    public class DictController : ApiController
    {
        // GET: api/Dict
        public async Task<dynamic> Get()
        {
            return await new DictManager().GetAll();
        }

        // GET: api/Dict/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Dict
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Dict/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Dict/5
        public void Delete(int id)
        {
        }
    }
}
