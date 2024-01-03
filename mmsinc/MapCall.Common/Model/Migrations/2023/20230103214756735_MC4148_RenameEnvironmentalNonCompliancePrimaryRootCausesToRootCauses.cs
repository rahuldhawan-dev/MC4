using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230103214756735), Tags("Production")]
    public class MC4148_RenameEnvironmentalNonCompliancePrimaryRootCausesToRootCauses : Migration
    {
        public override void Up()
        {
            Rename.Table("EnvironmentalNonComplianceEventPrimaryRootCauses")
                  .To("EnvironmentalNonComplianceEventRootCauses");

            Rename.Table("EnvironmentalNonComplianceEventsEnvironmentalNonComplianceEventPrimaryRootCauses")
                  .To("EnvironmentalNonComplianceEventsEnvironmentalNonComplianceEventRootCauses");

            Rename.Column("EnvironmentalNonComplianceEventPrimaryRootCauseId")
                  .OnTable("EnvironmentalNonComplianceEventsEnvironmentalNonComplianceEventRootCauses")
                  .To("EnvironmentalNonComplianceEventRootCauseId");
        }

        public override void Down()
        {
            Rename.Table("EnvironmentalNonComplianceEventRootCauses")
                  .To("EnvironmentalNonComplianceEventPrimaryRootCauses");

            Rename.Table("EnvironmentalNonComplianceEventsEnvironmentalNonComplianceEventRootCauses")
                  .To("EnvironmentalNonComplianceEventsEnvironmentalNonComplianceEventPrimaryRootCauses");

            Rename.Column("EnvironmentalNonComplianceEventRootCauseId")
                  .OnTable("EnvironmentalNonComplianceEventsEnvironmentalNonComplianceEventPrimaryRootCauses")
                  .To("EnvironmentalNonComplianceEventPrimaryRootCauseId");
        }
    }
}
