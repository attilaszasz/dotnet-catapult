using Autofac;
using Caching;
using ConfigurationService;
using DaprStateStore;
using Interfaces;
using Memory;
using Redis;
using Types;
using Types.Caching;

namespace Dummy
{
    public class DummyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterModule<ConfigurationServiceModule>();
            builder.RegisterModule<MemoryCacheModule>();
            builder.RegisterModule<DaprStateStoreCacheModule>();

            builder.RegisterType<WeatherSupplierCache>();
            builder.RegisterType<DummyWeatherSupplier>();

            builder.Register(c =>
            {
                var config = c.Resolve<IConfigurationService>();
                var cacheSettings = config.Get<CacheSettings>("Caching");
                IWeatherSupplier supplier = c.Resolve<DummyWeatherSupplier>();

                if (cacheSettings.Shared)
                {
                    supplier = c.Resolve<WeatherSupplierCache>(TypedParameter.From(supplier), new NamedParameter("cache", c.ResolveKeyed<ICache>(CacheType.Shared)));
                }

                if (cacheSettings.Local)
                {
                    supplier = c.Resolve<WeatherSupplierCache>(TypedParameter.From(supplier), new NamedParameter("cache", c.ResolveKeyed<ICache>(CacheType.Local)));
                }

                return supplier;
            })
                .As<IWeatherSupplier>()
                .WithMetadata<SupplierMetadata>(m => m.For(sm => sm.Name, DummyWeatherSupplier.Name));
        }
    }
}
