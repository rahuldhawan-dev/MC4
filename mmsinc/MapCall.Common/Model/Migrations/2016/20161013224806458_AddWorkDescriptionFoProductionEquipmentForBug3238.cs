using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161013224806458), Tags("Production")]
    public class AddWorkDescriptionFoProductionEquipmentForBug3238 : Migration
    {
        public override void Up()
        {
            Execute.Sql(" SET IDENTITY_INSERT WorkDescriptions ON;" +
                        " IF (SELECT COUNT(1) FROM WorkDescriptions WHERE WorkDescriptionID = 227) = 0 " +
                        " INSERT INTO WorkDescriptions([WorkDescriptionID], [Description], [AssetTypeID], [TimeToComplete], [WorkCategoryID], [AccountingTypeID], [FirstRestorationAccountingCodeID], [FirstRestorationCostBreakdown], [FirstRestorationProductCodeID], [SecondRestorationAccountingCodeID], [SecondRestorationCostBreakdown], [SecondRestorationProductCodeID], [ShowBusinessUnit], [ShowApprovalAccounting], [EditOnly], [Revisit])" +
                        " SELECT 227, N'Preventive Maintenance', 9, 2, 55, 2, 1, 100, 1, NULL, NULL, NULL, 0, 0, 0, 0 " +
                        " SET IDENTITY_INSERT WorkDescriptions OFF;");
        }

        public override void Down() { }
    }
}
