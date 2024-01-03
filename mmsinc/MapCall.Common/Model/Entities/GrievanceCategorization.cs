using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class GrievanceCategorization : ReadOnlyEntityLookup
    {
        public struct DisplayNames
        {
            public const string GRIEVANCE_CATEGORIZATION = "Grievance SubCategory";
        }

        [View(DisplayNames.GRIEVANCE_CATEGORIZATION)]
        [StringLength(MoveGrievanceCategorizationsAndStatusesToTheirOwnTable.DESCRIPTION_LENGTH)]
        public override string Description { get; set; }

        public virtual GrievanceCategory GrievanceCategory { get; set; }
    }
}