using TestHelpers;

namespace Dummy.Tests
{
    [TestClass]
    public class DummyWeatherSupplierTests
    {
        [TestMethod]
        public async Task TestSingleResult()
        {
            var access = new DummyWeatherSupplier();
            var result = await access.GetWeatherForecast(Parameters.TarguMures);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }
    }
}