using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140313160306795)]
    [Tags("Production")]
    public class CreateContractorLaborCostsTable : Migration
    {
        public struct StringLengths
        {
            public const int STOCK_NUMBER = 4, UNIT = 5, JOB_DESCRIPTION = 75, SUB_DESCRIPTION = 40;
        }

        public override void Up()
        {
            Create.Table("ContractorLaborCosts")
                  .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable().Unique()
                  .WithColumn("StockNumber").AsFixedLengthAnsiString(StringLengths.STOCK_NUMBER).NotNullable().Unique()
                  .WithColumn("Unit").AsString(StringLengths.UNIT).NotNullable()
                  .WithColumn("JobDescription").AsString(StringLengths.JOB_DESCRIPTION).NotNullable()
                  .WithColumn("SubDescription").AsString(StringLengths.SUB_DESCRIPTION).Nullable()
                  .WithColumn("Cost").AsCurrency().NotNullable();

            Create.Table("EstimatingProjectContractorLaborCosts")
                  .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable().Unique()
                  .WithColumn("EstimatingProjectId").AsInt32()
                  .ForeignKey("FK_EstimatingProjectContractorLaborCosts_EstimatingProjects_EstimatingProjectId",
                       "EstimatingProjects", "Id").NotNullable()
                  .WithColumn("ContractorLaborCostId").AsInt32()
                  .ForeignKey("FK_EstimatingProjectContractorLaborCosts_ContractorLaborCosts_ContractorLaborCostId",
                       "ContractorLaborCosts", "Id").NotNullable()
                  .WithColumn("Quantity").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("EstimatingProjectContractorLaborCosts");

            Delete.Table("ContractorLaborCosts");
        }
    }
}
