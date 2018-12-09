using System.Linq;
using System.Threading.Tasks;
using WebApi.EF;

namespace WebApi.Business.User
{
    public class UserManager:BaseManager<Entity.Sys.User>
    {

        internal async Task<bool> VerifyUser(string name,string password)
        {
            return await Task.Run((() =>
            {
                var user=  DataContext.context.Set<Entity.Sys.User>().FirstOrDefault(o => o.Name == name && o.Pwd == password);
                return user!=null;
            }));
        }

    }
}