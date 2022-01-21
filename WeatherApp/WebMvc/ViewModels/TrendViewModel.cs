using System;

namespace WebMvc.ViewModels
{
    /// <summary>
    /// Used in displaying trend data in the last few hours
    /// </summary>
    public class TrendViewModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string DateFormat { get; set; }
        public string ChartDescription { get; set; }
        public string Description { get; set; }
        public ChartEntryViewModel[] Data { get; set; }
        public string Cloudiness { get; set; }
    }
}
