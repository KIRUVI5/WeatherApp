using System;
using System.Collections.Generic;
using System.Text;

namespace weatherApp.Shared.Constant
{
    public struct ExternalEndpoints
    {
        public const string WeatherDataURL = "http://demo4567044.mockable.io/weather";
        public const string LoginUrl = "https://localhost:44324/api/Authenticate/Login";
        public const string GetAllWeatherData = "https://localhost:44324/api/Weather/GetAllWeatherData";
        public const string AddWeatherData = "https://localhost:44324/api/Weather/AddWeatherData";
    }
}
