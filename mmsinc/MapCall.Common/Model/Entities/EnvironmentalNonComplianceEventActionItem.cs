using System;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EnvironmentalNonComplianceEventActionItem : IEntity
    {
        #region Constants

        public struct StringLengths
        {
            public const int ACTION_ITEM = 255,
                             NOT_LISTED_TYPE = 50;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual EnvironmentalNonComplianceEvent EnvironmentalNonComplianceEvent { get; set; }
        public virtual EnvironmentalNonComplianceEventActionItemType Type { get; set; }
        public virtual User ResponsibleOwner { get; set; }
        public virtual string NotListedType { get; set; }
        public virtual string ActionItem { get; set; }
        public virtual DateTime TargetedCompletionDate { get; set; }
        public virtual DateTime? DateCompleted { get; set; }
        public virtual bool? In30DayIntervalFromTargetDate { get; set; }

        #endregion
    }
}
