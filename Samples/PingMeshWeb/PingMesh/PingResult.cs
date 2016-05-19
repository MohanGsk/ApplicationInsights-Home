namespace PingMesh
{
    /// <summary>
    /// PingMesh perf counters results definition
    /// </summary>
    public class PingResult
    {
        public string EndPoint { get; set; }
        public long Value { get; set; }
        public int IsSuccess { get; set; }
    }
}
