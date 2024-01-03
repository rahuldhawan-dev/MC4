using MapCall.Common.Model.ViewModels;
using MapCall.SAP.Model.Entities;

namespace MapCall.SAP.Model.Repositories
{
    public class SAPFunctionalLocationRepository : SAPRepositoryBase, ISAPFunctionalLocationRepository
    {
        #region Constants

        public const string ERROR_NO_SITE_CONNECTION =
                                "RETRY::CONNECTION FAILURE: No connection could be made to the site:",
                            SAP_NAMESPACE = "GetFunctionalLocation",
                            SAP_INTERFACE = "http://amwater.com/EAM/0021/MAPCALL/GetFunctionalLocation";

        #endregion

        #region Constructors

        public SAPFunctionalLocationRepository(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

        #endregion

        #region Exposed Methods

        public SAPFunctionalLocationCollection Search(SearchSapFunctionalLocation search)
        {
            SAPHttpClient.SAPInterface = SAP_NAMESPACE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_INTERFACE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                SAPFunctionalLocation sapFunctionalLocation = new SAPFunctionalLocation();
                sapFunctionalLocation.SAPErrorCode = ERROR_NO_SITE_CONNECTION;
                var sapFunctionalLocationCollection = new SAPFunctionalLocationCollection();
                sapFunctionalLocationCollection.Items.Add(sapFunctionalLocation);
                return sapFunctionalLocationCollection;
            }

            return SAPHttpClient.GetFunctionalLocation(search);
        }

        #endregion
    }

    public interface ISAPFunctionalLocationRepository
    {
        #region Abstract Methods

        SAPFunctionalLocationCollection Search(SearchSapFunctionalLocation search);

        #endregion
    }
}
