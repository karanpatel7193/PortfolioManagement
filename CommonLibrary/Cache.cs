using System.Runtime.Caching;
using System;

namespace CommonLibrary
{
    public class Cache
    {
        private static ObjectCache cache = MemoryCache.Default;
        public static ObjectCache MyCache
        {
            get
            {
                if (cache == null)
                    cache = MemoryCache.Default;
                return cache;
            }
        }

        public static void SetCache(string Key, object Value)
        {
            CacheItemPolicy Policy = new CacheItemPolicy();
            Policy.AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration;
            MyCache.Set(Key, Value, Policy);
        }

        public static void SetCache(string Key, object Value, double Seconds)
        {
            CacheItemPolicy Policy = new CacheItemPolicy();
            Policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(Seconds);
            MyCache.Set(Key, Value, Policy);
        }

        public static void SetCacheItem(string Key, double Seconds)
        {
            CacheItem cacheItem = MyCache.GetCacheItem(Key);
            CacheItemPolicy Policy = new CacheItemPolicy();
            Policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(Seconds);
            MyCache.Set(cacheItem, Policy);
        }
    }
}