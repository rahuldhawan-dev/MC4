using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180614104705619), Tags("Production")]
    public class AddAnotherFieldToShortCycleWorkOrders : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders")
                 .AddColumn("NotificationLongText").AsCustom("text").Nullable()
                 .AddColumn("AppointmentStart").AsAnsiString()
                 .Nullable()
                 .AddColumn("AppointmentEnd").AsAnsiString()
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("NotificationLongText").FromTable("ShortCycleWorkOrders");
            Delete.Column("AppointmentStart").FromTable("ShortCycleWorkOrders");
            Delete.Column("AppointmentEnd").FromTable("ShortCycleWorkOrders");
        }
    }
}
