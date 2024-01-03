using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PlanningPlantWasteWaterSystem : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual PlanningPlant PlanningPlant { get; set; }
        public virtual WasteWaterSystem WasteWaterSystem { get; set; }

        #endregion

        public override string ToString()
        {
            return $"{PlanningPlant.Display} - {WasteWaterSystem.Description}";
        }
    }
}

