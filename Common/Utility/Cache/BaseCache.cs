using System.Web.Caching;

namespace Utility.Cache
{
    internal abstract class BaseCache : ICache
    {
        private int _time;

        public int ExpirationSenconds
        {
            get { return _time; }
            set { _time = value; }
        }

        public int ExpirationMinutes
        {
            get { return _time / 60; }
            set { _time = value * 60; }
        }

        public int ExpirationHours
        {
            get { return _time / 60 / 60; }
            set { _time = value * 60 * 60; }
        }

        public string DependencyFilePath { get; set; }

        protected BaseCache()
        {
            _time = 3600;
            DependencyFilePath = null;
        }

        public bool Contains(string key)
        {
            bool result;
            if (string.IsNullOrEmpty(key))
            {
                result = false;
            }
            else
            {
                key = key.Trim().ToLower();
                result = ContainsKey(key);
            }
            return result;
        }

        protected abstract bool ContainsKey(string key);

        public void Add(string key, object target, int time)
        {
            if (!string.IsNullOrEmpty(key) && target != null)
            {
                key = key.Trim().ToLower();
                if (string.IsNullOrEmpty(DependencyFilePath))
                {
                    AddCache(key, target, time, null);
                }
                else
                {
                    CacheDependency dependency = new CacheDependency(DependencyFilePath);
                    AddCache(key, target, time, dependency);
                }
            }
        }

        protected abstract void AddCache(string key, object target, int time, CacheDependency dependency);

        public T Get<T>(string key) where T : class
        {
            T result;
            if (string.IsNullOrEmpty(key))
            {
                result = default(T);
            }
            else
            {
                key = key.Trim().ToLower();
                result = GetCache<T>(key);
            }
            return result;
        }

        protected abstract T GetCache<T>(string key) where T : class;


        public void Remove(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                key = key.Trim().ToLower();
                RemoveCache(key);
            }
        }

        protected abstract void RemoveCache(string key);

        public abstract void Clear();
    }
}
