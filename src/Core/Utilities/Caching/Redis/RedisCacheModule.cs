using Autofac;
using ConfigurationService;
using Interfaces;
using StackExchange.Redis;
using Types.Caching;

namespace Redis
{
    public class RedisCacheModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterModule<ConfigurationServiceModule>();

            builder.RegisterType<RedisConfig>();

            builder.Register(c => {
                var options = c.Resolve<RedisConfig>();
                var redisOptions = new ConfigurationOptions()
                {
                    Ssl = options.UseSSL,
                    Password = options.Password,
                    AllowAdmin = true,
                    ConnectTimeout = options.ConnectTimeout
                };
                foreach (var endpoint in options.Endpoints)
                {
                    redisOptions.EndPoints.Add(endpoint);
                }

                return redisOptions;
            })
                .As<ConfigurationOptions>()
                .SingleInstance();

            builder.RegisterType<RedisCache>()
                .As<ICache>()
                .Keyed<ICache>(CacheType.Shared)
                .SingleInstance();
        }
    }
}
