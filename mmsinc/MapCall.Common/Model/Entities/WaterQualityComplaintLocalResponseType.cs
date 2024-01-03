using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WaterQualityComplaintLocalResponseType : EntityLookup
    {
        #region Properties

        [Required]
        [StringLength(30)]
        public override string Description { get; set; }

        #endregion
    }
}