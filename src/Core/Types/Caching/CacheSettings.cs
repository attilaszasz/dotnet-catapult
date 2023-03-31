
namespace Types.Caching
{
    public class CacheSettings
    {
        public bool Local { get; set; }
        public bool Shared { get; set; }
        public Duration Duration { get; set; } = new Duration();
    }
}
