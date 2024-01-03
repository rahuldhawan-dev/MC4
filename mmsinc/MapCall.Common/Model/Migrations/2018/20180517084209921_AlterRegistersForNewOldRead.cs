using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180517084209921), Tags("Production")]
    public class AlterRegistersForNewOldRead : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrderCompletionRegisters")
                 .AddColumn("NewRead").AsAnsiString()
                 .Nullable();
            Rename.Column("Read").OnTable("ShortCycleWorkOrderCompletionRegisters").To("OldRead");
        }

        public override void Down()
        {
            Rename.Column("OldRead").OnTable("ShortCycleWorkOrderCompletionRegisters").To("Read");
            Delete.Column("NewRead").FromTable("ShortCycleWorkOrderCompletionRegisters");
        }
    }
}
