using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApi.EF;

namespace WebApi.Business
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseManager<T> where T:class
    {

        internal async Task<dynamic> GetAll()
        {
            return await Task.Run((() =>
            {
                var result = DataContext.context.Set<T>();
                return result;
            }));
        }

        internal async Task<dynamic> GetByFunc(Expression<Func<T, bool>> exp)
        {
            return await Task.Run((() =>
            {
                var result = DataContext.context.Set<T>().Where(exp).ToList();
                return result;
            }));
        }

        internal async Task<dynamic> GetFirstOrDefault(Expression<Func<T, bool>> exp)
        {
            return await Task.Run((() =>
            {
                var result = DataContext.context.Set<T>().FirstOrDefault(exp);
                return result;
            }));
        }

    }
}