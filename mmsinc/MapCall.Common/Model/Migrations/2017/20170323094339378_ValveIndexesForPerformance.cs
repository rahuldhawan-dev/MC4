using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170323094339378), Tags("Production")]
    public class ValveIndexesForPerformance : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
CREATE NONCLUSTERED INDEX [_dta_index_Valves_6_1031010754__K41_K57_K56_K61_3_4_5_8_9_10_12_18_22_25_28_29_30_32_33_34_38_40_45_46_49_51_52_53_54_55_58_59_] ON [dbo].[Valves] 
(
	[ValveSuffix] ASC,
	[OperatingCenterId] ASC,
	[NormalPositionId] ASC,
	[ValveControlsId] ASC
)
INCLUDE ( [BPUKPI],
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
[ControlsCrossing]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE STATISTICS [_dta_stat_1031010754_53_57_56] ON [dbo].[Valves]([ValveBillingId], [OperatingCenterId], [NormalPositionId])
go

CREATE STATISTICS [_dta_stat_1031010754_64_57_56] ON [dbo].[Valves]([ValveSizeId], [OperatingCenterId], [NormalPositionId])
go

CREATE STATISTICS [_dta_stat_1031010754_57_56_61_30] ON [dbo].[Valves]([OperatingCenterId], [NormalPositionId], [ValveControlsId], [StreetId])
go

CREATE STATISTICS [_dta_stat_1031010754_57_56_61_25_30] ON [dbo].[Valves]([OperatingCenterId], [NormalPositionId], [ValveControlsId], [Id], [StreetId])
go

CREATE STATISTICS [_dta_stat_1031010754_70_57_56_61_25_30] ON [dbo].[Valves]([CoordinateId], [OperatingCenterId], [NormalPositionId], [ValveControlsId], [Id], [StreetId])
go

CREATE STATISTICS [_dta_stat_1031010754_57_56_61_64_25_30_54] ON [dbo].[Valves]([OperatingCenterId], [NormalPositionId], [ValveControlsId], [ValveSizeId], [Id], [StreetId], [CrossStreetId])
go

CREATE STATISTICS [_dta_stat_1031010754_30_54_57_56_61_25_70] ON [dbo].[Valves]([StreetId], [CrossStreetId], [OperatingCenterId], [NormalPositionId], [ValveControlsId], [Id], [CoordinateId])
go

CREATE STATISTICS [_dta_stat_1031010754_65_57_56_61_25_30_54_70] ON [dbo].[Valves]([ValveStatusId], [OperatingCenterId], [NormalPositionId], [ValveControlsId], [Id], [StreetId], [CrossStreetId], [CoordinateId])
go

CREATE STATISTICS [_dta_stat_1031010754_57_56_61_53_25_30_54_70_64] ON [dbo].[Valves]([OperatingCenterId], [NormalPositionId], [ValveControlsId], [ValveBillingId], [Id], [StreetId], [CrossStreetId], [CoordinateId], [ValveSizeId])
go

CREATE STATISTICS [_dta_stat_1031010754_25_30_54_70_64_61_65_53_57] ON [dbo].[Valves]([Id], [StreetId], [CrossStreetId], [CoordinateId], [ValveSizeId], [ValveControlsId], [ValveStatusId], [ValveBillingId], [OperatingCenterId])
go

CREATE STATISTICS [_dta_stat_1031010754_56_25_57_30_54_70_64_61_65_53_41] ON [dbo].[Valves]([NormalPositionId], [Id], [OperatingCenterId], [StreetId], [CrossStreetId], [CoordinateId], [ValveSizeId], [ValveControlsId], [ValveStatusId], [ValveBillingId], [ValveSuffix])
go

CREATE NONCLUSTERED INDEX [_dta_index_WorkOrders_6_1888777836__K1_23_35_39_91] ON [dbo].[WorkOrders] 
(
	[WorkOrderID] ASC
)
INCLUDE ( [DateCompleted],
[WorkDescriptionID],
[ValveID],
[CancelledAt]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE NONCLUSTERED INDEX [_dta_index_Streets_6_1365579903__K1_3_5_6_7_11] ON [dbo].[Streets] 
(
	[StreetID] ASC
)
INCLUDE ( [FullStName],
[StreetPrefix],
[StreetName],
[StreetSuffix],
[TownID]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE NONCLUSTERED INDEX [_dta_index_BlowOffInspections_6_203967903__K19_K4_15_16_17] ON [dbo].[BlowOffInspections] 
(
	[ValveId] ASC,
	[DateInspected] ASC
)
INCLUDE ( [WorkOrderRequest1],
[WorkOrderRequest2],
[WorkOrderRequest3]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE NONCLUSTERED INDEX [_dta_index_BlowOffInspections_6_203967903__K19] ON [dbo].[BlowOffInspections] 
(
	[ValveId] ASC
)WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE STATISTICS [_dta_stat_203967903_4_19] ON [dbo].[BlowOffInspections]([DateInspected], [ValveId])
go

CREATE STATISTICS [_dta_stat_4911089_9_6] ON [dbo].[ValveInspections]([Operated], [MinimumRequiredTurns])
go

CREATE STATISTICS [_dta_stat_4911089_9_2] ON [dbo].[ValveInspections]([Operated], [DateInspected])
go

CREATE STATISTICS [_dta_stat_4911089_22_2_6] ON [dbo].[ValveInspections]([ValveID], [DateInspected], [MinimumRequiredTurns])
go

CREATE STATISTICS [_dta_stat_4911089_2_6_9] ON [dbo].[ValveInspections]([DateInspected], [MinimumRequiredTurns], [Operated])
go

CREATE STATISTICS [_dta_stat_4911089_22_16_6] ON [dbo].[ValveInspections]([ValveID], [Turns], [MinimumRequiredTurns])
go

CREATE STATISTICS [_dta_stat_4911089_16_2_9] ON [dbo].[ValveInspections]([Turns], [DateInspected], [Operated])
go

CREATE STATISTICS [_dta_stat_4911089_2_22_9] ON [dbo].[ValveInspections]([DateInspected], [ValveID], [Operated])
go

CREATE STATISTICS [_dta_stat_4911089_22_16_9_2] ON [dbo].[ValveInspections]([ValveID], [Turns], [Operated], [DateInspected])
go

CREATE STATISTICS [_dta_stat_4911089_22_6_9_2] ON [dbo].[ValveInspections]([ValveID], [MinimumRequiredTurns], [Operated], [DateInspected])
go

CREATE STATISTICS [_dta_stat_4911089_22_9_6_16_2] ON [dbo].[ValveInspections]([ValveID], [Operated], [MinimumRequiredTurns], [Turns], [DateInspected])
go

");
        }

        public override void Down()
        {
            Execute.Sql(
                @"IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Valves]') AND name = N'_dta_index_Valves_6_1031010754__K41_K57_K56_K61_3_4_5_8_9_10_12_18_22_25_28_29_30_32_33_34_38_40_45_46_49_51_52_53_54_55_58_59_')
DROP INDEX [_dta_index_Valves_6_1031010754__K41_K57_K56_K61_3_4_5_8_9_10_12_18_22_25_28_29_30_32_33_34_38_40_45_46_49_51_52_53_54_55_58_59_] ON [dbo].[Valves] WITH ( ONLINE = OFF )

if  exists (select * from sys.stats where name = N'_dta_stat_1031010754_53_57_56' and object_id = object_id(N'[dbo].[Valves]'))
DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_53_57_56]
GO

if  exists (select * from sys.stats where name = N'_dta_stat_1031010754_64_57_56' and object_id = object_id(N'[dbo].[Valves]'))
DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_64_57_56]
GO

if  exists (select * from sys.stats where name = N'_dta_stat_1031010754_57_56_61_30' and object_id = object_id(N'[dbo].[Valves]'))
DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_57_56_61_30]
GO

if  exists (select * from sys.stats where name = N'_dta_stat_1031010754_57_56_61_25_30' and object_id = object_id(N'[dbo].[Valves]'))
DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_57_56_61_25_30]
GO
if  exists (select * from sys.stats where name = N'_dta_stat_1031010754_70_57_56_61_25_30' and object_id = object_id(N'[dbo].[Valves]'))
DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_70_57_56_61_25_30]

if  exists (select * from sys.stats where name = N'_dta_stat_1031010754_57_56_61_64_25_30_54' and object_id = object_id(N'[dbo].[Valves]'))
DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_57_56_61_64_25_30_54]

if  exists (select * from sys.stats where name = N'_dta_stat_1031010754_30_54_57_56_61_25_70' and object_id = object_id(N'[dbo].[Valves]'))
DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_30_54_57_56_61_25_70]

if  exists (select * from sys.stats where name = N'_dta_stat_1031010754_65_57_56_61_25_30_54_70' and object_id = object_id(N'[dbo].[Valves]'))
DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_65_57_56_61_25_30_54_70]

if  exists (select * from sys.stats where name = N'_dta_stat_1031010754_57_56_61_53_25_30_54_70_64' and object_id = object_id(N'[dbo].[Valves]'))
DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_57_56_61_53_25_30_54_70_64]

if  exists (select * from sys.stats where name = N'_dta_stat_1031010754_25_30_54_70_64_61_65_53_57' and object_id = object_id(N'[dbo].[Valves]'))
DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_25_30_54_70_64_61_65_53_57]

if  exists (select * from sys.stats where name = N'_dta_stat_1031010754_56_25_57_30_54_70_64_61_65_53_41' and object_id = object_id(N'[dbo].[Valves]'))
DROP STATISTICS [dbo].[Valves].[_dta_stat_1031010754_56_25_57_30_54_70_64_61_65_53_41]

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[WorkOrders]') AND name = N'_dta_index_WorkOrders_6_1888777836__K1_23_35_39_91')
DROP INDEX [_dta_index_WorkOrders_6_1888777836__K1_23_35_39_91] ON [dbo].[WorkOrders] WITH ( ONLINE = OFF )

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Streets]') AND name = N'_dta_index_Streets_6_1365579903__K1_3_5_6_7_11')
DROP INDEX [_dta_index_Streets_6_1365579903__K1_3_5_6_7_11] ON [dbo].[Streets] WITH ( ONLINE = OFF )

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BlowOffInspections]') AND name = N'_dta_index_BlowOffInspections_6_203967903__K19_K4_15_16_17')
DROP INDEX [_dta_index_BlowOffInspections_6_203967903__K19_K4_15_16_17] ON [dbo].[BlowOffInspections] WITH ( ONLINE = OFF )

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[BlowOffInspections]') AND name = N'_dta_index_BlowOffInspections_6_203967903__K19')
DROP INDEX [_dta_index_BlowOffInspections_6_203967903__K19] ON [dbo].[BlowOffInspections] WITH ( ONLINE = OFF )

if  exists (select * from sys.stats where name = N'_dta_stat_203967903_4_19' and object_id = object_id(N'[dbo].[BlowOffInspections]'))
DROP STATISTICS [dbo].[BlowOffInspections].[_dta_stat_203967903_4_19]

if  exists (select * from sys.stats where name = N'_dta_stat_4911089_9_6' and object_id = object_id(N'[dbo].[ValveInspections]'))
DROP STATISTICS [dbo].[ValveInspections].[_dta_stat_4911089_9_6]

if  exists (select * from sys.stats where name = N'_dta_stat_4911089_9_2' and object_id = object_id(N'[dbo].[ValveInspections]'))
DROP STATISTICS [dbo].[ValveInspections].[_dta_stat_4911089_9_2]

if  exists (select * from sys.stats where name = N'_dta_stat_4911089_22_2_6' and object_id = object_id(N'[dbo].[ValveInspections]'))
DROP STATISTICS [dbo].[ValveInspections].[_dta_stat_4911089_22_2_6]

if  exists (select * from sys.stats where name = N'_dta_stat_4911089_2_6_9' and object_id = object_id(N'[dbo].[ValveInspections]'))
DROP STATISTICS [dbo].[ValveInspections].[_dta_stat_4911089_2_6_9]

if  exists (select * from sys.stats where name = N'_dta_stat_4911089_22_16_6' and object_id = object_id(N'[dbo].[ValveInspections]'))
DROP STATISTICS [dbo].[ValveInspections].[_dta_stat_4911089_22_16_6]

if  exists (select * from sys.stats where name = N'_dta_stat_4911089_16_2_9' and object_id = object_id(N'[dbo].[ValveInspections]'))
DROP STATISTICS [dbo].[ValveInspections].[_dta_stat_4911089_16_2_9]

if  exists (select * from sys.stats where name = N'_dta_stat_4911089_2_22_9' and object_id = object_id(N'[dbo].[ValveInspections]'))
DROP STATISTICS [dbo].[ValveInspections].[_dta_stat_4911089_2_22_9]

if  exists (select * from sys.stats where name = N'_dta_stat_4911089_22_16_9_2' and object_id = object_id(N'[dbo].[ValveInspections]'))
DROP STATISTICS [dbo].[ValveInspections].[_dta_stat_4911089_22_16_9_2]

if  exists (select * from sys.stats where name = N'_dta_stat_4911089_22_6_9_2' and object_id = object_id(N'[dbo].[ValveInspections]'))
DROP STATISTICS [dbo].[ValveInspections].[_dta_stat_4911089_22_6_9_2]

if  exists (select * from sys.stats where name = N'_dta_stat_4911089_22_9_6_16_2' and object_id = object_id(N'[dbo].[ValveInspections]'))
DROP STATISTICS [dbo].[ValveInspections].[_dta_stat_4911089_22_9_6_16_2]
");
        }
    }
}
