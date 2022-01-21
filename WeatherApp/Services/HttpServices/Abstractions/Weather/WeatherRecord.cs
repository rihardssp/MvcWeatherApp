using Services.HttpServices.GeneratedResponseTypes.Weather;
using System;

namespace Services.HttpServices.Abstractions.Weather
{
    /// <summary>
    /// Weather api response data
    /// </summary>
    public class WeatherRecord
    {
        public double WindSpeed { get; set; }
        public double Temperature { get; set; }
        public int LocationApiId { get; set; }
        public double Cloudiness { get; set; }
        public DateTime Date { get; set; }

        internal static WeatherRecord MapRecord(List data)
        {
            var date = DateTimeOffset.FromUnixTimeSeconds(data.Dt).DateTime;
            return new WeatherRecord
            {
                WindSpeed = data.Wind.Speed,
                Temperature = data.Main.Temp,
                LocationApiId = data.Id,
                Date = date,
                Cloudiness = data.Clouds.All
            };
        }
    }
}
