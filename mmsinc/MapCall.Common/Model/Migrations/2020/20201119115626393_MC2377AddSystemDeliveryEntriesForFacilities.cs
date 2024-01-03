using FluentMigrator;
using MapCall.Common.Data;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201119115626393), Tags("Production")]
    public class MC2377AddSystemDeliveryEntriesForFacilities : Migration
    {
        public override void Up()
        {
            Create.Table("SystemDeliveryEntries")
                  .WithIdentityColumn()
                  .WithColumn("WeekOf").AsDate().NotNullable()
                  .WithColumn("IsSubmitted").AsBoolean().Nullable()
                  .WithColumn("IsValidated").AsBoolean().Nullable()
                  .WithForeignKeyColumn("EnteredById", "tblEmployee", "tblEmployeeId");

            Create.Table("SystemDeliveryEntriesOperatingCenters")
                  .WithForeignKeyColumn("SystemDeliveryEntryId", "SystemDeliveryEntries")
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterId");

            Create.Table("SystemDeliveryEntriesFacilities")
                  .WithForeignKeyColumn("SystemDeliveryEntryId", "SystemDeliveryEntries")
                  .WithForeignKeyColumn("FacilityId", "tblFacilities", "RecordId");

            Create.Table("SystemDeliveryEquipmentEntries")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("SystemDeliveryTypeId", "SystemDeliveryTypes", nullable: false)
                  .WithForeignKeyColumn("SystemDeliveryEntryTypeId", "SystemDeliveryEntryTypes", nullable: false)
                  .WithForeignKeyColumn("SystemDeliveryEntryId", "SystemDeliveryEntries").Indexed()
                  .WithForeignKeyColumn("EquipmentId", "Equipment", "EquipmentId")
                  .WithForeignKeyColumn("EnteredById", "tblEmployee", "tblEmployeeId")
                  .WithColumn("MondayEntry").AsDecimal(19, 3).Nullable()
                  .WithColumn("MondayEntryDate").AsDateTime().Nullable()
                  .WithColumn("TuesdayEntry").AsDecimal(19, 3).Nullable()
                  .WithColumn("TuesdayEntryDate").AsDateTime().Nullable()
                  .WithColumn("WednesdayEntry").AsDecimal(19, 3).Nullable()
                  .WithColumn("WednesdayEntryDate").AsDateTime().Nullable()
                  .WithColumn("ThursdayEntry").AsDecimal(19, 3).Nullable()
                  .WithColumn("ThursdayEntryDate").AsDateTime().Nullable()
                  .WithColumn("FridayEntry").AsDecimal(19, 3).Nullable()
                  .WithColumn("FridayEntryDate").AsDateTime().Nullable()
                  .WithColumn("SaturdayEntry").AsDecimal(19, 3).Nullable()
                  .WithColumn("SaturdayEntryDate").AsDateTime().Nullable()
                  .WithColumn("SundayEntry").AsDecimal(19, 3).Nullable()
                  .WithColumn("SundayEntryDate").AsDateTime().Nullable()
                  .WithColumn("WeeklyTotal").AsDecimal(19, 3).Nullable();

            this.CreateModule("System Delivery Approver", "Production", 87);

            Execute.Sql($@"UPDATE Modules 
            SET Name = 'System Delivery Configuration'
            WHERE ModuleID = 85 And ApplicationID = 2");
        }

        public override void Down()
        {
            this.DeleteModuleAndAssociatedRoles("Production", "System Delivery Approver");
            Delete.Table("SystemDeliveryEntriesFacilities");
            Delete.Table("SystemDeliveryEntriesOperatingCenters");
            Delete.Table("SystemDeliveryEquipmentEntries");
            Delete.Table("SystemDeliveryEntries");

            Execute.Sql($@"UPDATE Modules 
            SET Name = 'System Delivery'
            WHERE ModuleID = 85 And ApplicationID = 2");
        }
    }
}