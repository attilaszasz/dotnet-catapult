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
            //Note: this will test with the hardcoded DummyWeatherSupplier
            var service = new WeatherForecastService(
                dummy: new DummyWeatherSupplier(), 
                openWeather: new OpenWeatherSupplier(new OpenWeatherAdapter(new ConfigurationBuilder().AddUserSecrets("7b91147d-502c-4fa9-b973-294be01c474b").Build()))
            );
            var results = await service.GetWeatherForecast(Parameters.TarguMures, supplierName: "Dummy");
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

            var service = new WeatherForecastService(mockDummySupplier.Object, mockOpenWeatherSupplier.Object);
            var results = await service.GetWeatherForecast(Parameters.TarguMures, DummyWeatherSupplier.Name);

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());
            mockDummySupplier.Verify(s => s.GetWeatherForecast(Parameters.TarguMures), Times.Once());
        }
    }
}