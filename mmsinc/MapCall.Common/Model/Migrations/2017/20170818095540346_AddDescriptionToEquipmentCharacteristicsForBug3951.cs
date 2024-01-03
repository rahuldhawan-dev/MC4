using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170818095540346), Tags("Production")]
    public class AddDescriptionToEquipmentCharacteristicsForBug3951 : Migration
    {
        public override void Up()
        {
            Alter.Table("EquipmentCharacteristicFields").AddColumn("Description").AsAnsiString(100).Nullable();
            Alter.Table("Equipment").AddColumn("DateRetired").AsDateTime().Nullable();
            Execute.Sql("insert into NotificationPurposes values(29, 'Equipment Manufacturer Unknown')");
            this.CreateLookupTableWithValues("ProductionPrerequisites", "Has Lockout Requirement", "Is Confined Space",
                "Job Safety Checklist", "Air Permit", "Hot Work");
            Create.Table("EquipmentProductionPrerequisites")
                   //.WithIdentityColumn()
                  .WithForeignKeyColumn("EquipmentId", "Equipment", "EquipmentId", nullable: false)
                  .WithForeignKeyColumn("ProductionPrerequisiteId", "ProductionPrerequisites", "Id", nullable: false);
            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 FROM EquipmentStatuses WHERE Description = 'Cancelled') INSERT INTO EquipmentStatuses Values('Cancelled')");
            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 FROM EquipmentStatuses WHERE Description = 'Field Installed') INSERT INTO EquipmentStatuses Values('Field Installed')");
            Execute.Sql(
                "INSERT INTO EquipmentProductionPrerequisites SELECT EquipmentId, (SELECT Id from ProductionPrerequisites where Description = 'Has Lockout Requirement') FROM Equipment where HasLockoutRequirement = 1");
            Execute.Sql(
                "INSERT INTO EquipmentProductionPrerequisites SELECT EquipmentId, (SELECT Id from ProductionPrerequisites where Description = 'Is Confined Space') FROM Equipment where IsConfinedSpace = 1");
            Delete.Column("HasLockoutRequirement").FromTable("Equipment");
            Delete.Column("IsConfinedSpace").FromTable("Equipment");
            Execute.Sql("INSERT INTO EquipmentCharacteristics(EquipmentId, FieldId, Value) " +
                        " select EquipmentId, ECF.Id as FieldID, CriticalNotes as Value from Equipment E LEFT JOIN EquipmentCharacteristicFields ECF ON ECF.SAPEquipmentTypeID = E.SAPEquipmentTypeID WHERE Criticalnotes is not null AND ECF.FieldName like 'SPECIAL%' order by E.EquipmentID");
            Delete.ForeignKey("FK_tblFacilities_FunctionalLocations_FunctionalLocationId").OnTable("tblFacilities");
            Alter.Table("tblFacilities").AlterColumn("FunctionalLocationId").AsAnsiString(30).Nullable();
            Execute.Sql(
                "UPDATE tblFacilities SET FunctionalLocationID = (Select Description from FunctionalLocations FL where FL.FunctionalLocationID = tblFacilities.FunctionalLocationID) FROM tblFacilities WHERE FunctionalLocationID is not null");
            Alter.Table("Equipment").AddColumn("ManufacturerOther").AsAnsiString(50).Nullable();
            Alter.Table("SAPEquipmentManufacturers").AddColumn("MapCallDescription").AsAnsiString(50).Nullable();
            Execute.Sql(
                "Update SAPEquipmentManufacturers set MapCallDescription = 'UNKNOWN/OTHER' where Description = 'Unknown';");
            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 FROM NotificationPurposes WHERE Purpose = 'Field Installed') INSERT INTO NotificationPurposes Values(29, 'Field Installed')");
        }

        public override void Down()
        {
            Execute.Sql(
                "update tblFacilities set FunctionalLocationID = (select top 1 FunctionalLocationID from FunctionalLocations where FunctionalLocations.[Description] = tblFacilities.FunctionalLocationID and tblFacilities.TownID = FunctionalLocations.TownID);");
            Alter.Column("FunctionalLocationId").OnTable("tblFacilities").AsInt32().Nullable();
            Create.ForeignKey("FK_tblFacilities_FunctionalLocations_FunctionalLocationId").FromTable("tblFacilities")
                  .ForeignColumn("FunctionalLocationId").ToTable("FunctionalLocations")
                  .PrimaryColumn("FunctionalLocationId");
            Alter.Table("Equipment").AddColumn("HasLockoutRequirement").AsBoolean().WithDefaultValue(false)
                 .NotNullable();
            Execute.Sql(
                "Update Equipment SET HasLockoutRequirement = 1 WHERE EXISTS (SELECT 1 FROM EquipmentProductionPrerequisites EPP where Equipment.EquipmentID = EPP.EquipmentId AND EPP.ProductionPrerequisiteId = (SELECT Id from ProductionPrerequisites where Description = 'Has Lockout Requirement'))");
            Alter.Table("Equipment").AddColumn("IsConfinedSpace").AsBoolean().WithDefaultValue(false).NotNullable();
            Execute.Sql(
                "Update Equipment SET IsConfinedSpace = 1 WHERE EXISTS(SELECT 1 FROM EquipmentProductionPrerequisites EPP where Equipment.EquipmentID = EPP.EquipmentId AND EPP.ProductionPrerequisiteId = (SELECT Id from ProductionPrerequisites where Description = 'Is Confined Space'))");
            Delete.Column("Description").FromTable("EquipmentCharacteristicFields");
            Delete.Column("DateRetired").FromTable("Equipment");
            Execute.Sql(
                "delete from NotificationPurposes where ModuleId = 29 AND Purpose = 'Equipment Manufacturer Unknown'");
            Delete.Table("EquipmentProductionPrerequisites");
            Delete.Table("ProductionPrerequisites");
            Delete.Column("ManufacturerOther").FromTable("Equipment");
            Delete.Column("MapCallDescription").FromTable("SAPEquipmentManufacturers");
        }
    }
}
