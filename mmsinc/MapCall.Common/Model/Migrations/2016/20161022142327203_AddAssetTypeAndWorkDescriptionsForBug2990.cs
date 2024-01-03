using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161022142327203), Tags("Production")]
    public class AddAssetTypeAndWorkDescriptionsForBug2990 : Migration
    {
        public override void Up()
        {
            Execute.Sql(" SET IDENTITY_INSERT AssetTypes ON;" +
                        " IF (SELECT COUNT(1) FROM AssetTypes WHERE Description = 'Main Crossing') = 0 INSERT INTO AssetTypes(AssetTypeID, Description) Values(12, 'Main Crossing');" +
                        " SET IDENTITY_INSERT AssetTypes OFF;" +
                        " SET IDENTITY_INSERT WorkDescriptions ON;" +
                        " IF (SELECT COUNT(1) FROM WorkDescriptions WHERE WorkDescriptionID = 223) = 0 " +
                        " INSERT INTO WorkDescriptions([WorkDescriptionID], [Description], [AssetTypeID], [TimeToComplete], [WorkCategoryID], [AccountingTypeID], [FirstRestorationAccountingCodeID], [FirstRestorationCostBreakdown], [FirstRestorationProductCodeID], [SecondRestorationAccountingCodeID], [SecondRestorationCostBreakdown], [SecondRestorationProductCodeID], [ShowBusinessUnit], [ShowApprovalAccounting], [EditOnly], [Revisit])" +
                        " SELECT 223, N'Crossing Investigation', 12, 1, 9, 2, 2, 100, 1, NULL, NULL, NULL, 0, 1, 0, 0 UNION ALL" +
                        " SELECT 224, N'Crossing Repair',		12, 3, 49, 2, 2, 100, 1, NULL, NULL, NULL, 0, 1, 0, 0 UNION ALL" +
                        " SELECT 225, N'Crossing Retirement',	12, 3, 24, 1, 26, 100, 1, NULL, NULL, NULL, 0, 1, 0, 0 UNION ALL" +
                        " SELECT 226, N'Crossing Installation',	12, 5, 20, 1, 1, 100, 1, NULL, NULL, NULL, 0, 1, 0, 0" +
                        " SET IDENTITY_INSERT WorkDescriptions OFF;");
            Execute.Sql(
                " IF (SELECT COUNT(1) FROM OperatingCenterAssetTypes WHERE AssetTypeID = 12) = 0 INSERT INTO OperatingCenterAssetTypes SELECT OperatingCenterID, 12 FROM OperatingCenters WHERE WorkOrdersEnabled = 1");

            Alter.Table("WorkOrders").AddForeignKeyColumn("MainCrossingId", "MainCrossings", "MainCrossingID");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("WorkOrders", "MainCrossingId", "MainCrossings", "MainCrossingID");
        }
    }
}
