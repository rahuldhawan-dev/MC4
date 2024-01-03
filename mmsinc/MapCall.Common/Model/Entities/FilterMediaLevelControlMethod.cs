using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FilterMediaLevelControlMethod : EntityLookup
    {
        [Required]
        [StringLength(Migrations.CreateTablesForBug1510.StringLengths.FilterMediaLevelControlMethods.DESCRIPTION)]
        public override string Description { get; set; }
    }
}
