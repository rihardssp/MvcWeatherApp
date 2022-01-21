namespace WebMvc.Configuration
{
    /// <summary>
    /// Datetime util, so dates are formatted the same way and are configurable.
    /// </summary>
    public interface IDateTimeConfiguration
    {
        public string TimeFormat { get; }
        public string ShortenedDayTimeFormat { get; }
        public string FullFormat { get; }
    }
}
