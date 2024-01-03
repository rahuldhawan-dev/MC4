﻿using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201214150428171), Tags("Production")]
    public class MC2640ProductionPreJobSafetyBriefs : Migration
    {
        private const string PRODUCTION_PRE_JOB_SAFETY_BRIEFS_TABLE = "ProductionPreJobSafetyBriefs";

        private void CreateLookupTableAndManyToMany(string tableName, string shortTableNameForFK,
            string manyToManyColumnName, params string[] descriptions)
        {
            this.CreateLookupTableWithValues(tableName, descriptions);
            var manyToManyTableName = $"{PRODUCTION_PRE_JOB_SAFETY_BRIEFS_TABLE}{tableName}";
            Create.Table(manyToManyTableName)
                   // The autogenerated FK names we create are too long so we need to make our own.
                  .WithColumn("ProductionPreJobSafetyBriefId").AsInt32().ForeignKey($"FK_{manyToManyTableName}_PJSBId",
                       PRODUCTION_PRE_JOB_SAFETY_BRIEFS_TABLE, "Id")
                  .Indexed("IX_ProductionPreJobSafetyBriefId")
                  .WithColumn(manyToManyColumnName).AsInt32()
                  .ForeignKey($"FK_PJSB_{shortTableNameForFK}", tableName, "Id");
        }

        private void DeleteLookupTableAndManyToMany(string tableName)
        {
            Delete.Table($"{PRODUCTION_PRE_JOB_SAFETY_BRIEFS_TABLE}{tableName}");
            Delete.Table(tableName);
        }

        public override void Up()
        {
            Create.Table(PRODUCTION_PRE_JOB_SAFETY_BRIEFS_TABLE)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithForeignKeyColumn("ProductionWorkOrderId", "ProductionWorkOrders").NotNullable()
                  .WithForeignKeyColumn("CreatedByUserId", "tblPermissions", "RecId").NotNullable()
                  .WithColumn("CreatedAt").AsDateTime().NotNullable()
                  .WithColumn("SafetyBriefDateTime").AsDateTime().NotNullable()
                  .WithColumn("AnyPotentialWeatherHazards").AsBoolean().NotNullable()
                  .WithColumn("HadConversationAboutWeatherHazard").AsBoolean().NotNullable()
                  .WithColumn("HadConversationAboutWeatherHazardNotes").AsAnsiString(255).Nullable()
                  .WithColumn("AnyTimeOfDayConstraints").AsBoolean().NotNullable()
                  .WithColumn("AnyTrafficHazards").AsBoolean().NotNullable()
                  .WithColumn("InvolveConfinedSpace").AsBoolean().NotNullable()
                  .WithColumn("AnyPotentialOverheadHazards").AsBoolean().NotNullable()
                  .WithColumn("AnyUndergroundHazards").AsBoolean().NotNullable()
                  .WithColumn("AreThereElectricalOrOtherEnergyHazards").AsBoolean().NotNullable()
                  .WithColumn("AnyWorkPerformedGreaterThanOrEqualToFourFeetAboveGroundLevel").AsBoolean().NotNullable()
                  .WithColumn("TypeOfFallPreventionProtectionSystemBeingUsed").AsAnsiString(255).Nullable()
                  .WithColumn("DoesJobInvolveUseOfChemicals").AsBoolean().NotNullable()
                  .WithColumn("IsSafetyDataSheetAvailableForEachChemical").AsBoolean()
                  .Nullable() // Only answered if DoesJobInvolve... is true
                  .WithColumn("HaveEquipmentToDoJobSafely").AsBoolean().NotNullable()
                  .WithColumn("HaveEquipmentToDoJobSafelyNotes").AsAnsiString(255).Nullable()
                  .WithColumn("HasPreUseInspectionBeenCompletedForEquipmentRequiringPreUseInspection").AsBoolean()
                  .NotNullable()
                  .WithColumn("ReviewedErgonomicHazards").AsBoolean().NotNullable()
                  .WithColumn("ReviewedErgonomicHazardsNotes").AsAnsiString(255).Nullable()
                  .WithColumn("HasStretchAndFlexBeenPerformed").AsBoolean().NotNullable()
                  .WithColumn("ReviewedLocationOfSafetyEquipment").AsBoolean().NotNullable()
                  .WithColumn("OtherHazardsIdentified").AsBoolean().NotNullable()
                  .WithColumn("OtherHazardNotes").AsAnsiString(255).Nullable()
                  .WithColumn("HadDiscussionAboutHazardsAndPrecautions").AsBoolean().NotNullable()
                  .WithColumn("HadDiscussionAboutHazardsAndPrecautionsNotes").AsAnsiString(255).Nullable()
                  .WithColumn("CrewMembersRemindedOfStopWorkAuthority").AsBoolean().NotNullable()
                  .WithColumn("FallProtection").AsBoolean().NotNullable()
                  .WithColumn("HeadProtection").AsBoolean().NotNullable()
                  .WithColumn("HandProtection").AsBoolean().NotNullable()
                  .WithColumn("ElectricalProtection").AsBoolean().NotNullable()
                  .WithColumn("FootProtection").AsBoolean().NotNullable()
                  .WithColumn("EyeProtection").AsBoolean().NotNullable()
                  .WithColumn("FaceShield").AsBoolean().NotNullable()
                  .WithColumn("SafetyGarment").AsBoolean().NotNullable()
                  .WithColumn("HearingProtection").AsBoolean().NotNullable()
                  .WithColumn("RespiratoryProtection").AsBoolean().NotNullable()
                  .WithColumn("PPEOther").AsBoolean().NotNullable()
                  .WithColumn("PPEOtherNotes").AsAnsiString(255).Nullable();

            Create.Table("ProductionPreJobSafetyBriefEmployees")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ProductionPreJobSafetyBriefId", PRODUCTION_PRE_JOB_SAFETY_BRIEFS_TABLE)
                  .NotNullable().Indexed()
                  .WithForeignKeyColumn("EmployeeId", "tblEmployee", "tblEmployeeId").NotNullable()
                  .WithColumn("SignedAt").AsDateTime().NotNullable();

            this.AddDataType(PRODUCTION_PRE_JOB_SAFETY_BRIEFS_TABLE);
            this.AddDocumentType("Hazards", PRODUCTION_PRE_JOB_SAFETY_BRIEFS_TABLE);

            CreateLookupTableAndManyToMany("ProductionPreJobSafetyBriefWeatherHazardTypes", "PPJSBWeatherHazardTypes",
                "ProductionPreJobSafetyBriefWeatherHazardTypeId",
                "Wet", "Snow", "Ice", "Excessive Heat", "Excessive Cold", "Excessive Winds");
            CreateLookupTableAndManyToMany("ProductionPreJobSafetyBriefTimeOfDayConstraintTypes",
                "PPJSBConstraintTypes", "ProductionPreJobSafetyBriefTimeOfDayConstraintTypeId",
                "Inadequate Lighting", "Poor Visibility", "Sun Glare", "Fog", "Darkness");
            CreateLookupTableAndManyToMany("ProductionPreJobSafetyBriefTrafficHazardTypes", "PPJSBTrafficHazardTypes",
                "ProductionPreJobSafetyBriefTrafficHazardTypeId",
                "High speed roadway", "High volume traffic area", "In or near roadway", "Limited shoulder space",
                "Near Fire Station/ EMS", "Work in roadway");
            CreateLookupTableAndManyToMany("ProductionPreJobSafetyBriefOverheadHazardTypes", "PPJSBOverheadHazardTypes",
                "ProductionPreJobSafetyBriefOverheadHazardTypeId",
                "Bridge/Overpass", "Electrical Wires", "Equipment", "Process Piping / Chemical Feed Line",
                "Tree Limbs");
            CreateLookupTableAndManyToMany("ProductionPreJobSafetyBriefUndergroundHazardTypes",
                "PPJSBUndergroundHazardTypes", "ProductionPreJobSafetyBriefUndergroundHazardTypeId",
                "Contaminated Soil", "Gas", "Electrical", "Sewer", "Ground Water Level", "Piping", "Soil Type",
                "Communication Lines");
            CreateLookupTableAndManyToMany("ProductionPreJobSafetyBriefElectricalHazardTypes",
                "PPJSBElectricalHazardTypes", "ProductionPreJobSafetyBriefElectricalHazardTypeId",
                "Chemical", "Electrical", "Gravity", "Hydraulic", "Pneumatic", "Pressure / Tension", "Steam",
                "Surface Temperature");
        }

        public override void Down()
        {
            this.RemoveDocumentTypeAndAllRelatedDocuments("Hazards", PRODUCTION_PRE_JOB_SAFETY_BRIEFS_TABLE);
            this.RemoveDataType(PRODUCTION_PRE_JOB_SAFETY_BRIEFS_TABLE);

            Delete.Table("ProductionPreJobSafetyBriefEmployees");

            DeleteLookupTableAndManyToMany("ProductionPreJobSafetyBriefWeatherHazardTypes");
            DeleteLookupTableAndManyToMany("ProductionPreJobSafetyBriefTimeOfDayConstraintTypes");
            DeleteLookupTableAndManyToMany("ProductionPreJobSafetyBriefElectricalHazardTypes");
            DeleteLookupTableAndManyToMany("ProductionPreJobSafetyBriefTrafficHazardTypes");
            DeleteLookupTableAndManyToMany("ProductionPreJobSafetyBriefOverheadHazardTypes");
            DeleteLookupTableAndManyToMany("ProductionPreJobSafetyBriefUndergroundHazardTypes");

            Delete.Table(PRODUCTION_PRE_JOB_SAFETY_BRIEFS_TABLE);
        }
    }
}
