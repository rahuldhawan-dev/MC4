using System;
using MMSINC.Interface;

namespace MMSINC.DataPages
{
    public interface IDataPageRouteHelper
    {
        #region Properties

        bool HasKnownRoute { get; }

        bool IsHomeRoute { get; }
        bool IsCreateRoute { get; }
        bool IsResultsRoute { get; }
        bool IsSearchRoute { get; }
        bool IsExcelExportRoute { get; }
        bool IsViewRoute { get; }

        int ViewRecordId { get; }
        Guid SearchKey { get; }

        #endregion
    }

    public class DataPageRouteHelper : IDataPageRouteHelper
    {
        #region Fields

        private bool _isInitialized;
        private IQueryString _qs;

        #endregion

        #region Properties

        public bool HasKnownRoute
        {
            get
            {
                return (IsHomeRoute
                        || IsCreateRoute
                        || IsResultsRoute
                        || IsSearchRoute
                        || IsExcelExportRoute
                        || IsViewRoute);
            }
        }

        // Is true if home=
        public bool IsHomeRoute { get; private set; }

        // Is true if create=
        public bool IsCreateRoute { get; private set; }

        // Is true if search=some-valid-guid
        public bool IsResultsRoute { get; private set; }

        // Is true if search=invalid-guid or search=
        public bool IsSearchRoute { get; private set; }

        // Is true if excel=some-valid-guid
        public bool IsExcelExportRoute { get; private set; }

        // Is true if view=a-value
        // Is also true view is valid and there's also a search parameter
        public bool IsViewRoute { get; private set; }

        public int ViewRecordId { get; private set; }
        public Guid SearchKey { get; private set; }

        #endregion

        #region Constructors

        public DataPageRouteHelper(IDataPageBase owner)
        {
            _qs = owner.IRequest.IQueryString;
            Initialize();
        }

        #endregion

        #region Exposed Methods

        #region ParseQueryString related methods

        public void Initialize()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;

                if (QueryStringContainsHome())
                {
                    IsHomeRoute = true;
                }
                else if (QueryStringContainsCreate())
                {
                    IsCreateRoute = true;
                }
                else
                {
                    var qView = _qs.GetValue(DataPageUtility.QUERY.VIEW);
                    if (qView != null)
                    {
                        int recordId;
                        if (int.TryParse(qView, out recordId) && recordId > 0)
                        {
                            IsViewRoute = true;
                            ViewRecordId = recordId;
                        }
                    }

                    ParseSearchQueryString();
                }
            }
        }

        private bool QueryStringContainsKey(string key)
        {
            return (_qs.GetValue(key) != null);
        }

        private bool QueryStringContainsHome()
        {
            return QueryStringContainsKey(DataPageUtility.QUERY.HOME);
        }

        private bool QueryStringContainsCreate()
        {
            return QueryStringContainsKey(DataPageUtility.QUERY.CREATE);
        }

        protected void ParseSearchQueryString()
        {
            var qSearch = _qs.GetValue(DataPageUtility.QUERY.SEARCH);

            if (qSearch != null)
            {
                // if value is null, go back to Search mode.
                // if value is bad, go back to search mode. 

                Guid searchGuid;
                var isValidSearchGuid = Guid.TryParse(qSearch, out searchGuid);

                if (!isValidSearchGuid)
                {
                    // Forward empty or malformed values back to the main search page. 
                    IsSearchRoute = true;
                }
                else
                {
                    SearchKey = searchGuid;

                    if (QueryStringContainsKey(DataPageUtility.QUERY.EXPORT))
                    {
                        IsExcelExportRoute = (true && !IsViewRoute); // I'm aware of how stupid that true is.
                    }
                    else
                    {
                        IsResultsRoute = (true && !IsViewRoute);
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}
