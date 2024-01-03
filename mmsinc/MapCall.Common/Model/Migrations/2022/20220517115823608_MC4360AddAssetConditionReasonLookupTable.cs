using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220517115823609)]
    [Tags("Production")]
    public class Mc4360AddAssetConditionReasonLookupTable : Migration
    {
        #region Exposed Methods

        public override void Up()
        {
            this.CreateLookupTableWithValues("ConditionTypes", "As Found", "As Left");

            Create.Table("ConditionDescriptions")
                  .WithIdentityColumn()
                  .WithColumn("Description")
                  .AsAnsiString(50).NotNullable()
                  .WithForeignKeyColumn("ConditionTypeId", "ConditionTypes", "Id", false);

            Execute.Sql(
                "INSERT INTO ConditionDescriptions (Description, ConditionTypeId) VALUES ('Unable to Inspect',1);");
            Execute.Sql(
                "INSERT INTO ConditionDescriptions (Description, ConditionTypeId) VALUES ('Unable to Inspect',2);");

            Create.Table("AssetConditionReasons")
                  .WithIdentityColumn()
                  .WithColumn("Code").AsAnsiString(4).NotNullable()
                  .WithColumn("Description").AsAnsiString(50).NotNullable()
                  .WithForeignKeyColumn("ConditionDescriptionId", "ConditionDescriptions").NotNullable();

            Insert.IntoTable("AssetConditionReasons").Row(new
                { Code = "AFCL", Description = "Cannot Locate", ConditionDescriptionId = 1 });
            Insert.IntoTable("AssetConditionReasons").Row(new
                { Code = "ALCL", Description = "Cannot Locate", ConditionDescriptionId = 2 });
            Insert.IntoTable("AssetConditionReasons").Row(new {
                Code = "AFOC",
                Description = "Operational Constraint",
                ConditionDescriptionId = 1
            });
            Insert.IntoTable("AssetConditionReasons").Row(new {
                Code = "ALOC",
                Description = "Operational Constraint",
                ConditionDescriptionId = 2
            });
            Insert.IntoTable("AssetConditionReasons").Row(new
                { Code = "AFOO", Description = "Out Of Service", ConditionDescriptionId = 1 });
            Insert.IntoTable("AssetConditionReasons").Row(new
                { Code = "ALOO", Description = "Out Of Service", ConditionDescriptionId = 2 });
            Insert.IntoTable("AssetConditionReasons").Row(new
                { Code = "AFSP", Description = "Scheduling Problem", ConditionDescriptionId = 1 });
            Insert.IntoTable("AssetConditionReasons").Row(new
                { Code = "ALSP", Description = "Scheduling Problem", ConditionDescriptionId = 2 });
            Insert.IntoTable("AssetConditionReasons").Row(new {
                Code = "AFSC",
                Description = "Seasonal Constraint",
                ConditionDescriptionId = 1
            });
            Insert.IntoTable("AssetConditionReasons").Row(new {
                Code = "ALSC",
                Description = "Seasonal Constraint",
                ConditionDescriptionId = 2
            });
        }

        public override void Down()
        {
            Delete.Table("AssetConditionReasons");
            Delete.Table("ConditionDescriptions");
            Delete.Table("ConditionTypes");
        }

        #endregion
    }
}
