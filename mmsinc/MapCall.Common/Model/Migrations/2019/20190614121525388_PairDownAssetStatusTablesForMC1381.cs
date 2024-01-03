using FluentMigrator;
using MapCall.Common.Model.Migrations._2018;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190614121525388), Tags("Production")]
    public class PairDownAssetStatusTablesForMC1381 : Migration
    {
        #region Constants

        public const string ORIGINAL_INDEXES_AND_STATISTICS = @"
CREATE NONCLUSTERED INDEX [_dta_index_Valves_5_1031010754__K57_K12_K61_K66_K3_K55_K65_K53_K64_K25] ON [dbo].[Valves]
(
	[OperatingCenterId] ASC,
	[InspectionFrequency] ASC,
	[ValveControlsId] ASC,
	[ValveZoneId] ASC,
	[BPUKPI] ASC,
	[InspectionFrequencyUnitId] ASC,
	[ValveStatusId] ASC,
	[ValveBillingId] ASC,
	[ValveSizeId] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

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
	[GISUID],
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
	[GISUID],
	[SAPErrorCode],
	[ControlsCrossing]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO

CREATE STATISTICS [_dta_stat_1031010754_12_55_61_3_66_65_57] ON [dbo].[Valves]([InspectionFrequency], [InspectionFrequencyUnitId], [ValveControlsId], [BPUKPI], [ValveZoneId], [ValveStatusId], [OperatingCenterId])
GO

CREATE STATISTICS [_dta_stat_1031010754_12_61_66_3_55_25_65_57_53] ON [dbo].[Valves]([InspectionFrequency], [ValveControlsId], [ValveZoneId], [BPUKPI], [InspectionFrequencyUnitId], [Id], [ValveStatusId], [OperatingCenterId], [ValveBillingId])
GO

CREATE STATISTICS [_dta_stat_1031010754_25_30_54_70_64_61_65_53_57] ON [dbo].[Valves]([Id], [StreetId], [CrossStreetId], [CoordinateId], [ValveSizeId], [ValveControlsId], [ValveStatusId], [ValveBillingId], [OperatingCenterId])
GO

CREATE STATISTICS [_dta_stat_1031010754_3_64_65_25_57_53] ON [dbo].[Valves]([BPUKPI], [ValveSizeId], [ValveStatusId], [Id], [OperatingCenterId], [ValveBillingId])
GO

CREATE STATISTICS [_dta_stat_1031010754_3_65_57_53_64_12_55] ON [dbo].[Valves]([BPUKPI], [ValveStatusId], [OperatingCenterId], [ValveBillingId], [ValveSizeId], [InspectionFrequency], [InspectionFrequencyUnitId])
GO

CREATE STATISTICS [_dta_stat_1031010754_53_12_61_66_3_55_65_57_64_25] ON [dbo].[Valves]([ValveBillingId], [InspectionFrequency], [ValveControlsId], [ValveZoneId], [BPUKPI], [InspectionFrequencyUnitId], [ValveStatusId], [OperatingCenterId], [ValveSizeId], [Id])
GO

CREATE STATISTICS [_dta_stat_1031010754_55_65_57_53_64_12] ON [dbo].[Valves]([InspectionFrequencyUnitId], [ValveStatusId], [OperatingCenterId], [ValveBillingId], [ValveSizeId], [InspectionFrequency])
GO

CREATE STATISTICS [_dta_stat_1031010754_56_25_57_30_54_70_64_61_65_53_41] ON [dbo].[Valves]([NormalPositionId], [Id], [OperatingCenterId], [StreetId], [CrossStreetId], [CoordinateId], [ValveSizeId], [ValveControlsId], [ValveStatusId], [ValveBillingId], [ValveSuffix])
GO

CREATE STATISTICS [_dta_stat_1031010754_57_61_66_3_53_64_65] ON [dbo].[Valves]([OperatingCenterId], [ValveControlsId], [ValveZoneId], [BPUKPI], [ValveBillingId], [ValveSizeId], [ValveStatusId])
GO

CREATE STATISTICS [_dta_stat_1031010754_61_64_65_25_57_53_3] ON [dbo].[Valves]([ValveControlsId], [ValveSizeId], [ValveStatusId], [Id], [OperatingCenterId], [ValveBillingId], [BPUKPI])
GO

CREATE STATISTICS [_dta_stat_1031010754_61_65_57_53_64_12_55_3] ON [dbo].[Valves]([ValveControlsId], [ValveStatusId], [OperatingCenterId], [ValveBillingId], [ValveSizeId], [InspectionFrequency], [InspectionFrequencyUnitId], [BPUKPI])
GO

CREATE STATISTICS [_dta_stat_1031010754_64_12_61_66_3_55_65_57] ON [dbo].[Valves]([ValveSizeId], [InspectionFrequency], [ValveControlsId], [ValveZoneId], [BPUKPI], [InspectionFrequencyUnitId], [ValveStatusId], [OperatingCenterId])
GO

CREATE STATISTICS [_dta_stat_1031010754_64_53_12_61_66_3_55_65] ON [dbo].[Valves]([ValveSizeId], [ValveBillingId], [InspectionFrequency], [ValveControlsId], [ValveZoneId], [BPUKPI], [InspectionFrequencyUnitId], [ValveStatusId])
GO

CREATE STATISTICS [_dta_stat_1031010754_64_57_61_66_3_65_25_53] ON [dbo].[Valves]([ValveSizeId], [OperatingCenterId], [ValveControlsId], [ValveZoneId], [BPUKPI], [ValveStatusId], [Id], [ValveBillingId])
GO

CREATE STATISTICS [_dta_stat_1031010754_64_65_25_53_57] ON [dbo].[Valves]([ValveSizeId], [ValveStatusId], [Id], [ValveBillingId], [OperatingCenterId])
GO

CREATE STATISTICS [_dta_stat_1031010754_65_12_61_66_3] ON [dbo].[Valves]([ValveStatusId], [InspectionFrequency], [ValveControlsId], [ValveZoneId], [BPUKPI])
GO

CREATE STATISTICS [_dta_stat_1031010754_65_57_53_64_25_12_55_61_3] ON [dbo].[Valves]([ValveStatusId], [OperatingCenterId], [ValveBillingId], [ValveSizeId], [Id], [InspectionFrequency], [InspectionFrequencyUnitId], [ValveControlsId], [BPUKPI])
GO

CREATE STATISTICS [_dta_stat_1031010754_65_57_56_61_25_30_54_70] ON [dbo].[Valves]([ValveStatusId], [OperatingCenterId], [NormalPositionId], [ValveControlsId], [Id], [StreetId], [CrossStreetId], [CoordinateId])
GO

CREATE STATISTICS [_dta_stat_1031010754_65_57_61_66_3] ON [dbo].[Valves]([ValveStatusId], [OperatingCenterId], [ValveControlsId], [ValveZoneId], [BPUKPI])
GO

CREATE STATISTICS [_dta_stat_1031010754_66_64_65_25_57_53_61] ON [dbo].[Valves]([ValveZoneId], [ValveSizeId], [ValveStatusId], [Id], [OperatingCenterId], [ValveBillingId], [ValveControlsId])
GO

CREATE STATISTICS [_dta_stat_1031010754_66_65_57_53_64_12_55_61] ON [dbo].[Valves]([ValveZoneId], [ValveStatusId], [OperatingCenterId], [ValveBillingId], [ValveSizeId], [InspectionFrequency], [InspectionFrequencyUnitId], [ValveControlsId])
GO";

        public const string NEW_INDEXES_AND_STATISTICS = @"
CREATE NONCLUSTERED INDEX [_dta_index_Valves_5_1031010754__K57_K12_K61_K66_K3_K55_K65_K53_K64_K25] ON [dbo].[Valves]
(
	[OperatingCenterId] ASC,
	[InspectionFrequency] ASC,
	[ValveControlsId] ASC,
	[ValveZoneId] ASC,
	[BPUKPI] ASC,
	[InspectionFrequencyUnitId] ASC,
	[AssetStatusId] ASC,
	[ValveBillingId] ASC,
	[ValveSizeId] ASC,
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

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
	[GISUID],
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
	[AssetStatusId],
	[ValveZoneId],
	[WaterSystemId],
	[FunctionalLocationId],
	[InitiatorId],
	[CoordinateId],
	[Route],
	[Stop],
	[FacilityId],
	[GISUID],
	[SAPErrorCode],
	[ControlsCrossing]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO

CREATE STATISTICS [_dta_stat_1031010754_12_55_61_3_66_65_57] ON [dbo].[Valves]([InspectionFrequency], [InspectionFrequencyUnitId], [ValveControlsId], [BPUKPI], [ValveZoneId], [AssetStatusId], [OperatingCenterId])
GO

CREATE STATISTICS [_dta_stat_1031010754_12_61_66_3_55_25_65_57_53] ON [dbo].[Valves]([InspectionFrequency], [ValveControlsId], [ValveZoneId], [BPUKPI], [InspectionFrequencyUnitId], [Id], [AssetStatusId], [OperatingCenterId], [ValveBillingId])
GO

CREATE STATISTICS [_dta_stat_1031010754_25_30_54_70_64_61_65_53_57] ON [dbo].[Valves]([Id], [StreetId], [CrossStreetId], [CoordinateId], [ValveSizeId], [ValveControlsId], [AssetStatusId], [ValveBillingId], [OperatingCenterId])
GO

CREATE STATISTICS [_dta_stat_1031010754_3_64_65_25_57_53] ON [dbo].[Valves]([BPUKPI], [ValveSizeId], [AssetStatusId], [Id], [OperatingCenterId], [ValveBillingId])
GO

CREATE STATISTICS [_dta_stat_1031010754_3_65_57_53_64_12_55] ON [dbo].[Valves]([BPUKPI], [AssetStatusId], [OperatingCenterId], [ValveBillingId], [ValveSizeId], [InspectionFrequency], [InspectionFrequencyUnitId])
GO

CREATE STATISTICS [_dta_stat_1031010754_53_12_61_66_3_55_65_57_64_25] ON [dbo].[Valves]([ValveBillingId], [InspectionFrequency], [ValveControlsId], [ValveZoneId], [BPUKPI], [InspectionFrequencyUnitId], [AssetStatusId], [OperatingCenterId], [ValveSizeId], [Id])
GO

CREATE STATISTICS [_dta_stat_1031010754_55_65_57_53_64_12] ON [dbo].[Valves]([InspectionFrequencyUnitId], [AssetStatusId], [OperatingCenterId], [ValveBillingId], [ValveSizeId], [InspectionFrequency])
GO

CREATE STATISTICS [_dta_stat_1031010754_56_25_57_30_54_70_64_61_65_53_41] ON [dbo].[Valves]([NormalPositionId], [Id], [OperatingCenterId], [StreetId], [CrossStreetId], [CoordinateId], [ValveSizeId], [ValveControlsId], [AssetStatusId], [ValveBillingId], [ValveSuffix])
GO

CREATE STATISTICS [_dta_stat_1031010754_57_61_66_3_53_64_65] ON [dbo].[Valves]([OperatingCenterId], [ValveControlsId], [ValveZoneId], [BPUKPI], [ValveBillingId], [ValveSizeId], [AssetStatusId])
GO

CREATE STATISTICS [_dta_stat_1031010754_61_64_65_25_57_53_3] ON [dbo].[Valves]([ValveControlsId], [ValveSizeId], [AssetStatusId], [Id], [OperatingCenterId], [ValveBillingId], [BPUKPI])
GO

CREATE STATISTICS [_dta_stat_1031010754_61_65_57_53_64_12_55_3] ON [dbo].[Valves]([ValveControlsId], [AssetStatusId], [OperatingCenterId], [ValveBillingId], [ValveSizeId], [InspectionFrequency], [InspectionFrequencyUnitId], [BPUKPI])
GO

CREATE STATISTICS [_dta_stat_1031010754_64_12_61_66_3_55_65_57] ON [dbo].[Valves]([ValveSizeId], [InspectionFrequency], [ValveControlsId], [ValveZoneId], [BPUKPI], [InspectionFrequencyUnitId], [AssetStatusId], [OperatingCenterId])
GO

CREATE STATISTICS [_dta_stat_1031010754_64_53_12_61_66_3_55_65] ON [dbo].[Valves]([ValveSizeId], [ValveBillingId], [InspectionFrequency], [ValveControlsId], [ValveZoneId], [BPUKPI], [InspectionFrequencyUnitId], [AssetStatusId])
GO

CREATE STATISTICS [_dta_stat_1031010754_64_57_61_66_3_65_25_53] ON [dbo].[Valves]([ValveSizeId], [OperatingCenterId], [ValveControlsId], [ValveZoneId], [BPUKPI], [AssetStatusId], [Id], [ValveBillingId])
GO

CREATE STATISTICS [_dta_stat_1031010754_64_65_25_53_57] ON [dbo].[Valves]([ValveSizeId], [AssetStatusId], [Id], [ValveBillingId], [OperatingCenterId])
GO

CREATE STATISTICS [_dta_stat_1031010754_65_12_61_66_3] ON [dbo].[Valves]([AssetStatusId], [InspectionFrequency], [ValveControlsId], [ValveZoneId], [BPUKPI])
GO

CREATE STATISTICS [_dta_stat_1031010754_65_57_53_64_25_12_55_61_3] ON [dbo].[Valves]([AssetStatusId], [OperatingCenterId], [ValveBillingId], [ValveSizeId], [Id], [InspectionFrequency], [InspectionFrequencyUnitId], [ValveControlsId], [BPUKPI])
GO

CREATE STATISTICS [_dta_stat_1031010754_65_57_56_61_25_30_54_70] ON [dbo].[Valves]([AssetStatusId], [OperatingCenterId], [NormalPositionId], [ValveControlsId], [Id], [StreetId], [CrossStreetId], [CoordinateId])
GO

CREATE STATISTICS [_dta_stat_1031010754_65_57_61_66_3] ON [dbo].[Valves]([AssetStatusId], [OperatingCenterId], [ValveControlsId], [ValveZoneId], [BPUKPI])
GO

CREATE STATISTICS [_dta_stat_1031010754_66_64_65_25_57_53_61] ON [dbo].[Valves]([ValveZoneId], [ValveSizeId], [AssetStatusId], [Id], [OperatingCenterId], [ValveBillingId], [ValveControlsId])
GO

CREATE STATISTICS [_dta_stat_1031010754_66_65_57_53_64_12_55_61] ON [dbo].[Valves]([ValveZoneId], [AssetStatusId], [OperatingCenterId], [ValveBillingId], [ValveSizeId], [InspectionFrequency], [InspectionFrequencyUnitId], [ValveControlsId])
GO";

        public const string DROP_INDEXES_AND_STATISTICS = @"
DROP INDEX [_dta_index_Valves_5_1031010754__K57_K12_K61_K66_K3_K55_K65_K53_K64_K25] ON [dbo].[Valves]
GO

DROP INDEX [_dta_index_Valves_5_1031010754__K57_K56_K61_K25_K64_K54_K30_K70_K65_K53_K41_3_4_5_8_9_10_12_18_22_28_29_32_33_34_38_40_45_46_] ON [dbo].[Valves]
GO

DROP INDEX [_dta_index_Valves_6_1031010754__K41_K57_K56_K61_3_4_5_8_9_10_12_18_22_25_28_29_30_32_33_34_38_40_45_46_49_51_52_53_54_55_58_59_] ON [dbo].[Valves]
GO

DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_12_55_61_3_66_65_57];
GO

DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_12_61_66_3_55_25_65_57_53];
GO

DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_25_30_54_70_64_61_65_53_57];
GO

DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_3_64_65_25_57_53];
GO

DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_3_65_57_53_64_12_55];
GO

DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_53_12_61_66_3_55_65_57_64_25];
GO

DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_55_65_57_53_64_12];
GO

DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_56_25_57_30_54_70_64_61_65_53_41];
GO

DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_57_61_66_3_53_64_65];
GO

DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_61_64_65_25_57_53_3];
GO

DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_61_65_57_53_64_12_55_3];
GO

DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_64_12_61_66_3_55_65_57];
GO

DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_64_53_12_61_66_3_55_65];
GO

DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_64_57_61_66_3_65_25_53];
GO

DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_64_65_25_53_57];
GO

DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_65_12_61_66_3];
GO

DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_65_57_53_64_25_12_55_61_3];
GO

DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_65_57_56_61_25_30_54_70];
GO

DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_65_57_61_66_3];
GO

DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_66_64_65_25_57_53_61];
GO

DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_66_65_57_53_64_12_55_61];
GO";

        public struct Sql
        {
            public struct Valves
            {
                public const string REQUIRES_INSPECTION_SQL_FORMAT = @"
    CASE
	/*not active*/
	WHEN (val.AssetStatusId <> 1) THEN 0 
	WHEN ((val.ValveBillingId <> 3 and val.ValveBillingId is not null) AND (val.InspectionFrequency is null or val.InspectionFrequencyUnitId is null)) THEN 0 
	WHEN (val.ValveControlsId = 3) THEN 0
	WHEN val.BPUKPI = 1 THEN 0
	/*val.VALVE IS LESS THAN 2 val.inches AND IS val.A val.CONTROLS val.A val.BLOW OFF*/
    WHEN (vs.Size < 2.0 AND val.ValveControlsId = 2) THEN 0
    /*ZONE IS NOT val.REQUIRED FOR val.INSPECTION AND NO val.INSPECTION val.FREQUENCY IS SET*/
    WHEN (oc.UsesValveInspectionFrequency = 0 AND val.ValveZoneId in (1,2,3,4) AND val.ValveZoneId <> ((ABS(2011-YEAR({0}))%4)+1))
    THEN 0
    /* ZONE IS NOT val.REQUIRED FOR val.INSPECTION AND NO val.INSPECTION val.FREQUENCY IS SET*/
    WHEN (oc.UsesValveInspectionFrequency = 0 AND val.ValveZoneId in (5,6) AND val.ValveZoneId <> ((ABS(2011-YEAR({0}))%2)+5))
	THEN 0
    /*IF THE INSPECTION FREQUENCY IS SET AND ANNUAL, WE GO DEEPER DOWN THE RABBIT HOLE - SOUTH ORANGE VILLAGE INTRODUCED THIS */
    WHEN (oc.UsesValveInspectionFrequency = 1 AND val.InspectionFrequency is not null AND NullIf(val.InspectionFrequency,0) is not null AND val.InspectionFrequencyUnitId = 4 AND (val.ValveZoneId % nullif(val.InspectionFrequency, 0)) <> ((ABS(2011-YEAR({0}))%nullif(val.InspectionFrequency, 0))))
    THEN 0
    /*val.ALREADY val.INSPECTED FOR val.THE YEAR*/
    WHEN vi.Id IS NULL
        THEN 1
    ELSE 0
	END";

                public const string CREATE_VALVE_VIEW_SQL_FORMAT = @"
CREATE VIEW ValvesDueInspection AS
SELECT val.Id," + REQUIRES_INSPECTION_SQL_FORMAT +
                                                                   @" as RequiresInspection
FROM Valves val
INNER JOIN OperatingCenters oc
ON val.OperatingCenterId = oc.OperatingCenterId
LEFT OUTER JOIN ValveSizes vs
ON val.ValveSizeId = vs.Id
LEFT OUTER JOIN ValveInspections vi
ON vi.ValveID = val.Id AND YEAR(vi.DateInspected) = YEAR({0}) AND vi.Operated = 1
LEFT OUTER JOIN ValveInspections vi_newer
ON vi_newer.ValveID = val.Id AND YEAR(vi_newer.DateInspected) = YEAR({0}) AND vi_newer.Operated = 1 AND vi_newer.DateInspected > vi.DateInspected
WHERE vi_newer.ValveID IS NULL";

                public const string DROP_VALVE_VIEW = "DROP VIEW ValvesDueInspection";
            }

            public struct Hydrants
            {
                public const string REQUIRES_INSPECTION_SQL_FORMAT = @"
CASE
-- NOT this_.ACTIVE
WHEN (hyd.AssetStatusId <> 1)
THEN 0
WHEN hyd.IsNonBPUKPI = 1 
THEN 0

WHEN (hyd.InspectionFrequency IS NOT NULL AND hyd.InspectionFrequencyUnitId IS NOT NULL)
THEN CASE
    WHEN (SELECT COUNT(1) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id) < 1
    THEN 1
    ELSE CASE hyd.InspectionFrequencyUnitId
        -- Year
        WHEN 4
        THEN CASE
            WHEN (YEAR({0}) - (SELECT MAX(YEAR(hi.DateInspected)) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < hyd.InspectionFrequency
            THEN 0
            ELSE 1
        END
        -- Month
        WHEN 3
        THEN CASE
            WHEN DATEDIFF(mm, {0}, (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < hyd.InspectionFrequency
            THEN 0
            ELSE 1
        END
        -- Week
        WHEN 2
        THEN CASE
            WHEN DATEDIFF(WW, {0}, (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < hyd.InspectionFrequency
            THEN 0
            ELSE 1
        END
        -- Day
        WHEN 1
        THEN CASE
            WHEN DATEDIFF(D, {0}, (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < hyd.InspectionFrequency
            THEN 0
            ELSE 1
        END
    END
END

-- NOT PUBLIC OR PRIVATE
WHEN hyd.HydrantBillingId <> 2 AND hyd.HydrantBillingId <> 4 AND hyd.HydrantBillingId is not null
THEN 0

-- IN THIS YEAR'S ZONE
WHEN oc.ZoneStartYear IS NOT NULL
AND hyd.Zone IS NOT NULL
THEN CASE
    WHEN hyd.Zone = ((ABS(oc.ZoneStartYear - YEAR({0})) % oc.HydrantInspectionFrequency) + 1)
    AND ((SELECT COUNT(1) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id) = 0
        OR (SELECT MAX(YEAR(hi.DateInspected)) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id) < YEAR({0}))
    THEN 1
    ELSE 0
END

-- HAS NO INSPECTIONS
WHEN (SELECT COUNT(1) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id) < 1
THEN 1

ELSE CASE oc.HydrantInspectionFrequencyUnitId
    -- Year
    WHEN 4
    THEN CASE
        WHEN (YEAR({0}) - (SELECT MAX(YEAR(hi.DateInspected)) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < oc.HydrantInspectionFrequency
        THEN 0
        ELSE 1
    END
    -- Month
    WHEN 3
    THEN CASE
        WHEN DATEDIFF(mm, {0}, (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < oc.HydrantInspectionFrequency
        THEN 0
        ELSE 1
    END
    -- Week
    WHEN 2
    THEN CASE
        WHEN DATEDIFF(WW, {0}, (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < oc.HydrantInspectionFrequency
        THEN 0
        ELSE 1
    END
    -- Day
    WHEN 1
    THEN CASE
        WHEN DATEDIFF(D, {0}, (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < oc.HydrantInspectionFrequency
        THEN 0
        ELSE 1
    END
END

END";

                public const string CREATE_HYDRANT_VIEW_SQL_FORMAT = @"
CREATE VIEW HydrantsDueInspection AS
SELECT hyd.Id," + REQUIRES_INSPECTION_SQL_FORMAT +
                                                                     @" as RequiresInspection
FROM Hydrants hyd
INNER JOIN OperatingCenters oc
ON hyd.OperatingCenterId = oc.OperatingCenterID";

                public const string DROP_HYDRANT_VIEW = "DROP VIEW HydrantsDueInspection";
            }
        }

        #endregion

        public override void Up()
        {
            Execute.Sql(DROP_INDEXES_AND_STATISTICS);

            Alter.Table("AssetStatuses").AddColumn("IsUserAdminOnly").AsBoolean().NotNullable().WithDefaultValue(true);

            Execute.Sql(
                "UPDATE AssetStatuses SET IsUserAdminOnly = 0 WHERE Description IN ('INSTALLED', 'REQUEST RETIREMENT', 'REQUEST CANCELLATION')");

            Alter.Table("Hydrants")
                 .AddForeignKeyColumn("AssetStatusId", "AssetStatuses", "AssetStatusId");

            Execute.Sql(
                "UPDATE Hydrants SET AssetStatusId = s.AssetStatusId FROM AssetStatuses s INNER JOIN HydrantStatuses hs ON s.Description = hs.Description WHERE hs.Id = HydrantStatusId ");

            Delete.ForeignKeyColumn("Hydrants", "HydrantStatusId", "HydrantStatuses");

            Delete.Table("HydrantStatuses");

            Alter.Table("Valves")
                 .AddForeignKeyColumn("AssetStatusId", "AssetStatuses", "AssetStatusId");

            Execute.Sql(
                "UPDATE Valves SET AssetStatusId = s.AssetStatusId FROM AssetStatuses s INNER JOIN ValveStatuses vs ON s.Description = vs.Description WHERE vs.Id = ValveStatusId ");

            Delete.ForeignKeyColumn("Valves", "ValveStatusId", "ValveStatuses");

            Delete.Table("ValveStatuses");

            Execute.Sql(NEW_INDEXES_AND_STATISTICS);

            Execute.Sql(Sql.Valves.DROP_VALVE_VIEW);
            Execute.Sql(Sql.Valves.CREATE_VALVE_VIEW_SQL_FORMAT, "GETDATE()");
            Execute.Sql(Sql.Hydrants.DROP_HYDRANT_VIEW);
            Execute.Sql(Sql.Hydrants.CREATE_HYDRANT_VIEW_SQL_FORMAT, "GETDATE()");
        }

        public override void Down()
        {
            Execute.Sql(DROP_INDEXES_AND_STATISTICS);

            Create.Table("ValveStatuses")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(20).NotNullable()
                  .WithColumn("IsUserAdminOnly").AsBoolean().NotNullable().WithDefaultValue(true);

            Execute.Sql(@"
SET IDENTITY_INSERT ValveStatuses ON;
INSERT INTO ValveStatuses (Id, Description, IsUserAdminOnly) VALUES (1, 'ACTIVE', 1);
INSERT INTO ValveStatuses (Id, Description, IsUserAdminOnly) VALUES (2, 'CANCELLED', 1);
INSERT INTO ValveStatuses (Id, Description, IsUserAdminOnly) VALUES (3, 'PENDING', 1);
INSERT INTO ValveStatuses (Id, Description, IsUserAdminOnly) VALUES (4, 'RETIRED', 1);
INSERT INTO ValveStatuses (Id, Description, IsUserAdminOnly) VALUES (5, 'REMOVED', 1);
INSERT INTO ValveStatuses (Id, Description, IsUserAdminOnly) VALUES (6, 'INACTIVE', 1);
INSERT INTO ValveStatuses (Id, Description, IsUserAdminOnly) VALUES (8, 'INSTALLED', 0);
INSERT INTO ValveStatuses (Id, Description, IsUserAdminOnly) VALUES (9, 'REQUEST RETIREMENT', 0);
INSERT INTO ValveStatuses (Id, Description, IsUserAdminOnly) VALUES (10, 'REQUEST CANCELLATION', 0);
SET IDENTITY_INSERT ValveStatuses OFF;
");

            Alter.Table("Valves")
                 .AddForeignKeyColumn("ValveStatusId", "ValveStatuses");

            Execute.Sql(
                "UPDATE Valves SET ValveStatusId = vs.Id FROM ValveStatuses vs INNER JOIN AssetStatuses s ON s.Description = vs.Description WHERE s.AssetStatusId = Valves.AssetStatusId");

            Delete.ForeignKeyColumn("Valves", "AssetStatusId", "AssetStatuses", "AssetStatusId");

            Create.Table("HydrantStatuses")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(20).NotNullable()
                  .WithColumn("IsUserAdminOnly").AsBoolean().NotNullable().WithDefaultValue(true);

            Execute.Sql(@"
SET IDENTITY_INSERT HydrantStatuses ON;
INSERT INTO HydrantStatuses (Id, Description, IsUserAdminOnly) VALUES (1, 'ACTIVE', 1);
INSERT INTO HydrantStatuses (Id, Description, IsUserAdminOnly) VALUES (2, 'CANCELLED', 1);
INSERT INTO HydrantStatuses (Id, Description, IsUserAdminOnly) VALUES (3, 'PENDING', 1);
INSERT INTO HydrantStatuses (Id, Description, IsUserAdminOnly) VALUES (4, 'RETIRED', 1);
INSERT INTO HydrantStatuses (Id, Description, IsUserAdminOnly) VALUES (5, 'INSTALLED', 0);
INSERT INTO HydrantStatuses (Id, Description, IsUserAdminOnly) VALUES (6, 'REQUEST RETIREMENT', 0);
INSERT INTO HydrantStatuses (Id, Description, IsUserAdminOnly) VALUES (7, 'REQUEST CANCELLATION', 0);
INSERT INTO HydrantStatuses (Id, Description, IsUserAdminOnly) VALUES (10, 'INACTIVE', 1);
INSERT INTO HydrantStatuses (Id, Description, IsUserAdminOnly) VALUES (11, 'REMOVED', 1);
SET IDENTITY_INSERT HydrantStatuses OFF;");

            Alter.Table("Hydrants")
                 .AddForeignKeyColumn("HydrantStatusId", "HydrantStatuses");

            Execute.Sql(
                "UPDATE Hydrants SET HydrantStatusId = hs.Id FROM HydrantStatuses hs INNER JOIN AssetStatuses s ON s.Description = hs.Description WHERE s.AssetStatusId = Hydrants.AssetStatusId");

            Delete.ForeignKeyColumn("Hydrants", "AssetStatusId", "AssetStatuses", "AssetStatusId");

            Delete.Column("IsUserAdminOnly").FromTable("AssetStatuses");

            Execute.Sql(ORIGINAL_INDEXES_AND_STATISTICS);

            Execute.Sql(Sql.Valves.DROP_VALVE_VIEW);
            Execute.Sql(MC1218CreateValvesDueInspectionView.Sql.CREATE_VALVE_VIEW_SQL_FORMAT, "GETDATE()");
            Execute.Sql(Sql.Hydrants.DROP_HYDRANT_VIEW);
            Execute.Sql(
                AlterHydrantsDueInspectionViewToExcludeHydrantsNeverPreviouslyInspectedForMC78
                   .Sql.CREATE_HYDRANT_VIEW_SQL_FORMAT, "GETDATE()");
        }
    }
}
