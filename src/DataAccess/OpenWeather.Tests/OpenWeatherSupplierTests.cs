using Autofac;
using Dummy;
using Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using OpenWeatherMap.Standard.Models;
using TestHelpers;

namespace OpenWeather.Tests
{
    [TestClass]
    public class OpenWeatherSupplierTests
    {
        private IContainer? _container;
        private Mock<IOpenWeatherAdapter>? _adapter;

        [TestInitialize]
        public void Setup()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<OpenWeatherModule>();
            builder.RegisterModule<DummyModule>();

            _adapter = new Mock<IOpenWeatherAdapter>();
            _adapter.Setup(s => s.GetForecastDataByCoordinatesAsync(Parameters.TarguMures.Latitude, Parameters.TarguMures.Longitude))
                .Returns(Task.FromResult(new ForecastData()));


            //NOTE: Overriding the default registration of IOpenWeatherAPIAdapter with our mock
            //Last registered intance wins
            builder.Register(c => _adapter.Object)
                .As<IOpenWeatherAdapter>();

            _container = builder.Build();
        }


        //NOTE: This tests' purpose is to show that the OpenWeatherSupplier is working as expected
        // It is not intended to run in CI pipelines, hence the ignore attribute
        [Ignore]
        [TestMethod]
        public async Task TestSingleResult()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<OpenWeatherModule>();
            var container = builder.Build();

            var access = container!.ResolveNamed<IWeatherSupplier>(OpenWeatherSupplier.Name);
            var result = await access.GetWeatherForecast(Parameters.TarguMures);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public async Task TestOpenWeatherSupplierInIsolation()
        {
            var supplier = _container!.ResolveNamed<IWeatherSupplier>(OpenWeatherSupplier.Name);

            await supplier.GetWeatherForecast(Parameters.TarguMures);

            _adapter!.Verify(s => s.GetForecastDataByCoordinatesAsync(Parameters.TarguMures.Latitude, Parameters.TarguMures.Longitude), Times.Once);
        }
    }
}