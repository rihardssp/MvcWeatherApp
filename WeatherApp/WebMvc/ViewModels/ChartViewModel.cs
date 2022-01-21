using WebMvc.Code.Enums;

namespace WebMvc.ViewModels
{
    /// <summary>
    /// Full data of partial view _Chart
    /// </summary>
    public class ChartViewModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public ChartType ChartType { get; set; }
        public ChartEntryViewModel[] Data { get; set; }
        public string CssClass { get; set; }
    }
}
