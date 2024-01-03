using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230905134246486), Tags("Production")]
    public class Mc5784_AddSewerWorkDescriptions : Migration
    {
        private struct Sql
        {
            public const string
                SQL_UP = @"
INSERT INTO [dbo].[WorkDescriptions] ([Description],[AssetTypeID],[TimeToComplete],[WorkCategoryID],[AccountingTypeID],[FirstRestorationAccountingCodeID],[FirstRestorationCostBreakdown],[FirstRestorationProductCodeID],[SecondRestorationAccountingCodeID],[SecondRestorationCostBreakdown],[SecondRestorationProductCodeID],[ShowBusinessUnit],[ShowApprovalAccounting],[EditOnly],[Revisit],[MaintenanceActivityType],[PlantMaintenanceActivityTypeId],[IsActive],[MarkoutRequired],[MaterialsRequired],[JobSiteCheckListRequired],[DigitalAsBuiltRequired])
VALUES ('SEWER OPENING FRAME AND COVER REPLACE',5,4,40,1,1,100,1,NULL,NULL,1,0,1,0,0,NULL,4,1,1,1,1,1)
INSERT INTO [dbo].[WorkDescriptions] ([Description],[AssetTypeID],[TimeToComplete],[WorkCategoryID],[AccountingTypeID],[FirstRestorationAccountingCodeID],[FirstRestorationCostBreakdown],[FirstRestorationProductCodeID],[SecondRestorationAccountingCodeID],[SecondRestorationCostBreakdown],[SecondRestorationProductCodeID],[ShowBusinessUnit],[ShowApprovalAccounting],[EditOnly],[Revisit],[MaintenanceActivityType],[PlantMaintenanceActivityTypeId],[IsActive],[MarkoutRequired],[MaterialsRequired],[JobSiteCheckListRequired],[DigitalAsBuiltRequired])
VALUES ('SEWER OPENING STRUCTURAL LINING',5,8,40,1,1,100,1,NULL,NULL,1,0,1,0,0,NULL,4,1,1,1,1,1)
INSERT INTO [dbo].[WorkDescriptions] ([Description],[AssetTypeID],[TimeToComplete],[WorkCategoryID],[AccountingTypeID],[FirstRestorationAccountingCodeID],[FirstRestorationCostBreakdown],[FirstRestorationProductCodeID],[SecondRestorationAccountingCodeID],[SecondRestorationCostBreakdown],[SecondRestorationProductCodeID],[ShowBusinessUnit],[ShowApprovalAccounting],[EditOnly],[Revisit],[MaintenanceActivityType],[PlantMaintenanceActivityTypeId],[IsActive],[MarkoutRequired],[MaterialsRequired],[JobSiteCheckListRequired],[DigitalAsBuiltRequired])
VALUES ('SEWER OPENING CLEANING',5,1,39,2,2,100,1,NULL,NULL,NULL,1,0,0,0,NULL,12,1,0,0,1,0)
INSERT INTO [dbo].[WorkDescriptions] ([Description],[AssetTypeID],[TimeToComplete],[WorkCategoryID],[AccountingTypeID],[FirstRestorationAccountingCodeID],[FirstRestorationCostBreakdown],[FirstRestorationProductCodeID],[SecondRestorationAccountingCodeID],[SecondRestorationCostBreakdown],[SecondRestorationProductCodeID],[ShowBusinessUnit],[ShowApprovalAccounting],[EditOnly],[Revisit],[MaintenanceActivityType],[PlantMaintenanceActivityTypeId],[IsActive],[MarkoutRequired],[MaterialsRequired],[JobSiteCheckListRequired],[DigitalAsBuiltRequired])
VALUES ('SEWER OPENING COVER REPAIR',5,2,39,2,2,100,1,NULL,NULL,NULL,1,0,0,0,NULL,12,1,0,0,1,0)
INSERT INTO [dbo].[WorkDescriptions] ([Description],[AssetTypeID],[TimeToComplete],[WorkCategoryID],[AccountingTypeID],[FirstRestorationAccountingCodeID],[FirstRestorationCostBreakdown],[FirstRestorationProductCodeID],[SecondRestorationAccountingCodeID],[SecondRestorationCostBreakdown],[SecondRestorationProductCodeID],[ShowBusinessUnit],[ShowApprovalAccounting],[EditOnly],[Revisit],[MaintenanceActivityType],[PlantMaintenanceActivityTypeId],[IsActive],[MarkoutRequired],[MaterialsRequired],[JobSiteCheckListRequired],[DigitalAsBuiltRequired])
VALUES ('SEWER LATERAL-CHECK VALVE REPLACE',6,8,36,1,1,100,1,NULL,NULL,1,0,1,0,0,NULL,6,1,1,1,1,1)
INSERT INTO [dbo].[WorkDescriptions] ([Description],[AssetTypeID],[TimeToComplete],[WorkCategoryID],[AccountingTypeID],[FirstRestorationAccountingCodeID],[FirstRestorationCostBreakdown],[FirstRestorationProductCodeID],[SecondRestorationAccountingCodeID],[SecondRestorationCostBreakdown],[SecondRestorationProductCodeID],[ShowBusinessUnit],[ShowApprovalAccounting],[EditOnly],[Revisit],[MaintenanceActivityType],[PlantMaintenanceActivityTypeId],[IsActive],[MarkoutRequired],[MaterialsRequired],[JobSiteCheckListRequired],[DigitalAsBuiltRequired])
VALUES ('SEWER LATERAL-CHECK VALVE INSTALLATION',6,8,45,1,1,100,1,NULL,NULL,NULL,0,1,0,0,NULL,6,1,1,1,1,1)
INSERT INTO [dbo].[WorkDescriptions] ([Description],[AssetTypeID],[TimeToComplete],[WorkCategoryID],[AccountingTypeID],[FirstRestorationAccountingCodeID],[FirstRestorationCostBreakdown],[FirstRestorationProductCodeID],[SecondRestorationAccountingCodeID],[SecondRestorationCostBreakdown],[SecondRestorationProductCodeID],[ShowBusinessUnit],[ShowApprovalAccounting],[EditOnly],[Revisit],[MaintenanceActivityType],[PlantMaintenanceActivityTypeId],[IsActive],[MarkoutRequired],[MaterialsRequired],[JobSiteCheckListRequired],[DigitalAsBuiltRequired])
VALUES ('SEWER LATERAL-INSTALL PLUG AT CLEANOUT',6,1,38,2,2,100,1,NULL,NULL,NULL,1,0,0,0,NULL,11,1,0,0,1,0)
INSERT INTO [dbo].[WorkDescriptions] ([Description],[AssetTypeID],[TimeToComplete],[WorkCategoryID],[AccountingTypeID],[FirstRestorationAccountingCodeID],[FirstRestorationCostBreakdown],[FirstRestorationProductCodeID],[SecondRestorationAccountingCodeID],[SecondRestorationCostBreakdown],[SecondRestorationProductCodeID],[ShowBusinessUnit],[ShowApprovalAccounting],[EditOnly],[Revisit],[MaintenanceActivityType],[PlantMaintenanceActivityTypeId],[IsActive],[MarkoutRequired],[MaterialsRequired],[JobSiteCheckListRequired],[DigitalAsBuiltRequired])
VALUES ('SEWER LATERAL-REMOVE PLUG AT CLEANOUT',6,1,38,2,2,100,1,NULL,NULL,NULL,1,0,0,0,NULL,11,1,0,0,1,0)
INSERT INTO [dbo].[WorkDescriptions] ([Description],[AssetTypeID],[TimeToComplete],[WorkCategoryID],[AccountingTypeID],[FirstRestorationAccountingCodeID],[FirstRestorationCostBreakdown],[FirstRestorationProductCodeID],[SecondRestorationAccountingCodeID],[SecondRestorationCostBreakdown],[SecondRestorationProductCodeID],[ShowBusinessUnit],[ShowApprovalAccounting],[EditOnly],[Revisit],[MaintenanceActivityType],[PlantMaintenanceActivityTypeId],[IsActive],[MarkoutRequired],[MaterialsRequired],[JobSiteCheckListRequired],[DigitalAsBuiltRequired])
VALUES ('SEWER LATERAL-CHECK VALVE REPAIR',6,8,45,2,2,100,1,NULL,NULL,NULL,1,0,0,0,NULL,11,1,1,0,1,0)
INSERT INTO [dbo].[WorkDescriptions] ([Description],[AssetTypeID],[TimeToComplete],[WorkCategoryID],[AccountingTypeID],[FirstRestorationAccountingCodeID],[FirstRestorationCostBreakdown],[FirstRestorationProductCodeID],[SecondRestorationAccountingCodeID],[SecondRestorationCostBreakdown],[SecondRestorationProductCodeID],[ShowBusinessUnit],[ShowApprovalAccounting],[EditOnly],[Revisit],[MaintenanceActivityType],[PlantMaintenanceActivityTypeId],[IsActive],[MarkoutRequired],[MaterialsRequired],[JobSiteCheckListRequired],[DigitalAsBuiltRequired])
VALUES ('SEWER MAIN-STRUCTURAL LINING',7,8,30,1,1,100,1,NULL,NULL,NULL,0,1,0,0,NULL,1,1,1,1,1,0)
GO";
        }

        public override void Up()
        {
            Execute.Sql(Sql.SQL_UP);
        }

        public override void Down() {}
    }
}

