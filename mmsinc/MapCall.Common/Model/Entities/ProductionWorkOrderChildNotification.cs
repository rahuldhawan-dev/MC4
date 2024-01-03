using System;
using System.Collections.Generic;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ProductionWorkOrderChildNotification
    {
        #region properties

        public virtual string NotificationType { get; set; }
        public virtual string NotificationNumber { get; set; }
        public virtual string SAPFunctionalLocation { get; set; }
        public virtual string SAPEquipmentNumber { get; set; }
        public virtual string ReqStartDate { get; set; }
        public virtual string NotificationLongText { get; set; }
        public virtual string Purpose { get; set; }
        public virtual string CompleteFlag { get; set; }
        public virtual IList<ProductionWorkOrderDependencies> ProductionWorkOrderDependencies { get; set; }
        public virtual IList<ProductionWorkOrderActions> ProductionWorkOrderActions { get; set; }
        public virtual IList<ProductionWorkOrderMeasuringPoints> ProductionWorkOrderMeasuringPoints { get; set; }

        #endregion
    }
}
