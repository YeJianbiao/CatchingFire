
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Controllers.Base;

namespace WebApi.Controllers.User
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("user")]
    public class UserController : BaseApiController
    {
        // GET: api/User
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<dynamic> Get()
        {
            return await new Business.User.UserManager().GetAll();
        }

        // GET: api/User/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByUser/{name}")]
        public async Task<dynamic> GetByUuser(string name)
        {
            var result = await new Business.User.UserManager().GetFirstOrDefault(o => o.Name == name);
            return result;
        }

        // POST: api/User
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public void Post(dynamic value)
        {
        }

        // PUT: api/User/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut]
        public void Put(int id, dynamic value)
        {
        }

        // DELETE: api/User/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        [HttpDelete]
        public void DeleteByName(string name)
        {

        }
    }
}
