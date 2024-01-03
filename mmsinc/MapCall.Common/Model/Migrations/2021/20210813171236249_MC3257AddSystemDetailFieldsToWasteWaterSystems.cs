using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210813171236249), Tags("Production")]
    public class MC3257AddSystemDetailFieldsToWasteWaterSystems : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("WasteWaterSystemStatuses", "Active", "Inactive", "Inactive-see note", "Pending", "Pending Merger");
            this.CreateLookupTableWithValues("WasteWaterSystemOwnerships", "AW Contract", "AW Owned/Operated", "CSG", "MSG");
            this.CreateLookupTableWithValues("WasteWaterSystemTypes", "Collection Only", "Treatment Only", "Treatment and Collection");
            this.CreateLookupTableWithValues("WasteWaterSystemSubTypes", "Fully Separated Sewer", "Partially Combined Sewer", "Fully Combined Sewer");
            Alter.Table("WasteWaterSystems").AddForeignKeyColumn("StatusId", "WasteWaterSystemStatuses");
            Alter.Table("WasteWaterSystems").AddForeignKeyColumn("OwnershipId", "WasteWaterSystemOwnerships");
            Alter.Table("WasteWaterSystems").AddForeignKeyColumn("TypeId", "WasteWaterSystemTypes");
            Alter.Table("WasteWaterSystems").AddForeignKeyColumn("SubTypeId", "WasteWaterSystemSubTypes");
            Alter.Table("WasteWaterSystems").AddColumn("DateOfOwnership").AsDateTime().Nullable();
            Alter.Table("WasteWaterSystems").AddColumn("DateOfResponsibility").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("WasteWaterSystems", "StatusId", "WasteWaterSystemStatuses");
            Delete.ForeignKeyColumn("WasteWaterSystems", "OwnershipId", "WasteWaterSystemOwnerships");
            Delete.ForeignKeyColumn("WasteWaterSystems", "TypeId", "WasteWaterSystemTypes");
            Delete.ForeignKeyColumn("WasteWaterSystems", "SubTypeId", "WasteWaterSystemSubTypes");
            Delete.Table("WasteWaterSystemStatuses");
            Delete.Table("WasteWaterSystemOwnerships");
            Delete.Table("WasteWaterSystemTypes");
            Delete.Table("WasteWaterSystemSubTypes");
            Delete.Column("DateOfOwnership").FromTable("WasteWaterSystems");
            Delete.Column("DateOfResponsibility").FromTable("WasteWaterSystems");
        }
    }
}
