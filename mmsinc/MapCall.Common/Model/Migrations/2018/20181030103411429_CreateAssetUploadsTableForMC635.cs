using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181030103411429), Tags("Production")]
    public class CreateAssetUploadsTableForMC635 : Migration
    {
        public override void Up()
        {
            Create.LookupTable("AssetUploadStatuses", 10);

            Insert.IntoTable("AssetUploadStatuses").Row(new {Description = "Pending"});
            Insert.IntoTable("AssetUploadStatuses").Row(new {Description = "Success"});
            Insert.IntoTable("AssetUploadStatuses").Row(new {Description = "Error"});

            Create.Table("AssetUploads")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("CreatedById", "tblPermissions", "RecId", false)
                  .WithColumn("CreatedAt").AsDateTime().NotNullable()
                  .WithColumn("FileName").AsString(50).NotNullable()
                  .WithColumn("FileGuid").AsGuid().NotNullable()
                  .WithColumn("ErrorText").AsText().Nullable()
                  .WithForeignKeyColumn("StatusId", "AssetUploadStatuses", nullable: false);
        }

        public override void Down()
        {
            Delete.Table("AssetUploads");
            Delete.Table("AssetUploadStatuses");
        }
    }
}
