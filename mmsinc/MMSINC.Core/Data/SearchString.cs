namespace MMSINC.Data
{
    public enum SearchStringMatchType
    {
        Exact = 0,
        Wildcard = 1
    }

    public class SearchString
    {
        #region Properties

        public string Value { get; set; }
        public SearchStringMatchType MatchType { get; set; }

        #endregion
    }
}
