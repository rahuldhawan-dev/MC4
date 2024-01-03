using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190621145345411), Tags("Production")]
    public class MC1465FixPipeMaterialForeignKeyConstraint : Migration
    {
        public override void Up()
        {
            Delete.ForeignKey("FK_SewerManholeConnections_SewerPipeMaterials_SewerPipeMaterialID")
                  .OnTable("SewerManholeConnections");
            Create.ForeignKey("FK_SewerManholeConnections_PipeMaterials_SewerPipeMaterialID")
                  .FromTable("SewerManholeConnections").ForeignColumn("SewerPipeMaterialID")
                  .ToTable("PipeMaterials").PrimaryColumn("Id");
            Delete.Table("SewerPipeMaterials");
        }

        public override void Down()
        {
            //there really is no coming back from this.
            this.CreateLookupTableFromQuery("SewerPipeMaterials", "SELECT Description FROM PipeMaterials");
            Rename.Column("Id").OnTable("SewerPipeMaterials").To("SewerPipeMaterialID");
            Delete.ForeignKey("FK_SewerManholeConnections_PipeMaterials_SewerPipeMaterialID")
                  .OnTable("SewerManholeConnections");
            Create.ForeignKey("FK_SewerManholeConnections_SewerPipeMaterials_SewerPipeMaterialID")
                  .FromTable("SewerManholeConnections").ForeignColumn("SewerPipeMaterialID")
                  .ToTable("SewerPipeMaterials").PrimaryColumn("SewerPipeMaterialID");
        }
    }
}
