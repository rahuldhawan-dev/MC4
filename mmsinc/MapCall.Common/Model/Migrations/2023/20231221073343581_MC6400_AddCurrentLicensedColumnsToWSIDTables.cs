using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20231221073343581), Tags("Production")]
    public class MC6400_AddCurrentLicensedColumnsToWWSIDTables : Migration
    {
        public override void Up()
        {
            Create.Table("LicensedOperatorCategories")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsAnsiString(50);

            this.EnableIdentityInsert("LicensedOperatorCategories");
            Insert.IntoTable("LicensedOperatorCategories")
                  .Row(new { Id = 1, Description = "Internal Employee" })
                  .Row(new { Id = 2, Description = "No Licensed Operator Required" })
                  .Row(new { Id = 3, Description = "Contracted Licensed Operator" });
            this.DisableIdentityInsert("LicensedOperatorCategories");

            Alter.Table("WasteWaterSystems")
                 .AddForeignKeyColumn("LicensedOperatorStatusId", "LicensedOperatorCategories").Nullable()
                 .AddColumn("CurrentLicensedContractor").AsAnsiString(100).Nullable();

            Alter.Table("PublicWaterSupplies")
                 .AddForeignKeyColumn("LicensedOperatorStatusId", "LicensedOperatorCategories").Nullable()
                 .AddColumn("CurrentLicensedContractor").AsAnsiString(100).Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("WasteWaterSystems", "LicensedOperatorStatusId", "LicensedOperatorCategories");
            Delete.Column("CurrentLicensedContractor").FromTable("WasteWaterSystems");
            Delete.ForeignKeyColumn("PublicWaterSupplies", "LicensedOperatorStatusId", "LicensedOperatorCategories");
            Delete.Column("CurrentLicensedContractor").FromTable("PublicWaterSupplies");
            Delete.Table("LicensedOperatorCategories");
        }
    }
}

