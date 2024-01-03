using MapCall.SAP.Model.Entities;

namespace MapCall.SAP.Model.Repositories
{
    public class SAPMaintenancePlanRepository : SAPRepositoryBase, ISAPMaintenancePlanLookupRepository
    {
        #region Constants

        public const string ERROR_NO_SITE_CONNECTION =
                                "RETRY::CONNECTION FAILURE: No connection could be made to the site:",
                            SAP_NAMESPACE_SEARCH = "MaintenancePlanLookup",
                            SAP_INTERFACE_SEARCH = "http://amwater.com/EAM/0027/MAPCALL/MaintenancePlan",
                            SAP_NAMESPACE_UPDATE = "MaintenancePlanUpdate",
                            SAP_INTERFACE_UPDATE = "http://amwater.com/EAM/0027/MAPCALL/MaintenancePlan";

        #endregion

        #region Constructors

        public SAPMaintenancePlanRepository(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

        #endregion

        #region Exposed Methods

        public virtual SAPMaintenancePlanLookupCollection Search(SAPMaintenancePlanLookup entity)
        {
            SAPHttpClient.SAPInterface = SAP_NAMESPACE_SEARCH;
            SAPHttpClient.SAPInterfaceNamespace = SAP_INTERFACE_SEARCH;

            if (!SAPHttpClient.IsSiteRunning)
            {
                var sapMaintenancePlanLookup = new SAPMaintenancePlanLookup();
                sapMaintenancePlanLookup.SAPErrorCode = ERROR_NO_SITE_CONNECTION;
                var sapMaintenancePlanLookupCollection = new SAPMaintenancePlanLookupCollection();
                sapMaintenancePlanLookupCollection.Items.Add(sapMaintenancePlanLookup);
                return sapMaintenancePlanLookupCollection;
            }

            return SAPHttpClient.GetMaintenancePlan(entity);
        }

        public virtual SAPMaintenancePlanUpdateCollection Save(SAPMaintenancePlanUpdate entity)
        {
            SAPHttpClient.SAPInterface = SAP_NAMESPACE_UPDATE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_INTERFACE_UPDATE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                var sapMaintenancePlanUpdate = new SAPMaintenancePlanUpdate();
                sapMaintenancePlanUpdate.SAPErrorCode = ERROR_NO_SITE_CONNECTION;
                var sapMaintenancePlanUpdateCollection = new SAPMaintenancePlanUpdateCollection();
                sapMaintenancePlanUpdateCollection.Items.Add(sapMaintenancePlanUpdate);
                return sapMaintenancePlanUpdateCollection;
            }

            return SAPHttpClient.UpdateMaintenancePlan(entity);
        }

        #endregion
    }

    public interface ISAPMaintenancePlanLookupRepository
    {
        #region Abstract Methods

        SAPMaintenancePlanLookupCollection Search(SAPMaintenancePlanLookup entity);
        SAPMaintenancePlanUpdateCollection Save(SAPMaintenancePlanUpdate entity);

        #endregion
    }
}
