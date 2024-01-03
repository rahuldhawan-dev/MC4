using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.service;
using System;

namespace MapCall.SAP.Model.Services
{
    public class GetWorkOrderInvoker : SAPServiceInvoker
        <GetNotificationAggregate, GetNotification_OB_SYNClient, GetNotification_OB_SYN>
    {
        #region Constants

        public const string NO_DATA_FOUND = "Response not yet configure";

        #endregion

        #region Constructors

        public GetWorkOrderInvoker(ISAPHttpClient sapHttpClient) : base(sapHttpClient)
        {
            SendTimeOut = TimeSpan.FromSeconds(60);
        }

        #endregion

        protected override GetNotificationAggregate InvokeInternal(GetNotificationAggregate sapEntity,
            GetNotification_OB_SYN client)
        {
            SAPNotificationCollection sapNotificationCollection = new SAPNotificationCollection();
            SAPNotification sapNotification;

            try
            {
                var notificationRecord = GetNotificationCreateWorkOrder(sapEntity.SAPNotification);

                GetNotification_OB_SYNResponse retVal =
                    client.GetNotification_OB_SYNAsync(new GetNotification_OB_SYNRequest
                        { GetNotification_Request = notificationRecord }).GetAwaiter().GetResult();

                if (retVal.GetNotification_Response.Length > 0)
                {
                    for (int i = 0; i < retVal.GetNotification_Response.Length; i++)
                    {
                        if (retVal.GetNotification_Response[i].SAPNotificationNo != null)
                            sapNotification = new SAPNotification(retVal.GetNotification_Response[i]);
                        else
                            sapNotification = new SAPNotification
                                { SAPErrorCode = retVal.GetNotification_Response[i]?.SAPStatusMessage };

                        sapNotificationCollection.Items.Add(sapNotification);
                    }
                }
                else
                {
                    sapNotification = new SAPNotification { SAPErrorCode = NO_DATA_FOUND };
                    sapNotificationCollection.Items.Add(sapNotification);
                }
            }
            catch (Exception ex)
            {
                sapNotification = new SAPNotification { SAPErrorCode = RETRY_ERROR_TEXT + ex };
                sapNotificationCollection.Items.Add(sapNotification);
            }
            sapEntity.SAPNotificationCollections = sapNotificationCollection;

            return sapEntity;
        }

        private GetNotification GetNotificationCreateWorkOrder(SAPNotification sapNotification)
        {
            GetNotificationNotifications[] notifications = new GetNotificationNotifications[1];

            notifications[0] = new GetNotificationNotifications {
                PlanningPlant = null,
                DateCreatedFrom = null,
                DateCreatedTo = null,
                NotificationType = null,
                Code = null,
                CodeGroup = null,
                Priority = null
            };

            GetNotificationCreateWorkOrder createWorkOrder = new GetNotificationCreateWorkOrder {
                SAPNotificationNumber = sapNotification.CreateWorkOrderNotificationNumber
            };

            GetNotification getNotification = new GetNotification
                { Notifications = notifications, CreateWorkOrder = createWorkOrder };

            return getNotification;
        }
    }
}
