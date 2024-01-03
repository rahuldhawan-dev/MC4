using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150714094722845), Tags("Production")]
    public class CreateAuthenticationLogTableBug2161 : Migration
    {
        // This length covers the max string length possible for an IPV6 address.
        public const int MAX_IP_ADDRESS_LENGTH = 50;

        public const string MAPCALL_TABLE_NAME = "AuthenticationLogs",
                            CONTRACTORS_TABLE_NAME = "ContractorAuthenticationLogs",
                            INDEX_NAME = "IDX_AuthCookie";

        public override void Up()
        {
            Create.Table(MAPCALL_TABLE_NAME)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("UserId").AsInt32().NotNullable()
                  .ForeignKey("FK_tblPermissions_UserId", "tblPermissions", "RecID")
                  .WithColumn("IpAddress").AsString(MAX_IP_ADDRESS_LENGTH).NotNullable()
                  .WithColumn("LoggedInAt").AsDateTime().NotNullable()
                  .WithColumn("LoggedOutAt").AsDateTime().Nullable()
                  .WithColumn("AuthCookieHash").AsGuid().NotNullable().Unique()
                  .WithColumn("ExpiresAt").AsDateTime().NotNullable();

            Create.Table(CONTRACTORS_TABLE_NAME)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("UserId").AsInt32().NotNullable()
                  .ForeignKey("FK_ContractorUsers_UserId", "ContractorUsers", "ContractorUserId")
                  .WithColumn("IpAddress").AsString(MAX_IP_ADDRESS_LENGTH).NotNullable()
                  .WithColumn("LoggedInAt").AsDateTime().NotNullable()
                  .WithColumn("LoggedOutAt").AsDateTime().Nullable()
                  .WithColumn("AuthCookieHash").AsGuid().NotNullable().Unique()
                  .WithColumn("ExpiresAt").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table(MAPCALL_TABLE_NAME);
            Delete.Table(CONTRACTORS_TABLE_NAME);
        }
    }
}
