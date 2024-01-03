using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200730115857432), Tags("Production")]
    public class MC2400AddCommentsToAsBuilts : Migration
    {
        public override void Up()
        {
            Alter.Table("AsBuiltImages").AddColumn("Comments").AsAnsiString(150).Nullable();
        }

        public override void Down()
        {
            Delete.Column("Comments").FromTable("AsBuiltImages");
        }
    }
}
