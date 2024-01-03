using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180316110438605), Tags("Production")]
    public class AddActiveMQStatusToShortCycleWorkOrderForWO0000000230579 : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders")
                 .AddColumn("ActiveMQStatus").AsCustom("varchar(max)")
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("ActiveMQStatus").FromTable("ShortCycleWorkOrders");
        }
    }
}
