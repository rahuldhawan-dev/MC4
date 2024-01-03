using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class OperatingCenterTown : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Town Town { get; set; }
        public virtual string Abbreviation { get; set; }
        public virtual int? MainSAPEquipmentId { get; set; }
        public virtual int? SewerMainSAPEquipmentId { get; set; }
        public virtual FunctionalLocation MainSAPFunctionalLocation { get; set; }
        public virtual FunctionalLocation SewerMainSAPFunctionalLocation { get; set; }
        public virtual PlanningPlant DistributionPlanningPlant { get; set; }
        public virtual PlanningPlant SewerPlanningPlant { get; set; }

        #endregion

        public override bool Equals(object obj)
        {
            var other = obj as OperatingCenterTown;

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return OperatingCenter == other.OperatingCenter && Town == other.Town;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
