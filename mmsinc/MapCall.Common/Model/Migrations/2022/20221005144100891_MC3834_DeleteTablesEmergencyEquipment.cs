using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221005144100891), Tags("Production")]
    public class MC3834_DeleteTablesEmergencyEquipment : Migration
    {
        public override void Up()
        {
            Delete.Table("EmergencyEquipment");
            Delete.Table("EmergencyEquipmentCategories");
        }

        public override void Down()
        {
            Create.Table("EmergencyEquipmentCategories")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("Description").AsString();

            Create.Table("EmergencyEquipment")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("OperatingCenterId").AsInt32().ForeignKey("FK_EmergencyEquipment_OperatingCenters_OperatingCenterId", "OperatingCenters", "OperatingCenterId")
                  .WithColumn("WaterTypeId").AsInt32().ForeignKey("FK_EmergencyEquipment_WaterTypes_WaterTypeId", "WaterTypes", "Id")
                  .WithColumn("IsExternalResource").AsBoolean()
                  .WithColumn("EmergencyEquipmentCategoryId").AsInt32().ForeignKey("FK_EmergencyEquipment_EmergencyEquipmentCategories_EmergencyEquipmentCategoryId", "EmergencyEquipmentCategories", "Id")
                  .WithColumn("Description").AsString()
                  .WithColumn("Quantity").AsInt32()
                  .WithColumn("EquipmentId").AsInt32().ForeignKey("FK_EmergencyEquipment_Equipment_EquipmentId", "Equipment", "EquipmentID")
                  .WithColumn("StorageLocationFacilityId").AsInt32().ForeignKey("FK_EmergencyEquipment_tblFacilities_StorageLocationFacilityId", "tblFacilities", "RecordId")
                  .WithColumn("CoordinateId").AsInt32().ForeignKey("FK_EmergencyEquipment_Coordinates_CoordinateId", "Coordinates", "CoordinateId")
                  .WithColumn("StorageLocation").AsString()
                  .WithColumn("StorageRequirements").AsString()
                  .WithColumn("InspectionFrequencyMonths").AsString()
                  .WithColumn("IsOnTrailer").AsBoolean()
                  .WithColumn("TrailerHitchTypeId").AsInt32().ForeignKey("FK_EmergencyEquipment_TrailerHitchTypes_TrailerHitchTypeId", "TrailerHitchTypes", "Id")
                  .WithColumn("TransportationRequirementId").AsInt32().ForeignKey("FK_EmergencyEquipment_TransportationRequirements_TransportationRequirementId", "TransportationRequirements", "Id")
                  .WithColumn("OperatingInstructions").AsString()
                  .WithColumn("PrimaryContactId").AsInt32().ForeignKey("FK_EmergencyEquipment_Contacts_PrimaryContactId", "Contacts", "ContactID");
        }
    }
}
  