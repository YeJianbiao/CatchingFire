using System.Linq;
using System.Threading.Tasks;
using Entity.Sys;
using WebApi.EF;

namespace WebApi.Business.Sys
{
    public class DictManager:BaseManager<Dict>
    {

        internal async Task<Dict> GetDictByCode(string code)
        {
            return await Task.Run((() =>
            {
                var dict = DataContext.context.Set<Entity.Sys.Dict>().FirstOrDefault(o => o.Code == code);
                return dict;
            }));
        }

    }
}