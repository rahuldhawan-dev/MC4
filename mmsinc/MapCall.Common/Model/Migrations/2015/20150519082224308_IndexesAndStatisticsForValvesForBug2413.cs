using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150519082224308), Tags("Production")]
    public class IndexesAndStatisticsForValvesForBug2413 : Migration
    {
        public const string CREATE_INDEXES = @"
                CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWValves_5_1031010754__K32_K20_K25_K40_K30_K41_1_6_29_33_35_37_43_44_45_47_49_51_52] ON [dbo].[tblNJAWValves](	[Town] ASC,[OpCntr] ASC,[RecID] ASC,[ValNum] ASC,[StName] ASC,[ValSuf] ASC)
	                INCLUDE ( 	[BillInfo],[CrossStreet],[StNum],[Traffic],[TwnSection],[ValCtrl],[ValveSize],[ValveStatus],[WONum],[ValveZone],[DateInst],[LastUpdated],[SAPEquipmentID]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
                CREATE STATISTICS [_dta_stat_1031010754_25_30_40_32] ON [dbo].[tblNJAWValves]([RecID], [StName], [ValNum], [Town])
                CREATE STATISTICS [_dta_stat_1031010754_25_32_20_41] ON [dbo].[tblNJAWValves]([RecID], [Town], [OpCntr], [ValSuf])
                CREATE STATISTICS [_dta_stat_1031010754_25_32_20_30_40_41] ON [dbo].[tblNJAWValves]([RecID], [Town], [OpCntr], [StName], [ValNum], [ValSuf])
                CREATE NONCLUSTERED INDEX [_dta_index_FunctionalLocations_5_1757353425__K1_2] ON [dbo].[FunctionalLocations]([FunctionalLocationID] ASC)INCLUDE ( [Description]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
                CREATE NONCLUSTERED INDEX [_dta_index_ValveImages_5_1653685039__K2_K37_K36_K28_1] ON [dbo].[ValveImages]([ValveNumber] ASC,[OperatingCenterId] ASC,[IsDefault] ASC,[DateAdded] ASC)
	                INCLUDE ( 	[ValveImageID]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
                CREATE STATISTICS [_dta_stat_1653685039_37_35] ON [dbo].[ValveImages]([OperatingCenterId], [ValveID])
                CREATE STATISTICS [_dta_stat_1653685039_35_2_37_36] ON [dbo].[ValveImages]([ValveID], [ValveNumber], [OperatingCenterId], [IsDefault])
                CREATE STATISTICS [_dta_stat_1653685039_35_36_28_2_37] ON [dbo].[ValveImages]([ValveID], [IsDefault], [DateAdded], [ValveNumber], [OperatingCenterId])
                CREATE STATISTICS [_dta_stat_1031010754_32_20_40_30] ON [dbo].[tblNJAWValves]([Town], [OpCntr], [ValNum], [StName])
                CREATE STATISTICS [_dta_stat_1031010754_30_25_40_20] ON [dbo].[tblNJAWValves]([StName], [RecID], [ValNum], [OpCntr])
                ",
                            ROLLBACK_INDEXES = @"
                if exists (select 1 from sysindexes where name = '_dta_index_tblNJAWValves_5_1031010754__K32_K20_K25_K40_K30_K41_1_6_29_33_35_37_43_44_45_47_49_51_52') drop index [_dta_index_tblNJAWValves_5_1031010754__K32_K20_K25_K40_K30_K41_1_6_29_33_35_37_43_44_45_47_49_51_52] on tblNJAWValves
                if exists (select 1 from sysindexes where name = '_dta_index_FunctionalLocations_5_1757353425__K1_2') drop index _dta_index_FunctionalLocations_5_1757353425__K1_2 on [FunctionalLocations]
                if exists (select 1 from sysindexes where name = '_dta_index_ValveImages_5_1653685039__K2_K37_K36_K28_1') drop index _dta_index_ValveImages_5_1653685039__K2_K37_K36_K28_1 on ValveImages
                if exists (select 1 from sysindexes where name = '_dta_stat_1031010754_25_30_40_32') drop statistics tblNJAWValves._dta_stat_1031010754_25_30_40_32 
                if exists (select 1 from sysindexes where name = '_dta_stat_1031010754_25_32_20_41') drop statistics tblNJAWValves._dta_stat_1031010754_25_32_20_41
                if exists (select 1 from sysindexes where name = '_dta_stat_1031010754_25_32_20_30_40_41') drop statistics tblNJAWValves._dta_stat_1031010754_25_32_20_30_40_41
                if exists (select 1 from sysindexes where name = '_dta_stat_1653685039_37_35') drop statistics ValveImages._dta_stat_1653685039_37_35
                if exists (select 1 from sysindexes where name = '_dta_stat_1653685039_35_2_37_36') drop statistics ValveImages._dta_stat_1653685039_35_2_37_36
                if exists (select 1 from sysindexes where name = '_dta_stat_1653685039_35_36_28_2_37') drop statistics ValveImages._dta_stat_1653685039_35_36_28_2_37
                if exists (select 1 from sysindexes where name = '_dta_stat_1031010754_32_20_40_30') drop statistics tblNJAWValves._dta_stat_1031010754_32_20_40_30
                if exists (select 1 from sysindexes where name = '_dta_stat_1031010754_30_25_40_20') drop statistics tblNJAWValves._dta_stat_1031010754_30_25_40_20
            ";

        public override void Up()
        {
            Execute.Sql(CREATE_INDEXES);
        }

        public override void Down()
        {
            Execute.Sql(ROLLBACK_INDEXES);
        }
    }
}
