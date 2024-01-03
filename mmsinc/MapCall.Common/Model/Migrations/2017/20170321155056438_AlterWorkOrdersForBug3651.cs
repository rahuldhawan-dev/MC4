using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170321155056438), Tags("Production")]
    public class AlterWorkOrdersForBug3651 : Migration
    {
        public override void Up()
        {
            Alter.Table("WorkOrders").AddColumn("MeterSerialNumber").AsAnsiString(30).Nullable();
            Alter.Table("WorkOrders").AddColumn("DeviceLocation").AsAnsiString(30).Nullable();
            Alter.Table("WorkOrders").AddColumn("Installation").AsAnsiString(10).Nullable();
            Alter.Table("WorkOrders").AddColumn("SAPEquipmentNumber").AsAnsiString(18).Nullable();
        }

        public override void Down()
        {
            Delete.Column("MeterSerialNumber").FromTable("WorkOrders");
            Delete.Column("DeviceLocation").FromTable("WorkOrders");
            Delete.Column("Installation").FromTable("WorkOrders");
            //Delete.Column("SAPEquipmentNumber").FromTable("WorkOrders");
        }
    }
}
