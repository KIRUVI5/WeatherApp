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

        /// <summary>
        /// This method used to sync data from cloud to database
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult<WeatherDataResponse>> SyncWeatherData()
        {
            try
            {
                //consume external endpoin data
                var weatherData = _consumeExternalAPIService.ConsumeWetherData(ExternalEndpoints.WeatherDataURL);

                //Save into database
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

        /// <summary>
        /// This method used to get weather data by date-time
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task<ActionResult<WeatherAPIResponse>> GetWeatherData(DateTime dateTime)
        {
            try
            {
                //get all data from database
                var weatherData = await _weatherRepository.GetAll();

                List<WeatherDataResponse> weatherDataResponse = new List<WeatherDataResponse>();

                WeatherAPIResponse weatherAPIResponse = new WeatherAPIResponse();

                //filter data by date-time , the filter works acording to dd:hh
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

        /// <summary>
        /// Get all weather data from database
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult<WeatherAPIResponse>> GetAllWeatherData()
        {
            try
            {
                //get all data from the database
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

        /// <summary>
        /// This method used to manually add weather data into database
        /// </summary>
        /// <param name="weatherDataRequest"></param>
        /// <returns></returns>
        public async Task<ActionResult<WeatherDataResponse>> AddWeatherData(WeatherDataRequest weatherDataRequest)
        {
            try
            {
                //Save into database
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
