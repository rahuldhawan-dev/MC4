using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140307160043), Tags("Production")]
    public class CreateTablesForBug1780 : Migration
    {
        public struct TableNames
        {
            public const string ESTIMATING_PROJECT_OTHER_COSTS = "EstimatingProjectOtherCosts";
        }

        public struct ColumNames
        {
            public struct EstimatingProjectOtherCosts
            {
                public const string ID = "Id",
                                    QUANTITY = "Quantity",
                                    DESCRIPTION = "Description",
                                    COST = "Cost",
                                    ESTIMATING_PROJECT_ID = "EstimatingProjectId";
            }
        }

        public const int DESCRIPTION_LENGTH = 50;

        public override void Up()
        {
            Create.Table(TableNames.ESTIMATING_PROJECT_OTHER_COSTS)
                  .WithColumn(ColumNames.EstimatingProjectOtherCosts.ESTIMATING_PROJECT_ID).AsInt32().NotNullable()
                  .ForeignKey(
                       String.Format("FK_{0}_{1}_{2}", TableNames.ESTIMATING_PROJECT_OTHER_COSTS,
                           CreateTablesForBug1774.TableNames.ESTIMATING_PROJECTS,
                           ColumNames.EstimatingProjectOtherCosts.ESTIMATING_PROJECT_ID),
                       CreateTablesForBug1774.TableNames.ESTIMATING_PROJECTS, "Id")
                  .WithColumn(ColumNames.EstimatingProjectOtherCosts.ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(ColumNames.EstimatingProjectOtherCosts.QUANTITY).AsInt32().NotNullable()
                  .WithColumn(ColumNames.EstimatingProjectOtherCosts.DESCRIPTION).AsAnsiString(DESCRIPTION_LENGTH)
                  .NotNullable()
                  .WithColumn(ColumNames.EstimatingProjectOtherCosts.COST).AsCurrency().NotNullable();
        }

        public override void Down()
        {
            Delete.Table(TableNames.ESTIMATING_PROJECT_OTHER_COSTS);
        }
    }
}
