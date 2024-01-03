using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140814160444155), Tags("Production")]
    public class FixForeignKeyForBug2027 : Migration
    {
        public override void Up()
        {
            Delete.ForeignKey(String.Format("FK_{0}_tblPermissions_ContactId",
                       CreateBAPPTeamIdeasTableForBug1999.TableNames.BAPP_TEAM_IDEAS))
                  .OnTable(CreateBAPPTeamIdeasTableForBug1999.TableNames.BAPP_TEAM_IDEAS);
            Create.ForeignKey(String.Format("FK_{0}_tblEmployee_tblEmployeeId",
                       CreateBAPPTeamIdeasTableForBug1999.TableNames.BAPP_TEAM_IDEAS))
                  .FromTable(CreateBAPPTeamIdeasTableForBug1999.TableNames.BAPP_TEAM_IDEAS).ForeignColumn("ContactId")
                  .ToTable("tblEmployee").PrimaryColumn("tblEmployeeId");
        }

        public override void Down()
        {
            Delete.ForeignKey(String.Format("FK_{0}_tblEmployee_tblEmployeeId",
                       CreateBAPPTeamIdeasTableForBug1999.TableNames.BAPP_TEAM_IDEAS))
                  .OnTable(CreateBAPPTeamIdeasTableForBug1999.TableNames.BAPP_TEAM_IDEAS);
            Create.ForeignKey(String.Format("FK_{0}_tblPermissions_ContactId",
                       CreateBAPPTeamIdeasTableForBug1999.TableNames.BAPP_TEAM_IDEAS))
                  .FromTable(CreateBAPPTeamIdeasTableForBug1999.TableNames.BAPP_TEAM_IDEAS).ForeignColumn("ContactId")
                  .ToTable("tblPermissions").PrimaryColumn("RecId");
        }
    }
}
