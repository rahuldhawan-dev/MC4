using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class StateWorkDescription : IEntity
    {
        public virtual int Id { get; set; }
        public virtual State State { get; set; }
        public virtual WorkDescription WorkDescription { get; set; }
        public virtual PlantMaintenanceActivityType PlantMaintenanceActivityType { get; set; }
    }
}
