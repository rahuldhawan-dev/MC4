using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190624105951989), Tags("Production")]
    public class CreateTypesOfPlumbingForMC814 : Migration
    {
        public override void Up()
        {
            Create.LookupTable("TypesOfPlumbing");

            Insert.IntoTable("TypesOfPlumbing")
                  .Rows(new {Description = "Galvanized"},
                       new {Description = "Lead"},
                       new {Description = "Copper"},
                       new {Description = "Plastic"});

            Alter.Table("MeterChangeOuts").AddForeignKeyColumn("TypeOfPlumbingId", "TypesOfPlumbing");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("MeterChangeOuts", "TypeOfPlumbingId", "TypesOfPlumbing");

            Delete.Table("TypesOfPlumbing");
        }
    }
}
