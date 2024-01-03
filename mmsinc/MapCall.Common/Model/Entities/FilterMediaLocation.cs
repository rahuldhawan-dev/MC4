using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FilterMediaLocation : EntityLookup
    {
        [Required]
        [StringLength(Migrations.CreateTablesForBug1510.StringLengths.FilterMediaLocations.DESCRIPTION)]
        public override string Description { get; set; }
    }
}
