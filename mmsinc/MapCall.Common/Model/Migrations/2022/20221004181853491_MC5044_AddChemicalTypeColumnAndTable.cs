using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221004181853493), Tags("Production")]
    public class MC5044AddChemicalTypeColumnAndTable : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues(
                "ChemicalTypes",
                "Algicide",
                "Carbon",
                "Coagulant",
                "Disinfectant",
                "Oxidizer",
                "Polymer",
                "Sequestrant");
            Alter.Table("Chemicals")
                 .AddForeignKeyColumn("ChemicalTypeId", "ChemicalTypes");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Chemicals", "ChemicalTypeId", "ChemicalTypes");
            Delete.Table("ChemicalTypes");
        }
    }
}