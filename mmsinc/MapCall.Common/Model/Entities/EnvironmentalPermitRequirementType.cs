using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EnvironmentalPermitRequirementType : EntityLookup
    {
        [Required]
        [StringLength(50)]
        public override string Description { get; set; }
    }
}
