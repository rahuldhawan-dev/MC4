using System;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;
using System.ComponentModel.DataAnnotations;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ReasonForDeparture : EntityLookup
    {
        [StringLength(FixTableAndColumnNamesForBug1623.NewColumnSizes.ReasonsForDeparture.DESCRIPTION)]
        public override string Description { get; set; }
    }
}
