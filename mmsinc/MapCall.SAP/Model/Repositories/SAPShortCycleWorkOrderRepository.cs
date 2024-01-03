using System;
using MapCall.SAP.PreDispatchWS;

namespace MapCall.SAP.Model.Repositories
{
    public class SAPShortCycleWorkOrderRepository : SAPRepositoryBase, ISAPShortCycleWorkOrderRepository
    {
        #region Constants

        public const string ERROR_NO_SITE_CONNECTION =
                                "RETRY::CONNECTION FAILURE: No connection could be made to the site:",
                            SAP_NAMESPACE = "WO_Predispatch_Pull",
                            SAP_INTERFACE = "http://amwater.com/PTB/200018/MAPCALL/WorkOrderPreDispatch_Pull";

        #endregion

        #region Constructors

        public SAPShortCycleWorkOrderRepository(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

        #endregion

        #region Exposed Methods

        public WO_Predispatch_PULL_StatusRecord[] Search(WO_Predispatch_PULL_QueryRecord[] search)
        {
            SAPHttpClient.SAPInterface = SAP_NAMESPACE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_INTERFACE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                return null;
            }

            return SAPHttpClient.SearchShortCycleWorkOrders(search);
        }

        #endregion
    }

    public interface ISAPShortCycleWorkOrderRepository
    {
        #region Abstract Methods

        WO_Predispatch_PULL_StatusRecord[] Search(WO_Predispatch_PULL_QueryRecord[] search);

        #endregion
    }
}
