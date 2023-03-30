using Microsoft.AspNetCore.Mvc;
using Types;
using WeatherForecastService.Api.Controllers;

namespace WeatherForecastService.Api.Tests
{
    [TestClass]
    public class WeatherForecastControllerTests
    {
        [TestMethod]
        public async Task TestSingleResult()
        {
            //NOTE: we cannot test WeatherForecastService in isolation because the weather suppliers are tightly coupled to it
            // For example, we'd like to test if logging is done correctly
            var controller = new WeatherForecastController();

            var result = (await controller.Get(1)).Result as OkObjectResult;
            Assert.IsNotNull(result);
            var value = result.Value as IEnumerable<WeatherForecast>;
            Assert.IsNotNull(value);
            Assert.AreEqual(1, value.Count());
        }
    }
}