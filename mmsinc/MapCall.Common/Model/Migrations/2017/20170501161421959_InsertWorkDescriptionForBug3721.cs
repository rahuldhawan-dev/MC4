using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170501161421959), Tags("Production")]
    public class InsertWorkDescriptionForBug3721 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"INSERT INTO [WorkDescriptions]
           ([Description]
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
           'FLUSHING-MAIN WQ',
           (select assetTypeID from assetTypes where Description = 'MAIN') as AssetTypeID,
           1 as TimeToComplete,
           (select WorkCategoryId from WOrkCategories where Description = 'Water Quality') as WorkCategoryID,
           2 as AccountingTypeID,
           1 as FirstRestorationAccountingCodeID,
           100 as FirstRestorationCostBreakdown,
           1 as FirstRestorationProductCodeID,
           null as SecondRestorationAccountingCodeID,
           null as SecondRestorationCostBreakdown,
           null as SecondRestorationProductCodeID,
           1 as ShowBusinessUnit,
           0 as ShowApprovalAccounting,
           0 as EditOnly,
           0 as Revisit,
           0 as MaintenanceActivityType,
           12 as PlantMaintenanceActivityTypeId");
        }

        public override void Down()
        {
            Execute.Sql("DELETE FROM WorkDescriptions WHERE Description = 'FLUSHING-MAIN WQ';");
        }
    }
}
