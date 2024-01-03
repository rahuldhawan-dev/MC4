using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230111135759144), Tags("Production")]
    public class MC4149_NonComplianceEventsNOVInformationSectionEnhancements : AutoReversingMigration
    {
        public override void Up() => Create.Column("DateOfEnvironmentalLeadershipTeamReview").OnTable("EnvironmentalNonComplianceEvents").AsDateTime().Nullable();
    }
}