using Autofac;
using Dapr.Client;
using Interfaces;
using Types.Caching;

namespace DaprStateStore
{
    public class DaprStateStoreCacheModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => new DaprClientBuilder().Build())
                .As<DaprClient>()
                .SingleInstance();

            builder.RegisterType<DaprStateStoreCache>()
                .As<ICache>()
                .Keyed<ICache>(CacheType.Shared)
                .SingleInstance();
        }
    }
}
