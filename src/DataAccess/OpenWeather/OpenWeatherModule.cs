using Autofac;
using Interfaces;
using OpenWeatherMap.Standard.Enums;
using OpenWeatherMap.Standard;
using ConfigurationService;
using Types;

namespace OpenWeather
{
    public class OpenWeatherModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterModule<ConfigurationServiceModule>();

            builder.Register(c => {
                var configuration = c.Resolve<IConfigurationService>();
                var _apiToken = configuration.GetString("OpenWeatherAPIToken");
                if (string.IsNullOrWhiteSpace(_apiToken)) throw new NullReferenceException("Please set the OpenWeatherAPIToken user secret");
                return new Current(_apiToken, WeatherUnits.Metric);
            })
                .As<Current>()
                .SingleInstance();

            builder.RegisterType<OpenWeatherAdapter>()
                .As<IOpenWeatherAdapter>();

            //NOTE: registering the supplier with metadata attached
            builder.RegisterType<OpenWeatherSupplier>()
                .As<IWeatherSupplier>()
                .WithMetadata<SupplierMetadata>(meta => meta.For(sm => sm.Name, OpenWeatherSupplier.Name))
                .InstancePerLifetimeScope();
        }
    }
}
