using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210824172347265), Tags("Production")]
    public class MC2715AddingPremiseIdToShortCycleWorkOrders : Migration
    {
        public override void Up()
        {
            Execute.Sql("EXEC sp_rename 'ShortCycleWorkOrders.Premise', 'PremiseNumber', 'COLUMN';");
            Alter.Table("ShortCycleWorkOrders").AddForeignKeyColumn("PremiseId", "Premises", nullable: true);
            Execute.Sql(@"UPDATE ShortCycleWorkOrders SET PremiseId = P.Id FROM ShortCycleWorkOrders S 
                                 JOIN Premises P 
                                 ON S.Installation = P.Installation and S.PremiseNumber = P.PremiseNumber");
            Execute.Sql(@"SET IDENTITY_INSERT WorkDescriptions ON
                          IF NOT Exists(SELECT 1 from WorkDescriptions where Description = 'SERVICE LINE LEAK, CUST. SIDE-LEAD')
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
                                      ,[JobSiteCheckListRequired])
                                SELECT (SELECT MAX( WorkDescriptionID ) FROM WorkDescriptions) + 1,
                                    'SERVICE LINE LEAK, CUST. SIDE-LEAD' as Description,
                                    4 as  AssetTypeID, 
                                    1.00 as TimeToComplete, 
                                    9 as WorkCategoryID, 
                                    2 as AccountingTypeID,
                                    2 as FirstRestorationAccountingCodeID, 
                                    100 as FirstRestorationCostBreakdown, 
                                    1 as FirstRestorationProductCodeID, 
                                    null as SecondRestorationAccountingCodeID, 
                                    null as SecondRestorationCostBreakdown, 
                                    null as SecondRestorationProductCodeID, 
                                    1 as ShowBusinessUnit,
                                    0 as ShowApprovalAccounting,
                                    0 as EditOnly,
                                    0 as Revisit, 
                                    null as MaintenanceActivityType, 
                                    11 as PlantMaintenanceActivityTypeId, 
                                    1 as IsActive,
                                    0 as MarkoutRequired, 
                                    0 as MaterialsRequired, 
                                    1 as JobSiteCheckListRequired
                          IF NOT Exists(SELECT 1 from WorkDescriptions where Description = 'SERVICE LINE RENEWAL-LEAD')
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
                                      ,[JobSiteCheckListRequired])
                                SELECT (SELECT MAX( WorkDescriptionID ) FROM WorkDescriptions) + 2,
                                      'SERVICE LINE RENEWAL-LEAD' as Description,
                                      4 as AssetTypeID, 
                                      2.75 as TimeToComplete, 
                                      14 as WorkCategoryID, 
                                      1 as AccountingTypeID,
                                      1 as FirstRestorationAccountingCodeID, 
                                      100 as FirstRestorationCostBreakdown, 
                                      1 as FirstRestorationProductCodeID, 
                                      null as SecondRestorationAccountingCodeID, 
                                      null as SecondRestorationCostBreakdown, 
                                      1 as SecondRestorationProductCodeID, 
                                      0 as ShowBusinessUnit,
                                      1 as ShowApprovalAccounting,
                                      0 as EditOnly,
                                      0 as Revisit, 
                                      null as MaintenanceActivityType, 
                                      6 as PlantMaintenanceActivityTypeId, 
                                      1 as IsActive,
                                      1 as MarkoutRequired, 
                                      1 as MaterialsRequired, 
                                      1 as JobSiteCheckListRequired
                          IF NOT Exists(SELECT 1 from WorkDescriptions where Description = 'SERVICE LINE RETIRE-LEAD')
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
                                      ,[JobSiteCheckListRequired])
                                SELECT (SELECT MAX( WorkDescriptionID ) FROM WorkDescriptions) + 3,
                                      'SERVICE LINE RETIRE-LEAD' as Description,
                                      4 as AssetTypeID, 
                                      2.50 as TimeToComplete, 
                                      15 as WorkCategoryID, 
                                      3 as AccountingTypeID,
                                      26 as FirstRestorationAccountingCodeID, 
                                      100 as FirstRestorationCostBreakdown, 
                                      1 as FirstRestorationProductCodeID, 
                                      null as SecondRestorationAccountingCodeID, 
                                      null as SecondRestorationCostBreakdown, 
                                      null as SecondRestorationProductCodeID, 
                                      0 as ShowBusinessUnit,
                                      1 as ShowApprovalAccounting,
                                      0 as EditOnly,
                                      0 as Revisit, 
                                      null as MaintenanceActivityType, 
                                      6 as PlantMaintenanceActivityTypeId, 
                                      1 as IsActive,
                                      1 as MarkoutRequired, 
                                      0 as MaterialsRequired, 
                                      1 as JobSiteCheckListRequired
                          IF NOT Exists(SELECT 1 from WorkDescriptions where Description = 'SERVICE LINE REPAIR-LEAD')
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
                                      ,[JobSiteCheckListRequired])
                                SELECT (SELECT MAX( WorkDescriptionID ) FROM WorkDescriptions) + 4,
                                      'SERVICE LINE REPAIR-LEAD' as Description,
                                      4 as AssetTypeID, 
                                      '0.50' as TimeToComplete, 
                                      12 as WorkCategoryID, 
                                      2 as AccountingTypeID,
                                      2 as FirstRestorationAccountingCodeID, 
                                      100 as FirstRestorationCostBreakdown, 
                                      1 as FirstRestorationProductCodeID, 
                                      null as SecondRestorationAccountingCodeID, 
                                      null as SecondRestorationCostBreakdown, 
                                      null as SecondRestorationProductCodeID, 
                                      0 as ShowBusinessUnit,
                                      1 as ShowApprovalAccounting,
                                      0 as EditOnly,
                                      0 as Revisit, 
                                      null as MaintenanceActivityType, 
                                      11 as PlantMaintenanceActivityTypeId, 
                                      1 as IsActive,
                                      1 as MarkoutRequired, 
                                      1 as MaterialsRequired, 
                                      1 as JobSiteCheckListRequired
                          IF NOT Exists(SELECT 1 from WorkDescriptions where Description = 'WATER SERVICE RENEWAL CUST SIDE-LEAD')
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
                                      ,[JobSiteCheckListRequired])
                                SELECT (SELECT MAX( WorkDescriptionID ) FROM WorkDescriptions) + 5,
                                      'WATER SERVICE RENEWAL CUST SIDE-LEAD' as Description,
                                      4 as AssetTypeID, 
                                      '2.75' as TimeToComplete, 
                                      14 as WorkCategoryID, 
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
                                      null as MaintenanceActivityType, 
                                      5 as PlantMaintenanceActivityTypeId, 
                                      1 as IsActive,
                                      1 as MarkoutRequired, 
                                      1 as MaterialsRequired, 
                                      1 as JobSiteCheckListRequired
                          SET IDENTITY_INSERT WorkDescriptions OFF");
        }

        public override void Down()
        {
            Execute.Sql("EXEC sp_rename 'ShortCycleWorkOrders.PremiseNumber', 'Premise', 'COLUMN';");
            Delete.ForeignKeyColumn("ShortCycleWorkOrders", "PremiseId", "Premises");
        }
    }
}

