using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160229143805196), Tags("Production")]
    public class AddScadaTagNamesToEqiupmentForBug2800 : Migration
    {
        public override void Up()
        {
            Alter.Table("Equipment").AddForeignKeyColumn("ScadaTagNameId", "ScadaTagNames");
            Alter.Table("Equipment")
                 .AddForeignKeyColumn("ScadaUnitOfMeasureId", "UnitsOfMeasure", "UnitOfMeasureId");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Equipment", "ScadaTagNameId", "ScadaTagNames");
            Delete.ForeignKeyColumn("Equipment", "ScadaUnitOfMeasureId", "UnitsOfMeasure",
                "UnitOfMeasureId");
        }
    }
}
