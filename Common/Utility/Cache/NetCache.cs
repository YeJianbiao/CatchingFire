
using System.Web;
using System.Web.Caching;

namespace Utility.Cache
{
    internal class NetCache : BaseCache
    {
        protected override bool ContainsKey(string key)
        {
            bool result;
            if (HttpContext.Current == null)
            {
                result = (HttpRuntime.Cache.Get(key) != null);
            }
            else
            {
                result = (HttpContext.Current.Cache.Get(key) != null);
            }
            return result;
        }

        protected override void AddCache(string key, object target, int time, CacheDependency dependency)
        {
            if (HttpContext.Current == null)
            {
                AddCache_CS(key, target, time, dependency);
            }
            else
            {
                AddCache_BS(key, target, time, dependency);
            }
        }

        private void AddCache_CS(string key, object target, int time, CacheDependency dependency)
        {
            if (dependency == null)
            {
                HttpRuntime.Cache.Insert(key, target, null, System.DateTime.Now.AddSeconds(time), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            else
            {
                HttpRuntime.Cache.Insert(key, target, dependency, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
        }

        private void AddCache_BS(string key, object target, int time, CacheDependency dependency)
        {
            if (dependency == null)
            {
                HttpContext.Current.Cache.Insert(key, target, null, System.DateTime.Now.AddSeconds(time), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            else
            {
                HttpContext.Current.Cache.Insert(key, target, dependency, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
        }

        protected override T GetCache<T>(string key)
        {
            T result;
            if (HttpContext.Current == null)
            {
                result = (HttpRuntime.Cache.Get(key) as T);
            }
            else
            {
                result = (HttpContext.Current.Cache.Get(key) as T);
            }
            return result;
        }

        protected override void RemoveCache(string key)
        {
            if (HttpContext.Current == null)
            {
                HttpRuntime.Cache.Remove(key);
                return;
            }
            HttpContext.Current.Cache.Remove(key);
        }

        public override void Clear()
        {
            if (HttpContext.Current == null)
            {
                System.Collections.IDictionaryEnumerator enumerator = HttpRuntime.Cache.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    HttpRuntime.Cache.Remove(enumerator.Key.ToString());
                }
            }
            else
            {
                System.Collections.IDictionaryEnumerator enumerator = HttpContext.Current.Cache.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    HttpContext.Current.Cache.Remove(enumerator.Key.ToString());
                }
            }
        }
    }
}
