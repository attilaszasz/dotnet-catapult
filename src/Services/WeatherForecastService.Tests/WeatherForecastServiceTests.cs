using Autofac;
using Dummy;
using Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using OpenWeather;
using TestHelpers;
using Types;

namespace WeatherForecastService.Tests
{
    [TestClass]
    public class WeatherForecastServiceTests
    {
        [TestMethod]
        public async Task TestSingleResult()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<WeatherForecastServiceModule>();
            //NOTE: need to register the logger, because it is not registered in the WeatherServiceModule
            var mockLogger = new Mock<ILogger>();
            builder.Register(c => mockLogger.Object).As<ILogger>();

            var _container = builder.Build();

            var service = _container!.Resolve<IWeatherForecastService>();

            var results = await service.GetWeatherForecast(Parameters.TarguMures, DummyWeatherSupplier.Name);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());
        }

        [TestMethod]
        public async Task WeatherForecastServiceInIsolation()
        {
            var mockDummySupplier = new Mock<IWeatherSupplier>();
            var mockOpenWeatherSupplier = new Mock<IWeatherSupplier>();

            mockDummySupplier.Setup(s => s.GetWeatherForecast(Parameters.TarguMures)).Returns(Task.FromResult(
                Enumerable.Range(1, Parameters.TarguMures.Days).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = string.Empty
                })));

            var builder = new ContainerBuilder();
            builder.RegisterModule<WeatherForecastServiceModule>();

            //NOTE: overwriting default registrations with our mocks
            builder.Register(c => mockDummySupplier.Object).As<IWeatherSupplier>().WithMetadata<SupplierMetadata>(m => m.For(sm => sm.Name, DummyWeatherSupplier.Name));
            builder.Register(c => mockOpenWeatherSupplier.Object).As<IWeatherSupplier>().WithMetadata<SupplierMetadata>(m => m.For(sm => sm.Name, OpenWeatherSupplier.Name));

            //NOTE: need to register the logger, because it is not registered in the WeatherServiceModule
            var mockLogger = new Mock<ILogger>();
            builder.Register(c => mockLogger.Object).As<ILogger>();

            var _container = builder.Build();

            var service = _container.Resolve<IWeatherForecastService>();
            var results = await service.GetWeatherForecast(Parameters.TarguMures, DummyWeatherSupplier.Name);

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());
            mockDummySupplier.Verify(s => s.GetWeatherForecast(Parameters.TarguMures), Times.Once());
        }
    }
}