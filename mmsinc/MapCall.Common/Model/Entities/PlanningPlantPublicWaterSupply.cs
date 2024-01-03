using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PlanningPlantPublicWaterSupply : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual PlanningPlant PlanningPlant { get; set; }
        public virtual PublicWaterSupply PublicWaterSupply { get; set; }

        #endregion

        public override string ToString()
        {
            return $"{PlanningPlant.Display} - {PublicWaterSupply.Identifier}";
        }
    }
}
