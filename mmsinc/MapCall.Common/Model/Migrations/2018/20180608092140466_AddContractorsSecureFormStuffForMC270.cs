using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180608092140466), Tags("Production")]
    public class AddContractorsSecureFormStuffForMC270 : Migration
    {
        public override void Up()
        {
            Create.Table("ContractorsSecureFormTokens")
                  .WithIdentityColumn()
                  .WithColumn("Token").AsGuid().NotNullable()
                  .WithForeignKeyColumn("UserId", "ContractorUsers", "ContractorUserId", false)
                  .WithColumn("Area").AsAnsiString(60).Nullable()
                  .WithColumn("Controller").AsString(60).NotNullable()
                  .WithColumn("Action").AsString(75).NotNullable()
                  .WithColumn("CreatedAt").AsDateTime().NotNullable();

            Create.Table("ContractorsSecureFormDynamicValues")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("SecureFormTokenId", "ContractorsSecureFormTokens", nullable: false)
                  .WithColumn("Key").AsAnsiString(50).NotNullable()
                  .WithColumn("XmlValue").AsAnsiString(255).NotNullable()
                  .WithColumn("Type").AsAnsiString(100).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("ContractorsSecureFormDynamicValues");
            Delete.Table("ContractorsSecureFormTokens");
        }
    }
}
