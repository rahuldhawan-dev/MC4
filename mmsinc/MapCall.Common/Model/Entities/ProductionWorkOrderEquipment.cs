using System;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    //TODO: Figure out the proper name for this class. It's a join of Production Work Orders and Equipment, but
    // it also refers to and keeps track of the Notification Numbers from SAP
    [Serializable]
    public class ProductionWorkOrderEquipment : IEntity
    {
        #region Constants

        public struct StringLengths
        {
            public const int COMMENT = 100;
        }

        public struct Display
        {
            public const string REPAIR_COMMENT = "Repair Comments";
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual ProductionWorkOrder ProductionWorkOrder { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual long? SAPNotificationNumber { get; set; }
        public virtual DateTime? CompletedOn { get; set; }
        public virtual int? SAPEquipmentId { get; set; }
        public virtual bool? IsParent { get; set; }
        public virtual AsLeftCondition AsLeftCondition { get; set; }
        public virtual AsFoundCondition AsFoundCondition { get; set; }
        public virtual AssetConditionReason AsFoundConditionReason { get; set; }
        public virtual AssetConditionReason AsLeftConditionReason { get; set; }
        public virtual string AsFoundConditionComment { get; set; }
        public virtual string AsLeftConditionComment { get; set; }

        [View(Display.REPAIR_COMMENT)]
        public virtual string RepairComment { get; set; }

        public virtual ProductionWorkOrderPriority Priority { get; set; }

        #endregion
    }
}