using System;
using System.Collections.Generic;
using MMSINC.Utilities;

namespace MMSINC.DataPages
{
    /// <summary>
    /// Represents methods used to create various urls for a DataPageBase instance. 
    /// </summary>
    public interface IDataPagePath
    {
        IDataPageBase Owner { get; }

        string GetBaseUrl();
        string GetCreateNewRecordUrl();
        string GetExportToExcelUrl();
        string GetSearchResultsUrl();
        string GetNewSearchUrl();
    }

    public class DataPagePath : IDataPagePath
    {
        #region Properties

        public IDataPageBase Owner { get; private set; }

        #endregion

        #region Constructors

        public DataPagePath(IDataPageBase owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            Owner = owner;
        }

        #endregion

        #region Private methods

        private string BuildSearchUrl(string queryValue)
        {
            return QueryStringHelper.BuildFromKeyValuePair(GetBaseUrl(), DataPageUtility.QUERY.SEARCH, queryValue);
        }

        #endregion

        #region Exposed Methods

        public string GetBaseUrl()
        {
            return Owner.IRequest.Url;
        }

        public string GetCreateNewRecordUrl()
        {
            return QueryStringHelper.BuildFromKeyValuePair(GetBaseUrl(), DataPageUtility.QUERY.CREATE, null);
        }

        public string GetExportToExcelUrl()
        {
            var q = new Dictionary<string, object>();
            q.Add(DataPageUtility.QUERY.SEARCH, Owner.CachedFilterKey.ToString());
            q.Add(DataPageUtility.QUERY.EXPORT, string.Empty);

            return QueryStringHelper.BuildFromDictionary(GetBaseUrl(), q);
        }

        public string GetSearchResultsUrl()
        {
            return BuildSearchUrl(Owner.CachedFilterKey.ToString());
        }

        public string GetNewSearchUrl()
        {
            return BuildSearchUrl(string.Empty);
        }

        #endregion
    }
}
