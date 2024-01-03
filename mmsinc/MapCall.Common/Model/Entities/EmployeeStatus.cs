using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EmployeeStatus : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int ACTIVE = 1, INACTIVE = 2, WITHDRAWN = 3, RETIREE = 4;
        }

        [StringLength(FixTableAndColumnNamesForBug1623.NewColumnSizes.EmployeeStatuses.DESCRIPTION)]
        public override string Description { get; set; }
    }
}
