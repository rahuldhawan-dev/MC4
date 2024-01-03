using System;
using FluentMigrator;
using Humanizer;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20230314092846124), Tags("Production")]
    public class MC5116_NormalizeNullabilityOfChangeTrackingColumns : Migration
    {
        private void UnNullifyCreatedAt(
            string entityName,
            string tableName = null,
            string idColumn = "Id")
        {
            tableName = tableName ?? entityName.Pluralize();
            
            this.UpdateColumnFromAuditLog(
                entityName,
                "CreatedAt",
                "Timestamp",
                "Create",
                tableName,
                idColumn);

            Update
               .Table(tableName)
               .Set(new {CreatedAt = (DateTime?)new DateTime(1753, 1, 1)})
               .Where(new {CreatedAt = (DateTime?)null});

            Alter.Column("CreatedAt").OnTable(tableName).AsDateTime().NotNullable();
        }

        private void UnNullifyUpdatedAt(
            string entityName,
            string tableName = null,
            string idColumn = "Id",
            bool useCreatedAt = true)
        {
            tableName = tableName ?? entityName.Pluralize();

            this.UpdateColumnFromAuditLog(
                entityName,
                "UpdatedAt",
                "Timestamp",
                "Update",
                tableName,
                idColumn);

            if (useCreatedAt)
            {
                Execute.Sql($"UPDATE {tableName} SET UpdatedAt = CreatedAt WHERE UpdatedAt IS NULL;");
            }

            Update
               .Table(tableName)
               .Set(new {UpdatedAt = (DateTime?)new DateTime(1753, 1, 1)})
               .Where(new {UpdatedAt = (DateTime?)null});
            
            Alter.Column("UpdatedAt").OnTable(tableName).AsDateTime().NotNullable();
        }

        private void ReNullifyCreatedAt(string tableName)
        {
            Alter.Column("CreatedAt").OnTable(tableName).AsDateTime().Nullable();
        }

        private void ReNullifyUpdatedAt(string tableName)
        {
            Alter.Column("UpdatedAt").OnTable(tableName).AsDateTime().Nullable();
        }
        
        public override void Up()
        {
            UnNullifyCreatedAt("ActionItem");
            UnNullifyCreatedAt("AllocationPermit", idColumn: "AllocationPermitID");
            UnNullifyCreatedAt("AsBuiltImage", idColumn: "AsBuiltImageID");
            UnNullifyCreatedAt("BlowOffInspection");
            UnNullifyCreatedAt("ConfinedSpaceForm");
            UnNullifyCreatedAt("Document", "Document", "DocumentID");
            UnNullifyCreatedAt("Equipment", "Equipment", "EquipmentID");
            UnNullifyCreatedAt("Hydrant");
            UnNullifyCreatedAt("HydrantInspection");
            UnNullifyCreatedAt("Interconnection", idColumn: "InterconnectionId");
            UnNullifyCreatedAt("Meter", idColumn: "MeterId");
            UnNullifyCreatedAt("Note", "Note", "NoteId");
            UnNullifyCreatedAt("PublicWaterSupplyCustomerData", "tblPWSID_Customer_Data", "CustomerDataID");
            UnNullifyCreatedAt("Regulation", idColumn: "RegulationID");
            UnNullifyCreatedAt("RoadwayImprovementNotification");
            UnNullifyCreatedAt("Service");
            
            Execute.Sql(@"
DROP INDEX [_dta_index_SewerMainCleanings_19_1225823479__K39_1_2_3_4_5_6_7_8_9_13_14_16_17_18_20_21_22_24_25_26_28_35_37_40_41_42_44_45_46_] ON [dbo].[SewerMainCleanings];
DROP INDEX [_dta_index_SewerMainCleanings_19_1225823479__K41_1_2_3_4_5_6_7_8_9_13_14_16_17_18_20_21_22_24_25_26_28_35_37_39_40_42_44_45_46_] ON [dbo].[SewerMainCleanings]");
            
            UnNullifyCreatedAt("SewerMainCleaning", idColumn: "SewerMainCleaningID");
            
            Execute.Sql(@"
CREATE NONCLUSTERED INDEX [_dta_index_SewerMainCleanings_19_1225823479__K39_1_2_3_4_5_6_7_8_9_13_14_16_17_18_20_21_22_24_25_26_28_35_37_40_41_42_44_45_46_] ON [dbo].[SewerMainCleanings]
(
	[Opening1Id] ASC
)
INCLUDE([SewerMainCleaningID],[OperatingCenterID],[Date],[StreetID],[IntersectingStreetID],[TownID],[PayTypeID],[CodeTypeID],[RootCutter],[HydrantUsedID],[OpeningCatchbasin1],[ConditionOfOpening1Id],[Opening1FrameAndCoverId],[OpeningCatchbasin2],[ConditionOfOpening2Id],[Opening2FrameAndCoverId],[MapPage],[NumberOfEmployees],[Overflow],[CleaningScheduleID],[OverflowOpening1],[Opening2Id],[OverflowOpening2],[IntersectingStreet2],[CreatedBy],[CreatedAt],[InspectedDate],[SewerOverflowId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [_dta_index_SewerMainCleanings_19_1225823479__K41_1_2_3_4_5_6_7_8_9_13_14_16_17_18_20_21_22_24_25_26_28_35_37_39_40_42_44_45_46_] ON [dbo].[SewerMainCleanings]
(
	[Opening2Id] ASC
)
INCLUDE([SewerMainCleaningID],[OperatingCenterID],[Date],[StreetID],[IntersectingStreetID],[TownID],[PayTypeID],[CodeTypeID],[RootCutter],[HydrantUsedID],[OpeningCatchbasin1],[ConditionOfOpening1Id],[Opening1FrameAndCoverId],[OpeningCatchbasin2],[ConditionOfOpening2Id],[Opening2FrameAndCoverId],[MapPage],[NumberOfEmployees],[Overflow],[CleaningScheduleID],[Opening1Id],[OverflowOpening1],[OverflowOpening2],[IntersectingStreet2],[CreatedBy],[CreatedAt],[InspectedDate],[SewerOverflowId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
");
            
            UnNullifyCreatedAt("SewerOpening");
            UnNullifyCreatedAt("SewerOpeningConnection");

            Execute.Sql(@"
IF (OBJECT_ID('dbo.DF_TapImages_DateAdded', 'D') IS NOT NULL)
BEGIN
    ALTER TABLE dbo.TapImages
        DROP CONSTRAINT DF_TapImages_DateAdded
END");

            Alter.Column("CreatedAt").OnTable("TapImages").AsDateTime().Nullable();

            UnNullifyCreatedAt("TapImage", idColumn: "TapImageID");
            
            Execute.Sql(@"
DROP INDEX [_dta_index_Valves_5_1031010754__K57_K56_K61_K25_K64_K54_K30_K70_K65_K53_K41_3_4_5_8_9_10_12_18_22_28_29_32_33_34_38_40_45_46_] ON [dbo].[Valves]
DROP INDEX [_dta_index_Valves_6_1031010754__K41_K57_K56_K61_3_4_5_8_9_10_12_18_22_25_28_29_30_32_33_34_38_40_45_46_49_51_52_53_54_55_58_59_] ON [dbo].[Valves]");
            
            UnNullifyCreatedAt("Valve");
            
            Execute.Sql(@"
DROP INDEX [_dta_index_ValveImages_5_1653685039__K35_K36_K28_1] ON [dbo].[ValveImages]
DROP STATISTICS [dbo].[ValveImages].[_dta_stat_1653685039_36_28]");
            
            UnNullifyCreatedAt("ValveImage", idColumn: "ValveImageID");
            
            Execute.Sql(@"
CREATE NONCLUSTERED INDEX [_dta_index_ValveImages_5_1653685039__K35_K36_K28_1] ON [dbo].[ValveImages]
(
	[ValveID] ASC,
	[IsDefault] ASC,
	[CreatedAt] ASC
)
INCLUDE([ValveImageID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

CREATE STATISTICS [_dta_stat_1653685039_36_28] ON [dbo].[ValveImages]([IsDefault], [CreatedAt])
");
            
            UnNullifyCreatedAt("ValveInspection");
            UnNullifyCreatedAt("Vehicle", idColumn: "VehicleID");
            UnNullifyCreatedAt("WaterSample");
            
            UnNullifyUpdatedAt("AsBuiltImage", idColumn: "AsBuiltImageID");
            UnNullifyUpdatedAt("Document", "Document", "DocumentID");
            UnNullifyUpdatedAt("Hydrant");
            UnNullifyUpdatedAt("Incident");
            UnNullifyUpdatedAt("PublicWaterSupply", "PublicWaterSupplies", useCreatedAt: false);
            UnNullifyUpdatedAt("PublicWaterSupplyCustomerData", "tblPWSID_Customer_Data", "CustomerDataID");
            UnNullifyUpdatedAt(
                "PublicWaterSupplyFirmCapacity",
                "PublicWaterSupplyFirmCapacities",
                useCreatedAt: false);
            UnNullifyUpdatedAt("Service");
            UnNullifyUpdatedAt("SewerOpening");
            UnNullifyUpdatedAt("SystemDeliveryEntry", "SystemDeliveryEntries", useCreatedAt: false);
            UnNullifyUpdatedAt("Valve");
            
            Execute.Sql(@"
CREATE NONCLUSTERED INDEX [_dta_index_Valves_5_1031010754__K57_K56_K61_K25_K64_K54_K30_K70_K65_K53_K41_3_4_5_8_9_10_12_18_22_28_29_32_33_34_38_40_45_46_] ON [dbo].[Valves]
(
	[OperatingCenterId] ASC,
	[NormalPositionId] ASC,
	[ValveControlsId] ASC,
	[Id] ASC,
	[ValveSizeId] ASC,
	[CrossStreetId] ASC,
	[StreetId] ASC,
	[CoordinateId] ASC,
	[AssetStatusId] ASC,
	[ValveBillingId] ASC,
	[ValveSuffix] ASC
)
INCLUDE([BPUKPI],[Critical],[CriticalNotes],[DateRetired],[DateTested],[Elevation],[InspectionFrequency],[MapPage],[ObjectID],[SketchNumber],[StreetNumber],[Town],[Traffic],[Turns],[ValveLocation],[ValveNumber],[WorkOrderNumber],[CreatedAt],[DateInstalled],[UpdatedAt],[SAPEquipmentID],[InspectionFrequencyUnitId],[OpensId],[TownSectionId],[MainTypeId],[ValveMakeId],[ValveTypeId],[ValveZoneId],[WaterSystemId],[FunctionalLocationId],[InitiatorId],[Route],[Stop],[FacilityId],[GISUID],[SAPErrorCode],[ControlsCrossing]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [_dta_index_Valves_6_1031010754__K41_K57_K56_K61_3_4_5_8_9_10_12_18_22_25_28_29_30_32_33_34_38_40_45_46_49_51_52_53_54_55_58_59_] ON [dbo].[Valves]
(
	[ValveSuffix] ASC,
	[OperatingCenterId] ASC,
	[NormalPositionId] ASC,
	[ValveControlsId] ASC
)
INCLUDE([BPUKPI],[Critical],[CriticalNotes],[DateRetired],[DateTested],[Elevation],[InspectionFrequency],[MapPage],[ObjectID],[Id],[SketchNumber],[StreetNumber],[StreetId],[Town],[Traffic],[Turns],[ValveLocation],[ValveNumber],[WorkOrderNumber],[CreatedAt],[DateInstalled],[UpdatedAt],[SAPEquipmentID],[ValveBillingId],[CrossStreetId],[InspectionFrequencyUnitId],[OpensId],[TownSectionId],[MainTypeId],[ValveMakeId],[ValveTypeId],[ValveSizeId],[AssetStatusId],[ValveZoneId],[WaterSystemId],[FunctionalLocationId],[InitiatorId],[CoordinateId],[Route],[Stop],[FacilityId],[GISUID],[SAPErrorCode],[ControlsCrossing]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
");
        }

        public override void Down()
        {
            ReNullifyCreatedAt("ActionItems");
            ReNullifyCreatedAt("AllocationPermits");
            ReNullifyCreatedAt("AsBuiltImages");
            ReNullifyCreatedAt("BlowOffInspections");
            ReNullifyCreatedAt("ConfinedSpaceForms");
            ReNullifyCreatedAt("Document");
            ReNullifyCreatedAt("Equipment");
            ReNullifyCreatedAt("Hydrants");
            ReNullifyCreatedAt("HydrantInspections");
            ReNullifyCreatedAt("Interconnections");
            ReNullifyCreatedAt("Meters");
            ReNullifyCreatedAt("Note");
            ReNullifyCreatedAt("tblPWSID_Customer_Data");
            ReNullifyCreatedAt("Regulations");
            ReNullifyCreatedAt("RoadwayImprovementNotifications");
            ReNullifyCreatedAt("Services");

            Execute.Sql(@"
DROP INDEX [_dta_index_SewerMainCleanings_19_1225823479__K39_1_2_3_4_5_6_7_8_9_13_14_16_17_18_20_21_22_24_25_26_28_35_37_40_41_42_44_45_46_] ON [dbo].[SewerMainCleanings];
DROP INDEX [_dta_index_SewerMainCleanings_19_1225823479__K41_1_2_3_4_5_6_7_8_9_13_14_16_17_18_20_21_22_24_25_26_28_35_37_39_40_42_44_45_46_] ON [dbo].[SewerMainCleanings]");
            
            ReNullifyCreatedAt("SewerMainCleanings");
            
            Execute.Sql(@"
CREATE NONCLUSTERED INDEX [_dta_index_SewerMainCleanings_19_1225823479__K39_1_2_3_4_5_6_7_8_9_13_14_16_17_18_20_21_22_24_25_26_28_35_37_40_41_42_44_45_46_] ON [dbo].[SewerMainCleanings]
(
	[Opening1Id] ASC
)
INCLUDE([SewerMainCleaningID],[OperatingCenterID],[Date],[StreetID],[IntersectingStreetID],[TownID],[PayTypeID],[CodeTypeID],[RootCutter],[HydrantUsedID],[OpeningCatchbasin1],[ConditionOfOpening1Id],[Opening1FrameAndCoverId],[OpeningCatchbasin2],[ConditionOfOpening2Id],[Opening2FrameAndCoverId],[MapPage],[NumberOfEmployees],[Overflow],[CleaningScheduleID],[OverflowOpening1],[Opening2Id],[OverflowOpening2],[IntersectingStreet2],[CreatedBy],[CreatedAt],[SewerOverflowId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [_dta_index_SewerMainCleanings_19_1225823479__K41_1_2_3_4_5_6_7_8_9_13_14_16_17_18_20_21_22_24_25_26_28_35_37_39_40_42_44_45_46_] ON [dbo].[SewerMainCleanings]
(
	[Opening2Id] ASC
)
INCLUDE([SewerMainCleaningID],[OperatingCenterID],[Date],[StreetID],[IntersectingStreetID],[TownID],[PayTypeID],[CodeTypeID],[RootCutter],[HydrantUsedID],[OpeningCatchbasin1],[ConditionOfOpening1Id],[Opening1FrameAndCoverId],[OpeningCatchbasin2],[ConditionOfOpening2Id],[Opening2FrameAndCoverId],[MapPage],[NumberOfEmployees],[Overflow],[CleaningScheduleID],[Opening1Id],[OverflowOpening1],[OverflowOpening2],[IntersectingStreet2],[CreatedBy],[CreatedAt],[SewerOverflowId]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
");
            
            ReNullifyCreatedAt("SewerOpenings");
            ReNullifyCreatedAt("SewerOpeningConnections");
            ReNullifyCreatedAt("TapImages");
            
            Execute.Sql(@"
DROP INDEX [_dta_index_ValveImages_5_1653685039__K35_K36_K28_1] ON [dbo].[ValveImages]
DROP STATISTICS [dbo].[ValveImages].[_dta_stat_1653685039_36_28]");
            
            ReNullifyCreatedAt("ValveImages");
            
            Execute.Sql(@"
CREATE NONCLUSTERED INDEX [_dta_index_ValveImages_5_1653685039__K35_K36_K28_1] ON [dbo].[ValveImages]
(
	[ValveID] ASC,
	[IsDefault] ASC,
	[CreatedAt] ASC
)
INCLUDE([ValveImageID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

CREATE STATISTICS [_dta_stat_1653685039_36_28] ON [dbo].[ValveImages]([IsDefault], [CreatedAt])
");

            Execute.Sql(@"
DROP INDEX [_dta_index_Valves_5_1031010754__K57_K56_K61_K25_K64_K54_K30_K70_K65_K53_K41_3_4_5_8_9_10_12_18_22_28_29_32_33_34_38_40_45_46_] ON [dbo].[Valves]
DROP INDEX [_dta_index_Valves_6_1031010754__K41_K57_K56_K61_3_4_5_8_9_10_12_18_22_25_28_29_30_32_33_34_38_40_45_46_49_51_52_53_54_55_58_59_] ON [dbo].[Valves]");
            
            ReNullifyCreatedAt("Valves");
            
            ReNullifyCreatedAt("ValveInspections");
            ReNullifyCreatedAt("Vehicles");
            ReNullifyCreatedAt("WaterSamples");

            ReNullifyUpdatedAt("AsBuiltImages");
            ReNullifyUpdatedAt("Document");
            ReNullifyUpdatedAt("Hydrants");
            ReNullifyUpdatedAt("Incidents");
            ReNullifyUpdatedAt("PublicWaterSupplies");
            ReNullifyUpdatedAt("tblPWSID_Customer_Data");
            ReNullifyUpdatedAt("PublicWaterSupplyFirmCapacities");
            ReNullifyUpdatedAt("Services");
            ReNullifyUpdatedAt("SewerOpenings");
            ReNullifyUpdatedAt("SystemDeliveryEntries");
            ReNullifyUpdatedAt("Valves");
            
            Execute.Sql(@"
CREATE NONCLUSTERED INDEX [_dta_index_Valves_5_1031010754__K57_K56_K61_K25_K64_K54_K30_K70_K65_K53_K41_3_4_5_8_9_10_12_18_22_28_29_32_33_34_38_40_45_46_] ON [dbo].[Valves]
(
	[OperatingCenterId] ASC,
	[NormalPositionId] ASC,
	[ValveControlsId] ASC,
	[Id] ASC,
	[ValveSizeId] ASC,
	[CrossStreetId] ASC,
	[StreetId] ASC,
	[CoordinateId] ASC,
	[AssetStatusId] ASC,
	[ValveBillingId] ASC,
	[ValveSuffix] ASC
)
INCLUDE([BPUKPI],[Critical],[CriticalNotes],[DateRetired],[DateTested],[Elevation],[InspectionFrequency],[MapPage],[ObjectID],[SketchNumber],[StreetNumber],[Town],[Traffic],[Turns],[ValveLocation],[ValveNumber],[WorkOrderNumber],[CreatedAt],[DateInstalled],[UpdatedAt],[SAPEquipmentID],[InspectionFrequencyUnitId],[OpensId],[TownSectionId],[MainTypeId],[ValveMakeId],[ValveTypeId],[ValveZoneId],[WaterSystemId],[FunctionalLocationId],[InitiatorId],[Route],[Stop],[FacilityId],[GISUID],[SAPErrorCode],[ControlsCrossing]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [_dta_index_Valves_6_1031010754__K41_K57_K56_K61_3_4_5_8_9_10_12_18_22_25_28_29_30_32_33_34_38_40_45_46_49_51_52_53_54_55_58_59_] ON [dbo].[Valves]
(
	[ValveSuffix] ASC,
	[OperatingCenterId] ASC,
	[NormalPositionId] ASC,
	[ValveControlsId] ASC
)
INCLUDE([BPUKPI],[Critical],[CriticalNotes],[DateRetired],[DateTested],[Elevation],[InspectionFrequency],[MapPage],[ObjectID],[Id],[SketchNumber],[StreetNumber],[StreetId],[Town],[Traffic],[Turns],[ValveLocation],[ValveNumber],[WorkOrderNumber],[CreatedAt],[DateInstalled],[UpdatedAt],[SAPEquipmentID],[ValveBillingId],[CrossStreetId],[InspectionFrequencyUnitId],[OpensId],[TownSectionId],[MainTypeId],[ValveMakeId],[ValveTypeId],[ValveSizeId],[AssetStatusId],[ValveZoneId],[WaterSystemId],[FunctionalLocationId],[InitiatorId],[CoordinateId],[Route],[Stop],[FacilityId],[GISUID],[SAPErrorCode],[ControlsCrossing]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
");
        }
    }
}

