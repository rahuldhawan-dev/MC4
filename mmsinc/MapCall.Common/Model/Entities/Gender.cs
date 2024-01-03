using System;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;
using System.ComponentModel.DataAnnotations;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Gender : ReadOnlyEntityLookup
    {
        [StringLength(FixTableAndColumnNamesForBug1623.NewColumnSizes.Genders.DESCRIPTION)]
        public override string Description { get; set; }
    }
}
