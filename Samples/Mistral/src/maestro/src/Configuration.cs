namespace Microsoft.ApplicationInsights.Ingestion.Maestro
{
    public static class Configuration {
        public static string OUTGOING_USERAGENT = "Maestro/1.0";
        public static int MAX_LRU_CACHE_SIZE = 1000;
        public static bool ENABLE_ERROR_TRACES = true;
        public static bool ENABLE_REQUEST_LOGGING = true;
        public static bool ENABLE_GZIP_DECOMPRESSION = true;
    }
}