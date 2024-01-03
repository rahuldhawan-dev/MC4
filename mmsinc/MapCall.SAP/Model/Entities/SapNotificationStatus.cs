using MapCall.Common.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.ViewModels;
using MapCall.SAP.NotificationStatusWS;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class SAPNotificationStatus : SAPEntity
    {
        #region Properties

        #region WebService Request Properties

        public virtual string SAPNotificationNo { get; set; }
        public virtual string Complete { get; set; }
        public virtual string Cancel { get; set; }
        public virtual string Remarks { get; set; }
        public virtual string UserName { get; set; }
        public virtual DateTime Date { get; set; }

        #endregion

        #region WebService Response Properties

        public virtual string NotificationID { get; set; }
        public virtual string SAPMessage { get; set; }

        #endregion

        #endregion

        #region Exposed Methods

        public SAPNotificationStatus(SearchSapNotification searchSapNotification)
        {
            SAPNotificationNo = searchSapNotification.SAPNotificationNo;
            Cancel = searchSapNotification.Cancel;
            Complete = searchSapNotification.Complete;
            Remarks = searchSapNotification.Remarks;
        }

        public SAPNotificationStatus(NotificationStatusStatus notificationStatus)
        {
            NotificationID = notificationStatus.NotificationID;
            SAPMessage = notificationStatus.SAPMessage;
        }

        public SAPNotificationStatus() { }

        public CancelUpdate_NotificationUpdateRequest[] UpdateNotificationStatus(
            SAPNotificationStatus sapNotificationStatus)
        {
            CancelUpdate_NotificationUpdateRequest[] NotificationStatus = new CancelUpdate_NotificationUpdateRequest[1];

            NotificationStatus[0] = new CancelUpdate_NotificationUpdateRequest {
                SAPNotificationNo = sapNotificationStatus.SAPNotificationNo,
                Cancel = sapNotificationStatus.Cancel,
                Complete = sapNotificationStatus.Complete,
                Remarks = UserName?.ToString() + " " + Date.Date.ToString(SAP_DATE_FORMAT) + ":\n" +
                          sapNotificationStatus?.Remarks
            };

            return NotificationStatus;
        }

        #endregion
    }
}
