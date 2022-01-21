namespace WebMvc.Configuration
{
    /// <summary>
    /// Datetime util, so dates are formatted the same way and are configurable.
    /// </summary>
    public class DateTimeConfiguration : IDateTimeConfiguration
    {
        public const string Section = "DateTimeConfiguration";
        public string TimeFormat { get; set; }
        public string ShortenedDayTimeFormat { get; set; }
        public string FullFormat { get; set; }
    }
}
