using Autofac;
using Interfaces;
using Helpers;
using TestHelpers;
using Types;

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