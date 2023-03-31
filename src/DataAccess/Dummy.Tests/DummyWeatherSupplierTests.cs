using Autofac;
using Interfaces;
using Helpers;
using TestHelpers;
using Types;
using Moq;

namespace Dummy.Tests
{
    [TestClass]
    public class DummyWeatherSupplierTests
    {
        private IContainer? _container;

        [TestInitialize]
        public void Setup()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<DummyModule>();

            //NOTE: need to register the logger, because it is not registered in the DummyModule
            var mockLogger = new Mock<ILogger>();
            builder.Register(c => mockLogger.Object).As<ILogger>();

            _container = builder.Build();
        }

        [TestMethod]
        public async Task TestSingleResult()
        {
            var suppliers = _container!.Resolve<IEnumerable<Lazy<IWeatherSupplier, SupplierMetadata>>>();
            var supplier = suppliers.GetSupplier(DummyWeatherSupplier.Name);
            var result = await supplier.GetWeatherForecast(Parameters.TarguMures);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }
    }
}