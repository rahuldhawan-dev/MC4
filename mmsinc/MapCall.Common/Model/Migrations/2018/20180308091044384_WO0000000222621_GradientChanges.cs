using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20180308091044384), Tags("Production")]
    public class WO0000000222621 : Migration
    {
        public override void Up()
        {
            Create.Table("GradientTowns")
                  .WithColumn("TownID").AsInt32().NotNullable()
                  .ForeignKey("FK_GradientTowns_Towns_TownID", "Towns", "TownID")
                  .WithColumn("GradientID").AsInt32().NotNullable()
                  .ForeignKey("FK_GradientTowns_Gradients_GradientID", "Gradients", "Id");
            Alter.Table("tblWQSample_Sites").AddForeignKeyColumn("GradientID", "Gradients");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_GradientTowns_Towns_TownID").OnTable("GradientTowns");
            Delete.ForeignKey("FK_GradientTowns_Gradients_GradientID").OnTable("GradientTowns");
            Delete.Table("GradientTowns");
            Delete.ForeignKeyColumn("tblWQSample_Sites", "GradientID",
                "Gradients");
        }
    }
}
