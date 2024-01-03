using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131104091457), Tags("Production")]
    public class CreateSecureFormTokenDynamicValuesTable : Migration
    {
        public const string TABLE_NAME = "SecureFormDynamicValues";

        public struct ColumnNames
        {
            public const string ID = "Id",
                                SECURE_FORM_TOKEN_ID = "SecureFormTokenId",
                                KEY = "Key",
                                XML_VALUE = "XmlValue",
                                TYPE = "Type";
        }

        public struct StringLengths
        {
            public const int KEY = 50,
                             XML_VALUE = 255,
                             TYPE = 100;
        }

        public override void Up()
        {
            Create.Table(TABLE_NAME)
                  .WithColumn(ColumnNames.ID).AsInt32().Identity().PrimaryKey().NotNullable()
                  .WithColumn(ColumnNames.SECURE_FORM_TOKEN_ID).AsInt32().NotNullable().ForeignKey(
                       "FK_SecureFormDynamicValues_SecureFormTokens_SecureFormTokenId",
                       CreateSecureFormTokensTable.TABLE_NAME, CreateSecureFormTokensTable.ColumnNames.ID)
                  .WithColumn(ColumnNames.KEY).AsAnsiString(StringLengths.KEY).NotNullable()
                  .WithColumn(ColumnNames.XML_VALUE).AsAnsiString(StringLengths.XML_VALUE).NotNullable()
                  .WithColumn(ColumnNames.TYPE).AsAnsiString(StringLengths.TYPE).NotNullable();
        }

        public override void Down()
        {
            Delete.Table(TABLE_NAME);
        }
    }
}
