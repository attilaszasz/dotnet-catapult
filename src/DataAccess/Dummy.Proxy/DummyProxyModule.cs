using Autofac;
using Dapr.Client;
using Interfaces;
using Types;

namespace Dummy.Proxy
{
    public class DummyProxyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => new DaprClientBuilder().Build())
                .As<DaprClient>()
                .SingleInstance();

            builder.RegisterType<DummyWeatherSupplierProxy>()
                .As<IWeatherSupplier>()
                .WithMetadata<SupplierMetadata>(m => m.For(sm => sm.Name, "Dummy"))
                .InstancePerLifetimeScope();
        }
    }
}