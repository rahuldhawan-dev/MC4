using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230208091158159), Tags("Production")]
    public class UpdateInterconnectionContractEndsNotificationPurposeName : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"update NotificationPurposes set Purpose = 'Interconnection Contract Ends'
                          Where ModuleId = 102 and Purpose = 'Interconnection Contract Ends In 30 Days'");
            Delete.Column("ContractEndNotificationSent").FromTable("Interconnections");
        }

        public override void Down()
        {
            Create.Column("ContractEndNotificationSent").OnTable("Interconnections").AsBoolean().NotNullable().WithDefaultValue(false);
            Execute.Sql(@"update NotificationPurposes set Purpose = 'Interconnection Contract Ends In 30 Days'
                          Where ModuleId = 102 and Purpose = 'Interconnection Contract Ends'");
        }
    }
}

