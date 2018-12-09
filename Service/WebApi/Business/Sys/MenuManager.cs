
using System.Threading.Tasks;
using Entity.Sys;
using WebApi.EF;

namespace WebApi.Business.Sys
{
    /// <summary>
    /// 
    /// </summary>
    public class MenuManager : BaseManager<Menu>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<dynamic> GetMenusByUser(string name)
        {
            return await Task.Run((() =>
            {
                var result = DataContext.context.Set<Menu>();
                return result;
            }));
        }

    }
}