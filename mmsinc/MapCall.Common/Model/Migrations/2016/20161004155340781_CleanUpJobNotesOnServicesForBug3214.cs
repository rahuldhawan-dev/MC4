using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161004155340781), Tags("Production")]
    public class CleanUpJobNotesOnServicesForBug3214 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "update Services set JobNotes = replace(replace(replace(cast(JobNotes as varchar(max)), '<br>', CHAR(13) + CHAR(10)), '>', ']'), '<', '[')");
        }

        public override void Down() { }
    }
}
