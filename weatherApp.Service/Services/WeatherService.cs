using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using weatherApp.Repository.Repositories;
using weatherApp.Service.Interface;
using weatherApp.Shared.Constant;
using weatherApp.Shared.Request;
using weatherApp.Shared.Response;

namespace weatherApp.Service.Services
{
    public class WeatherService : IWeatherService
    {
        private WeatherRepository _weatherRepository;
        private readonly IConsumeExternalAPIService _consumeExternalAPIService;

        public WeatherService(WeatherRepository weatherRepository, IConsumeExternalAPIService consumeExternalAPIService)
        {
            _weatherRepository = weatherRepository;
            _consumeExternalAPIService = consumeExternalAPIService;
        }
        public async Task<ActionResult<WeatherDataResponse>> SyncWeatherData()
        {
            try
            {
                var weatherData = _consumeExternalAPIService.ConsumeWetherData(ExternalEndpoints.WeatherDataURL);

                if (weatherData != null)
                {
                    await _weatherRepository.Add(weatherData);

                    return weatherData;
                }
                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<ActionResult<WeatherAPIResponse>> GetWeatherData(DateTime dateTime)
        {
            try
            {
                var weatherData = await _weatherRepository.GetAll();

                List<WeatherDataResponse> weatherDataResponse = new List<WeatherDataResponse>();

                WeatherAPIResponse weatherAPIResponse = new WeatherAPIResponse();

                if (weatherData != null)
                {
                    weatherData = weatherData.Where(d => d.WDateTime.Date == dateTime.Date && d.WDateTime.Hour == dateTime.Hour).ToList();
                }

                foreach (var item in weatherData)
                {
                    weatherDataResponse.Add(item);
                }

                weatherAPIResponse.weatherListDataResponses = weatherDataResponse;

                return weatherAPIResponse;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ActionResult<WeatherAPIResponse>> GetAllWeatherData()
        {
            try
            {
                var weatherData = await _weatherRepository.GetAll();

                List<WeatherDataResponse> weatherDataResponse = new List<WeatherDataResponse>();

                WeatherAPIResponse weatherAPIResponse = new WeatherAPIResponse();               

                foreach (var item in weatherData)
                {
                    weatherDataResponse.Add(item);
                }

                weatherAPIResponse.weatherListDataResponses = weatherDataResponse;

                return weatherAPIResponse;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ActionResult<WeatherDataResponse>> AddWeatherData(WeatherDataRequest weatherDataRequest)
        {
            try
            {
                var weatherData = await _weatherRepository.Add(weatherDataRequest);

                if (weatherData != null)
                {
                    WeatherDataResponse weatherDataResponse = new WeatherDataResponse()
                    {
                        humidity = weatherData.WHumidity,
                        temperature = weatherData.WTemperature,
                        max_temperature = weatherData.WMinTemperature,
                        min_temperature = weatherData.WMaxTemperature,
                        dateTime = weatherData.WDateTime,

                    };

                    return weatherDataResponse;
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
