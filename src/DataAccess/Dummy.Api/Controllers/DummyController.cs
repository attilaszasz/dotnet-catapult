using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Types;

namespace Dummy.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DummyController : ControllerBase
    {
        private readonly IWeatherSupplier _supplier;

        public DummyController(IWeatherSupplier supplier)
        {
            _supplier = supplier;
        }

        [HttpPost]
        [Route("/GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Post(WeatherForecastCriteria criteria)
        {
            return await _supplier.GetWeatherForecast(criteria);
        }
    }
}