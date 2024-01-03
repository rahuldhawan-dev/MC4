using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150922143259926), Tags("Production")]
    public class AddDurationToFamilyMedicalLeaveActForBug2623 : Migration
    {
        public const string TABLE_NAME = AddAbsenseIDToFMLATableForBug2621.TABLE_NAME, COLUMN_NAME = "Duration";
        public const int STRING_LENGTH = 25;

        public override void Up()
        {
            Alter.Table(TABLE_NAME).AddColumn(COLUMN_NAME).AsString(STRING_LENGTH).Nullable();
        }

        public override void Down()
        {
            Delete.Column(COLUMN_NAME).FromTable(TABLE_NAME);
        }
    }
}
