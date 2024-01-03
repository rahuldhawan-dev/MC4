using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20171114082702866), Tags("Production")]
    public class IncreaseMarkoutNumberLength : Migration
    {
        public override void Up()
        {
            Alter.Column("MarkoutNumber").OnTable("Markouts").AsAnsiString(13).NotNullable();
        }

        public override void Down()
        {
            //noop
        }
    }
}
