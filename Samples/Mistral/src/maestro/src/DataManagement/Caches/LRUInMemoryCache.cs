using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.ApplicationInsights.Ingestion.Maestro
{
    /** A data cache that retains items in local memory. */
    public class LRUInMemoryCache : IDataCache
    {
        private int maxSize;
        private Dictionary<string, object> cache = new Dictionary<string, object>();
        private LinkedList<string> recencyList = new LinkedList<string>();

        private static SemaphoreSlim semaphore = new SemaphoreSlim(1,1);

        public LRUInMemoryCache(int maxItems)
        {
            maxSize = maxItems;
        }

        public Task<T> Get<T>(string key)
        {
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
            object value;

            if (cache.TryGetValue(key, out value))
            {
                tcs.SetResult((T)cache[key]);
            }
            else
            {
                tcs.SetResult(default(T));
            }
            
            return tcs.Task;
        }

        public async Task<bool> Set(string key, object value)
        {
            await semaphore.WaitAsync();

            if (cache.ContainsKey(key))
            {
                recencyList.Remove(key);
            }

            while (cache.Count > maxSize)
            {
                cache.Remove(recencyList.Last.Value);
                recencyList.RemoveLast();
            }

            cache[key] = value;
            recencyList.AddFirst(key);       

            semaphore.Release();
            return true;
        }
    }
}