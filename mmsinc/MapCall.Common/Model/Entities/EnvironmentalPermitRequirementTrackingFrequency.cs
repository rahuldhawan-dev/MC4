using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EnvironmentalPermitRequirementTrackingFrequency : EntityLookup
    {
        [Required]
        [StringLength(20)]
        public override string Description { get; set; }
    }
}
