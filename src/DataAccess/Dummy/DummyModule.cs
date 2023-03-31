using Autofac;
using Interfaces;
using Types;

namespace Dummy
{
    public class DummyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            //NOTE: registering the supplier with metadata attached
            builder.RegisterType<DummyWeatherSupplier>()
                .As<IWeatherSupplier>()
                .WithMetadata<SupplierMetadata>(meta => meta.For(sm => sm.Name, DummyWeatherSupplier.Name))
                .InstancePerLifetimeScope();
        }
    }
}
