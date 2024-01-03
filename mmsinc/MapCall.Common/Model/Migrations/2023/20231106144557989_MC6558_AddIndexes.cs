using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20231106144557989), Tags("Production")]
    public class MC6558_AddIndexes : Migration
    {
        public const string ADD_INDEXES = @"
if not exists (select 1 from sysindexes where name = '_dta_index_HydrantPaintings_5_1991886363__K2_K5_1')
begin
    CREATE NONCLUSTERED INDEX [_dta_index_HydrantPaintings_5_1991886363__K2_K5_1] ON [dbo].[HydrantPaintings]
        ([HydrantId] ASC, [PaintedAt] ASC) 
    INCLUDE([Id]) 
    WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
end

if not exists (select 1 from sysindexes where name = '_dta_index_Hydrants_5_580913141__K79_K41_K34_K96_K84_K85_K3_21_90_102_103_104')
BEGIN
    CREATE NONCLUSTERED INDEX [_dta_index_Hydrants_5_580913141__K79_K41_K34_K96_K84_K85_K3_21_90_102_103_104] ON [dbo].[Hydrants]
        ([OperatingCenterId] ASC, [Town] ASC, [Id] ASC, [AssetStatusId] ASC, [HydrantBillingId] ASC, [CoordinateId] ASC, [IsNonBPUKPI] ASC)
    INCLUDE([HydrantSuffix],[Stop],[PaintingFrequency],[PaintingFrequencyUnitId],[PaintingZone]) 
    WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
END

if not exists (select 1 from sysindexes where name = '_dta_index_HydrantsOutOfService_5_1090818948__K4_K17')
begin
    CREATE NONCLUSTERED INDEX [_dta_index_HydrantsOutOfService_5_1090818948__K4_K17] ON [dbo].[HydrantsOutOfService]
        ([BackInServiceDate] ASC,[HydrantId] ASC) 
    WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
end

if not exists (select 1 from sysindexes where name = '_dta_index_OperatingCenters_5_1805301541__K1_32_33_55_60_61_62')
begin
    CREATE NONCLUSTERED INDEX [_dta_index_OperatingCenters_5_1805301541__K1_32_33_55_60_61_62] ON [dbo].[OperatingCenters]
	    ([OperatingCenterID] ASC)
    INCLUDE([HydrantInspectionFrequency],[HydrantInspectionFrequencyUnitId],[ZoneStartYear],[PaintingZoneStartYear],[HydrantPaintingFrequency],[HydrantPaintingFrequencyUnitId]) 
    WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
end

if not exists (select 1 from sysindexes where name = '_dta_index_Towns_5_354152357__K80')
begin
    CREATE NONCLUSTERED INDEX [_dta_index_Towns_5_354152357__K80] ON [dbo].[Towns]
        ([StateID] ASC)
    WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
end

if not exists (select 1 from sysindexes where name = 'ShortCycleCustomerMaterials_PremisID')
begin
    CREATE NONCLUSTERED INDEX [ShortCycleCustomerMaterials_PremisID]
    ON [dbo].[ShortCycleCustomerMaterials] ([PremiseId])
END

if not exists (select 1 from sysindexes where name = 'ContractorsSecureFormTokens_Token')
begin
    create nonclustered index ContractorsSecureFormTokens_Token on dbo.ContractorsSecureFormTokens ([Token]);
end",
                            DROP_INDEXES = @"
if exists (select 1 from sysindexes where name = '_dta_index_HydrantPaintings_5_1991886363__K2_K5_1')
begin
    DROP INDEX [_dta_index_HydrantPaintings_5_1991886363__K2_K5_1] ON [dbo].[HydrantPaintings]
end

if exists (select 1 from sysindexes where name = '_dta_index_Hydrants_5_580913141__K79_K41_K34_K96_K84_K85_K3_21_90_102_103_104')
BEGIN
    DROP INDEX [_dta_index_Hydrants_5_580913141__K79_K41_K34_K96_K84_K85_K3_21_90_102_103_104] ON [dbo].[Hydrants]
END

if exists (select 1 from sysindexes where name = '_dta_index_HydrantsOutOfService_5_1090818948__K4_K17')
begin
    DROP INDEX [_dta_index_HydrantsOutOfService_5_1090818948__K4_K17] ON [dbo].[HydrantsOutOfService]
end

if exists (select 1 from sysindexes where name = '_dta_index_OperatingCenters_5_1805301541__K1_32_33_55_60_61_62')
begin
    DROP INDEX [_dta_index_OperatingCenters_5_1805301541__K1_32_33_55_60_61_62] ON [dbo].[OperatingCenters]
end

if exists (select 1 from sysindexes where name = '_dta_index_Towns_5_354152357__K80')
begin
    DROP INDEX [_dta_index_Towns_5_354152357__K80] ON [dbo].[Towns]
end

if exists (select 1 from sysindexes where name = 'ShortCycleCustomerMaterials_PremisID')
begin
    drop INDEX [ShortCycleCustomerMaterials_PremisID] ON [dbo].[ShortCycleCustomerMaterials]
END

if exists (select 1 from sysindexes where name = 'ContractorsSecureFormTokens_Token')
begin
    drop index ContractorsSecureFormTokens_Token on dbo.ContractorsSecureFormTokens
end";
        public override void Up()
        {
            Execute.Sql(ADD_INDEXES);
        }

        public override void Down()
        {
            Execute.Sql(DROP_INDEXES);
        }
    }
}

