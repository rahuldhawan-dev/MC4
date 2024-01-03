using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180410104653856), Tags("Production")]
    public class AddMoreFieldsToShortCycleWorkOrderCompletionOtherForWO230579 : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrderCompletionOthers")
                 .AddColumn("Installation").AsAnsiString()
                 .Nullable()
                 .AddColumn("ActivityReason").AsAnsiString()
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("Installation").FromTable("ShortCycleWorkOrderCompletionOthers");
            Delete.Column("ActivityReason").FromTable("ShortCycleWorkOrderCompletionOthers");
        }
    }
}
