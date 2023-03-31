using Autofac;
using Interfaces;
using Types.Caching;

namespace Memory
{
    public class MemoryCacheModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => System.Runtime.Caching.MemoryCache.Default).As<System.Runtime.Caching.MemoryCache>().SingleInstance();

            builder.RegisterType<MemoryCache>()
                .As<ICache>()
                .Keyed<ICache>(CacheType.Local)
                .SingleInstance();
        }
    }
}
