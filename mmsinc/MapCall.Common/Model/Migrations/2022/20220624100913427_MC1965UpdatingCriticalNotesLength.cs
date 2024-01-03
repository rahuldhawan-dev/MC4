using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220624100913427), Tags("Production")]
    public class MC1965UpdatingCriticalNotesLength : Migration
    {
        public override void Up()
        {
            Alter.Table("SewerOpenings").AlterColumn("CriticalNotes").AsAnsiString(150).Nullable();
        }

        public override void Down()
        {
            Alter.Table("SewerOpenings").AlterColumn("CriticalNotes").AsAnsiString(15).Nullable();
        }
    }
}

