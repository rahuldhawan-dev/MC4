namespace MMSINC.DataPages
{
    /// <summary>
    /// Static class that holds stuff(like structs/consts) that's common to classes 
    /// related to DataPageBase.
    /// </summary>
    public static class DataPageUtility
    {
        #region Structs

        /// <summary>
        /// Querystring keys. 
        /// </summary>
        public struct QUERY
        {
            public const string CREATE = "create";
            public const string EXPORT = "export";
            public const string SEARCH = "search";
            public const string VIEW = "view";
            public const string HOME = "home";
        }

        #endregion
    }
}
