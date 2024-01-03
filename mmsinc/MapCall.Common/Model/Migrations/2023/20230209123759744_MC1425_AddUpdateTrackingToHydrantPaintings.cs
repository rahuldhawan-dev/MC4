using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230209123759744), Tags("Production")]
    public class MC1425_AddUpdateTrackingToHydrantPaintings : AutoReversingMigration
    {
        public override void Up()
        {
            Create
               .Column("UpdatedAt")
               .OnTable("HydrantPaintings")
               .AsDateTime()
               .NotNullable();

            Create
               .ForeignKeyColumn(
                    "HydrantPaintings",
                    "UpdatedById",
                    "tblPermissions",
                    "RecId",
                    false);
        }
    }
}

