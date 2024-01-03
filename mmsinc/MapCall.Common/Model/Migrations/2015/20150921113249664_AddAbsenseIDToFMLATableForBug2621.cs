using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150921113249664), Tags("Production")]
    public class AddAbsenseIDToFMLATableForBug2621 : Migration
    {
        public const string TABLE_NAME = "FMLACases", COLUMN_NAME = "AbsenseID";
        public const int STRING_LENGTH = 15;

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
