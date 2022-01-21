namespace WebMvc.ViewModels
{
    /// <summary>
    /// Data to be displayed in charts
    /// </summary>
    public class ChartEntryViewModel
    {
        public double Value { get; set; }
        public string Label { get; set; }
        public string[] Tooltip { get; set; }

        /// <summary>
        /// Used when binding a redirection event, so this property is accessible through ChartJs
        /// </summary>
        public int ActionId { get; set; }
    }
}
