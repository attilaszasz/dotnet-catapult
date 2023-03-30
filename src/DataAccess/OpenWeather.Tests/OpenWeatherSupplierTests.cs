using Microsoft.Extensions.Configuration;
using TestHelpers;

namespace OpenWeather.Tests
{
    [TestClass]
    public class OpenWeatherSupplierTests
    {
        [TestMethod]
        public async Task TestSingleResult()
        {
            var access = new OpenWeatherSupplier(new ConfigurationBuilder().AddUserSecrets("7b91147d-502c-4fa9-b973-294be01c474b").Build());
            var result = await access.GetWeatherForecast(Parameters.TarguMures);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }
    }
}