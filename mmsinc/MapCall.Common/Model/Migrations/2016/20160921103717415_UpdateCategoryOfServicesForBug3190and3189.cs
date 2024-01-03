using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160921103717415), Tags("Production")]
    public class UpdateCategoryOfServicesForBug3190and3189 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "insert into WorkDescriptions(Description, AssetTypeID, TimeToComplete, WorkCategoryID, AccountingTypeID, FirstRestorationAccountingCodeID, FirstRestorationCostBreakdown, FirstRestorationProductCodeID, ShowApprovalAccounting, ShowBusinessUnit, EditOnly)" +
                "select 'WATER SERVICE RENEWAL CUST SIDE', (select AssetTypeID from AssetTypes where Description = 'Service'), 2.75, " +
                "(select WorkCategoryId from WorkCategories where Description = 'Service Line Replacement'), " +
                "(select accountingTypeId from AccountingTypes where Description = 'Capital'), " +
                "(select top 1 RestorationAccountingCodeID from RestorationAccountingCodes where Code = '62520700' and SubCode = '.'), " +
                "100, 1, 1, 0, 0");
            Execute.Sql("insert into ServiceCategories Values('Water Service Renewal Cust Side')");

            Execute.Sql(
                "update WorkDescriptions set Description = 'SERVICE OFF AT MAIN' where WorkDescriptionID = 169;" +
                "update WorkDescriptions set Description = 'SERVICE OFF AT CURB STOP' where WorkDescriptionID = 170;" +
                "update WorkDescriptions set Description = 'SERVICE OFF AT METER PIT' where WorkDescriptionID = 171;" +
                "update WorkDescriptions set Description = 'VALVE TURNED OFF' where WorkDescriptionID = 172;" +
                "update WorkDescriptions set Description = 'HYDRANT TURNED OFF' where WorkDescriptionID = 175;" +
                "update WorkOrders SET PurposeID =  (SELECT WorkOrderPurposeID from WorkOrderPurposes where Description = 'Hurricane Sandy') WHERE WorkDescriptionID in (169, 170, 171, 172, 175);" +
                "update WorkOrders SET WorkDescriptionID = 74, PurposeID = (SELECT WorkOrderPurposeID from WorkOrderPurposes where Description = 'Hurricane Sandy') WHERE WorkDescriptionID = 173;" +
                "update WorkOrders SET WorkDescriptionID = 80, PurposeID = (SELECT WorkOrderPurposeID from WorkOrderPurposes where Description = 'Hurricane Sandy') WHERE WorkDescriptionID = 174;" +
                "update WorkOrders SET WorkDescriptionID = 30, PurposeID = (SELECT WorkOrderPurposeID from WorkOrderPurposes where Description = 'Hurricane Sandy') WHERE WorkDescriptionID = 176;" +
                "update WorkOrders SET WorkDescriptionID = 118,PurposeID = (SELECT WorkOrderPurposeID from WorkOrderPurposes where Description = 'Hurricane Sandy') WHERE WorkDescriptionID = 177;" +
                "update WorkOrders SET WorkDescriptionID = 71, PurposeID = (SELECT WorkOrderPurposeID from WorkOrderPurposes where Description = 'Hurricane Sandy') WHERE WorkDescriptionID = 178;" +
                "update WorkOrders SET WorkDescriptionID = 5,  PurposeID = (SELECT WorkOrderPurposeID from WorkOrderPurposes where Description = 'Hurricane Sandy') WHERE WorkDescriptionID = 179;" +
                "update WorkOrders SET WorkDescriptionID = 50, PurposeID = (SELECT WorkOrderPurposeID from WorkOrderPurposes where Description = 'Hurricane Sandy') WHERE WorkDescriptionID = 190;" +
                "update WorkOrders SET WorkDescriptionID = 72, PurposeID = (SELECT WorkOrderPurposeID from WorkOrderPurposes where Description = 'Hurricane Sandy') WHERE WorkDescriptionID = 191;" +
                "update WorkOrders SET WorkDescriptionID = 14, PurposeID = (SELECT WorkOrderPurposeID from WorkOrderPurposes where Description = 'Hurricane Sandy') WHERE WorkDescriptionID = 192;" +
                "update WorkOrders SET WorkDescriptionID = 59, PurposeID = (SELECT WorkOrderPurposeID from WorkOrderPurposes where Description = 'Hurricane Sandy') WHERE WorkDescriptionID = 193;" +
                "update WorkOrders SET WorkDescriptionID = 132,PurposeID = (SELECT WorkOrderPurposeID from WorkOrderPurposes where Description = 'Hurricane Sandy') WHERE WorkDescriptionID = 194;" +
                "update WorkOrders SET WorkDescriptionID = 76, PurposeID = (SELECT WorkOrderPurposeID from WorkOrderPurposes where Description = 'Hurricane Sandy') WHERE WorkDescriptionID = 195;" +
                "update WorkOrders SET WorkDescriptionID = 60, PurposeID = (SELECT WorkOrderPurposeID from WorkOrderPurposes where Description = 'Hurricane Sandy') WHERE WorkDescriptionID = 196;" +
                "update WorkOrders SET WorkDescriptionID = 50, PurposeID = (SELECT WorkOrderPurposeID from WorkOrderPurposes where Description = 'Hurricane Sandy') WHERE WorkDescriptionID = 197;" +
                "update WorkOrderDescriptionChanges set FromWorkDescriptionID =	74	WHERE FromWorkDescriptionID	= 173;" +
                "update WorkOrderDescriptionChanges set ToWorkDescriptionID = 74  WHERE ToWorkDescriptionID = 173;" +
                "update WorkOrderDescriptionChanges set FromWorkDescriptionID = 80  WHERE FromWorkDescriptionID = 174;" +
                "update WorkOrderDescriptionChanges set ToWorkDescriptionID = 80  WHERE ToWorkDescriptionID = 174;" +
                "update WorkOrderDescriptionChanges set FromWorkDescriptionID = 30  WHERE FromWorkDescriptionID = 176;" +
                "update WorkOrderDescriptionChanges set ToWorkDescriptionID = 30  WHERE ToWorkDescriptionID = 176;" +
                "update WorkOrderDescriptionChanges set FromWorkDescriptionID = 118 WHERE FromWorkDescriptionID = 177;" +
                "update WorkOrderDescriptionChanges set ToWorkDescriptionID = 118 WHERE ToWorkDescriptionID = 177;" +
                "update WorkOrderDescriptionChanges set FromWorkDescriptionID = 71  WHERE FromWorkDescriptionID = 178;" +
                "update WorkOrderDescriptionChanges set ToWorkDescriptionID = 71  WHERE ToWorkDescriptionID = 178;" +
                "update WorkOrderDescriptionChanges set FromWorkDescriptionID = 5   WHERE FromWorkDescriptionID = 179;" +
                "update WorkOrderDescriptionChanges set ToWorkDescriptionID = 5   WHERE ToWorkDescriptionID = 179;" +
                "update WorkOrderDescriptionChanges set FromWorkDescriptionID = 50  WHERE FromWorkDescriptionID = 190;" +
                "update WorkOrderDescriptionChanges set ToWorkDescriptionID = 50  WHERE ToWorkDescriptionID = 190;" +
                "update WorkOrderDescriptionChanges set FromWorkDescriptionID = 72  WHERE FromWorkDescriptionID = 191;" +
                "update WorkOrderDescriptionChanges set ToWorkDescriptionID = 72  WHERE ToWorkDescriptionID = 191;" +
                "update WorkOrderDescriptionChanges set FromWorkDescriptionID = 14  WHERE FromWorkDescriptionID = 192;" +
                "update WorkOrderDescriptionChanges set ToWorkDescriptionID = 14  WHERE ToWorkDescriptionID = 192;" +
                "update WorkOrderDescriptionChanges set FromWorkDescriptionID = 59  WHERE FromWorkDescriptionID = 193;" +
                "update WorkOrderDescriptionChanges set ToWorkDescriptionID = 59  WHERE ToWorkDescriptionID = 193;" +
                "update WorkOrderDescriptionChanges set FromWorkDescriptionID = 132 WHERE FromWorkDescriptionID = 194;" +
                "update WorkOrderDescriptionChanges set ToWorkDescriptionID = 132 WHERE ToWorkDescriptionID = 194;" +
                "update WorkOrderDescriptionChanges set FromWorkDescriptionID = 76  WHERE FromWorkDescriptionID = 195;" +
                "update WorkOrderDescriptionChanges set ToWorkDescriptionID = 76  WHERE ToWorkDescriptionID = 195;" +
                "update WorkOrderDescriptionChanges set FromWorkDescriptionID = 60  WHERE FromWorkDescriptionID = 196;" +
                "update WorkOrderDescriptionChanges set ToWorkDescriptionID = 60  WHERE ToWorkDescriptionID = 196;" +
                "update WorkOrderDescriptionChanges set FromWorkDescriptionID = 50  WHERE FromWorkDescriptionID = 197;" +
                "update WorkOrderDescriptionChanges set ToWorkDescriptionID = 50  WHERE ToWorkDescriptionID = 197;" +
                "DELETE FROM WorkDescriptions WHERE WorkDescriptionID IN (173,174,176,177,178,179,190,191,192,193,194,195,196,197)");
        }

        public override void Down()
        {
            // can't down the new work descriptions, newly created orders may be linked to them
            Execute.Sql(
                "SET IDENTITY_INSERT [dbo].[WorkDescriptions] ON;" +
                "INSERT INTO[dbo].[WorkDescriptions]([WorkDescriptionID], [Description], [AssetTypeID], [TimeToComplete], [WorkCategoryID], [AccountingTypeID], [FirstRestorationAccountingCodeID], [FirstRestorationCostBreakdown], [FirstRestorationProductCodeID], [SecondRestorationAccountingCodeID], [SecondRestorationCostBreakdown], [SecondRestorationProductCodeID], [ShowBusinessUnit], [ShowApprovalAccounting], [EditOnly], [Revisit])" +
                "SELECT 173, N'MAIN REPAIR-STORM RESTORATION', 3, 4.00, 3, 2, 1, 100, 1, NULL, NULL, NULL, 1, 1, 1, 0 UNION ALL " +
                "SELECT 174, N'MAIN REPLACE - STORM RESTORATION', 3, 8.00, 3, 1, 1, 100, 1, NULL, NULL, NULL, 1, 1, 1, 0 UNION ALL " +
                "SELECT 176, N'HYDRANT REPLACE - STORM RESTORATION', 2, 4.00, 5, 1, 1, 100, 1, NULL, NULL, NULL, 1, 1, 0, 0 UNION ALL " +
                "SELECT 177, N'VALVE INSTALLATION - STORM RESTORATION', 1, 4.00, 2, 1, 1, 100, 1, NULL, NULL, NULL, 1, 1, 0, 0 UNION ALL " +
                "SELECT 178, N'VALVE REPLACEMENT - STORM RESTORATION', 1, 4.00, 2, 1, 1, 100, 1, NULL, NULL, NULL, 1, 1, 0, 0 UNION ALL " +
                "SELECT 179, N'CURB BOX LOCATE - STORM RESTORATION', 4, 0.50, 4, 2, 1, 100, 1, NULL, NULL, NULL, 1, 1, 0, 0 UNION ALL " +
                "SELECT 190, N'METER PIT LOCATE - STORM RESTORATION', 4, 0.50, 4, 2, 1, 100, 1, NULL, NULL, NULL, 1, 1, 0, 0 UNION ALL " +
                "SELECT 191, N'VALVE RETIREMENT - STORM RESTORATION', 1, 2.00, 17, 3, 26, 100, 1, NULL, NULL, NULL, 1, 1, 0, 0 UNION ALL " +
                "SELECT 192, N'EXCAVATE METER PIT- STORM RESTORATION', 4, 0.50, 1, 2, 1, 100, 1, NULL, NULL, NULL, 1, 1, 0, 0 UNION ALL " +
                "SELECT 193, N'SERVICE LINE RENEWAL - STORM RESTORATION', 4, 4.00, 1, 1, 1, 100, 1, NULL, NULL, NULL, 1, 1, 0, 0 UNION ALL " +
                "SELECT 194, N'CURB BOX REPLACEMENT - STORM RESTORATION', 4, 1.00, 28, 1, 1, 100, 1, NULL, NULL, NULL, 1, 1, 0, 0 UNION ALL " +
                "SELECT 195, N'WATER MAIN RETIREMENT - STORM RESTORATION', 4, 4.00, 3, 1, 1, 100, 1, NULL, NULL, NULL, 1, 1, 0, 0 UNION ALL " +
                "SELECT 196, N'SERVICE LINE RETIREMENT - STORM RESTORATION', 4, 2.00, 4, 1, 1, 100, 1, NULL, NULL, NULL, 1, 1, 0, 0 UNION ALL " +
                "SELECT 197, N'FRAME AND COVER REPLACE - STORM RESTORATION', 4, 0.50, 4, 2, 1, 100, 1, NULL, NULL, NULL, 1, 1, 0, 0" +
                "SET IDENTITY_INSERT[dbo].[WorkDescriptions] OFF;");

            Execute.Sql(
                "UPDATE WorkDescriptions SET Description = 'SERVICE OFF AT MAIN-STORM RESTORATION' WHERE WorkDescriptionID = 169;" +
                "UPDATE WorkDescriptions SET Description = 'SERVICE OFF AT CURB STOP-STORM RESTORATION' WHERE WorkDescriptionID = 170;" +
                "UPDATE WorkDescriptions SET Description = 'SERVICE OFF AT METER PIT-STORM RESTORATION' WHERE WorkDescriptionID = 171;" +
                "UPDATE WorkDescriptions SET Description = 'VALVE TURNED OFF STORM RESTORATION' WHERE WorkDescriptionID = 172;" +
                "UPDATE WorkDescriptions SET Description = 'HYDRANT TURNED OFF - STORM RESTORATION' WHERE WorkDescriptionID = 175;" +
                "UPDATE WorkOrders SET WorkDescriptionID = 173   WHERE WorkDescriptionID = 74    AND PurposeID = (SELECT WorkOrderPurposeID FROM WorkOrderPurposes WHERE Description = 'Hurricane Sandy');" +
                "UPDATE WorkOrders SET WorkDescriptionID = 174   WHERE WorkDescriptionID = 80    AND PurposeID = (SELECT WorkOrderPurposeID FROM WorkOrderPurposes WHERE Description = 'Hurricane Sandy');" +
                "UPDATE WorkOrders SET WorkDescriptionID = 176   WHERE WorkDescriptionID = 30    AND PurposeID = (SELECT WorkOrderPurposeID FROM WorkOrderPurposes WHERE Description = 'Hurricane Sandy');" +
                "UPDATE WorkOrders SET WorkDescriptionID = 177   WHERE WorkDescriptionID = 118   AND PurposeID = (SELECT WorkOrderPurposeID FROM WorkOrderPurposes WHERE Description = 'Hurricane Sandy');" +
                "UPDATE WorkOrders SET WorkDescriptionID = 178   WHERE WorkDescriptionID = 71    AND PurposeID = (SELECT WorkOrderPurposeID FROM WorkOrderPurposes WHERE Description = 'Hurricane Sandy');" +
                "UPDATE WorkOrders SET WorkDescriptionID = 179   WHERE WorkDescriptionID = 5     AND PurposeID = (SELECT WorkOrderPurposeID FROM WorkOrderPurposes WHERE Description = 'Hurricane Sandy');" +
                "UPDATE WorkOrders SET WorkDescriptionID = 190   WHERE WorkDescriptionID = 50    AND PurposeID = (SELECT WorkOrderPurposeID FROM WorkOrderPurposes WHERE Description = 'Hurricane Sandy');" +
                "UPDATE WorkOrders SET WorkDescriptionID = 191   WHERE WorkDescriptionID = 72    AND PurposeID = (SELECT WorkOrderPurposeID FROM WorkOrderPurposes WHERE Description = 'Hurricane Sandy');" +
                "UPDATE WorkOrders SET WorkDescriptionID = 192   WHERE WorkDescriptionID = 14    AND PurposeID = (SELECT WorkOrderPurposeID FROM WorkOrderPurposes WHERE Description = 'Hurricane Sandy');" +
                "UPDATE WorkOrders SET WorkDescriptionID = 193   WHERE WorkDescriptionID = 59    AND PurposeID = (SELECT WorkOrderPurposeID FROM WorkOrderPurposes WHERE Description = 'Hurricane Sandy');" +
                "UPDATE WorkOrders SET WorkDescriptionID = 194   WHERE WorkDescriptionID = 132   AND PurposeID = (SELECT WorkOrderPurposeID FROM WorkOrderPurposes WHERE Description = 'Hurricane Sandy');" +
                "UPDATE WorkOrders SET WorkDescriptionID = 195   WHERE WorkDescriptionID = 76    AND PurposeID = (SELECT WorkOrderPurposeID FROM WorkOrderPurposes WHERE Description = 'Hurricane Sandy');" +
                "UPDATE WorkOrders SET WorkDescriptionID = 196   WHERE WorkDescriptionID = 60    AND PurposeID = (SELECT WorkOrderPurposeID FROM WorkOrderPurposes WHERE Description = 'Hurricane Sandy');" +
                "UPDATE WorkOrders SET WorkDescriptionID = 197   WHERE WorkDescriptionID = 50    AND PurposeID = (SELECT WorkOrderPurposeID FROM WorkOrderPurposes WHERE Description = 'Hurricane Sandy');");
        }
    }
}
