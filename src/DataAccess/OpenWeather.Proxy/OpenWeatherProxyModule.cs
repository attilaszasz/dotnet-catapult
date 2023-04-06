using Autofac;
using Dapr.Client;
using Interfaces;
using Types;

namespace OpenWeather.Proxy
{
    public class OpenWeatherProxyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => new DaprClientBuilder().Build())
                .As<DaprClient>()
                .IfNotRegistered(typeof(DaprClient))
                .SingleInstance();

            builder.RegisterType<OpenWeatherSupplierProxy>()
                .As<IWeatherSupplier>()
                .WithMetadata<SupplierMetadata>(m => m.For(sm => sm.Name, Constants.Suppliers.OpenWeather))
                .InstancePerLifetimeScope();
        }
    }
}
