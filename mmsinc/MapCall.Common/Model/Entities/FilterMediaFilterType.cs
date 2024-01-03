using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FilterMediaFilterType : EntityLookup
    {
        [Required]
        [StringLength(Migrations.CreateTablesForBug1510.StringLengths.FilterMediaFilterTypes.DESCRIPTION)]
        public override string Description { get; set; }
    }
}
