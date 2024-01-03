using System;
using MMSINC.Data;
using MapCall.Common.Model.Migrations;
using System.ComponentModel.DataAnnotations;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class GrievanceStatus : ReadOnlyEntityLookup
    {
        [Display(Name = "Grievance Status")]
        [StringLength(MoveGrievanceCategorizationsAndStatusesToTheirOwnTable.DESCRIPTION_LENGTH)]
        public override string Description { get; set; }
    }
}
