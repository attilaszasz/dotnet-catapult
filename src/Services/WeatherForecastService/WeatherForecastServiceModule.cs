using Autofac;
using Dummy;
using Dummy.Proxy;
using Interfaces;
using OpenWeather;
using OpenWeather.Proxy;

namespace WeatherForecastService
{
    public class WeatherForecastServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterModule<OpenWeatherModule>();
            builder.RegisterModule<DummyModule>();

            //NOTE: no more messing around with parameters, just explicitely register
            builder.RegisterType<WeatherForecastService>()
                .As<IWeatherForecastService>();
        }
    }
}
