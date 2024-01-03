using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;

namespace MapCall.SAP.Model.Repositories
{
    public class SAPProgressUnscheduledWorkOrderRepository : SAPRepositoryBase,
        ISAPProgressUnscheduledWorkOrderRepository
    {
        #region Constants

        public const string ERROR_NO_SITE_CONNECTION =
                                "RETRY::CONNECTION FAILURE: No connection could be made to the site:",
                            SAP_NAMESPACE = "ProgressScheduledUnscheduledWO",
                            SAP_INTERFACE = "http://amwater.com/EAM/0008/MAPCALL/ProgressWorkOrder";

        #endregion

        #region Constructors

        public SAPProgressUnscheduledWorkOrderRepository(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

        #endregion

        #region Exposed Methods

        public virtual SAPProgressUnscheduledWorkOrder Save(SAPProgressUnscheduledWorkOrder entity)
        {
            if (!entity.IsSAPEnabled)
            {
                return entity;
            }

            SAPHttpClient.SAPInterface = SAP_NAMESPACE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_INTERFACE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                //error
                entity.SAPErrorCode = ERROR_NO_SITE_CONNECTION;
                return entity;
            }

            return SAPHttpClient.ProgressUnscheduleWorkOrder(entity);
        }

        #endregion
    }

    public interface ISAPProgressUnscheduledWorkOrderRepository
    {
        #region Abstract Methods

        SAPProgressUnscheduledWorkOrder Save(SAPProgressUnscheduledWorkOrder entity);

        #endregion
    }
}
