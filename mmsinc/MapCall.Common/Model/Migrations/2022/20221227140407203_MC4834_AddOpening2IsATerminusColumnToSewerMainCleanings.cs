using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221227140407203), Tags("Production")]
    public class MC4834_AddOpening2IsATerminusColumnToSewerMainCleanings : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Column("Opening2IsATerminus").OnTable("SewerMainCleanings").AsBoolean().Nullable();
        }
    }
}

