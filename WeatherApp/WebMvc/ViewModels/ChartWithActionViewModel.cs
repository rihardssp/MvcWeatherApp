namespace WebMvc.ViewModels
{
    /// <summary>
    /// Used in displaying charts with a frontend action
    /// </summary>
    public class ChartWithActionViewModel
    {
        public ChartViewModel ChartModel { get; set; }
        public string ActionUrl { get; set; }
    }
}
