using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using weatherApp.Service.Interface;
using weatherApp.Shared.Request;
using weatherApp.Shared.Response;

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

        /// <summary>
        /// The api controller used to sync weather data from endpoint to database
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("SyncWeatherData")]
        public async Task<ActionResult<WeatherDataResponse>> SyncWeatherData()
        {
            return await _weatherService.SyncWeatherData();
        }

        /// <summary>
        /// The api endpoin used to get weather data by date-time
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("GetWeatherData")]
        public async Task<ActionResult<WeatherAPIResponse>> GetWeatherData(DateTime dateTime)
        {
            return await _weatherService.GetWeatherData(dateTime);
        }

        /// <summary>
        /// This api endpoint used to get all weather data from the database
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("GetAllWeatherData")]
        public async Task<ActionResult<WeatherAPIResponse>> GetAllWeatherData()
        {
            return await _weatherService.GetAllWeatherData();
        }

        /// <summary>
        /// This api end point used to add weatherdata to database
        /// </summary>
        /// <param name="weatherDataRequest"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("AddWeatherData")]
        public async Task<ActionResult<WeatherDataResponse>> AddWeatherData(WeatherDataRequest weatherDataRequest)
        {
            return await _weatherService.AddWeatherData(weatherDataRequest);
        }
    }
}
