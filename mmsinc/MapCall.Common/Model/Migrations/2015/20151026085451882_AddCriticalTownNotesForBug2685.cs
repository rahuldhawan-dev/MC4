using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151026085451882), Tags("Production")]
    public class AddCriticalTownNotesForBug2685 : Migration
    {
        public override void Up()
        {
            Alter.Table("Towns").AddColumn("CriticalMainBreakNotes").AsAnsiString(255).Nullable();
        }

        public override void Down()
        {
            Delete.Column("CriticalMainBreakNotes").FromTable("Towns");
        }
    }
}
