using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221212094839145), Tags("Production")]
    public class MC4834_CreateSewerMainInspectionTypesTable : AutoReversingMigration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues(
                "SewerMainInspectionTypes",
                "ACOUSTIC",
                "CCTV",
                "MAIN CLEANING PM",
                "SMOKE TEST");

            Create.Column("InspectionTypeId")
                  .OnTable("SewerMainCleanings")
                  .AsInt32()
                  .Nullable()
                  .ForeignKey(
                       "FK_SewerMainCleanings_SewerMainInspectionTypes_InspectionTypeId",
                       "SewerMainInspectionTypes",
                       "Id");
        }
    }
}

