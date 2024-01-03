using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190225131619583), Tags("Production")]
    public class MC1002AddNewFieldsForSAPwsdl : Migration
    {
        public override void Up()
        {
            //RemoveReplace/Misc
            Alter.Table("ShortCycleWorkOrderCompletions")
                 .AddColumn("Register1ReasonCode").AsAnsiString(30).Nullable()
                 .AddColumn("Register2ReasonCode").AsAnsiString(30).Nullable()
                 .AddColumn("InvestigationExpiryDate").AsDateTime().Nullable()
                 .AddColumn("NotificationItemText").AsAnsiString(40).Nullable();
        }

        public override void Down()
        {
            Delete.Column("NotificationItemText").FromTable("ShortCycleWorkOrderCompletions");
            Delete.Column("InvestigationExpiryDate").FromTable("ShortCycleWorkOrderCompletions");
            Delete.Column("Register2ReasonCode").FromTable("ShortCycleWorkOrderCompletions");
            Delete.Column("Register1ReasonCode").FromTable("ShortCycleWorkOrderCompletions");
        }
    }
}
