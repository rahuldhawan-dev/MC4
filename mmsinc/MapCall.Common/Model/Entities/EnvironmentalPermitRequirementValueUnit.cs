using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EnvironmentalPermitRequirementValueUnit : EntityLookup
    {
        #region Properties

        [Required]
        [StringLength(10)]
        public override string Description { get; set; }

        #endregion
    }
}
