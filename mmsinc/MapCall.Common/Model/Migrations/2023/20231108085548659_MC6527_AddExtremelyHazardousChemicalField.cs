using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20231108085548659), Tags("Production")]
    public class MC6527_AddExtremelyHazardousChemicalField : Migration
    {
        public override void Up()
        {
            Create.Column("ExtremelyHazardousChemical")
                  .OnTable("Chemicals")
                  .AsBoolean()
                  .NotNullable()
                  .SetExistingRowsTo(false);
        }

        public override void Down()
        {
            Delete.Column("ExtremelyHazardousChemical").FromTable("Chemicals");
        }
    }
}

