using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151007101244026), Tags("Production")]
    public class ModificationsToProcessesBug2645 : Migration
    {
        public override void Up()
        {
            Rename.Column("NormalRange").OnTable("FacilityProcessSteps").To("NormalRangeMin");
            Alter.Table("FacilityProcessSteps")
                 .AddColumn("NormalRangeMax").AsDecimal(18, 6).NotNullable().WithDefaultValue(0m)
                 .AddColumn("ProcessTarget").AsDecimal(18, 6).NotNullable().WithDefaultValue(0m);

            Rename.Table("FacilityProcessStepApplications").To("FacilityProcessStepSubProcesses");
            Rename.Column("FacilityProcessStepApplicationId").OnTable("FacilityProcessSteps")
                  .To("FacilityProcessStepSubProcessId");
        }

        public override void Down()
        {
            Rename.Column("FacilityProcessStepSubProcessId").OnTable("FacilityProcessSteps")
                  .To("FacilityProcessStepApplicationId");
            Rename.Table("FacilityProcessStepSubProcesses").To("FacilityProcessStepApplications");
            Delete.Column("ProcessTarget").FromTable("FacilityProcessSteps");
            Delete.Column("NormalRangeMax").FromTable("FacilityProcessSteps");
            Rename.Column("NormalRangeMin").OnTable("FacilityProcessSteps").To("NormalRange");
        }
    }
}
