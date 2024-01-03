using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class UnionContractProposalAffectedDepartment : EntityLookup
    {
        [Required]
        [StringLength(14)]
        public override string Description { get; set; }
    }
}
