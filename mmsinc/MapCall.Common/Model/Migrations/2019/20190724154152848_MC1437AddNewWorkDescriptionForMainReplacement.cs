using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190724154152848), Tags("Production")]
    public class MC1437AddNewWorkDescriptionForMainReplacement : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"SET IDENTITY_INSERT WorkDescriptions ON
            INSERT INTO[WorkDescriptions]
                ([WorkDescriptionID], [Description]
                ,[AssetTypeID]
                ,[TimeToComplete]
                ,[WorkCategoryID]
                ,[AccountingTypeID]
                ,[FirstRestorationAccountingCodeID]
                ,[FirstRestorationCostBreakdown]
                ,[FirstRestorationProductCodeID]
                ,[SecondRestorationAccountingCodeID]
                ,[SecondRestorationCostBreakdown]
                ,[SecondRestorationProductCodeID]
                ,[ShowBusinessUnit]
                ,[ShowApprovalAccounting]
                ,[EditOnly]
                ,[Revisit]
                ,[MaintenanceActivityType]
                ,[PlantMaintenanceActivityTypeId])
            SELECT
                274,
                'WATER MAIN REPLACEMENT',
                (select assetTypeID from assetTypes where Description = 'Main') as AssetTypeID,
                4.25 as TimeToComplete,
                (select WorkCategoryId from WOrkCategories where Description = 'Water Main Maintenance') as WorkCategoryID,
                1 as AccountingTypeID,
                1 as FirstRestorationAccountingCodeID,
                100 as FirstRestorationCostBreakdown,
                1 as FirstRestorationProductCodeID,
                null as SecondRestorationAccountingCodeID,
                null as SecondRestorationCostBreakdown,
                null as SecondRestorationProductCodeID,
                0 as ShowBusinessUnit,
                1 as ShowApprovalAccounting,
                0 as EditOnly,
                0 as Revisit,
                NULL as MaintenanceActivityType,
                1 as PlantMaintenanceActivityTypeId;
            SET IDENTITY_INSERT WorkDescriptions OFF");
        }

        public override void Down()
        {
            // noop
        }
    }
}
