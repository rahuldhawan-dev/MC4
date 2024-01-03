using MapCall.Common.Model.ViewModels;
using MapCall.SAP.Model.Entities;

namespace MapCall.SAP.Model.Repositories
{
    public class SAPTechnicalMasterAccountRepository : SAPRepositoryBase, ISAPTechnicalMasterAccountRepository
    {
        #region Constants

        public const string ERROR_NO_SITE_CONNECTION =
                                "RETRY::CONNECTION FAILURE: No connection could be made to the site:",
                            SAP_NAMESPACE = "GetTechnicalMaster_AccountDetails",
                            SAP_INTERFACE = "http://amwater.com/EAM/0015/MAPCALL/GetTechnicalMaster_AccountDetails";

        #endregion

        #region Constructors

        public SAPTechnicalMasterAccountRepository(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

        #endregion

        #region Exposed Methods

        public virtual SAPTechnicalMasterAccountCollection Search(SearchSapTechnicalMaster entity)
        {
            SAPHttpClient.SAPInterface = SAP_NAMESPACE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_INTERFACE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                var sapTechnicalMasterAccountDetails = new SAPTechnicalMasterAccount();
                sapTechnicalMasterAccountDetails.SAPError = ERROR_NO_SITE_CONNECTION;
                var sapTechnicalMasterAccountCollection = new SAPTechnicalMasterAccountCollection();
                sapTechnicalMasterAccountCollection.Items.Add(sapTechnicalMasterAccountDetails);
                return sapTechnicalMasterAccountCollection;
            }

            return SAPHttpClient.GetTechnicalMasterAccountResponse(entity);
        }

        #endregion
    }

    public interface ISAPTechnicalMasterAccountRepository
    {
        #region Abstract Methods

        SAPTechnicalMasterAccountCollection Search(SearchSapTechnicalMaster entity);

        #endregion
    }
}
