using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180424093812201), Tags("Production")]
    public class RenameCompletionOtherToCompletion : Migration
    {
        public override void Up()
        {
            Rename.Table("ShortCycleWorkOrderCompletionOthers").To("ShortCycleWorkOrderCompletions");
            Rename.Table("ShortCycleWorkOrderCompletionOtherRegisters").To("ShortCycleWorkOrderCompletionRegisters");
            Rename.Column("ShortCycleWorkOrderCompletionOtherId").OnTable("ShortCycleWorkOrderCompletionRegisters")
                  .To("ShortCycleWorkOrderCompletionId");
            Rename.Table("ShortCycleWorkOrderCompletionOtherTestResults")
                  .To("ShortCycleWorkOrderCompletionTestResults");
            Rename.Column("ShortCycleWorkOrderCompletionOtherId").OnTable("ShortCycleWorkOrderCompletionTestResults")
                  .To("ShortCycleWorkOrderCompletionId");

            //Fix Foreign Keys
        }

        public override void Down()
        {
            Rename.Table("ShortCycleWorkOrderCompletions").To("ShortCycleWorkOrderCompletionOthers");
            Rename.Column("ShortCycleWorkOrderCompletionId").OnTable("ShortCycleWorkOrderCompletionRegisters")
                  .To("ShortCycleWorkOrderCompletionOtherId");
            Rename.Table("ShortCycleWorkOrderCompletionRegisters").To("ShortCycleWorkOrderCompletionOtherRegisters");
            Rename.Column("ShortCycleWorkOrderCompletionId").OnTable("ShortCycleWorkOrderCompletionTestResults")
                  .To("ShortCycleWorkOrderCompletionOtherId");
            Rename.Table("ShortCycleWorkOrderCompletionTestResults")
                  .To("ShortCycleWorkOrderCompletionOtherTestResults");
        }
    }
}
