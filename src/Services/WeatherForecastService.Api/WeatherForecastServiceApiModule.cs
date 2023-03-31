using Autofac;
using WeatherForecastService.Api.Controllers;

namespace WeatherForecastService.Api
{
    public class WeatherForecastServiceApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterModule<WeatherForecastServiceModule>();

            builder.Register(c => Log.Logger)
                .As<Serilog.ILogger>()
                .SingleInstance();

            //NOTE: registering this, so we can resolve from tests
            builder.RegisterType<WeatherForecastController>();
        }
    }
}
