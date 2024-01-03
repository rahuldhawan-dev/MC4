using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Data;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150527151258668), Tags("Production")]
    public class AddEmergencyResponseTablesForBug2231 : Migration
    {
        public struct TableNames
        {
            public const string
                CONTACTS = "Contacts",
                COORDINATES = "Coordinates",
                DRILL_FREQUENCIES = "DrillFrequencies",
                EMERGENCY_EQUIPMENT = "EmergencyEquipment",
                EMERGENCY_EQUIPMENT_CATEGORIES = "EmergencyEquipmentCategories",
                EMERGENCY_RESPONSE_PLANS = "EmergencyResponsePlans",
                EMERGENCY_PLAN_CATEGORIES = "EmergencyPlanCategories",
                EMERGENCY_PLAN_SUB_CATEGORIES = "EmergencyPlanSubCategories",
                FACILITIES = "tblFacilities",
                OPERATING_CENTERS = "OperatingCenters",
                TABLETOP_FREQUENCIES = "TabletopFrequencies",
                TRAILER_HITCH_TYPES = "TrailerHitchTypes",
                TRANSPORTATION_REQUIREMENTS = "TransportationRequirements",
                WATER_TYPES = "WaterTypes";
        }

        public struct StringLengths
        {
            public const int
                STORAGE_LOCATION = 255,
                STORAGE_REQUIREMENTS = 255,
                TITLE = 255,
                DESCRIPTION = 255;
        }

        public override void Up()
        {
            this.CreateLookupTableWithValues(TableNames.WATER_TYPES, "Water", "Wastewater", "Water & Wastewater");
            this.CreateLookupTableWithValues(TableNames.EMERGENCY_EQUIPMENT_CATEGORIES, "Chemical Feed",
                "Chemical Tank",
                "Fitting", "Generator", "Instrumentation", "Hose/Pipe", "Lighting", "Pump", "Tool",
                "Signage & Traffic Control", "Water-Bottled", "Water Distribution", "Water Storage");
            this.CreateLookupTableWithValues(TableNames.TRAILER_HITCH_TYPES, "None", "Ball - 2\"", "Ball - 2 1/4\"",
                "Ball - 2 5/16\"", "Ball - 3\"", "Pintel Hook");
            this.CreateLookupTableWithValues(TableNames.TRANSPORTATION_REQUIREMENTS, "Pick Up", "Crew Truck",
                "Dump Truck", "Flat Bed");

            Create.Table(TableNames.EMERGENCY_EQUIPMENT)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("OperatingCenterId", TableNames.OPERATING_CENTERS, "OperatingCenterId")
                  .NotNullable()
                  .WithForeignKeyColumn("WaterTypeId", TableNames.WATER_TYPES)
                  .WithColumn("IsExternalResource").AsBoolean().NotNullable()
                  .WithForeignKeyColumn("EmergencyEquipmentCategoryId", TableNames.EMERGENCY_EQUIPMENT_CATEGORIES)
                  .NotNullable()
                  .WithColumn("Description").AsCustom("text").Nullable()
                  .WithColumn("Quantity").AsInt32().Nullable()
                  .WithForeignKeyColumn("EquipmentId", "Equipment", "EquipmentId").Nullable()
                  .WithForeignKeyColumn("StorageLocationFacilityId", TableNames.FACILITIES, "RecordId").Nullable()
                  .WithForeignKeyColumn("CoordinateId", TableNames.COORDINATES, "CoordinateId").Nullable()
                  .WithColumn("StorageLocation").AsAnsiString(StringLengths.STORAGE_LOCATION).Nullable()
                  .WithColumn("StorageRequirements").AsAnsiString(StringLengths.STORAGE_REQUIREMENTS).Nullable()
                  .WithColumn("InspectionFrequencyMonths").AsInt32().Nullable()
                  .WithColumn("IsOnTrailer").AsBoolean().NotNullable()
                  .WithForeignKeyColumn("TrailerHitchTypeId", TableNames.TRAILER_HITCH_TYPES).Nullable()
                  .WithForeignKeyColumn("TransportationRequirementId", TableNames.TRANSPORTATION_REQUIREMENTS)
                  .Nullable()
                  .WithColumn("TransportationInstructions").AsCustom("text").Nullable()
                  .WithColumn("OperatingInstructions").AsCustom("text").Nullable()
                  .WithForeignKeyColumn("PrimaryContactId", TableNames.CONTACTS, "ContactId").Nullable();

            this.CreateLookupTableWithValues(TableNames.EMERGENCY_PLAN_CATEGORIES);
            this.CreateLookupTableWithValues(TableNames.EMERGENCY_PLAN_SUB_CATEGORIES);
            this.CreateLookupTableWithValues(TableNames.TABLETOP_FREQUENCIES);
            this.CreateLookupTableWithValues(TableNames.DRILL_FREQUENCIES);

            Create.Table(TableNames.EMERGENCY_RESPONSE_PLANS)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("StateId", "States", "StateID")
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterId")
                  .WithForeignKeyColumn("FacilityId", TableNames.FACILITIES, "RecordId").Nullable()
                  .WithForeignKeyColumn("PlanCategoryId", TableNames.EMERGENCY_PLAN_CATEGORIES).Nullable()
                  .WithForeignKeyColumn("PlanSubcategoryId", TableNames.EMERGENCY_PLAN_SUB_CATEGORIES).Nullable()
                  .WithColumn("Title").AsAnsiString(StringLengths.TITLE).NotNullable()
                  .WithColumn("Description").AsCustom("text").NotNullable()
                  .WithForeignKeyColumn("TabletopFrequencyId", TableNames.TABLETOP_FREQUENCIES).Nullable()
                  .WithForeignKeyColumn("DrillFrequencyId", TableNames.DRILL_FREQUENCIES).Nullable();

            this.CreateDocumentType(TableNames.EMERGENCY_RESPONSE_PLANS, "EmergencyRepsonsePlan",
                new[] {"Emergency Response Plan"});
        }

        public override void Down()
        {
            this.DeleteDataType("EmergencyResponsePlans");
            Delete.ForeignKeyColumn(TableNames.EMERGENCY_EQUIPMENT, "TransportationRequirementId",
                TableNames.TRANSPORTATION_REQUIREMENTS);
            Delete.ForeignKeyColumn(TableNames.EMERGENCY_EQUIPMENT, "TrailerHitchTypeId",
                TableNames.TRAILER_HITCH_TYPES);
            Delete.ForeignKeyColumn(TableNames.EMERGENCY_EQUIPMENT, "EmergencyEquipmentCategoryId",
                TableNames.EMERGENCY_EQUIPMENT_CATEGORIES);
            Delete.ForeignKeyColumn(TableNames.EMERGENCY_EQUIPMENT, "WaterTypeId", TableNames.WATER_TYPES);

            Delete.Table(TableNames.EMERGENCY_EQUIPMENT);

            Delete.ForeignKeyColumn(TableNames.EMERGENCY_RESPONSE_PLANS, "PlanCategoryId",
                TableNames.EMERGENCY_PLAN_CATEGORIES);
            Delete.ForeignKeyColumn(TableNames.EMERGENCY_RESPONSE_PLANS, "PlanSubCategoryId",
                TableNames.EMERGENCY_PLAN_SUB_CATEGORIES);
            Delete.ForeignKeyColumn(TableNames.EMERGENCY_RESPONSE_PLANS, "TabletopFrequencyId",
                TableNames.TABLETOP_FREQUENCIES);
            Delete.ForeignKeyColumn(TableNames.EMERGENCY_RESPONSE_PLANS, "DrillFrequencyId",
                TableNames.DRILL_FREQUENCIES);

            Delete.Table(TableNames.EMERGENCY_RESPONSE_PLANS);

            Delete.Table(TableNames.DRILL_FREQUENCIES);
            Delete.Table(TableNames.TABLETOP_FREQUENCIES);
            Delete.Table(TableNames.EMERGENCY_PLAN_SUB_CATEGORIES);
            Delete.Table(TableNames.EMERGENCY_PLAN_CATEGORIES);

            Delete.Table(TableNames.TRANSPORTATION_REQUIREMENTS);
            Delete.Table(TableNames.TRAILER_HITCH_TYPES);
            Delete.Table(TableNames.EMERGENCY_EQUIPMENT_CATEGORIES);
            Delete.Table(TableNames.WATER_TYPES);
        }
    }
}
