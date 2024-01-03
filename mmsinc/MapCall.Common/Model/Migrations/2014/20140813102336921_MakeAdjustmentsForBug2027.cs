using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140813102336921), Tags("Production")]
    public class MakeAdjustmentsForBug2027 : Migration
    {
        public override void Up()
        {
            Delete.ForeignKey("FK_BAPPTeamIdeas_OperatingCenters_OperatingCenterId")
                  .OnTable("BAPPTeamIdeas");
            Delete.Column("OperatingCenterId")
                  .FromTable("BAPPTeamIdeas");

            Create.Column("OperatingCenterId")
                  .OnTable("BAPPTeams")
                  .AsInt32().ForeignKey("FK_BAPPTeams_OperatingCenters_OperatingCenterId", "OperatingCenters",
                       "OperatingCenterId").NotNullable();

            Create.Column("CreatedById")
                  .OnTable("BAPPTeams")
                  .AsInt32().ForeignKey("FK_BAPPTeams_tblPermissions_CreatedById", "tblPermissions", "RecId")
                  .NotNullable();

            Create.Column("DateAdded")
                  .OnTable("BAPPTeamIdeas")
                  .AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Create.Column("OperatingCenterId")
                  .OnTable("BAPPTeamIdeas")
                  .AsInt32().ForeignKey("FK_BAPPTeamIdeas_OperatingCenters_OperatingCenterId", "OperatingCenters",
                       "OperatingCenterId").Nullable();

            Delete.ForeignKey("FK_BAPPTeams_OperatingCenters_OperatingCenterId")
                  .OnTable("BAPPTeams");
            Delete.Column("OperatingCenterId")
                  .FromTable("BAPPTeams");

            Delete.ForeignKey("FK_BAPPTeams_tblPermissions_CreatedById")
                  .OnTable("BAPPTeams");
            Delete.Column("CreatedById")
                  .FromTable("BAPPTeams");

            Delete.Column("DateAdded")
                  .FromTable("BAPPTeamIdeas");
        }
    }
}
