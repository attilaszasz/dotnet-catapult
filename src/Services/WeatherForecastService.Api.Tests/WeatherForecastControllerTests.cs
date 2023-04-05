using Autofac.Core;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using TestHelpers;
using Types;
using WeatherForecastService.Api.Controllers;
using Moq;

namespace WeatherForecastService.Api.Tests
{
    [TestClass]
    public class WeatherForecastControllerTests
    {
        private IContainer? _container;

        [TestInitialize]
        public void Setup()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<WeatherForecastServiceApiModule>();

            //NOTE: While testing we don't want to actually write logs
            builder.Register(c => new Mock<ILogger>().Object)
                .As<ILogger>();

            _container = builder.Build();
        }

        [TestMethod]
        public async Task TestSingleResult()
        {
            var controller = _container!.Resolve<WeatherForecastController>();

            var result = (await controller.Post(Parameters.TarguMures, supplierName: Constants.Suppliers.Dummy)).Result as OkObjectResult;

            Assert.IsNotNull(result);
            var value = result.Value as IEnumerable<WeatherForecast>;
            Assert.IsNotNull(value);
            Assert.AreEqual(1, value.Count());
        }
    }
}