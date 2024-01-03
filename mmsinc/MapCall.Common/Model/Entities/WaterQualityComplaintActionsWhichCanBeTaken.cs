using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WaterQualityComplaintActionsWhichCanBeTaken : EntityLookup
    {
        #region Properties

        [Required]
        [StringLength(40)]
        public override string Description { get; set; }

        #endregion
    }
}
