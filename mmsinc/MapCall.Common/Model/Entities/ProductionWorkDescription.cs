using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ProductionWorkDescription : IEntity
    {
        #region Constants

        public struct StaticDescriptions
        {
            public const string MAINTENANCE_PLAN = "Maintenance Plan";
        }

        public struct StringLengths
        {
            public const int DESCRIPTION = 50;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Description { get; set; }
        public virtual EquipmentType EquipmentType { get; set; }
        public virtual PlantMaintenanceActivityType PlantMaintenanceActivityType { get; set; }
        public virtual OrderType OrderType { get; set; }
        public virtual bool BreakdownIndicator { get; set; }
        public virtual ProductionSkillSet ProductionSkillSet { get; set; }
        public virtual MaintenancePlanTaskType MaintenancePlanTaskType { get; set; }    
        public virtual TaskGroup TaskGroup { get; set; }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}
