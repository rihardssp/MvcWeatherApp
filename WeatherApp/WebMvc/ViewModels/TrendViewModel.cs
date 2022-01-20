using System;
using System.Collections.Generic;

namespace WebMvc.ViewModels
{
    public class TrendViewModel
    {
        public DateTime DateFrom;
        public DateTime DateTo;
        public string DateFormat;
        public string ChartDescription;
        public IList<TrendItemEntry> Entries { get; set; }
    }
}
