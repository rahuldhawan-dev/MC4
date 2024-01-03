using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180913121104055), Tags("Production")]
    public class MC255_AddGradientToValve : Migration
    {
        public override void Up()
        {
            Alter.Table("Valves").AddForeignKeyColumn("GradientId", "Gradients");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Valves", "GradientId", "Gradients");
        }
    }
}
