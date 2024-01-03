using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190808133238223), Tags("Production")]
    public class MC1532AddDoNotUnscheduleToShortCycleWorkOrders : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders").AddColumn("DoNotUnschedule").AsBoolean().WithDefaultValue(false)
                 .NotNullable();
        }

        public override void Down()
        {
            Delete.Column("DoNotUnschedule").FromTable("ShortCycleWorkOrders");
        }
    }
}
