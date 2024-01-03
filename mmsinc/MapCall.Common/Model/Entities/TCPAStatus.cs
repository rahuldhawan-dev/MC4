using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TCPAStatus : EntityLookup
    {
        [StringLength(FixTableAndColumnNamesForBug1623.NewColumnSizes.TCPAStatuses.DESCRIPTION)]
        public override string Description { get; set; }
    }
}
