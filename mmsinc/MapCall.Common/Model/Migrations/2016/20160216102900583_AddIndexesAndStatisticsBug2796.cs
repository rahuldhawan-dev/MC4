using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160216102900583), Tags("Production")]
    public class AddIndexesAndStatisticsBug2796 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
CREATE NONCLUSTERED INDEX [_dta_index_ValveInspections_5_4911089__K9_K22_K13_2] ON [dbo].[ValveInspections]
(
	[Operated] ASC,
	[ValveID] ASC,
	[Id] ASC
)
INCLUDE ( 	[DateInspected]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
go

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
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]

CREATE STATISTICS [_dta_stat_1031010754_12_55_61_3_66_65_57] ON [dbo].[Valves]([InspectionFrequency], [InspectionFrequencyUnitId], [ValveControlsId], [BPUKPI], [ValveZoneId], [ValveStatusId], [OperatingCenterId])

CREATE STATISTICS [_dta_stat_1031010754_12_61_66_3_55_25_65_57_53] ON [dbo].[Valves]([InspectionFrequency], [ValveControlsId], [ValveZoneId], [BPUKPI], [InspectionFrequencyUnitId], [Id], [ValveStatusId], [OperatingCenterId], [ValveBillingId])

CREATE STATISTICS [_dta_stat_1031010754_25_12_61_66_3] ON [dbo].[Valves]([Id], [InspectionFrequency], [ValveControlsId], [ValveZoneId], [BPUKPI])

CREATE STATISTICS [_dta_stat_1031010754_3_65_57_53_64_12_55] ON [dbo].[Valves]([BPUKPI], [ValveStatusId], [OperatingCenterId], [ValveBillingId], [ValveSizeId], [InspectionFrequency], [InspectionFrequencyUnitId])

CREATE STATISTICS [_dta_stat_1031010754_53_12_61_66_3_55_65_57_64_25] ON [dbo].[Valves]([ValveBillingId], [InspectionFrequency], [ValveControlsId], [ValveZoneId], [BPUKPI], [InspectionFrequencyUnitId], [ValveStatusId], [OperatingCenterId], [ValveSizeId], [Id])

CREATE STATISTICS [_dta_stat_1031010754_55_65_57_53_64_12] ON [dbo].[Valves]([InspectionFrequencyUnitId], [ValveStatusId], [OperatingCenterId], [ValveBillingId], [ValveSizeId], [InspectionFrequency])

CREATE STATISTICS [_dta_stat_1031010754_61_65_57_53_64_12_55_3] ON [dbo].[Valves]([ValveControlsId], [ValveStatusId], [OperatingCenterId], [ValveBillingId], [ValveSizeId], [InspectionFrequency], [InspectionFrequencyUnitId], [BPUKPI])

CREATE STATISTICS [_dta_stat_1031010754_64_12_61_66_3_55_65_57] ON [dbo].[Valves]([ValveSizeId], [InspectionFrequency], [ValveControlsId], [ValveZoneId], [BPUKPI], [InspectionFrequencyUnitId], [ValveStatusId], [OperatingCenterId])

CREATE STATISTICS [_dta_stat_1031010754_64_53_12_61_66_3_55_65] ON [dbo].[Valves]([ValveSizeId], [ValveBillingId], [InspectionFrequency], [ValveControlsId], [ValveZoneId], [BPUKPI], [InspectionFrequencyUnitId], [ValveStatusId])

CREATE STATISTICS [_dta_stat_1031010754_65_12_61_66_3] ON [dbo].[Valves]([ValveStatusId], [InspectionFrequency], [ValveControlsId], [ValveZoneId], [BPUKPI])

CREATE STATISTICS [_dta_stat_1031010754_65_57_53_64_25_12_55_61_3] ON [dbo].[Valves]([ValveStatusId], [OperatingCenterId], [ValveBillingId], [ValveSizeId], [Id], [InspectionFrequency], [InspectionFrequencyUnitId], [ValveControlsId], [BPUKPI])

CREATE STATISTICS [_dta_stat_1031010754_66_65_57_53_64_12_55_61] ON [dbo].[Valves]([ValveZoneId], [ValveStatusId], [OperatingCenterId], [ValveBillingId], [ValveSizeId], [InspectionFrequency], [InspectionFrequencyUnitId], [ValveControlsId])

");
        }

        public override void Down()
        {
            this.DeleteIndexIfItExists("ValveInspections", "_dta_index_ValveInspections_5_4911089__K9_K22_K13_2");
            this.DeleteIndexIfItExists("Valves",
                "_dta_index_Valves_5_1031010754__K57_K12_K61_K66_K3_K55_K65_K53_K64_K25");
            this.DeleteStatisticIfItExits("Valves", "_dta_stat_1031010754_12_55_61_3_66_65_57");
            this.DeleteStatisticIfItExits("Valves", "_dta_stat_1031010754_12_61_66_3_55_25_65_57_53");
            this.DeleteStatisticIfItExits("Valves", "_dta_stat_1031010754_25_12_61_66_3");
            this.DeleteStatisticIfItExits("Valves", "_dta_stat_1031010754_3_65_57_53_64_12_55");
            this.DeleteStatisticIfItExits("Valves", "_dta_stat_1031010754_53_12_61_66_3_55_65_57_64_25");
            this.DeleteStatisticIfItExits("Valves", "_dta_stat_1031010754_55_65_57_53_64_12");
            this.DeleteStatisticIfItExits("Valves", "_dta_stat_1031010754_61_65_57_53_64_12_55_3");
            this.DeleteStatisticIfItExits("Valves", "_dta_stat_1031010754_64_12_61_66_3_55_65_57");
            this.DeleteStatisticIfItExits("Valves", "_dta_stat_1031010754_64_53_12_61_66_3_55_65");
            this.DeleteStatisticIfItExits("Valves", "_dta_stat_1031010754_65_12_61_66_3");
            this.DeleteStatisticIfItExits("Valves", "_dta_stat_1031010754_65_57_53_64_25_12_55_61_3");
            this.DeleteStatisticIfItExits("Valves", "_dta_stat_1031010754_66_65_57_53_64_12_55_61");
        }
    }
}
