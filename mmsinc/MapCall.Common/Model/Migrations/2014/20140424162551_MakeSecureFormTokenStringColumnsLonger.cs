using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140424162551), Tags("Production")]
    public class MakeSecureFormTokenStringColumnsLonger : Migration
    {
        public const int NEW_LENGTH = 60;

        public override void Up()
        {
            Alter.Column(CreateSecureFormTokensTable.ColumnNames.ACTION)
                 .OnTable(CreateSecureFormTokensTable.TABLE_NAME)
                 .AsAnsiString(NEW_LENGTH)
                 .NotNullable();

            Alter.Column(CreateSecureFormTokensTable.ColumnNames.CONTROLLER)
                 .OnTable(CreateSecureFormTokensTable.TABLE_NAME)
                 .AsAnsiString(NEW_LENGTH)
                 .NotNullable();

            Alter.Column(CreateSecureFormTokensTable.ColumnNames.AREA)
                 .OnTable(CreateSecureFormTokensTable.TABLE_NAME)
                 .AsAnsiString(NEW_LENGTH)
                 .Nullable();
        }

        public override void Down()
        {
            // Need to truncate any values that are too long or else this fails.
            Execute.Sql(
                "UPDATE [SecureFormTokens] SET [Action] = LEFT(Action, 30), [Controller] = LEFT(Controller, 20), [Area] = LEFT(Area, 30)");

            Alter.Column(CreateSecureFormTokensTable.ColumnNames.ACTION)
                 .OnTable(CreateSecureFormTokensTable.TABLE_NAME)
                 .AsAnsiString(CreateSecureFormTokensTable.StringLengths.ACTION)
                 .NotNullable();

            Alter.Column(CreateSecureFormTokensTable.ColumnNames.CONTROLLER)
                 .OnTable(CreateSecureFormTokensTable.TABLE_NAME)
                 .AsAnsiString(CreateSecureFormTokensTable.StringLengths.CONTROLLER)
                 .NotNullable();

            Alter.Column(CreateSecureFormTokensTable.ColumnNames.AREA)
                 .OnTable(CreateSecureFormTokensTable.TABLE_NAME)
                 .AsAnsiString(CreateSecureFormTokensTable.StringLengths.AREA)
                 .Nullable();
        }
    }
}
