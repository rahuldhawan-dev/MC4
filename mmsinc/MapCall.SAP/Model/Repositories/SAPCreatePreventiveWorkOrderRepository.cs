using MapCall.Common.Model.ViewModels;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using StructureMap;

namespace MapCall.SAP.Model.Repositories
{
    public class SAPCreatePreventiveWorkOrderRepository : SAPRepositoryBase, ISAPCreatePreventiveWorkOrderRepository
    {
        #region Constants

        public const string ERROR_NO_SITE_CONNECTION =
                                "RETRY::CONNECTION FAILURE: No connection could be made to the site:",
                            SAP_NAMESPACE = "PMOrders",
                            SAP_INTERFACE = "http://amwater.com/EAM/0028/MAPCALL/PreventiveMaintenanceOrders";

        #endregion

        #region Exposed Methods

        public SAPCreatePreventiveWorkOrderRepository(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

        public virtual SAPCreatePreventiveWorkOrderCollection Search(SAPCreatePreventiveWorkOrder entity)
        {
            SAPHttpClient.SAPInterface = SAP_NAMESPACE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_INTERFACE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                SAPCreatePreventiveWorkOrder sapCreatePreventiveWorkOrder = new SAPCreatePreventiveWorkOrder();
                sapCreatePreventiveWorkOrder.SAPErrorCode = ERROR_NO_SITE_CONNECTION;
                var SapCreatePreventiveWorkOrderCollection = new SAPCreatePreventiveWorkOrderCollection();
                SapCreatePreventiveWorkOrderCollection.Items.Add(sapCreatePreventiveWorkOrder);
                return SapCreatePreventiveWorkOrderCollection;
            }

            return SAPHttpClient.CreatePreventiveWorkOrder(entity);
        }

        #endregion
    }

    public interface ISAPCreatePreventiveWorkOrderRepository
    {
        SAPCreatePreventiveWorkOrderCollection Search(SAPCreatePreventiveWorkOrder entity);
    }
}
