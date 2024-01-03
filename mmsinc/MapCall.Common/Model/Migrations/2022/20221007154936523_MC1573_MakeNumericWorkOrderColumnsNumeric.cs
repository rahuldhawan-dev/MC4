using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221007154936523), Tags("Production")]
    public class MC1573_MakeNumericWorkOrderColumnsNumeric : Migration
    {
        public override void Up()
        {
            // any DeviceLocation with a value longer than 11 chars is not convertable to a number
            Execute.Sql(@"
UPDATE WorkOrders
SET DeviceLocation = NULL
WHERE IsNumeric(DeviceLocation) = 0
OR Len(DeviceLocation) > 11;");
            Execute.Sql(@"
UPDATE WorkOrders
SET Installation = NULL
WHERE IsNumeric(Installation) = 0
OR Installation = '\';");
            Execute.Sql(@"
UPDATE WorkOrders
SET SAPEquipmentNumber = NULL
WHERE IsNumeric(SAPEquipmentNumber) = 0;");

            Alter.Table("WorkOrders")
                 .AlterColumn("DeviceLocation").AsInt64().Nullable()
                 .AlterColumn("Installation").AsInt64().Nullable()
                 .AlterColumn("SAPEquipmentNumber").AsInt64().Nullable();
        }

        public override void Down()
        {
            Alter.Table("WorkOrders")
                 .AlterColumn("DeviceLocation").AsString().Nullable()
                 .AlterColumn("Installation").AsString().Nullable()
                 .AlterColumn("SAPEquipmentNumber").AsString().Nullable();
        }
    }
}

