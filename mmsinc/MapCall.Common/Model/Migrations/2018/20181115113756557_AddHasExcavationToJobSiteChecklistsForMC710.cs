using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181115113756557), Tags("Production")]
    public class AddHasExcavationToJobSiteChecklistsForMC710 : Migration
    {
        public override void Up()
        {
            Create.Column("HasExcavation").OnTable("JobSiteCheckLists").AsBoolean().Nullable();

            Execute.Sql(
                "UPDATE JobSiteCheckLists SET HasExcavation = CASE WHEN (SELECT COUNT(1) FROM JobSiteExcavations WHERE JobSiteCheckListId = JobSiteCheckLists.Id) > 0 THEN 1 ELSE 0 END");

            Alter.Column("HasExcavation").OnTable("JobSiteCheckLists").AsBoolean().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("HasExcavation").FromTable("JobSiteCheckLists");
        }
    }
}
