using System;

namespace weatherApp.DataAccess.Models
{
    public partial class WeatherTbl
    {
        public int WId { get; set; }
        public int? WHumidity { get; set; }
        public int? WTemperature { get; set; }
        public int? WMinTemperature { get; set; }
        public int? WMaxTemperature { get; set; }
        public DateTime WDateTime { get; set; }
    }
}
