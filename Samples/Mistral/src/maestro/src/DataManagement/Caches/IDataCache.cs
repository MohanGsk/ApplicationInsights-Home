using System.Threading.Tasks;

namespace Microsoft.ApplicationInsights.Ingestion.Maestro
{
    public interface IDataCache 
    {
        Task<T> Get<T>(string key);
        Task<bool> Set(string key, object value);
    }

    public enum DataCacheTier 
    {
        One,
        Two,
        Three
    }
}