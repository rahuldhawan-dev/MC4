using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class UnionContractProposalPrintingSequence : EntityLookup
    {
        [Required]
        [StringLength(2)]
        public override string Description { get; set; }
    }
}
