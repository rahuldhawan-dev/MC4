using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20160105140901870), Tags("Production")]
    public class UpgradeVehiclesBug2712 : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("VehicleOwnershipTypes", "Owned", "Leased");
            this.CreateLookupTableWithValues("VehicleServiceCompanies", "Element", "Penske");

            this.ExtractLookupTableLookup("Vehicles", "AssignmentCategory", "VehicleAssignmentCategories", 50,
                "AssignmentCategory");
            this.ExtractLookupTableLookup("Vehicles", "AssignmentJustification", "VehicleAssignmentJustifications", 50,
                "AssignmentJustification");
            this.ExtractLookupTableLookup("Vehicles", "AssignedStatus", "VehicleAssignmentStatuses", 50,
                "AssignedStatus");
            this.ExtractLookupTableLookup("Vehicles", "AccountingRequirement", "VehicleAccountingRequirements", 50,
                "AccountingRequirement", deleteSafely: true);
            this.ExtractLookupTableLookup("Vehicles", "VehicleStatus", "VehicleStatuses", 50, "VehicleStatus");
            this.ExtractLookupTableLookup("Vehicles", "FuelType", "VehicleFuelTypes", 50, "FuelType");
            this.ExtractLookupTableLookup("Vehicles", "GPSType", "VehicleGPSTypes", 50, "GPSType");

            this.DeleteForeignKeyIfItExists("Vehicles", "VehicleType", "Lookup");
            this.ExtractLookupTableLookup("Vehicles", "VehicleType", "VehicleTypes", 50, "VehicleType",
                lookupIsTableSpecific: true, deleteSafely: true);
            this.DeleteForeignKeyIfItExists("Vehicles", "Department", "Lookup");
            this.ExtractLookupTableLookup("Vehicles", "Department", "VehicleDepartments", 50, "Department");
            this.DeleteForeignKeyIfItExists("Vehicles", "PrimaryVehicleUse", "Lookup");
            this.ExtractLookupTableLookup("Vehicles", "PrimaryVehicleUse", "VehiclePrimaryUses", 50,
                "PrimaryVehicleUse");

            Rename.Table("tblVehiclesEZPass").To("VehicleEZPasses");
            Rename.Column("EzPassId").OnTable("VehicleEZPasses").To("Id");
            Update.Table("DataType").Set(new {Table_Name = "VehicleEZPasses"})
                  .Where(new {Table_Name = "tblVehiclesEZPass"});

            Alter.Column("EzPassSerialNumber").OnTable("VehicleEZPasses")
                 .AsString(50).NotNullable();
            Alter.Column("BillingInfo").OnTable("VehicleEZPasses")
                 .AsString(255).NotNullable();

            Alter.Column("Flag").OnTable("Vehicles").AsBoolean().NotNullable();
            Alter.Column("PoolUse").OnTable("Vehicles").AsBoolean().NotNullable();
            Alter.Column("LogoWaiver").OnTable("Vehicles").AsBoolean().NotNullable();
            Alter.Column("PlannedReplacementYear").OnTable("Vehicles").AsInt32().Nullable();

            this.DeleteIndexIfItExists("Vehicles", "IX_Vehicles_DataTextField");

            Alter.Table("Vehicles")
                 .AddColumn("NedapSerialNumber").AsString(50).Nullable()
                 .AddColumn("WBSNumber").AsString(15).Nullable()
                 .AddColumn("VehicleOwnershipTypeId").AsInt32().Nullable()
                 .ForeignKey("FK_Vehicles_VehicleOwnershipTypes_VehicleOwnershipTypeId", "VehicleOwnershipTypes", "Id")
                 .AddColumn("VehicleServiceCompanyId").AsInt32().Nullable()
                 .ForeignKey("FK_Vehicles_VehicleServiceCompanies_VehicleServiceCompanyId", "VehicleServiceCompanies",
                      "Id")
                 .AlterColumn("RequisitionNumber").AsString(5).Nullable()
                 .AlterColumn("VINNumber").AsString(18).Nullable()
                 .AlterColumn("ARIVehicleNumber").AsString(6).Nullable()
                 .AlterColumn("Decal_Number").AsString(8).Nullable()
                 .AlterColumn("Make").AsString(25).Nullable()
                 .AlterColumn("Model").AsString(50).Nullable();

            Create.Table("VehicleAudits")
                  .WithColumn("Id").AsIdColumn()
                  .WithColumn("VehicleId").AsInt32().NotNullable()
                  .ForeignKey("FK_VehicleAudits_Vehicles_VehicleId", "Vehicles", "VehicleID")
                  .WithColumn("AuditedOn").AsDateTime().NotNullable()
                  .WithColumn("Mileage").AsInt32().NotNullable()
                  .WithColumn("DecalNumber").AsString(8).NotNullable()
                  .WithColumn("PlateNumber").AsString(8).NotNullable();

            // Data fixes for a few vehicles that have invalid operating center values.
            const int NJ8 = 34,
                      NJ4 = 14;
            Update.Table("Vehicles").Set(new {OpCenterId = NJ8}).Where(new {VehicleID = 165});
            Update.Table("Vehicles").Set(new {OpCenterId = NJ8}).Where(new {VehicleID = 181});
            Update.Table("Vehicles").Set(new {OpCenterId = NJ8}).Where(new {VehicleID = 182});
            Update.Table("Vehicles").Set(new {OpCenterId = NJ8}).Where(new {VehicleID = 183});
            Update.Table("Vehicles").Set(new {OpCenterId = NJ8}).Where(new {VehicleID = 184});
            Update.Table("Vehicles").Set(new {OpCenterId = NJ8}).Where(new {VehicleID = 578});
            Update.Table("Vehicles").Set(new {OpCenterId = NJ8}).Where(new {VehicleID = 1865});
            Update.Table("Vehicles").Set(new {OpCenterId = NJ8}).Where(new {VehicleID = 1870});
            Update.Table("Vehicles").Set(new {OpCenterId = NJ4}).Where(new {VehicleID = 661});
            Update.Table("Vehicles").Set(new {OpCenterId = NJ4}).Where(new {VehicleID = 673});
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_VehicleAudits_Vehicles_VehicleId").OnTable("VehicleAudits");
            Delete.Table("VehicleAudits");

            Alter.Table("Vehicles")
                 .AlterColumn("RequisitionNumber").AsString(50).Nullable()
                 .AlterColumn("VINNumber").AsString(50).Nullable()
                 .AlterColumn("ARIVehicleNumber").AsString(255).Nullable()
                 .AlterColumn("Decal_Number").AsString(255).Nullable()
                 .AlterColumn("Make").AsString(255).Nullable()
                 .AlterColumn("Model").AsString(255).Nullable();

            Delete.ForeignKey("FK_Vehicles_VehicleServiceCompanies_VehicleServiceCompanyId").OnTable("Vehicles");
            Delete.ForeignKey("FK_Vehicles_VehicleOwnershipTypes_VehicleOwnershipTypeId").OnTable("Vehicles");
            Delete.Column("VehicleServiceCompanyId").FromTable("Vehicles");
            Delete.Column("VehicleOwnershipTypeId").FromTable("Vehicles");
            Delete.Column("WBSNumber").FromTable("Vehicles");
            Delete.Column("NedapSerialNumber").FromTable("Vehicles");
            Alter.Column("PlannedReplacementYear").OnTable("Vehicles").AsString(4).Nullable();

            Alter.Column("LogoWaiver").OnTable("Vehicles").AsBoolean().Nullable();
            Alter.Column("PoolUse").OnTable("Vehicles").AsBoolean().Nullable();
            Alter.Column("Flag").OnTable("Vehicles").AsBoolean().Nullable();

            Update.Table("DataType").Set(new {Table_Name = "tblVehiclesEZPass"})
                  .Where(new {Table_Name = "VehicleEZPasses"});
            Rename.Column("Id").OnTable("VehicleEZPasses").To("EzPassId");
            Rename.Table("VehicleEZPasses").To("tblVehiclesEZPass");

            this.ReplaceLookupTableLookup("Vehicles", "GPSType", "VehicleGPSTypes", 50, "GPSType");
            this.ReplaceLookupTableLookup("Vehicles", "PrimaryVehicleUse", "VehiclePrimaryUses", 50,
                "PrimaryVehicleUse");
            this.ReplaceLookupTableLookup("Vehicles", "FuelType", "VehicleFuelTypes", 50, "FuelType");
            this.ReplaceLookupTableLookup("Vehicles", "VehicleStatus", "VehicleStatuses", 50, "VehicleStatus");
            this.ReplaceLookupTableLookup("Vehicles", "VehicleType", "VehicleTypes", 50, "VehicleType");
            this.ReplaceLookupTableLookup("Vehicles", "Department", "VehicleDepartments", 50, "Department");
            this.ReplaceLookupTableLookup("Vehicles", "AccountingRequirement", "VehicleAccountingRequirements", 50,
                "AccountingRequirement");
            this.ReplaceLookupTableLookup("Vehicles", "AssignedStatus", "VehicleAssignmentStatuses", 50,
                "AssignedStatus");
            this.ReplaceLookupTableLookup("Vehicles", "AssignmentJustification", "VehicleAssignmentJustifications", 50,
                "AssignmentJustification");
            this.ReplaceLookupTableLookup("Vehicles", "AssignmentCategory", "VehicleAssignmentCategories", 50,
                "AssignmentCategory");

            Delete.Table("VehicleOwnershipTypes");
            Delete.Table("VehicleServiceCompanies");
        }
    }
}
