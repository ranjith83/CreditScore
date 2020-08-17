using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using CreditScore.Business;
//using CreditScore.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CreditScore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        //private readonly IUserService _users;
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            //_users = users;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            //UserService userService = new UserService();
           // _users.AuthenticateUsers("", "");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
