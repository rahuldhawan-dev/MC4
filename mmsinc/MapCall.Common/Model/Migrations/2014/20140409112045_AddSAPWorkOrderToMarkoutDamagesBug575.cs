using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140409112045), Tags("Production")]
    public class AddSAPWorkOrderToMarkoutDamagesBug575 : Migration
    {
        public const int MAX_SAP_WORK_ORDER_ID_LENGTH = 50;

        public override void Up()
        {
            Alter.Table("MarkoutDamages")
                 .AddColumn("SAPWorkOrderId")
                 .AsString(MAX_SAP_WORK_ORDER_ID_LENGTH)
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("SAPWorkOrderId").FromTable("MarkoutDamages");
        }
    }
}
