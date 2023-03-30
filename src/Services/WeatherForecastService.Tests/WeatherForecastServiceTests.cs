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
            var results = await service.GetWeatherForecast(days: 1);
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());
        }
    }
}