using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using weatherApp.Shared.Request;
using weatherApp.Shared.Response;

namespace weatherApp.Service.Interface
{
    public interface IWeatherService
    {
        Task<ActionResult<WeatherDataResponse>> SyncWeatherData();

        Task<ActionResult<WeatherAPIResponse>> GetWeatherData(DateTime dateTime);

        Task<ActionResult<WeatherAPIResponse>> GetAllWeatherData();

        Task<ActionResult<WeatherDataResponse>> AddWeatherData(WeatherDataRequest weatherDataRequest);
    }
}
