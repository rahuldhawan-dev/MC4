using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class GrievanceCategory : ReadOnlyEntityLookup
    {
        public struct DisplayNames
        {
            public const string GRIEVANCE_CATEGORY = "Grievance Category";
        }

        [View(DisplayNames.GRIEVANCE_CATEGORY)]
        public override string Description { get; set; }
    }
}