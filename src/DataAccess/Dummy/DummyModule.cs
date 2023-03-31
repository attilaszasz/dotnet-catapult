using Autofac;
using Interfaces;

namespace Dummy
{
    public class DummyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            //NOTE: We have multiple implmentations of IWeatherSupplier, so we have to register them by name so we can resolve separately
            builder.RegisterType<DummyWeatherSupplier>()
                .Named<IWeatherSupplier>(DummyWeatherSupplier.Name);
        }
    }
}
