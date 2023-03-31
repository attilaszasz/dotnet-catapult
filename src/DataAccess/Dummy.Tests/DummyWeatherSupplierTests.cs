using Autofac;
using Interfaces;
using TestHelpers;

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
            var access = _container!.ResolveNamed<IWeatherSupplier>(DummyWeatherSupplier.Name);
            var result = await access.GetWeatherForecast(Parameters.TarguMures);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }
    }
}