using Autofac;
using Dummy.Proxy;
using Interfaces;
using OpenWeather.Proxy;

namespace WeatherForecastService
{
    public class WeatherForecastServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterModule<OpenWeatherProxyModule>();
            builder.RegisterModule<DummyProxyModule>();

            //NOTE: no more messing around with parameters, just explicitely register
            builder.RegisterType<WeatherForecastService>()
                .As<IWeatherForecastService>();
        }
    }
}
