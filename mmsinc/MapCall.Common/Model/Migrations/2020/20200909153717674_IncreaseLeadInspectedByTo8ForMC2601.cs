using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200909153717674), Tags("Production")]
    public class IncreaseLeadInspectedByTo8ForMC2601 : Migration
    {
        public override void Up()
        {
            Alter.Column("LeadInspectedBy")
                 .OnTable("ShortCycleWorkOrderCompletions")
                 .AsAnsiString(9).Nullable();
        }

        public override void Down()
        {
            //noop
        }
    }
}
