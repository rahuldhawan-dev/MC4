namespace MMSINC.Helpers
{
    // These need to match up with TYPES in the script.
    public enum ChartType
    {
        /// <summary>
        /// Default. Chart should be a line chart.
        /// </summary>
        Line = 0,

        /// <summary>
        /// Chart should be a bar chart.
        /// </summary>
        Bar,

        /// <summary>
        /// Chart should be a bar chart, but the values all come from one series.
        /// </summary>
        SingleSeriesBar
    }
}
