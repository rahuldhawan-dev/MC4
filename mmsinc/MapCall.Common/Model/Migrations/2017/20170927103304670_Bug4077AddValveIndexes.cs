using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170927103304670), Tags("Production")]
    public class Bug4077AddValveIndexes : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
CREATE NONCLUSTERED INDEX [_dta_index_Coordinates_5_816110048__K1_2_3_5] ON [dbo].[Coordinates]
(
	[CoordinateID] ASC
)
INCLUDE ( 	[Latitude],
	[Longitude],
	[IconID]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

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
	[GeoEFunctionalLocation],
	[SAPErrorCode],
	[ControlsCrossing]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE NONCLUSTERED INDEX [_dta_index_ValveInspections_5_4911089__K22_K9_K2_K6_K16_27_28_29_30] ON [dbo].[ValveInspections]
(
	[ValveID] ASC,
	[Operated] ASC,
	[DateInspected] ASC,
	[MinimumRequiredTurns] ASC,
	[Turns] ASC
)
INCLUDE ( 	[PositionLeftId],
	[WorkOrderRequest1Id],
	[WorkOrderRequest2Id],
	[WorkOrderRequest3Id]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE NONCLUSTERED INDEX [_dta_index_WorkOrders_5_1888777836__K39_K23_K91] ON [dbo].[WorkOrders]
(
	[ValveID] ASC,
	[DateCompleted] ASC,
	[CancelledAt] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

CREATE NONCLUSTERED INDEX [_dta_index_ValveImages_5_1653685039__K35] ON [dbo].[ValveImages]
(
	[ValveID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go
");
        }

        public override void Down()
        {
            this.DeleteIndexIfItExists("Coordinates", "_dta_index_Coordinates_5_816110048__K1_2_3_5");
            this.DeleteIndexIfItExists("Valves",
                "_dta_index_Valves_5_1031010754__K57_K56_K61_K25_K64_K54_K30_K70_K65_K53_K41_3_4_5_8_9_10_12_18_22_28_29_32_33_34_38_40_45_46_");
            this.DeleteIndexIfItExists("ValveInspections",
                "_dta_index_ValveInspections_5_4911089__K22_K9_K2_K6_K16_27_28_29_30");
            this.DeleteIndexIfItExists("WorkOrders", "_dta_index_WorkOrders_5_1888777836__K39_K23_K91");
            this.DeleteIndexIfItExists("ValveImages", "_dta_index_ValveImages_5_1653685039__K35");
        }
    }
}
