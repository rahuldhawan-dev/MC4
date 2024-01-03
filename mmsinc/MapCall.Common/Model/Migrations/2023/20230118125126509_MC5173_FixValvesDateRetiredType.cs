using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230118125126509), Tags("Production")]
    public class MC5173_FixValvesDateRetiredType : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
DROP INDEX [_dta_index_Valves_5_1031010754__K57_K56_K61_K25_K64_K54_K30_K70_K65_K53_K41_3_4_5_8_9_10_12_18_22_28_29_32_33_34_38_40_45_46_] ON [dbo].[Valves];
DROP INDEX [_dta_index_Valves_6_1031010754__K41_K57_K56_K61_3_4_5_8_9_10_12_18_22_25_28_29_30_32_33_34_38_40_45_46_49_51_52_53_54_55_58_59_] ON [dbo].[Valves];");

            Alter.Table("Valves").AlterColumn("DateRetired").AsDateTime().Nullable();

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
INCLUDE([BPUKPI],[Critical],[CriticalNotes],[DateRetired],[DateTested],[Elevation],[InspectionFrequency],[MapPage],[ObjectID],[SketchNumber],[StreetNumber],[Town],[Traffic],[Turns],[ValveLocation],[ValveNumber],[WorkOrderNumber],[DateAdded],[DateInstalled],[LastUpdated],[SAPEquipmentID],[InspectionFrequencyUnitId],[OpensId],[TownSectionId],[MainTypeId],[ValveMakeId],[ValveTypeId],[ValveZoneId],[WaterSystemId],[FunctionalLocationId],[InitiatorId],[Route],[Stop],[FacilityId],[GISUID],[SAPErrorCode],[ControlsCrossing]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY];

CREATE NONCLUSTERED INDEX [_dta_index_Valves_6_1031010754__K41_K57_K56_K61_3_4_5_8_9_10_12_18_22_25_28_29_30_32_33_34_38_40_45_46_49_51_52_53_54_55_58_59_] ON [dbo].[Valves]
(
	[ValveSuffix] ASC,
	[OperatingCenterId] ASC,
	[NormalPositionId] ASC,
	[ValveControlsId] ASC
)
INCLUDE([BPUKPI],[Critical],[CriticalNotes],[DateRetired],[DateTested],[Elevation],[InspectionFrequency],[MapPage],[ObjectID],[Id],[SketchNumber],[StreetNumber],[StreetId],[Town],[Traffic],[Turns],[ValveLocation],[ValveNumber],[WorkOrderNumber],[DateAdded],[DateInstalled],[LastUpdated],[SAPEquipmentID],[ValveBillingId],[CrossStreetId],[InspectionFrequencyUnitId],[OpensId],[TownSectionId],[MainTypeId],[ValveMakeId],[ValveTypeId],[ValveSizeId],[AssetStatusId],[ValveZoneId],[WaterSystemId],[FunctionalLocationId],[InitiatorId],[CoordinateId],[Route],[Stop],[FacilityId],[GISUID],[SAPErrorCode],[ControlsCrossing]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
");
        }

        public override void Down()
        {
            // no down needed
        }
    }
}

