using Autofac;
using Dummy;
using Interfaces;
using OpenWeather;

namespace WeatherForecastService
{
    public class WeatherForecastServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterModule<OpenWeatherModule>();
            builder.RegisterModule<DummyModule>();

            builder.Register(c => new WeatherForecastService(
                    c.ResolveNamed<IWeatherSupplier>(DummyWeatherSupplier.Name),
                    c.ResolveNamed<IWeatherSupplier>(OpenWeatherSupplier.Name),
                    c.Resolve<ILogger>()))
                .As<IWeatherForecastService>();
        }
    }
}
