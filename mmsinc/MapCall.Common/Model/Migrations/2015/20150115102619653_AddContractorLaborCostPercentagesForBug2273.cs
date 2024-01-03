using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150115102619653), Tags("Production")]
    public class AddContractorLaborCostPercentagesForBug2273 : Migration
    {
        public struct StringLengths
        {
            public const int JOB_DESCRIPTION = 100;
        }

        private void UpTable(string table)
        {
            Alter.Table(table)
                 .AddColumn("Percentage").AsInt16().Nullable();

            Alter.Table(table)
                 .AlterColumn("Cost").AsCurrency().Nullable();
        }

        private void DownTable(string table)
        {
            Delete.Column("Percentage").FromTable(table);

            Execute.Sql(
                "DELETE FROM ContractorOverrideLaborCosts WHERE EXISTS (SELECT 1 FROM ContractorLaborCosts WHERE Id = ContractorLaborCostId AND Cost IS NULL);");
            Execute.Sql(
                "DELETE FROM ContractorLaborCostsOperatingCenters WHERE EXISTS (SELECT 1 FROM ContractorLaborCosts WHERE Id = ContractorLaborCostId AND Cost IS NULL);");
            Execute.Sql(
                "DELETE FROM EstimatingProjectContractorLaborCosts WHERE EXISTS (SELECT 1 FROM ContractorLaborCosts WHERE ID = ContractorLaborCostId AND Cost IS NULL);");
            Execute.Sql("DELETE FROM ContractorLaborCosts WHERE Cost IS NULL");

            Alter.Table(table).AlterColumn("Cost").AsCurrency().NotNullable();
        }

        public override void Up()
        {
            Alter.Column("JobDescription").OnTable("ContractorLaborCosts").AsString(StringLengths.JOB_DESCRIPTION)
                 .NotNullable();

            UpTable("ContractorLaborCosts");
            UpTable("ContractorOverrideLaborCosts");
        }

        public override void Down()
        {
            DownTable("ContractorLaborCosts");
            DownTable("ContractorOverrideLaborCosts");
        }
    }
}
