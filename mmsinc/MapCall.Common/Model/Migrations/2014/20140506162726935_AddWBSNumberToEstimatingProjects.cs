using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140506162726935), Tags("Production")]
    public class AddWBSNumberToEstimatingProjects : Migration
    {
        public override void Up()
        {
            Alter.Table(CreateTablesForBug1774.TableNames.ESTIMATING_PROJECTS)
                 .AddColumn("WBSNumber")
                 .AsString(18)
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("WBSNumber").FromTable(CreateTablesForBug1774.TableNames.ESTIMATING_PROJECTS);
        }
    }
}
