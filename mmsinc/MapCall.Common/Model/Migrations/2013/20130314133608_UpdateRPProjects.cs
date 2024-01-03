using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130314133608), Tags("Production")]
    public class UpdateRPProjects : Migration
    {
        public override void Up()
        {
            Alter.Table("RPProjects").AddColumn("FoundationalFilingPeriod").AsCustom("varchar(50)").Nullable();
        }

        public override void Down()
        {
            Delete.Column("FoundationalFilingPeriod").FromTable("RPProjects");
        }
    }
}
