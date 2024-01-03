using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180413141935320), Tags("Production")]
    public class AddCancelFieldsForShortCycleWorkOrderFor230579 : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders")
                 .AddColumn("OperationNumber").AsAnsiString(4).Nullable()
                 .AddColumn("ModifiedBy").AsAnsiString().Nullable()
                 .AddColumn("TimeModified").AsAnsiString().Nullable()
                 .AddColumn("ReasonCode").AsAnsiString(35).Nullable()
                 .AddColumn("ReasonCodeComments").AsCustom("text").Nullable();
        }

        public override void Down()
        {
            Delete.Column("OperationNumber").FromTable("ShortCycleWorkOrders");
            Delete.Column("ModifiedBy").FromTable("ShortCycleWorkOrders");
            Delete.Column("TimeModified").FromTable("ShortCycleWorkOrders");
            Delete.Column("ReasonCode").FromTable("ShortCycleWorkOrders");
            Delete.Column("ReasonCodeComments").FromTable("ShortCycleWorkOrders");
        }
    }
}
