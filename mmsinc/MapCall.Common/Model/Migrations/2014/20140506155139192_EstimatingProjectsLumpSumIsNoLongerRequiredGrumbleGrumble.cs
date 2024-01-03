using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140506155139192), Tags("Production")]
    public class EstimatingProjectsLumpSumIsNoLongerRequiredGrumbleGrumble : Migration
    {
        public override void Up()
        {
            Alter.Column(CreateTablesForBug1774.ColumnNames.EstimatingProjects.LUMP_SUM)
                 .OnTable(CreateTablesForBug1774.TableNames.ESTIMATING_PROJECTS)
                 .AsCurrency()
                 .Nullable();
        }

        public override void Down()
        {
            // noop cause Jason said not to make it NotNullable and have it throw errors.
        }
    }
}
