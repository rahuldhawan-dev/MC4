using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220919145114314), Tags("Production")]
    public class MC4964_PareDownShortCycleWorkOrderTables : Migration
    {
        public override void Up()
        {
            Create
               .Table("ShortCycleCustomerMaterials")
               .WithIdentityColumn()
               .WithForeignKeyColumn("PremiseId", "Premises", nullable: false)
               .WithColumn("AssignmentStart").AsDateTime().Nullable()
               .WithForeignKeyColumn("CustomerSideMaterialId", "ServiceMaterials", "ServiceMaterialID")
               .WithColumn("ServiceLineSize").AsString(50).Nullable()
               .WithColumn("TechnicalInspectedOn").AsDateTime().Nullable()
               .WithForeignKeyColumn("ReadingDevicePositionalLocationId", "SmallMeterLocations")
               .WithForeignKeyColumn("ReadingDeviceDirectionalLocationId", "MeterDirections")
               .WithColumn("ShortCycleWorkOrderNumber").AsInt64().NotNullable().Unique();
            
            Execute.Sql(@"
INSERT INTO ShortCycleCustomerMaterials (
    PremiseId,
    AssignmentStart,
    CustomerSideMaterialId,
    ServiceLineSize,
    ShortCycleWorkOrderNumber)
SELECT DISTINCT
    PremiseId,
    AssignmentStart,
    CustomerSideMaterialId,
    ServiceLineSize,
    WorkOrder
FROM ShortCycleWorkOrders
WHERE PremiseId IS NOT NULL;");
            
            Execute.Sql(@"
WITH SelectedCustomerSideCompletions AS (
    SELECT
        c.ShortCycleWorkOrderId,
        c.CustomerSideMaterialId,
        c.TechnicalInspectedOn
    FROM ShortCycleWorkOrderCompletions c
    WHERE c.CustomerSideMaterialId IS NOT NULL
), MostRecentCustomerSideCompletions AS (
    SELECT
        recent.ShortCycleWorkOrderId,
        recent.CustomerSideMaterialId,
        recent.TechnicalInspectedOn
    FROM SelectedCustomerSideCompletions recent
    LEFT JOIN SelectedCustomerSideCompletions moreRecent
        ON moreRecent.ShortCycleWorkOrderId = recent.ShortCycleWorkOrderId
        AND moreRecent.TechnicalInspectedOn > recent.TechnicalInspectedOn
    WHERE moreRecent.ShortCycleWorkOrderId IS NULL
)
UPDATE ShortCycleCustomerMaterials
SET CustomerSideMaterialId = recent.CustomerSideMaterialId,
    TechnicalInspectedOn = recent.TechnicalInspectedOn
FROM MostRecentCustomerSideCompletions recent
WHERE recent.ShortCycleWorkOrderId = ShortCycleWorkOrderNumber;");
            
            Execute.Sql(@"
WITH SelectedCompletions AS (
    SELECT
        c.Id,
        c.ShortCycleWorkOrderId,
        c.ReadingDeviceDirectionalLocationId,
        c.ReadingDevicePositionalLocationId
    FROM ShortCycleWorkOrderCompletions c
    WHERE c.CustomerSideMaterialId IS NOT NULL
), MostRecentCustomerSideCompletions AS (
    SELECT
        recent.ShortCycleWorkOrderId,
        recent.ReadingDeviceDirectionalLocationId,
        recent.ReadingDevicePositionalLocationId
    FROM SelectedCompletions recent
    LEFT JOIN SelectedCompletions moreRecent
        ON moreRecent.ShortCycleWorkOrderId = recent.ShortCycleWorkOrderId
        AND moreRecent.Id > recent.Id
    WHERE moreRecent.ShortCycleWorkOrderId IS NULL
)
UPDATE ShortCycleCustomerMaterials
SET ReadingDeviceDirectionalLocationId = recent.ReadingDeviceDirectionalLocationId,
    ReadingDevicePositionalLocationId = recent.ReadingDevicePositionalLocationId
FROM MostRecentCustomerSideCompletions recent
WHERE recent.ShortCycleWorkOrderId = ShortCycleWorkOrderNumber;");
        }

        public override void Down()
        {
            Delete.Table("ShortCycleCustomerMaterials");
        }
    }
}

