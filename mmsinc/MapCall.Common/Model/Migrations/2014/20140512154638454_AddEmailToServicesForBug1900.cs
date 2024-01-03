using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140512154638454), Tags("Production")]
    public class AddEmailToServicesForBug1900 : Migration
    {
        public const string TABLE_NAME = "tblNJAWService";
        public const string COLUMN_NAME = "Email";

        public override void Up()
        {
            Alter.Table(TABLE_NAME).AddColumn(COLUMN_NAME).AsAnsiString(50).Nullable();
        }

        public override void Down()
        {
            Delete.Column(COLUMN_NAME).FromTable(TABLE_NAME);
        }
    }
}
