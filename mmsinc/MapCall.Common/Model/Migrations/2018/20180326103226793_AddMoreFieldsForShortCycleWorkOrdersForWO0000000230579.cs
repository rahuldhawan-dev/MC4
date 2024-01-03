using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180326103226793), Tags("Production")]
    public class AddMoreFieldsForShortCycleWorkOrdersForWO0000000230579 : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders")
                 .AddColumn("PhoneAhead").AsBoolean().NotNullable().WithDefaultValue(false)
                 .AddColumn("CustomerAtHome").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("CustomerAtHome").FromTable("ShortCycleWorkOrders");
            Delete.Column("PhoneAhead").FromTable("ShortCycleWorkOrders");
        }
    }
}
