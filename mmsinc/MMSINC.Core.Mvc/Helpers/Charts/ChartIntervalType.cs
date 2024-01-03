namespace MMSINC.Helpers
{
    public enum ChartIntervalType
    {
        /// <summary>
        /// Specifies a chart has no interval values used for placement.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies that a chart with Date values should allow for intervals
        /// that are minutes apart.
        /// </summary>
        Minute,

        /// <summary>
        /// Specifies a chart with Date values should display columns for
        /// every hour between the min and max values, regardless of whether
        /// or not 
        /// </summary>
        Hourly,

        Daily,

        Monthly,

        Yearly
    }
}
