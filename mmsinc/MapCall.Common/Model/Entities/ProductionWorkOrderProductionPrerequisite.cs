using System;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ProductionWorkOrderProductionPrerequisite : IEntity
    {
        public virtual int Id { get; set; }
        public virtual ProductionWorkOrder ProductionWorkOrder { get; set; }
        public virtual ProductionPrerequisite ProductionPrerequisite { get; set; }
        public virtual ProductionWorkOrderDocument Document { get; set; }

        [View(DisplayFormat = CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE)]
        public virtual DateTime? SatisfiedOn { get; set; }

        [View("Remove Requirement")]
        public virtual bool SkipRequirement { get; set; }

        [View("Remove Requirement Comments")]
        public virtual string SkipRequirementComments { get; set; }
    }
}
