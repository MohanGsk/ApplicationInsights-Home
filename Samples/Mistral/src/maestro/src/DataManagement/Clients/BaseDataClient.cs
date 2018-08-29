using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.ApplicationInsights.Ingestion.Maestro
{
    /** Common base class of all data clients. Provides common caching mechanisms and helper methods for inheritors to use caches. */
    public abstract class BaseDataClient 
    {
        private string clientId = Guid.NewGuid().ToString() + "_";
        private List<IDataCache>[] dataCaches = new List<IDataCache>[] {
            new List<IDataCache>(), new List<IDataCache>(), new List<IDataCache>()
        };


        /** Add a cache to this data client. Caches are read from in order of DataCacheTier priority. */
        public void RegisterCache(IDataCache cache, DataCacheTier priority)
        {
            dataCaches[(int)priority].Add(cache);
        }

        /** Read a value from the registered caches. Consider using CacheWrap instead. */
        protected async Task<T> GetFromCache<T>(string key)
        {
            key = clientId + key;
            for(int i = 0; i < dataCaches.Length; i++)
            {
                foreach(IDataCache cache in dataCaches[i])
                {
                    T ret = await cache.Get<T>(key);
                    if (!EqualityComparer<T>.Default.Equals(ret, default(T)))
                    {
                        return ret;
                    }
                }
            }

            return default(T);
        }

        /** Store a value into all registered caches. Consider using CacheWrap instead. */
        protected async Task<bool> StoreInCache(string key, object value)
        {
            key = clientId + key;
            bool success = true;
            for(int i = 0; i < dataCaches.Length; i++)
            {
                foreach(IDataCache cache in dataCaches[i])
                {
                    bool ret = await cache.Set(key, value);
                    if (!ret)
                    {
                        success = false;
                    }
                }
            }

            return success;
        }

        /** 
         * Returns a value from registered caches, if the key exists in any caches.
         * If no value is stored, {action} is executed and the result returned and stored into all caches.
         */
        protected async Task<T> CacheWrap<T>(string key, Func<Task<T>> action)
        {
            T cacheItem = await this.GetFromCache<T>(key);
            if (EqualityComparer<T>.Default.Equals(cacheItem, default(T)))
            {
                cacheItem = await action();
                await this.StoreInCache(key, cacheItem);
            }
            return cacheItem;
        }
    }
}