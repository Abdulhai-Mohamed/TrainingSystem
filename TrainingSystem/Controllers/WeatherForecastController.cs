using Contraacts;
using Microsoft.AspNetCore.Mvc;

namespace TrainingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]


    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILoggerManger _logger;

        public WeatherForecastController(ILoggerManger logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInfo("info message from the controller");//not show because nlogcongig rules
            _logger.LogDebug("debug message from the controller");//not show because nlogcongig rules
            _logger.LogWarning("warn message from the controller");
            _logger.LogError("error message from the controller");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}