using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PrimaryDriverForProposal : EntityLookup
    {
        [Required]
        [StringLength(123)]
        public override string Description { get; set; }
    }
}
