using System;

namespace WebMvc.ViewModels
{
    public class TrendViewModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string DateFormat { get; set; }
        public string ChartDescription { get; set; }
        public string Description { get; set; }
        public ChartEntryViewModel[] Data { get; set; }
    }
}
