namespace MMSINC.Helpers
{
    public enum ChartSortType
    {
        /// <summary>
        /// Ensures no sorting is done to the data.
        /// </summary>
        None = 0,

        /// <summary>
        /// Ensures that the X values are sorted from lowest
        /// to highest when the type of chart being used is a Line
        /// or Bar chart. 
        /// </summary>
        LowToHigh,

        /// <summary>
        /// Ensures that the X values are sorted alphabetically.
        /// </summary>
        Alphabetical = LowToHigh
    }
}
