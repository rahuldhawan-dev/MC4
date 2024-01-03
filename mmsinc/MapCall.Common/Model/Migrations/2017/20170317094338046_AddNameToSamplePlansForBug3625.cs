using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170317094338046), Tags("Production")]
    public class AddNameToSamplePlansForBug3625 : Migration
    {
        public override void Up()
        {
            Create.Column("Name").OnTable("SamplePlans").AsAnsiString(50).Nullable();
        }

        public override void Down()
        {
            Delete.Column("Name").FromTable("SamplePlans");
        }
    }
}
