using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140403152756), Tags("Production")]
    public class AlterTablesForBug1815 : Migration
    {
        public override void Up()
        {
            Create.Column("TopicLevelId").OnTable("tblTailgateTopics").AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Column("TopicLevelId").FromTable("tblTailgateTopics");
        }
    }
}
