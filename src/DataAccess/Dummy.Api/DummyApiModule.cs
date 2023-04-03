using Autofac;
using Dummy.Api.Controllers;

namespace Dummy.Api
{
    public class DummyApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterModule<DummyModule>();

            builder.Register(c => Log.Logger)
                .As<Serilog.ILogger>()
                .SingleInstance();

            //NOTE: registering this, so we can resolve from tests
            builder.RegisterType<DummyController>();
        }
    }
}
