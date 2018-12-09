using System;
using System.Threading;

namespace Utility.Sys
{
    public static class Singleton<TItem> where TItem : class, new()
    {
        private static TItem _Instance = default(TItem);

        public static TItem GetInstance()
        {
            if (_Instance == null)
            {
                Interlocked.CompareExchange(ref _Instance, Activator.CreateInstance<TItem>(), default(TItem));
            }
            return _Instance;
        }
    }
}
