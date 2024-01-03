using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150106125056578), Tags("Production")]
    public class CreateContractorLaborCostsOperatingCentersTableForBug2273 : Migration
    {
        public override void Up()
        {
            Create.Table("ContractorLaborCostsOperatingCenters")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterId", false)
                  .WithForeignKeyColumn("ContractorLaborCostId", "ContractorLaborCosts", nullable: false);

            Execute.Sql(@"
insert into ContractorLaborCostsOperatingCenters (ContractorLaborCostId, OperatingCenterId)
select clc.id as ContractorLaborCostId, oc.OperatingCenterID from ContractorLaborCosts clc
left outer join OperatingCenters oc on 1=1
where oc.OperatingCenterID not in (select OperatingCenterId from OperatingCenters where OperatingCenterCode = 'NJ4' OR OperatingCenterCode = 'NJ7')
order by clc.id, oc.OperatingCenterID;");

            Delete.Index("IX_ContractorLaborCosts_StockNumber").OnTable("ContractorLaborCosts");
        }

        public override void Down()
        {
            Delete.Table("ContractorLaborCostsOperatingCenters");

            Execute.Sql(
                @"
DELETE ContractorOverrideLaborCosts
FROM ContractorOverrideLaborCosts
INNER JOIN ContractorLaborCosts clc
ON clc.Id = ContractorOverrideLaborCosts.ContractorLaborCostId
LEFT OUTER JOIN (SELECT MIN(Id) as RowId, StockNumber FROM ContractorLaborCosts GROUP BY StockNumber) as KeepRows
ON clc.StockNumber = KeepRows.StockNumber AND clc.Id <> KeepRows.RowId
WHERE KeepRows.RowId IS NOT NULL;");
            //Execute.Sql(
            //    "DELETE ContractorLaborCostsOperatingCenters FROM ContractorLaborCostsOperatingCenters INNER JOIN ContractorLaborCosts clc ON clc.Id = ContractorLaborCostsOperatingCenters.ContractorLaborCostId LEFT OUTER JOIN (SELECT MIN(Id) as RowId, StockNumber FROM ContractorLaborCosts GROUP BY StockNumber) as KeepRows ON clc.StockNumber = KeepRows.StockNumber AND clc.Id <> KeepRows.RowId WHERE KeepRows.RowId IS NOT NULL;");
            Execute.Sql(
                @"
DELETE EstimatingProjectContractorLaborCosts
FROM EstimatingProjectContractorLaborCosts
INNER JOIN ContractorLaborCosts clc
ON clc.Id = EstimatingProjectContractorLaborCosts.ContractorLaborCostId
LEFT OUTER JOIN (SELECT MIN(Id) as RowId, StockNumber FROM ContractorLaborCosts GROUP BY StockNumber) as KeepRows
ON clc.StockNumber = KeepRows.StockNumber AND clc.Id <> KeepRows.RowId
WHERE KeepRows.RowId IS NOT NULL;");
            Execute.Sql(
                @"
DELETE ContractorLaborCosts
FROM ContractorLaborCosts
LEFT OUTER JOIN (SELECT MIN(Id) as RowId, StockNumber FROM ContractorLaborCosts GROUP BY StockNumber) as KeepRows
ON ContractorLaborCosts.StockNumber = KeepRows.StockNumber AND ContractorLaborCosts.Id <> KeepRows.RowId
WHERE KeepRows.RowId IS NOT NULL;");

            Alter.Column("StockNumber").OnTable("ContractorLaborCosts")
                 .AsFixedLengthAnsiString(CreateContractorLaborCostsTable.StringLengths.STOCK_NUMBER).NotNullable()
                 .Unique();
        }
    }
}
