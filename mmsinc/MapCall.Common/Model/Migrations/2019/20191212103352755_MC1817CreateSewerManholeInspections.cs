using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20191212103352755), Tags("Production")]
    public class MC1817CreateSewerManholeInspections : Migration
    {
        public override void Up()
        {
            Create.Table("SewerManholeInspections")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("SewerManholeId", "SewerManholes", "SewerManholeID", false)
                  .WithForeignKeyColumn("InspectedById", "tblPermissions", "RecID", false)
                  .WithColumn("DateInspected").AsDateTime().NotNullable()
                  .WithColumn("DateAdded").AsDateTime().NotNullable()
                  .WithColumn("RimToWaterLevelDepth").AsDecimal(4, 2).NotNullable()
                  .WithColumn("RimHeightAboveBelowGrade").AsDecimal(4, 2).NotNullable()
                  .WithColumn("PipesIn").AsString(50).Nullable()
                  .WithColumn("PipesOut").AsString(50).Nullable()
                  .WithColumn("Remarks").AsString(100).Nullable();
        }

        public override void Down()
        {
            Delete.Table("SewerManholeInspections");
        }
    }
}
