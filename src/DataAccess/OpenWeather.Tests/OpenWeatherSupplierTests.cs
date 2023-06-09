using Autofac;
using Dummy;
using Interfaces;
using Moq;
using OpenWeatherMap.Standard.Models;
using Helpers;
using TestHelpers;
using Types;

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

            var suppliers = container!.Resolve<IEnumerable<Lazy<IWeatherSupplier, SupplierMetadata>>>();
            var supplier = suppliers.GetSupplier(OpenWeatherSupplier.Name);
            var result = await supplier.GetWeatherForecast(Parameters.TarguMures);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public async Task TestOpenWeatherSupplierInIsolation()
        {
            var suppliers = _container!.Resolve<IEnumerable<Lazy<IWeatherSupplier, SupplierMetadata>>>();
            var supplier = suppliers.GetSupplier(OpenWeatherSupplier.Name);

            await supplier.GetWeatherForecast(Parameters.TarguMures);

            _adapter!.Verify(s => s.GetForecastDataByCoordinatesAsync(Parameters.TarguMures.Latitude, Parameters.TarguMures.Longitude), Times.Once);
        }
    }
}