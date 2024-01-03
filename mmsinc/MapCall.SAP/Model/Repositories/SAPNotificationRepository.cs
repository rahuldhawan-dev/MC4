using MapCall.Common.Model.ViewModels;
using MapCall.SAP.Model.Entities;

namespace MapCall.SAP.Model.Repositories
{
    public class SAPNotificationRepository : SAPRepositoryBase, ISAPNotificationRepository
    {
        #region Constants

        public const string ERROR_NO_SITE_CONNECTION =
                                "RETRY::CONNECTION FAILURE: No connection could be made to the site:",
                            SAP_GET_NOTIFICATION_NAMESPACE = "GetNotification",
                            SAP_GET_NOTIFICATION_INTERFACE = "http://amwater.com/EAM/0011/MAPCALL/GetNotification",
                            SAP_NOTIFICATION_STATUS_NAMESPACE = "NotificationStatus",
                            SAP_NOTIFICATION_STATUS_INTERFACE =
                                "http://amwater.com/EAM/0012/MAPCALL/CancelUpdateNotification";

        #endregion

        #region Constructors

        public SAPNotificationRepository(ISAPHttpClient sapHttpClient) : base(sapHttpClient) { }

        #endregion

        #region Exposed Methods

        public SAPNotificationCollection Search(SearchSapNotification entity)
        {
            SAPHttpClient.SAPInterface = SAP_GET_NOTIFICATION_NAMESPACE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_GET_NOTIFICATION_INTERFACE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                SAPNotification SAPNotification = new SAPNotification();
                SAPNotification.SAPErrorCode = ERROR_NO_SITE_CONNECTION;
                var SAPNotificationCollection = new SAPNotificationCollection();
                SAPNotificationCollection.Items.Add(SAPNotification);
                return SAPNotificationCollection;
            }

            return SAPHttpClient.GetNotificationResponse(entity);
        }

        public SAPNotificationStatus Save(SAPNotificationStatus entityCollection)
        {
            SAPHttpClient.SAPInterface = SAP_NOTIFICATION_STATUS_NAMESPACE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_NOTIFICATION_STATUS_INTERFACE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                entityCollection.SAPMessage = ERROR_NO_SITE_CONNECTION;
                return entityCollection;
            }

            return SAPHttpClient.GetNotificationStatusUpdate(entityCollection);
        }

        public SAPNotificationCollection SearchWorkOrder(SAPNotification entity)
        {
            SAPHttpClient.SAPInterface = SAP_GET_NOTIFICATION_NAMESPACE;
            SAPHttpClient.SAPInterfaceNamespace = SAP_GET_NOTIFICATION_INTERFACE;

            if (!SAPHttpClient.IsSiteRunning)
            {
                SAPNotification SAPNotification = new SAPNotification();
                SAPNotification.SAPErrorCode = ERROR_NO_SITE_CONNECTION;
                var SAPNotificationCollection = new SAPNotificationCollection();
                SAPNotificationCollection.Items.Add(SAPNotification);
                return SAPNotificationCollection;
            }

            return SAPHttpClient.GetWorkOrder(entity);
        }

        #endregion
    }

    public interface ISAPNotificationRepository
    {
        #region Abstract Methods

        SAPNotificationCollection Search(SearchSapNotification entity);
        SAPNotificationStatus Save(SAPNotificationStatus entityCollection);
        SAPNotificationCollection SearchWorkOrder(SAPNotification entity);

        #endregion
    }
}
