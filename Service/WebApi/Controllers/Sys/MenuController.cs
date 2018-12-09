using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Business.Sys;
using WebApi.Controllers.Base;

namespace WebApi.Controllers.Sys
{
    /// <summary>
    /// 
    /// </summary>
    //[Authorize]
    [RoutePrefix("menu")]
    public class MenuController : BaseApiController
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
            var result = await new MenuManager().GetAll();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("GetByUser/{user}")]
        [HttpGet]
        public async Task<dynamic> GetByUser(string user)
        {
            var result = await new MenuManager().GetMenusByUser(user);
            return result;
        }

        // POST: api/User
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [Route("")]
        [HttpPost]
        public void Post(dynamic value)
        {
        }

        // DELETE: api/User/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public void Delete(int id)
        {
        }

    }
}
