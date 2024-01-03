using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221221005527472), Tags("Production")]
    public class MC303AddingHydrantTypeAndOutletConfigurationToHydrants : Migration
    {
        public override void Up()
        {
            this.CreateSAPLookupTable("HydrantTypes", 10);
            Insert.IntoTable("HydrantTypes").Rows(
                new { SAPCode = "DRY", Description = "Dry" },
                new { SAPCode = "WET", Description = "Wet" });
            this.CreateSAPLookupTable("HydrantOutletConfigurations", 30);
            Insert.IntoTable("HydrantOutletConfigurations").Rows(
                new { SAPCode = "1 SIDE PORT/1 STEAMER", Description = "1 SIDE PORT/1 STEAMER" },
                new { SAPCode = "1 SIDE PORT/2 STEAMERS", Description = "1 SIDE PORT/2 STEAMERS" },
                new { SAPCode = "1 SIDE PORT", Description = "1 SIDE PORT" },
                new { SAPCode = "1 STEAMER", Description = "1 STEAMER" },
                new { SAPCode = "2 SIDE/1 STEAMER", Description = "2 SIDE/1 STEAMER" },
                new { SAPCode = "2 SIDE PORTS", Description = "2 SIDE PORTS" },
                new { SAPCode = "2 STEAMERS", Description = "2 STEAMERS" },
                new { SAPCode = "OTHER", Description = "OTHER" });
            Alter.Table("Hydrants")
                 .AddForeignKeyColumn("HydrantTypeId", "HydrantTypes")
                 .AddForeignKeyColumn("HydrantOutletConfigurationId", "HydrantOutletConfigurations");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Hydrants", "HydrantTypeId", "HydrantTypes");
            Delete.ForeignKeyColumn("Hydrants", "HydrantOutletConfigurationId", "HydrantOutletConfigurations");
            Delete.Table("HydrantTypes");
            Delete.Table("HydrantOutletConfigurations");
        }
    }
}

