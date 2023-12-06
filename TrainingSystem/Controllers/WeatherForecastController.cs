using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TrainingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
      
        private readonly ILoggerManager _logger;

        public WeatherForecastController(ILoggerManager logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogInfo("Here is the message info from the controller");
            _logger.LogWarn("Here is the message Warn from the controller");
            _logger.LogError("Here is the message Error from the controller");
            _logger.LogDebug("Here is the message Debug from the controller");
            return new string[] { "product 1" ,"product 2" };
        }
    }
}
