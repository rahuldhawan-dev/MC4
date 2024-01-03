using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230206102451759), Tags("Production")]
    public class MC1425_CreateHydrantPaintings : AutoReversingMigration
    {
        public override void Up()
        {
            Create
               .Table("HydrantPaintings")
               .WithIdentityColumn()
               .WithForeignKeyColumn("HydrantId", "Hydrants", nullable: false)
               .WithColumn("CreatedAt").AsDateTime().NotNullable()
               .WithForeignKeyColumn("CreatedById", "tblPermissions", "RecId", nullable: false)
               .WithColumn("PaintedAt").AsDateTime().NotNullable();
        }
    }
}

