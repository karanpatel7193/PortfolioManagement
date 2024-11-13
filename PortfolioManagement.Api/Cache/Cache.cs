using System;
using System.Runtime.Caching;

namespace PortfolioManagement.Api.Cache
{
    public class Cache
    {
        private static ObjectCache objCache = MemoryCache.Default;
        public static ObjectCache MyCache
        {
            get 
            {
                if(objCache == null)
                    objCache = MemoryCache.Default;
                return objCache;
            }
        }

        public static void SetCache(string Key, object Value)
        {
            CacheItemPolicy Policy = new CacheItemPolicy();
            Policy.AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration;
            MyCache.Set(Key, Value, Policy);
        }

        public static void SetCache(string Key, object Value, int Seconds)
        {
            CacheItemPolicy Policy = new CacheItemPolicy();
            Policy.AbsoluteExpiration = DateTime.UtcNow.AddSeconds(Seconds);
            MyCache.Set(Key, Value, Policy);
        }
    }
}