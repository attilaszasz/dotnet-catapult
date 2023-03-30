namespace WeatherForecastService.Tests
{
    [TestClass]
    public class WeatherForecastServiceTests
    {
        [TestMethod]
        public async Task TestSingleResult()
        {
            //Note: this will test with the hardcoded DummyWeatherSupplier
            var service = new WeatherForecastService();
            var results = await service.GetWeatherForecast(latitude: 46.542679, longitude: 24.557859, days: 1, supplierName: "Dummy");
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());
        }
    }
}