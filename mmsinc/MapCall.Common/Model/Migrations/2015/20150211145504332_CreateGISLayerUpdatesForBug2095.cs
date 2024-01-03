using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150211145504332), Tags("Production")]
    public class CreateGISLayerUpdatesForBug2095 : Migration
    {
        public override void Up()
        {
            Create.Table("GISLayerUpdates")
                  .WithIdentityColumn()
                  .WithColumn("Updated").AsDate().NotNullable()
                  .WithColumn("CreatedAt").AsDateTime().NotNullable()
                  .WithForeignKeyColumn("CreatedById", "tblPermissions", "RecId", false)
                  .WithColumn("IsActive").AsBoolean().NotNullable();

            Execute.Sql(
                "INSERT INTO GISLayerUpdates (Updated, CreatedAt, CreatedById, IsActive) SELECT '2014-10-26', GetDate(), RecId, 1 FROM tblPermissions WHERE UserName = 'mcadmin';");
        }

        public override void Down()
        {
            Delete.Table("GISLayerUpdates");
        }
    }
}
