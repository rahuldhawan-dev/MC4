using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EquipmentMaintenancePlan : IEntity
    {
        public virtual int Id { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual MaintenancePlan MaintenancePlan { get; set; }
    }
}
