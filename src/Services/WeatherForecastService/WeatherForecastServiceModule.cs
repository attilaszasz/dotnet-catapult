﻿using Autofac;
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

            //NOTE: no more messing around with parameters, just explicitely register
            builder.RegisterType<WeatherForecastService>()
                .As<IWeatherForecastService>();
        }
    }
}
