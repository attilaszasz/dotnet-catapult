using Microsoft.Extensions.Configuration;
using Moq;
using OpenWeatherMap.Standard.Models;
using TestHelpers;

namespace OpenWeather.Tests
{
    [TestClass]
    public class OpenWeatherSupplierTests
    {
        [TestMethod]
        public async Task TestSingleResult()
        {
            var access = new OpenWeatherSupplier(new OpenWeatherAdapter(new ConfigurationBuilder().AddUserSecrets("7b91147d-502c-4fa9-b973-294be01c474b").Build()));
            var result = await access.GetWeatherForecast(Parameters.TarguMures);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public async Task TestOpenWeatherSupplierInIsolation()
        {
            var mockApi = new Mock<IOpenWeatherAdapter>();

            mockApi.Setup(s => s.GetForecastDataByCoordinatesAsync(Parameters.TarguMures.Latitude, Parameters.TarguMures.Longitude)).Returns(Task.FromResult(new ForecastData()));

            var supplier = new OpenWeatherSupplier(mockApi.Object);

            await supplier.GetWeatherForecast(Parameters.TarguMures);

            mockApi.Verify(s => s.GetForecastDataByCoordinatesAsync(Parameters.TarguMures.Latitude, Parameters.TarguMures.Longitude), Times.Once);
        }
    }
}