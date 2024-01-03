using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Data;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200924145640960), Tags("Production")]
    public class AddSystemDeliveryTablesForMC2376 : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("SystemDeliveryTypes", "Water", "Wastewater");

            Create.Table("SystemDeliveryEntryTypes")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("SystemDeliveryTypeId", "SystemDeliveryTypes")
                  .WithColumn("Description").AsAnsiString(50).NotNullable();
            Insert.IntoTable("SystemDeliveryEntryTypes")
                  .Row(new {Description = "Purchase Point", SystemDeliveryTypeId = 1});
            Insert.IntoTable("SystemDeliveryEntryTypes")
                  .Row(new {Description = "Delivered Water", SystemDeliveryTypeId = 1});
            Insert.IntoTable("SystemDeliveryEntryTypes")
                  .Row(new {Description = "Transferred To", SystemDeliveryTypeId = 1});
            Insert.IntoTable("SystemDeliveryEntryTypes")
                  .Row(new {Description = "Transferred From", SystemDeliveryTypeId = 1});
            Insert.IntoTable("SystemDeliveryEntryTypes")
                  .Row(new {Description = "Wastewater Collected", SystemDeliveryTypeId = 2});
            Insert.IntoTable("SystemDeliveryEntryTypes")
                  .Row(new {Description = "Wastewater Treated", SystemDeliveryTypeId = 2});
            Insert.IntoTable("SystemDeliveryEntryTypes").Row(new
                {Description = "Untreated Eff. Discharched", SystemDeliveryTypeId = 2});
            Insert.IntoTable("SystemDeliveryEntryTypes")
                  .Row(new {Description = "Treated Eff. Discharged", SystemDeliveryTypeId = 2});
            Insert.IntoTable("SystemDeliveryEntryTypes")
                  .Row(new {Description = "Treated Eff. Reused", SystemDeliveryTypeId = 2});
            Insert.IntoTable("SystemDeliveryEntryTypes").Row(new
                {Description = "Biochemical Oxygen Demand", SystemDeliveryTypeId = 2});
            Alter.Table("tblFacilities")
                 .AddForeignKeyColumn("SystemDeliveryTypeId", "SystemDeliveryTypes").Nullable();

            Create.Table("FacilitiesSystemDeliveryEntryTypes")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("FacilityId", "tblFacilities", "RecordID", nullable: false)
                  .WithForeignKeyColumn("SystemDeliveryEntryTypeId", "SystemDeliveryEntryTypes", nullable: false)
                  .WithColumn("IsEnabled").AsBoolean().WithDefaultValue(true)
                  .WithColumn("MinimumValue").AsDecimal(19, 2).Nullable()
                  .WithColumn("MaximumValue").AsDecimal(19, 2).Nullable();
            this.CreateModule("System Delivery", "Production", 85);
            this.CreateModule("System Delivery Entry", "Production", 86);
        }

        public override void Down()
        {
            this.DeleteModuleAndAssociatedRoles("Production", "System Delivery Entry");
            this.DeleteModuleAndAssociatedRoles("Production", "System Delivery");
            Delete.Table("FacilitiesSystemDeliveryEntryTypes");
            Delete.ForeignKeyColumn("tblFacilities", "SystemDeliveryTypeId", "SystemDeliveryTypes");
            Delete.Table("SystemDeliveryEntryTypes");
            Delete.Table("SystemDeliveryTypes");
        }
    }
}
