using System;
using System.Collections.Generic;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class SAPProductionWorkOrderChildNotification
    {
        #region properties

        public virtual string NotificationType { get; set; }
        public virtual string NotificationNumber { get; set; }
        public virtual string SAPFunctionalLocation { get; set; }
        public virtual string SAPEquipmentNumber { get; set; }
        public virtual string ReqStartDate { get; set; }
        public virtual string NotificationLongText { get; set; }
        public virtual string PurposeCode { get; set; }
        public virtual string CodeGroup { get; set; }
        public virtual string CompleteFlag { get; set; }
        public virtual IList<SAPProductionWorkOrderDependencies> SapProductionWorkOrderDependencies { get; set; }
        public virtual IList<SAPProductionWorkOrderActions> SapProductionWorkOrderActions { get; set; }
        public virtual IList<SAPProductionWorkOrderMeasuringPoints> SapProductionWorkOrderMeasuringPoints { get; set; }

        #endregion

        public SAPProductionWorkOrderChildNotification()
        {
            SapProductionWorkOrderMeasuringPoints = new List<SAPProductionWorkOrderMeasuringPoints>();
        }
    }
}
