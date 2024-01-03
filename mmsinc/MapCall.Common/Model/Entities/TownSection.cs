using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TownSection : ReadOnlyEntityLookup
    {
        #region Properties

        public override string Description
        {
            get => Name;
            set => throw new InvalidOperationException("Description on entity TownSection cannot be set directly.");
        }

        [DisplayName("Town Section Name")]
        [StringLength(30)]
        public virtual string Name { get; set; }

        [StringLength(4)]
        public virtual string Abbreviation { get; set; }

        public virtual Town Town { get; set; }

        public virtual int? MainSAPEquipmentId { get; set; }
        public virtual int? SewerMainSAPEquipmentId { get; set; }
        public virtual FunctionalLocation MainSAPFunctionalLocation { get; set; }
        public virtual FunctionalLocation SewerMainSAPFunctionalLocation { get; set; }
        public virtual PlanningPlant DistributionPlanningPlant { get; set; }
        public virtual PlanningPlant SewerPlanningPlant { get; set; }

        [StringLength(10)]
        public virtual string ZipCode { get; set; }

        public virtual bool Active { get; set; }

        #endregion
    }
}
