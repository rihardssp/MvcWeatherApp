namespace WebMvc.Configuration
{
    /// <summary>
    /// Configuration regarding business related logic (in terms of weather controllers)
    /// </summary>
    public class WeatherConfiguration : IWeatherConfiguration
    {
        public const string Section = "WeatherConfiguration";
        private int _trendLengthHours;

        /// <summary>
        /// Trend length in hours - negatives not allowed
        /// </summary>
        public int TrendLengthHours
        {
            get
            {
                return _trendLengthHours;
            }

            set
            {
                _trendLengthHours = value < 0 ? -value : value;
            }
        }
    }
}
