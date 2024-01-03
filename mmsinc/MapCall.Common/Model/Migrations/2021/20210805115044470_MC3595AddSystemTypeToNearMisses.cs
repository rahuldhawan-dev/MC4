using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210805115044470), Tags("Production")]
    public class MC3595AddSystemTypeToNearMisses : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("SystemTypes", "Drinking Water", "Waste Water", "Other");

            Alter.Table("NearMisses")
                 .AddForeignKeyColumn("SystemTypeId", "SystemTypes")
                 .AddForeignKeyColumn("PublicWaterSupplyId", "PublicWaterSupplies")
                 .AddForeignKeyColumn("WasteWaterSystemId", "WasteWaterSystems")
                 .AddColumn("CompletedCorrectiveActions").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("NearMisses", "SystemTypeId", "SystemTypes");
            Delete.ForeignKeyColumn("NearMisses", "PublicWaterSupplyId", "PublicWaterSupplies");
            Delete.ForeignKeyColumn("NearMisses", "WasteWaterSystemId", "WasteWaterSystems");
            Delete.Column("CompletedCorrectiveActions").FromTable("NearMisses");

            Delete.Table("SystemTypes");
        }
    }
}

