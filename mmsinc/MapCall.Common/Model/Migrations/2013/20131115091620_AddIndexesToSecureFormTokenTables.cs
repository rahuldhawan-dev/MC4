using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131115091620), Tags("Production")]
    public class AddIndexesToSecureFormTokenTables : Migration
    {
        public override void Up()
        {
            Create.Index("IDX_SecureFormDynamicValues_SecureFormTokenId")
                  .OnTable("SecureFormDynamicValues")
                  .OnColumn("SecureFormTokenId")
                  .Ascending();

            Create.Index("IDX_SecureFormTokens_Id_CreatedAt")
                  .OnTable("SecureFormTokens")
                  .OnColumn("Id")
                  .Ascending()
                  .OnColumn("CreatedAt")
                  .Ascending();

            Create.Index("IDX_SecureFormTokens_Token")
                  .OnTable("SecureFormTokens")
                  .OnColumn("Token")
                  .Ascending();
        }

        public override void Down()
        {
            Delete.Index("IDX_SecureFormTokens_Token").OnTable("SecureFormTokens");
            Delete.Index("IDX_SecureFormTokens_Id_CreatedAt").OnTable("SecureFormTokens");
            Delete.Index("IDX_SecureFormDynamicValues_SecureFormTokenId").OnTable("SecureFormDynamicValues");
        }
    }
}
