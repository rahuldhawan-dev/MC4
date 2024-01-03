using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230123153329304), Tags("Production")]
    public class MC5263APCInspectionItemsFacilityInspectionAreaTypeManyToMany : Migration
    {
        public const string
            SAFETY_EQUIPMENT = "Safety equipment such as, safety showers, eyewashes, emergency stops, fire alarm, fire extinguishers, and medical supplies are unobstructed",
            SAFETY_EQUIPMENT_WRONG = "Safety equipment such as, safety showers, eywashes, emergency stops, fire alarm, fire extinguishers, and medical supplies are unobstructed";
        public override void Up()
        {
            Delete.ForeignKeyColumn("APCInspectionItems", "FacilityInspectionAreaTypeId", "FacilityInspectionAreaTypes");

            Create.Table("APCInspectionItemsFacilityInspectionAreaTypes")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("APCInspectionItemId", "APCInspectionItems", "Id", false, "FK_APCInspectionItems_APCInspectionItemId").NotNullable()
                  .WithForeignKeyColumn("FacilityInspectionAreaTypeId",
                       "FacilityInspectionAreaTypes", "Id", false, "FK_FacilityInspectionAreaTypes_FacilityInspectionAreaTypeId").NotNullable();

            Update.Table("FacilityInspectionFormQuestions").Set(new { Question = SAFETY_EQUIPMENT }).Where(new { DisplayOrder = 10 });

            Execute.Sql("update APCInspectionItems set DateInspected = DateRectified");
        }

        public override void Down()
        {
            Delete.Table("APCInspectionItemsFacilityInspectionAreaTypes");
            Alter.Table("APCInspectionItems")
                 .AddForeignKeyColumn("FacilityInspectionAreaTypeId", "FacilityInspectionAreaTypes");
            Update.Table("FacilityInspectionFormQuestions").Set(new { Question = SAFETY_EQUIPMENT_WRONG }).Where(new { DisplayOrder = 10 });
            Execute.Sql("update APCInspectionItems set DateInspected = null");
        }
    }
}

