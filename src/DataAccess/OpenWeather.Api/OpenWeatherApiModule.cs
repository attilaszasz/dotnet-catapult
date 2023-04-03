using Autofac;
using OpenWeather.Api.Controllers;

namespace OpenWeather.Api
{
    public class OpenWeatherApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterModule<OpenWeatherModule>();

            builder.Register(c => Log.Logger)
                .As<Serilog.ILogger>()
                .SingleInstance();

            //NOTE: registering this, so we can resolve from tests
            builder.RegisterType<OpenWeatherController>();
        }
    }
}
