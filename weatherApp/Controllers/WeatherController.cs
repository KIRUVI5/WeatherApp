using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using weatherApp.Service.Interface;
using weatherApp.Shared.Request;
using weatherApp.Shared.Response;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace weatherApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("SyncWeatherData")]
        public async Task<ActionResult<WeatherDataResponse>> SyncWeatherData()
        {
            return await _weatherService.SyncWeatherData();
        }

        [Authorize]
        [HttpGet]
        [Route("GetWeatherData")]
        public async Task<ActionResult<WeatherAPIResponse>> GetWeatherData(DateTime dateTime)
        {
            return await _weatherService.GetWeatherData(dateTime);
        }



        [HttpGet]
        [Route("GetAllWeatherData")]
        public async Task<ActionResult<WeatherAPIResponse>> GetAllWeatherData()
        {
            return await _weatherService.GetAllWeatherData();
        }

        [HttpPost]
        [Route("AddWeatherData")]
        public async Task<ActionResult<WeatherDataResponse>> AddWeatherData(WeatherDataRequest weatherDataRequest)
        {
            return await _weatherService.AddWeatherData(weatherDataRequest);
        }
    }
}
