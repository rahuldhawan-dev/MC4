using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170317104442196), Tags("Production")]
    public class CreateLEARNItemTypesForTrainingModulesForBug3606 : Migration
    {
        public const string TABLE_NAME = "LEARNItemTypes";

        public override void Up()
        {
            Create.LookupTable(TABLE_NAME)
                  .WithColumn("Abbreviation").AsAnsiString(4).NotNullable();
            Insert.IntoTable(TABLE_NAME).Rows(
                new {Description = "Classroom Instructor Led", Abbreviation = "CIL"},
                new {Description = "Online", Abbreviation = "OL"},
                new {Description = "Virtual Instructor Led", Abbreviation = "VIL"}
            );

            Alter.Table("tblTrainingModules")
                 .AddForeignKeyColumn("LEARNItemTypeId", TABLE_NAME);

            Execute.Sql($"UPDATE tblTrainingModules SET LEARNItemTypeId = 1");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("tblTrainingModules", "LEARNItemTypeId", TABLE_NAME);
            Delete.Table(TABLE_NAME);
        }
    }
}
