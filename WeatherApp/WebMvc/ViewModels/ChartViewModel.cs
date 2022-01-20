using WebMvc.Code.Enums;

namespace WebMvc.ViewModels
{
    public class ChartViewModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public ChartType ChartType { get; set; }
        public ChartEntryViewModel[] Data { get; set; }
        public string CssClass { get; set; }
    }
}
