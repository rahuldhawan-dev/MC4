using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FilterMediaWashType : EntityLookup
    {
        [Required]
        [StringLength(Migrations.CreateTablesForBug1510.StringLengths.FilterMediaWashTypes.DESCRIPTION)]
        public override string Description { get; set; }
    }
}
