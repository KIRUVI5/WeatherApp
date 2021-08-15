using System;
using System.Collections.Generic;
using System.Text;
using weatherApp.DataAccess.Models;
using weatherApp.Shared.Response;

namespace weatherApp.Shared.Request
{
    public class WeatherDataRequest
    {

        public int? humidity { get; set; }
        public int? temperature { get; set; }
        public int? min_temperature { get; set; }
        public int? max_temperature { get; set; }
        public DateTime dateTime { get; set; }


        public static implicit operator WeatherTbl(WeatherDataRequest data)
        {
            var wetherdata = new WeatherTbl();
            wetherdata.WHumidity = data.humidity;
            wetherdata.WTemperature = data.temperature;
            wetherdata.WMinTemperature = data.min_temperature;
            wetherdata.WMaxTemperature = data.max_temperature;
            wetherdata.WDateTime = DateTime.Now;

            return wetherdata;
        }

        public static implicit operator WeatherDataRequest(WeatherTbl data)
        {
            var wetherdata = new WeatherDataRequest();
            wetherdata.humidity = data.WHumidity;
            wetherdata.temperature = data.WTemperature;
            wetherdata.min_temperature = data.WMinTemperature;
            wetherdata.max_temperature = data.WMaxTemperature;
            wetherdata.dateTime = data.WDateTime;

            return wetherdata;
        }

    }
}
