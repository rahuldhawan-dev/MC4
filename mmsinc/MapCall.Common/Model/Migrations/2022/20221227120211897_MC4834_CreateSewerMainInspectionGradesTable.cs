using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221227120211897), Tags("Production")]
    public class MC4834_CreateSewerMainInspectionGradesTable : AutoReversingMigration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues(
                "SewerMainInspectionGrades",
                "EXCELLENT",
                "GOOD",
                "FAIR",
                "POOR",
                "IMMEDIATE ATTENTION");

            Create.Column("InspectionGradeId")
                  .OnTable("SewerMainCleanings")
                  .AsInt32()
                  .Nullable()
                  .ForeignKey(
                       "FK_SewerMainCleanings_SewerMainInspectionGrades_InspectionGradeId",
                       "SewerMainInspectionGrades",
                       "Id");
        }
    }
}

