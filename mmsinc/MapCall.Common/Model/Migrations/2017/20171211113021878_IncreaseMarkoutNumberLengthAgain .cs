using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20171211113021878), Tags("Production")]
    public class IncreaseMarkoutNumberLengthAgain : Migration
    {
        public override void Up()
        {
            Alter.Column("MarkoutNumber").OnTable("Markouts").AsAnsiString(20).NotNullable();
        }

        public override void Down() { }
    }
}
