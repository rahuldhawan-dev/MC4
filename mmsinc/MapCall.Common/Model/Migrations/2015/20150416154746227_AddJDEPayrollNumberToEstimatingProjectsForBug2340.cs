using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150416154746227), Tags("Production")]
    public class AddJDEPayrollNumberToEstimatingProjectsForBug2340 : Migration
    {
        public const int LENGTH = 8;

        public override void Up()
        {
            Alter.Table(CreateTablesForBug1774.TableNames.ESTIMATING_PROJECTS)
                 .AddColumn("JDEPayrollNumber").AsString(LENGTH).Nullable();
        }

        public override void Down()
        {
            Delete.Column("JDEPayrollNumber").FromTable(CreateTablesForBug1774.TableNames.ESTIMATING_PROJECTS);
        }
    }
}
