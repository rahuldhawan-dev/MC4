using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;
using System;

namespace MapCall.SAP.Model.Repositories
{
    public class SAPCompleteUnscheduledWorkOrderRepository : SAPRepositoryBase,
        ISAPCompleteUnscheduledWorkOrderRepository
    {
        #region Constants

        public const string ERROR_NO_SITE_CONNECTION =
                                "RETRY::CONNECTION FAILURE: No connection could be made to the site:",
                            SAP_NAMESPACE = "CompleteScheduledUnscheduledWO",
                            SAP_INTERFACE = "http://amwater.com/EAM/0009/MAPCALL/CompleteWorkOrder";

        #endregion

        #region Constructors

        public SAPCompleteUnscheduledWorkOrderRepository(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

        #endregion

        #region Exposed Methods

        public virtual SAPCompleteUnscheduledWorkOrder Save(SAPCompleteUnscheduledWorkOrder entity)
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

            return SAPHttpClient.CompleteUnscheduleWorkOrder(entity);
        }

        #endregion
    }

    public interface ISAPCompleteUnscheduledWorkOrderRepository
    {
        #region Abstract Methods

        SAPCompleteUnscheduledWorkOrder Save(SAPCompleteUnscheduledWorkOrder entity);

        #endregion
    }
}
