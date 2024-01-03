using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140430144330000), Tags("Production")]
    public class CreateContractorOverrideLaborCostsTable : Migration
    {
        public const string CONTRACTORS_CONTRACTOR_LABOR_COSTS_TABLE = "ContractorOverrideLaborCosts",
                            FK_CONTRACTORS = "FK_ContractorOverrideLaborCosts_Contractors_ContractorId",
                            FK_CONTRACTOR_LABOR_COSTS =
                                "FK_ContractorLaborCosts_ContractorOverrideLaborCosts_ContractorLaborCostId",
                            FK_OPERATING_CENTERS = "FK_ContractorOverrideLaborCosts_OperatingCenters_OperatingCenterId";

        public override void Up()
        {
            Create.Table(CONTRACTORS_CONTRACTOR_LABOR_COSTS_TABLE)
                  .WithColumn("Id")
                  .AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("ContractorId")
                  .AsInt32().NotNullable().ForeignKey(FK_CONTRACTORS, "Contractors", "ContractorId")
                  .WithColumn("ContractorLaborCostId")
                  .AsInt32().NotNullable().ForeignKey(FK_CONTRACTOR_LABOR_COSTS, "ContractorLaborCosts", "Id")
                  .WithColumn("OperatingCenterId")
                  .AsInt32().NotNullable().ForeignKey(FK_OPERATING_CENTERS, "OperatingCenters", "OperatingCenterId")
                  .WithColumn("Cost")
                  .AsCurrency().NotNullable()
                  .WithColumn("EffectiveDate")
                  .AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.ForeignKey(FK_OPERATING_CENTERS).OnTable(CONTRACTORS_CONTRACTOR_LABOR_COSTS_TABLE);
            Delete.ForeignKey(FK_CONTRACTOR_LABOR_COSTS).OnTable(CONTRACTORS_CONTRACTOR_LABOR_COSTS_TABLE);
            Delete.ForeignKey(FK_CONTRACTORS).OnTable(CONTRACTORS_CONTRACTOR_LABOR_COSTS_TABLE);

            Delete.Table(CONTRACTORS_CONTRACTOR_LABOR_COSTS_TABLE);
        }
    }
}
