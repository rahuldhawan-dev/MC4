using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20171128145409814), Tags("Production")]
    public class ExtendSomeAssetStringFieldLengthsForWO208357 : Migration
    {
        public const int WORK_ORDER_NUMBER_LENGTH = 25;

        public const string DROP_VALVES_INDEXES =
                                @"IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Valves]') AND name = N'_dta_index_Valves_5_1031010754__K57_K56_K61_K25_K64_K54_K30_K70_K65_K53_K41_3_4_5_8_9_10_12_18_22_28_29_32_33_34_38_40_45_46_')
DROP INDEX [_dta_index_Valves_5_1031010754__K57_K56_K61_K25_K64_K54_K30_K70_K65_K53_K41_3_4_5_8_9_10_12_18_22_28_29_32_33_34_38_40_45_46_] ON [dbo].[Valves] WITH ( ONLINE = OFF )
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Valves]') AND name = N'_dta_index_Valves_6_1031010754__K41_K57_K56_K61_3_4_5_8_9_10_12_18_22_25_28_29_30_32_33_34_38_40_45_46_49_51_52_53_54_55_58_59_')
DROP INDEX [_dta_index_Valves_6_1031010754__K41_K57_K56_K61_3_4_5_8_9_10_12_18_22_25_28_29_30_32_33_34_38_40_45_46_49_51_52_53_54_55_58_59_] ON [dbo].[Valves]
",
                            CREATE_VALVES_INDEXES =
                                @"CREATE NONCLUSTERED INDEX [_dta_index_Valves_5_1031010754__K57_K56_K61_K25_K64_K54_K30_K70_K65_K53_K41_3_4_5_8_9_10_12_18_22_28_29_32_33_34_38_40_45_46_] ON [dbo].[Valves]
(
	[OperatingCenterId] ASC,
	[NormalPositionId] ASC,
	[ValveControlsId] ASC,
	[Id] ASC,
	[ValveSizeId] ASC,
	[CrossStreetId] ASC,
	[StreetId] ASC,
	[CoordinateId] ASC,
	[ValveStatusId] ASC,
	[ValveBillingId] ASC,
	[ValveSuffix] ASC
)
INCLUDE ( 	[BPUKPI],
	[Critical],
	[CriticalNotes],
	[DateRetired],
	[DateTested],
	[Elevation],
	[InspectionFrequency],
	[MapPage],
	[ObjectID],
	[SketchNumber],
	[StreetNumber],
	[Town],
	[Traffic],
	[Turns],
	[ValveLocation],
	[ValveNumber],
	[WorkOrderNumber],
	[DateAdded],
	[DateInstalled],
	[LastUpdated],
	[SAPEquipmentID],
	[InspectionFrequencyUnitId],
	[OpensId],
	[TownSectionId],
	[MainTypeId],
	[ValveMakeId],
	[ValveTypeId],
	[ValveZoneId],
	[WaterSystemId],
	[FunctionalLocationId],
	[InitiatorId],
	[Route],
	[Stop],
	[FacilityId],
	[GeoEFunctionalLocation],
	[SAPErrorCode],
	[ControlsCrossing]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [_dta_index_Valves_6_1031010754__K41_K57_K56_K61_3_4_5_8_9_10_12_18_22_25_28_29_30_32_33_34_38_40_45_46_49_51_52_53_54_55_58_59_] ON [dbo].[Valves]
(
	[ValveSuffix] ASC,
	[OperatingCenterId] ASC,
	[NormalPositionId] ASC,
	[ValveControlsId] ASC
)
INCLUDE ( 	[BPUKPI],
	[Critical],
	[CriticalNotes],
	[DateRetired],
	[DateTested],
	[Elevation],
	[InspectionFrequency],
	[MapPage],
	[ObjectID],
	[Id],
	[SketchNumber],
	[StreetNumber],
	[StreetId],
	[Town],
	[Traffic],
	[Turns],
	[ValveLocation],
	[ValveNumber],
	[WorkOrderNumber],
	[DateAdded],
	[DateInstalled],
	[LastUpdated],
	[SAPEquipmentID],
	[ValveBillingId],
	[CrossStreetId],
	[InspectionFrequencyUnitId],
	[OpensId],
	[TownSectionId],
	[MainTypeId],
	[ValveMakeId],
	[ValveTypeId],
	[ValveSizeId],
	[ValveStatusId],
	[ValveZoneId],
	[WaterSystemId],
	[FunctionalLocationId],
	[InitiatorId],
	[CoordinateId],
	[Route],
	[Stop],
	[FacilityId],
	[GeoEFunctionalLocation],
	[SAPErrorCode],
	[ControlsCrossing]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO";

        public override void Up()
        {
            Execute.Sql(DROP_VALVES_INDEXES);

            Alter.Column("ValveLocation").OnTable("Valves").AsString(200).Nullable();
            Alter.Column("WorkOrderNumber").OnTable("Valves").AsString(WORK_ORDER_NUMBER_LENGTH).Nullable();
            Alter.Column("WorkOrderNumber").OnTable("Hydrants").AsString(WORK_ORDER_NUMBER_LENGTH).Nullable();

            Execute.Sql(CREATE_VALVES_INDEXES);
        }

        public override void Down() { }
    }
}
