namespace WebMvc.Configuration
{
    /// <summary>
    /// Configuration regarding business related logic (in terms of weather controllers)
    /// </summary>
    public interface IWeatherConfiguration
    {
        public int TrendLengthHours { get; }
    }
}
