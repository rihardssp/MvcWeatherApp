namespace WebMvc.ViewModels
{
    public class WeatherGraphItemViewModel
    {
        public ChartViewModel ChartModel { get; set; }
        public string ActionUrl { get; set; }
        public int[] ActionIds { get; set; }
    }
}
