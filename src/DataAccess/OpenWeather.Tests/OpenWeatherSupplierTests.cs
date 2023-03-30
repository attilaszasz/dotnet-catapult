namespace OpenWeather.Tests
{
    [TestClass]
    public class OpenWeatherSupplierTests
    {
        [TestMethod]
        public async Task TestSingleResult()
        {
            var access = new OpenWeatherSupplier();
            var result = await access.GetWeatherForecast(46.542679, 24.557859, 1);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }
    }
}