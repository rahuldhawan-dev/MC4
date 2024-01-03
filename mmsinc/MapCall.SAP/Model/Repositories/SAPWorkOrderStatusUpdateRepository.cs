using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.SAP.Model.Repositories
{
    public class SAPWorkOrderStatusUpdateRepository : SAPRepositoryBase, ISAPWorkOrderStatusUpdateRepository
    {
        #region Constants

        public const string ERROR_NO_SITE_CONNECTION =
                                "RETRY::CONNECTION FAILURE: No connection could be made to the site:",
                            SAP_NAMESPACE = "WOStatusUpdate",
                            SAP_INTERFACE = "http://amwater.com/PTB/200011/MAPCALL/WOStatusUpdates";

        #endregion

        #region Constructors

        public SAPWorkOrderStatusUpdateRepository(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

        #endregion

        #region Exposed Methods

        public virtual SAPWorkOrderStatusUpdateRequest Save(SAPWorkOrderStatusUpdateRequest entity)
        {
            SAPHttpClient.SAPInterface = SAP_NAMESPACE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_INTERFACE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                //error
                entity.SAPErrorCode = ERROR_NO_SITE_CONNECTION;
                return entity;
            }

            return SAPHttpClient.WorkOrderStatusUpdate(entity);
        }

        #endregion
    }

    public interface ISAPWorkOrderStatusUpdateRepository
    {
        #region Abstract Methods

        SAPWorkOrderStatusUpdateRequest Save(SAPWorkOrderStatusUpdateRequest entity);

        #endregion
    }
}
