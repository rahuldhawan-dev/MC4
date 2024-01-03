using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230804133717027), Tags("Production")]
    public class MC5535_AddWorkOrderDescription : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"SET IDENTITY_INSERT WorkDescriptions ON;

            INSERT INTO [dbo].[WorkDescriptions]
                       ([WorkDescriptionID]
                       ,[Description]
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
                       ,[PlantMaintenanceActivityTypeId]
                       ,[IsActive]
                       ,[MarkoutRequired]
                       ,[MaterialsRequired]
                       ,[JobSiteCheckListRequired]
                       ,[DigitalAsBuiltRequired])
                 SELECT
                       327,
                       'SERVICE LINE INSTALLATION COMPLETE PARTIAL' as Description, 
                       4 as AssetTypeID, 
                       '3.25' as TimeToComplete, 
                       13 as WorkCategoryID, 
                       1 as AccountingTypeID,
                       1 as FirstRestorationAccountingCodeID, 
                       100 as FirstRestorationCostBreakdown,
                       1 as FirstRestorationProductCodeID, 
                       NULL as SecondRestorationAccountingCodeID, 
                       NULL as SecondRestorationCostBreakdown, 
                       NULL as SecondRestorationProductCodeID, 
                       0 as ShowBusinessUnit,
                       1 as ShowApprovalAccounting,
                       0 as EditOnly,
                       1 as Revisit, 
                       NULL as MaintenanceActivityType, 
                       5 as PlantMaintenanceActivityTypeId, 
                       1 as IsActive, 
                       1 as MarkoutRequired,
                       1 as MaterialsRequired, 
                       1 as JobSiteCheckListRequired, 
                       1 as DigitalAsBuiltRequired;

            SET IDENTITY_INSERT WorkDescriptions OFF");
        }

        public override void Down()
        {
            Delete.FromTable("WorkDescriptions").Row(new { WorkDescriptionID = 327 });
        }
    }
}

