using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210506105524065), Tags("Production")]
    public class MC2956ConvertChemicalFormFieldOnChemicalsToLookupandUpdateData : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("StatesOfMatter", "Solid", "Liquid", "Gas");
            Create.Table("ChemicalsStatesOfMatter")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ChemicalId", "Chemicals").Indexed()
                  .WithForeignKeyColumn("StateOfMatterId", "StatesOfMatter").Indexed();
            // Normalize these ones
            Execute.Sql(@"UPDATE Chemicals
SET ChemicalForm = 'Gas'
Where ChemicalForm = 'Gaseous';

UPDATE Chemicals
SET ChemicalForm = 'Liquid'
Where ChemicalForm = 'Liquid Slurry';

UPDATE Chemicals
SET ChemicalForm = 'Solid'
WHERE ChemicalForm = 'Dry';");

            // These are technically two, so lets multi select them
            Execute.Sql(@"INSERT INTO ChemicalsStatesOfMatter
SELECT Id, 2 FROM Chemicals WHERE ChemicalForm = 'Liquid or Gas'

INSERT INTO ChemicalsStatesOfMatter
SELECT Id, 3 FROM Chemicals WHERE ChemicalForm = 'Liquid or Gas'");

            // Normalize the rest
            this.NormalizeToExistingTable("Chemicals", "ChemicalForm", "StatesOfMatter");
            // Get it into the many to many table
            Execute.Sql(@"INSERT INTO ChemicalsStatesOfMatter
SELECT Id, ChemicalFormId FROM Chemicals WHERE ChemicalFormId IS NOT NULL");
            // drop the column the normalization method made
            Delete.ForeignKeyColumn("Chemicals", "ChemicalFormId", "StatesOfMatter");
        }

        public override void Down()
        {
            // Add the column back
            Alter.Table("Chemicals").AddColumn("ChemicalForm").AsAnsiString(50).Nullable();
            // Update the values back
            Execute.Sql(@"UPDATE Chemicals 
SET ChemicalForm = 'Gas'
WHERE Id in (SELECT ChemicalId FROM ChemicalsStatesOfMatter WHERE StateOfMatterId = 3)

UPDATE Chemicals 
SET ChemicalForm = 'Liquid'
WHERE Id in (SELECT ChemicalId FROM ChemicalsStatesOfMatter WHERE StateOfMatterId = 2)

UPDATE Chemicals 
SET ChemicalForm = 'Dry'
WHERE Id in (SELECT ChemicalId FROM ChemicalsStatesOfMatter WHERE StateOfMatterId = 1)

UPDATE Chemicals SET ChemicalForm = 'Liquid Or Gas'
            WHERE Id IN (
                SELECT DISTINCT ChemicalId
            FROM ChemicalsStatesOfMatter
            WHERE ChemicalId IN (
                SELECT ChemicalId
            FROM ChemicalsStatesOfMatter
            GROUP BY ChemicalId
                HAVING COUNT(*) > 1))");
            // Drop tables
            Delete.Table("ChemicalsStatesOfMatter");
            Delete.Table("StatesOfMatter");
        }
    }
}

