using Autofac;
using Dapr.Client;
using Dapr.Extensions.Configuration;
using Interfaces;
using Microsoft.Extensions.Configuration;
using Types;

namespace ConfigurationService
{
    public class ConfigurationServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => new DaprClientBuilder().Build())
                .As<DaprClient>()
                .IfNotRegistered(typeof(DaprClient))
                .SingleInstance();

            //NOTE: using Dapr secret store
            builder.Register(c => new ConfigurationBuilder().AddDaprSecretStore(Constants.SecretStoreName, c.Resolve<DaprClient>(), TimeSpan.FromSeconds(30)).Build())
                .As<IConfigurationRoot>()
                .SingleInstance();

            builder.RegisterType<ConfigurationService>()
                .As<IConfigurationService>();
        }
    }
}
