using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;

namespace MapCall.SAP.Model.Repositories
{
    public class SAPCreateUnscheduledWorkOrderRepository : SAPRepositoryBase, ISAPCreateUnscheduledWorkOrderRepository
    {
        #region Constants

        public const string ERROR_NO_SITE_CONNECTION =
                                "RETRY::CONNECTION FAILURE: No connection could be made to the site:",
                            SAP_NAMESPACE = "CreateUnscheduledWO",
                            SAP_INTERFACE = "http://amwater.com/EAM/0007/MAPCALL/CreateWorkOrder";

        #endregion

        #region Constructors

        public SAPCreateUnscheduledWorkOrderRepository(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

        #endregion

        #region Exposed Methods

        public virtual SAPCreateUnscheduledWorkOrder Save(SAPCreateUnscheduledWorkOrder entity)
        {
            if (!entity.IsSAPEnabled)
            {
                return entity;
            }

            SAPHttpClient.SAPInterface = SAP_NAMESPACE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_INTERFACE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                entity.SAPErrorCode = ERROR_NO_SITE_CONNECTION;
                return entity;
            }

            return SAPHttpClient.CreateUnscheduleWorkOrder(entity);
        }

        #endregion
    }

    public interface ISAPCreateUnscheduledWorkOrderRepository
    {
        #region Abstract Methods

        SAPCreateUnscheduledWorkOrder Save(SAPCreateUnscheduledWorkOrder entity);

        #endregion
    }
}
