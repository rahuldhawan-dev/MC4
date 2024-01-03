using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WaterQualityComplaintCustomerSatisfaction : EntityLookup
    {
        #region Properties

        [Required]
        [StringLength(25)]
        public override string Description { get; set; }

        #endregion
    }
}
