using MapCall.SAP.Model.Entities;

namespace MapCall.SAP.Model.Repositories
{
    public class SAPWorkOrderRepository : SAPRepositoryBase, ISAPWorkOrderRepository
    {
        #region Constants

        public const string ERROR_NO_SITE_CONNECTION =
                                "RETRY::CONNECTION FAILURE: No connection could be made to the site:",
                            SAP_NAMESPACE = "CreateWorkOrder",
                            SAP_INTERFACE = "http://amwater.com/EAM/0007/MAPCALL/CreateWorkOrder",
                            SAP_PROCESS_NAMESPACE = "ProgressWorkOrder",
                            SAP_PROCESS_INTERFACE = "http://amwater.com/EAM/0008/MAPCALL/ProgressWorkOrder",
                            SAP_COMPLETE_NAMESPACE = "CompleteWorkOrder",
                            SAP_COMPLETE_INTERFACE = "http://amwater.com/EAM/0009/MAPCALL/CompleteWorkOrder",
                            SAP_GOODSISSUE_NAMESPACE = "GoodsIssue",
                            SAP_GOODSISSUE_INTERFACE = "http://amwater.com/EAM/0009/MAPCALL/CompleteWorkOrder";

        #endregion

        #region Constructors

        public SAPWorkOrderRepository(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

        #endregion

        #region Exposed Methods

        public SAPWorkOrder Save(SAPWorkOrder entity)
        {
            SAPHttpClient.SAPInterface = SAP_NAMESPACE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_INTERFACE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                entity.SAPErrorCode = ERROR_NO_SITE_CONNECTION;
                return entity;
            }

            return SAPHttpClient.CreateWorkOrder(entity);
        }

        public SAPProgressWorkOrder Update(SAPProgressWorkOrder entity)
        {
            SAPHttpClient.SAPInterface = SAP_PROCESS_NAMESPACE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_PROCESS_INTERFACE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                entity.Status = ERROR_NO_SITE_CONNECTION;
                return entity;
            }

            return SAPHttpClient.ProgressWorkOrder(entity);
        }

        public SAPCompleteWorkOrder Complete(SAPCompleteWorkOrder entity)
        {
            SAPHttpClient.SAPInterface = SAP_COMPLETE_NAMESPACE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_COMPLETE_INTERFACE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                entity.Status = ERROR_NO_SITE_CONNECTION;
                return entity;
            }

            return SAPHttpClient.CompleteWorkOrder(entity);
        }

        public SAPGoodsIssueCollection Approve(SAPGoodsIssue entity)
        {
            SAPHttpClient.SAPInterface = SAP_GOODSISSUE_NAMESPACE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_GOODSISSUE_INTERFACE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                entity.Status = ERROR_NO_SITE_CONNECTION;
                var sapGoodsIssueCollection = new SAPGoodsIssueCollection();
                sapGoodsIssueCollection.Items.Add(entity);
                return sapGoodsIssueCollection;
            }

            return SAPHttpClient.ApproveGoodsIssue(entity);
        }

        #endregion
    }

    public interface ISAPWorkOrderRepository
    {
        #region Abstract Methods

        SAPWorkOrder Save(SAPWorkOrder entity);
        SAPProgressWorkOrder Update(SAPProgressWorkOrder entity);
        SAPCompleteWorkOrder Complete(SAPCompleteWorkOrder entity);
        SAPGoodsIssueCollection Approve(SAPGoodsIssue entity);

        #endregion
    }
}
