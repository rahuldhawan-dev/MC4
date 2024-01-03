using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131104084425), Tags("Production")]
    public class CreateSecureFormTokensTable : Migration
    {
        public const string TABLE_NAME = "SecureFormTokens";

        public struct ColumnNames
        {
            public const string ID = "Id",
                                USER_ID = "UserId",
                                TOKEN = "Token",
                                AREA = "Area",
                                CONTROLLER = "Controller",
                                ACTION = "Action",
                                CREATED_AT = "CreatedAt";
        }

        public struct StringLengths
        {
            public const int AREA = 20,
                             CONTROLLER = 20,
                             ACTION = 30;

            public const int SECONDARY_AREA = AREA,
                             SECONDARY_CONTROLLER = CONTROLLER,
                             SECONDARY_ACTION = ACTION;
        }

        public override void Up()
        {
            Create.Table(TABLE_NAME)
                  .WithColumn(ColumnNames.ID).AsInt32().Identity().PrimaryKey().NotNullable()
                  .WithColumn(ColumnNames.TOKEN).AsGuid().NotNullable()
                  .WithColumn(ColumnNames.USER_ID).AsInt32().NotNullable()
                  .ForeignKey("FK_SecureFormTokens_tblPermissions_UserId", "tblPermissions", "RecId")
                  .WithColumn(ColumnNames.AREA).AsAnsiString(StringLengths.AREA).Nullable()
                  .WithColumn(ColumnNames.CONTROLLER).AsAnsiString(StringLengths.CONTROLLER).NotNullable()
                  .WithColumn(ColumnNames.ACTION).AsAnsiString(StringLengths.ACTION).NotNullable()
                  .WithColumn(ColumnNames.CREATED_AT).AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table(TABLE_NAME);
        }
    }
}
