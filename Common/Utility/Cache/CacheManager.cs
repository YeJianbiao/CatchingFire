using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Cache
{
    public static class CacheManager
    {

        private static readonly ICache _cacheService;

        static CacheManager()
        {
            _cacheService = new NetCache();
        }

        public static void AddObject<TInput>(string key, TInput ob, int duration) where TInput : class
        {
            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrEmpty(key))
            {
                _cacheService.Add(key, ob, duration);
            }
        }

        public static void Update<TInput>(string key, TInput ob, int duration) where TInput : class
        {
            if (_cacheService.Contains(key))
            {
                _cacheService.Remove(key);
                AddObject(key, ob, duration);
            }

        }

        public static TResult GetObject<TResult>(string key) where TResult : class
        {
            var result = _cacheService.Contains(key) ? _cacheService.Get<TResult>(key) : default(TResult);
            return result;
        }

        public static TResult GetCache<TResult>(string key, int duration, System.Func<TResult> func) where TResult : class
        {
            TResult result;
            if (_cacheService.Contains(key))
            {
                TResult tResult = _cacheService.Get<TResult>(key);
                if (tResult != null)
                {
                    result = tResult;
                    return result;
                }
            }
            TResult tResult2 = func();
            if (tResult2 != null)
            {
                _cacheService.Add(key, tResult2, duration);
            }
            result = tResult2;
            return result;
        }

        public static TResult GetCache<T1, TResult>(string key, int duration, System.Func<T1, TResult> func, T1 arg) where TResult : class
        {
            TResult result;
            if (_cacheService.Contains(key))
            {
                TResult tResult = _cacheService.Get<TResult>(key);
                if (tResult != null)
                {
                    result = tResult;
                    return result;
                }
            }
            TResult tResult2 = func(arg);
            if (tResult2 != null)
            {
                _cacheService.Add(key, tResult2, duration);
            }
            result = tResult2;
            return result;
        }

        public static TResult GetCache<T1, T2, TResult>(string key, int duration, System.Func<T1, T2, TResult> func, T1 arg1, T2 arg2) where TResult : class
        {
            TResult result;
            if (_cacheService.Contains(key))
            {
                TResult tResult = _cacheService.Get<TResult>(key);
                if (tResult != null)
                {
                    result = tResult;
                    return result;
                }
            }
            TResult tResult2 = func(arg1, arg2);
            if (tResult2 != null)
            {
                _cacheService.Add(key, tResult2, duration);
            }
            result = tResult2;
            return result;
        }

        public static TResult GetCache<T1, T2, T3, TResult>(string key, int duration, System.Func<T1, T2, T3, TResult> func, T1 arg1, T2 arg2, T3 arg3) where TResult : class
        {
            TResult result;
            if (_cacheService.Contains(key))
            {
                TResult tResult = _cacheService.Get<TResult>(key);
                if (tResult != null)
                {
                    result = tResult;
                    return result;
                }
            }
            TResult tResult2 = func(arg1, arg2, arg3);
            if (tResult2 != null)
            {
                _cacheService.Add(key, tResult2, duration);
            }
            result = tResult2;
            return result;
        }

        public static void Remove(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                if (_cacheService.Contains(key))
                {
                    _cacheService.Remove(key);
                }
            }
        }

        public static void Clear()
        {
            _cacheService.Clear();
        }


    }
}
