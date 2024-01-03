using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class InstitutionalKnowledge : EntityLookup
    {
        [StringLength(FixTableAndColumnNamesForBug1623.NewColumnSizes.InstitutionalKnowledge.DESCRIPTION)]
        public override string Description { get; set; }
    }
}
