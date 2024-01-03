using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150521155243832), Tags("Production")]
    public class AddStreetToMainCrossingsForBug2412 : Migration
    {
        public override void Up()
        {
            Alter.Table("MainCrossings").AddForeignKeyColumn("StreetId", "Streets", "StreetID");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("MainCrossings", "StreetId", "Streets", "StreetID");
        }
    }
}
