using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150519082225002), Tags("Production")]
    public class UpdateValvesForBug2224 : Migration
    {
        #region Constants

        public struct Sql
        {
            public const string
                DROP_INDEXES_STATS =
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = '_dta_stat_1012914680_18_8_2_9') DROP STATISTICS [tblNJAWValInspData].[_dta_stat_1012914680_18_8_2_9]" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = '_dta_stat_1012914680_8_18_2_9_19') DROP STATISTICS [tblNJAWValInspData].[_dta_stat_1012914680_8_18_2_9_19]" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = '_dta_stat_4911089_13_8') DROP STATISTICS [tblNJAWValInspData].[_dta_stat_4911089_13_8]" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = '_dta_stat_4911089_8_18_13_2_16_17_3_4_5_1_9_10_11_19_20_21') DROP STATISTICS [tblNJAWValInspData].[_dta_stat_4911089_8_18_13_2_16_17_3_4_5_1_9_10_11_19_20_21]" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = '_dta_stat_4911089_8_18_9_19_13') DROP STATISTICS [tblNJAWValInspData].[_dta_stat_4911089_8_18_9_19_13]" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = '_dta_stat_4911089_8_18_9_2_19_13') DROP STATISTICS [tblNJAWValInspData].[_dta_stat_4911089_8_18_9_2_19_13]" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = '_dta_stat_4911089_9_19_13') DROP STATISTICS [tblNJAWValInspData].[_dta_stat_4911089_9_19_13]" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = '_dta_stat_4911089_9_2_19_13_8') DROP STATISTICS [tblNJAWValInspData].[_dta_stat_4911089_9_2_19_13_8]" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = '_dta_index_tblNJAWValInspData_7_1012914680__K18_K8_K2_K9_K19') DROP INDEX [_dta_index_tblNJAWValInspData_7_1012914680__K18_K8_K2_K9_K19] ON [dbo].[tblNJAWValInspData]" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = '_dta_index_tblNJAWValInspData_7_4911089__K22_K13_2') DROP INDEX [_dta_index_tblNJAWValInspData_7_4911089__K22_K13_2] ON [dbo].[tblNJAWValInspData]" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = '_dta_index_tblNJAWValInspData_8_4911089__K18_K8_K9_K2') DROP INDEX [_dta_index_tblNJAWValInspData_8_4911089__K18_K8_K9_K2] ON [dbo].[tblNJAWValInspData]" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = 'InspectDate') DROP INDEX [InspectDate] ON [dbo].[tblNJAWValInspData]" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = 'RecID') DROP INDEX [RecID] ON [dbo].[tblNJAWValInspData] WITH ( ONLINE = OFF )" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = 'ValNumOpCntr') DROP INDEX [ValNumOpCntr] ON [dbo].[tblNJAWValInspData]" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = 'ValNumOpCntrOperated') DROP INDEX [ValNumOpCntrOperated] ON [dbo].[tblNJAWValInspData];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = '_dta_index_tblNJAWValves_15_1031010754__K20_K40_K14_K16_K37') DROP INDEX [_dta_index_tblNJAWValves_15_1031010754__K20_K40_K14_K16_K37] ON [dbo].[tblNJAWValves];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = '_dta_index_tblNJAWValves_15_1031010754__K27_K32') DROP INDEX [_dta_index_tblNJAWValves_15_1031010754__K27_K32] ON [dbo].[tblNJAWValves];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = '_dta_index_tblNJAWValves_15_1031010754__K30_K41_K40_K25_K35_K32_K44_K20_K6_K16_K14') DROP INDEX [_dta_index_tblNJAWValves_15_1031010754__K30_K41_K40_K25_K35_K32_K44_K20_K6_K16_K14] ON [dbo].[tblNJAWValves];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = '_dta_index_tblNJAWValves_15_1031010754__K32_K20_K41_K40') DROP INDEX [_dta_index_tblNJAWValves_15_1031010754__K32_K20_K41_K40] ON [dbo].[tblNJAWValves];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = '_dta_index_tblNJAWValves_15_1031010754__K40') DROP INDEX [_dta_index_tblNJAWValves_15_1031010754__K40] ON [dbo].[tblNJAWValves];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = '_dta_index_tblNJAWValves_15_1031010754__K44_K43_K20') DROP INDEX [_dta_index_tblNJAWValves_15_1031010754__K44_K43_K20] ON [dbo].[tblNJAWValves];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = '_dta_index_tblNJAWValves_7_1031010754__K20_K25_43') DROP INDEX [_dta_index_tblNJAWValves_7_1031010754__K20_K25_43] ON [dbo].[tblNJAWValves];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = '_dta_index_tblNJAWValves_7_1031010754__K32_K20_K25_K40_K43_K30_K27_K41_1_3_4_6_18_29_33_37_39_44_47_49') DROP INDEX [_dta_index_tblNJAWValves_7_1031010754__K32_K20_K25_K40_K43_K30_K27_K41_1_3_4_6_18_29_33_37_39_44_47_49] ON [dbo].[tblNJAWValves];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = '_dta_index_tblNJAWValves_8_1031010754__K20_K40_K32_K43_K37_K44_K3_K1_K7') DROP INDEX [_dta_index_tblNJAWValves_8_1031010754__K20_K40_K32_K43_K37_K44_K3_K1_K7] ON [dbo].[tblNJAWValves];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes where Name = 'IDX_ValNum_OpCntr_Town_ValveSize_ValCtrl_ValveStatus_BPUKPI_BillInfo_DateInst_ValveZone') DROP INDEX [IDX_ValNum_OpCntr_Town_ValveSize_ValCtrl_ValveStatus_BPUKPI_BillInfo_DateInst_ValveZone] ON [dbo].[tblNJAWValves];" +
                    "IF EXISTS (SELECT 1 FROM SysObjects where name = 'DF_tblNJAWValves_InspFreq') ALTER TABLE [dbo].[tblNJAWValves] DROP CONSTRAINT [DF_tblNJAWValves_InspFreq];" +
                    "IF EXISTS (SELECT 1 FROM SysObjects where name = 'DF_tblNJAWValves_InspFreqUnitDF_tblNJAWValves_InspFreqUnit') ALTER TABLE [dbo].[tblNJAWValves] DROP CONSTRAINT [DF_tblNJAWValves_InspFreqUnit];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_20_25') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_20_25];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_20_32_40_43_37_44_3_1_7') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_20_32_40_43_37_44_3_1_7];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_20_32_41_40') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_20_32_41_40];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_20_37') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_20_37];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_20_40_14_16_37_43_44_3_1_7_12_13') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_20_40_14_16_37_43_44_3_1_7_12_13];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_20_40_37_43_44_3_1_7_12_13_14') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_20_40_37_43_44_3_1_7_12_13_14];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_20_40_38_21_19_34_4_5') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_20_40_38_21_19_34_4_5];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_20_40_38_41_4_5_25') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_20_40_38_41_4_5_25];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_20_40_43') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_20_40_43];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_20_40_43_37_44_3_1_7_12_13') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_20_40_43_37_44_3_1_7_12_13];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_20_40_44_1_7_12_13') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_20_40_44_1_7_12_13];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_20_41_32_30_40_6_7_25_29_45_35_18_44_43_33_37') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_20_41_32_30_40_6_7_25_29_45_35_18_44_43_33_37];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_20_43') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_20_43];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_25_43_32') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_25_43_32];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_27_41_40_43_32_20') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_27_41_40_43_32_20];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_27_43') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_27_43];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_30_20_40_41') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_30_20_40_41];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_30_32_20') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_30_32_20];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_30_32_25_20_40') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_30_32_25_20_40];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_30_32_41') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_30_32_41];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_30_43_25_32_20_40_27_41') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_30_43_25_32_20_40_27_41];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_32_20_25_40_41_43_30') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_32_20_25_40_41_43_30];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_32_20_43_25') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_32_20_43_25];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_32_40') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_32_40];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_32_41') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_32_41];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_33_20_37_43') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_33_20_37_43];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_40_20_25_43_37_44_3_1_7_12_13') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_40_20_25_43_37_44_3_1_7_12_13];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_40_20_32_38_21_19_34_4_5') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_40_20_32_38_21_19_34_4_5];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_40_20_42_43_34_33_4_5_25_37_44_3_1_7_12_13') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_40_20_42_43_34_33_4_5_25_37_44_3_1_7_12_13];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_40_32_20_25') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_40_32_20_25];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_41_32') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_41_32];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_41_40_32_20_43_30_27') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_41_40_32_20_43_30_27];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_43_20_37') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_43_20_37];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_43_25_20') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_43_25_20];" +
                    "IF EXISTS (SELECT 1 FROM SysIndexes WHERE NAME = '_dta_stat_1031010754_43_25_30_27') DROP STATISTICS [dbo].[tblNJAWValves].[_dta_stat_1031010754_43_25_30_27];",
                ROLLBACK_INDEXES_STATS =
                    "CREATE STATISTICS [_dta_stat_1012914680_18_8_2_9] ON [dbo].[tblNJAWValInspData]([ValNum], [OpCntr], [DateInspect], [Operated], [RecID])" +
                    "CREATE STATISTICS [_dta_stat_1012914680_8_18_2_9_19] ON [dbo].[tblNJAWValInspData]([OpCntr], [ValNum], [DateInspect], [Operated], [WOReq1], [RecID])" +
                    "CREATE STATISTICS [_dta_stat_4911089_13_8] ON [dbo].[tblNJAWValInspData]([RecID], [OpCntr])" +
                    "CREATE STATISTICS [_dta_stat_4911089_8_18_13_2_16_17_3_4_5_1_9_10_11_19_20_21] ON [dbo].[tblNJAWValInspData]([OpCntr], [ValNum], [RecID], [DateInspect], [Turns], [TurnsNotCompleted], [Inaccessible], [InspectBy], [InspectorNum], [DateAdded], [Operated], [PosFound], [PosLeft], [WOReq1], [WOReq2], [WOReq3])" +
                    "CREATE STATISTICS [_dta_stat_4911089_8_18_9_19_13] ON [dbo].[tblNJAWValInspData]([OpCntr], [ValNum], [Operated], [WOReq1], [RecID])" +
                    "CREATE STATISTICS [_dta_stat_4911089_8_18_9_2_19_13] ON [dbo].[tblNJAWValInspData]([OpCntr], [ValNum], [Operated], [DateInspect], [WOReq1], [RecID])" +
                    "CREATE STATISTICS [_dta_stat_4911089_9_19_13] ON [dbo].[tblNJAWValInspData]([Operated], [WOReq1], [RecID])" +
                    "CREATE STATISTICS [_dta_stat_4911089_9_2_19_13_8] ON [dbo].[tblNJAWValInspData]([Operated], [DateInspect], [WOReq1], [RecID], [OpCntr])" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWValInspData_7_1012914680__K18_K8_K2_K9_K19] ON [dbo].[tblNJAWValInspData]([ValNum] ASC,[OpCntr] ASC,[DateInspect] ASC,[Operated] ASC,[WOReq1] ASC) ON [PRIMARY]" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWValInspData_7_4911089__K22_K13_2] ON [dbo].[tblNJAWValInspData]([ValveID] ASC,[RecID] ASC) INCLUDE ( [DateInspect])  ON [PRIMARY]" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWValInspData_8_4911089__K18_K8_K9_K2] ON [dbo].[tblNJAWValInspData]([ValNum] ASC,[OpCntr] ASC,[Operated] ASC,[DateInspect] ASC) ON [PRIMARY]" +
                    "CREATE NONCLUSTERED INDEX [InspectDate] ON [dbo].[tblNJAWValInspData]([DateInspect] ASC) ON [PRIMARY]" +
                    "CREATE UNIQUE CLUSTERED INDEX [RecID] ON [dbo].[tblNJAWValInspData] ([RecID] ASC) ON [PRIMARY]" +
                    "CREATE NONCLUSTERED INDEX [ValNumOpCntr] ON [dbo].[tblNJAWValInspData]([OpCntr] ASC, [ValNum] ASC) ON [PRIMARY]" +
                    "CREATE NONCLUSTERED INDEX [ValNumOpCntrOperated] ON [dbo].[tblNJAWValInspData] ([OpCntr] ASC, [Operated] ASC, [ValNum] ASC) ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWValves_15_1031010754__K20_K40_K14_K16_K37] ON [dbo].[tblNJAWValves] ([OpCntr] ASC,[ValNum] ASC,[Lat] ASC,[Lon] ASC,[ValCtrl] ASC) ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWValves_15_1031010754__K27_K32] ON [dbo].[tblNJAWValves]([Route] ASC,[Town] ASC) ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWValves_15_1031010754__K30_K41_K40_K25_K35_K32_K44_K20_K6_K16_K14] ON [dbo].[tblNJAWValves]([StName] ASC,[ValSuf] ASC,[ValNum] ASC,[RecID] ASC,[TwnSection] ASC,[Town] ASC,[ValveStatus] ASC,[OpCntr] ASC,[CrossStreet] ASC,[Lon] ASC,[Lat] ASC) ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWValves_15_1031010754__K32_K20_K41_K40] ON [dbo].[tblNJAWValves]([Town] ASC,[OpCntr] ASC,[ValSuf] ASC,[ValNum] ASC) ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWValves_15_1031010754__K40] ON [dbo].[tblNJAWValves]([ValNum] ASC) ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWValves_15_1031010754__K44_K43_K20] ON [dbo].[tblNJAWValves]([ValveStatus] ASC,[ValveSize] ASC,[OpCntr] ASC) ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWValves_7_1031010754__K20_K25_43] ON [dbo].[tblNJAWValves]([OpCntr] ASC,[RecID] ASC) INCLUDE ([ValveSize]) ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWValves_7_1031010754__K32_K20_K25_K40_K43_K30_K27_K41_1_3_4_6_18_29_33_37_39_44_47_49] ON [dbo].[tblNJAWValves] ([Town] ASC,[OpCntr] ASC,[RecID] ASC,[ValNum] ASC,[ValveSize] ASC,[StName] ASC,[Route] ASC,[ValSuf] ASC) INCLUDE ( 	[BillInfo],[BPUKPI],[Critical],[CrossStreet],[MapPage],[StNum],[Traffic],[ValCtrl],[ValMake],[ValveStatus],[ValveZone],[DateInst])  ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWValves_8_1031010754__K20_K40_K32_K43_K37_K44_K3_K1_K7] ON [dbo].[tblNJAWValves]([OpCntr] ASC,[ValNum] ASC,[Town] ASC,[ValveSize] ASC,[ValCtrl] ASC,[ValveStatus] ASC,[BPUKPI] ASC,[BillInfo] ASC,[DateInst] ASC) ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [IDX_ValNum_OpCntr_Town_ValveSize_ValCtrl_ValveStatus_BPUKPI_BillInfo_DateInst_ValveZone] ON [dbo].[tblNJAWValves]([ValNum] ASC,[OpCntr] ASC,[Town] ASC,[ValveSize] ASC,[ValCtrl] ASC,[ValveStatus] ASC,[BPUKPI] ASC,[BillInfo] ASC,[DateInst] ASC,[ValveZone] ASC) ON [PRIMARY];" +
                    "ALTER TABLE [dbo].[tblNJAWValves] ADD  CONSTRAINT [DF_tblNJAWValves_InspFreq]  DEFAULT (1) FOR [InspFreq];" +
                    "ALTER TABLE [dbo].[tblNJAWValves] ADD  CONSTRAINT [DF_tblNJAWValves_InspFreqUnit]  DEFAULT ('Y') FOR [InspFreqUnit];" +
                    "CREATE STATISTICS [_dta_stat_1031010754_20_25] ON [dbo].[tblNJAWValves]([OpCntr], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_20_32_40_43_37_44_3_1_7] ON [dbo].[tblNJAWValves]([OpCntr], [Town], [ValNum], [ValveSize], [ValCtrl], [ValveStatus], [BPUKPI], [BillInfo], [DateInst], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_20_32_41_40] ON [dbo].[tblNJAWValves]([OpCntr], [Town], [ValSuf], [ValNum], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_20_37] ON [dbo].[tblNJAWValves]([OpCntr], [ValCtrl]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_20_40_14_16_37_43_44_3_1_7_12_13] ON [dbo].[tblNJAWValves]([OpCntr], [ValNum], [Lat], [Lon], [ValCtrl], [ValveSize], [ValveStatus], [BPUKPI], [BillInfo], [DateInst], [InspFreq], [InspFreqUnit], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_20_40_37_43_44_3_1_7_12_13_14] ON [dbo].[tblNJAWValves]([OpCntr], [ValNum], [ValCtrl], [ValveSize], [ValveStatus], [BPUKPI], [BillInfo], [DateInst], [InspFreq], [InspFreqUnit], [Lat], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_20_40_38_21_19_34_4_5] ON [dbo].[tblNJAWValves]([OpCntr], [ValNum], [ValLoc], [Opens], [NorPos], [Turns], [Critical], [CriticalNotes], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_20_40_38_41_4_5_25] ON [dbo].[tblNJAWValves]([OpCntr], [ValNum], [ValLoc], [ValSuf], [Critical], [CriticalNotes], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_20_40_43] ON [dbo].[tblNJAWValves]([OpCntr], [ValNum], [ValveSize], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_20_40_43_37_44_3_1_7_12_13] ON [dbo].[tblNJAWValves]([OpCntr], [ValNum], [ValveSize], [ValCtrl], [ValveStatus], [BPUKPI], [BillInfo], [DateInst], [InspFreq], [InspFreqUnit], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_20_40_44_1_7_12_13] ON [dbo].[tblNJAWValves]([OpCntr], [ValNum], [ValveStatus], [BillInfo], [DateInst], [InspFreq], [InspFreqUnit], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_20_41_32_30_40_6_7_25_29_45_35_18_44_43_33_37] ON [dbo].[tblNJAWValves]([OpCntr], [ValSuf], [Town], [StName], [ValNum], [CrossStreet], [DateInst], [RecID], [StNum], [WONum], [TwnSection], [MapPage], [ValveStatus], [ValveSize], [Traffic], [ValCtrl]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_20_43] ON [dbo].[tblNJAWValves]([OpCntr], [ValveSize]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_25_43_32] ON [dbo].[tblNJAWValves]([RecID], [ValveSize], [Town]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_27_41_40_43_32_20] ON [dbo].[tblNJAWValves]([Route], [ValSuf], [ValNum], [ValveSize], [Town], [OpCntr]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_27_43] ON [dbo].[tblNJAWValves]([Route], [ValveSize]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_30_20_40_41] ON [dbo].[tblNJAWValves]([StName], [OpCntr], [ValNum], [ValSuf], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_30_32_20] ON [dbo].[tblNJAWValves]([StName], [Town], [OpCntr], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_30_32_25_20_40] ON [dbo].[tblNJAWValves]([StName], [Town], [RecID], [OpCntr], [ValNum]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_30_32_41] ON [dbo].[tblNJAWValves]([StName], [Town], [ValSuf], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_30_43_25_32_20_40_27_41] ON [dbo].[tblNJAWValves]([StName], [ValveSize], [RecID], [Town], [OpCntr], [ValNum], [Route], [ValSuf]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_32_20_25_40_41_43_30] ON [dbo].[tblNJAWValves]([Town], [OpCntr], [RecID], [ValNum], [ValSuf], [ValveSize], [StName]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_32_20_43_25] ON [dbo].[tblNJAWValves]([Town], [OpCntr], [ValveSize], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_32_40] ON [dbo].[tblNJAWValves]([Town], [ValNum], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_32_41] ON [dbo].[tblNJAWValves]([Town], [ValSuf], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_33_20_37_43] ON [dbo].[tblNJAWValves]([Traffic], [OpCntr], [ValCtrl], [ValveSize]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_40_20_25_43_37_44_3_1_7_12_13] ON [dbo].[tblNJAWValves]([ValNum], [OpCntr], [RecID], [ValveSize], [ValCtrl], [ValveStatus], [BPUKPI], [BillInfo], [DateInst], [InspFreq], [InspFreqUnit]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_40_20_32_38_21_19_34_4_5] ON [dbo].[tblNJAWValves]([ValNum], [OpCntr], [Town], [ValLoc], [Opens], [NorPos], [Turns], [Critical], [CriticalNotes], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_40_20_42_43_34_33_4_5_25_37_44_3_1_7_12_13] ON [dbo].[tblNJAWValves]([ValNum], [OpCntr], [ValType], [ValveSize], [Turns], [Traffic], [Critical], [CriticalNotes], [RecID], [ValCtrl], [ValveStatus], [BPUKPI], [BillInfo], [DateInst], [InspFreq], [InspFreqUnit]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_40_32_20_25] ON [dbo].[tblNJAWValves]([ValNum], [Town], [OpCntr], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_41_32] ON [dbo].[tblNJAWValves]([ValSuf], [Town], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_41_40_32_20_43_30_27] ON [dbo].[tblNJAWValves]([ValSuf], [ValNum], [Town], [OpCntr], [ValveSize], [StName], [Route]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_43_20_37] ON [dbo].[tblNJAWValves]([ValveSize], [OpCntr], [ValCtrl]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_43_25_20] ON [dbo].[tblNJAWValves]([ValveSize], [RecID], [OpCntr]);" +
                    "CREATE STATISTICS [_dta_stat_1031010754_43_25_30_27] ON [dbo].[tblNJAWValves]([ValveSize], [RecID], [StName], [Route]);",
                UPDATE_VALVES =
                    "DELETE FROM ValvesSAP where ValveID in (Select RecID FROM tblNJAWValves where isNull(OpCntr, '') = '');" +
                    "DELETE FROM tblNJAWValves where isNull(OpCntr, '') = '';" +
                    "UPDATE tblNJAWValves SET DateRetired = NULL where DateRetired = '01/01/1900';" +
                    "UPDATE tblNJAWValves SET DateTested = NULL where DateTested = '01/01/1900';" +
                    "UPDATE tblNJAWValves SET DateInst = NULL where DateInst = '01/01/1900';" +
                    "UPDATE tblNJAWValves SET NorPos = 'OPEN' where NorPos = 'NORMALLY OPEN';" +
                    "UPDATE tblNJAWValves SET NorPos = 'CLOSED' where NorPos = 'NORMALLY CLOSED';" +
                    "UPDATE tblNJAWValves SET BillInfo = 'PUBLIC' where isNull(BillInfo, '') = '';" +
                    "UPDATE tblNJAWValves SET ValveStatus = 'PENDING' where isNull(ValveStatus, '') = ''",
                ROLLBACK_VALVES = "",
                UPDATE_VALVE_INSPECTION =
                    "UPDATE tblNJAWValInspData set Inaccessible = 'Temporarily Inaccessible' where Inaccessible = 'Temp Inaccessible';" +
                    "UPDATE tblNJAWValInspData set Inaccessible = 'Traffic Control Required' where Inaccessible = 'Traffic Control';" +
                    "UPDATE tblNJAWValInspData set Inaccessible = NULL where Inaccessible = 'DUP';" +
                    "UPDATE tblNJAWValInspData set NorPos = 'CLOSED' WHERE NorPos = 'NORMALLY CLOSED';" +
                    "UPDATE tblNJAWValInspData set NorPos = 'OPEN' WHERE NorPos = 'NORMALLY OPEN';" +
                    "UPDATE tblNJAWValInspData set NorPos = NULL WHERE isNull(NorPos, '') = '';" +
                    "UPDATE tblNJAWValInspData SET Turns = null WHERE IsNumeric(Turns) = 0;" +
                    "UPDATE tblNJAWValInspData SET MinReq = null WHERE IsNumeric(MinReq) = 0;" +
                    "UPDATE tblNJAWValInspData SET Operated = 0 WHERE isNull(Operated, '') = '' OR IsNull(Operated, '') = 'NO';" +
                    "UPDATE tblNJAWValInspData SET Operated = 1 WHERE IsNull(Operated, '') = 'YES';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'mortara' where inspectBy = 'Anthony Mortarulo';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'bertbaker' where inspectBy = 'bakerb';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'diskinc' where inspectBy = 'Charles Diskin';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'bruenod' where inspectBy = 'David Brueno';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'williamsd' where inspectBy = 'Dock Williams';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'jonesdo' where inspectBy = 'Douglas Jones';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'dougthorn' where inspectBy = 'Douglas Thorn';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'eilbacher' where inspectBy = 'eilbachg';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'picconef' where inspectBy = 'Frank Piccone';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'amatog' where inspectBy = 'Gerald Amato';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'joegreen' where inspectBy = 'greenjo';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'joshgwyn' where inspectBy = 'gwynj';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'davisj' where inspectBy = 'James Davis';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'straskoj' where inspectBy = 'James Strasko';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'viladej' where inspectBy = 'James Vilade';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'perezj' where inspectBy = 'Jorge Parez';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'muhajo' where inspectBy = 'Joseph Muha';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'pierjo' where inspectBy = 'Joseph Pier';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'smithka' where inspectBy = 'karlsm';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'mobleyl' where inspectBy = 'Leroy Mobley';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'kevinmaloney' where inspectBy = 'maloneyk';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'oeckinm' where inspectBy = 'Mark Oeckinghaus';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'pannellam' where inspectBy = 'Mark Pannella';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'stevemason' where inspectBy = 'masonst';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'bormanmw' where inspectBy = 'Mike Bormann';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'amermanp' where inspectBy = 'Paul Amerman';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'baceri' where inspectBy = 'Richard Bace';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'allainr' where inspectBy = 'Robert Allain';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'sullivanr' where inspectBy = 'Robert Sullivan';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'bibbor' where inspectBy = 'Rosario Bibbo';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'hubertsingley' where inspectBy = 'singleyh';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'joestankiewicz' where inspectBy = 'stankiej';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'sablacks' where inspectBy = 'Steve Sablack';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'spaint' where inspectBy = 'Tod Spain';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'boburban' where inspectBy = 'urbanr';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'williamsd' where inspectBy = 'willimasd';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'sutulaz' where inspectBy = 'Ziggy Sutula';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'dennisdadamo' where inspectBy = 'dadamod';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'joedenisco' where inspectBy = 'deniscoj';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'eilbacher' where inspectBy = 'eilbachg';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'mikefranzoso' where inspectBy = 'franzosom';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'joegreen' where inspectBy = 'greenjo';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'joshgwyn' where inspectBy = 'gwynj';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'rolandhargrove' where inspectBy = 'hargrover';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'smithka' where inspectBy = 'karlsm';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'stevemason' where inspectBy = 'masonst';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'hubertsingley' where inspectBy = 'singleyh';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'joestankiewicz' where inspectBy = 'stankiej';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'boburban' where inspectBy = 'urbanr';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'chrisweidele' where inspectBy = 'weidelec';" +
                    "UPDATE tblNJAWValInspData SET InspectBy = 'cicerowa' where inspectBy = 'ciceroneb';" +
                    "update tblNJAWValInspData set DateInspect = DateAdded where DateInspect is null;",
                ROLLBACK_VALVE_INSPECTIONS = "UPDATE tblNJAWValInspData SET Operated = 'YES' WHERE Operated = '1';" +
                                             "UPDATE tblNJAWValInspData SET Operated = 'NO' WHERE Operated = '0';",
                UPDATE_VALVE_COORDINATES = @"
                        SET NOCOUNT ON 
                        DECLARE @latitude float
                        DECLARE @longitude float
                        DECLARE @id int
                        DECLARE @coordinateID int

                        DECLARE	tableCursor 
                        CURSOR FOR 
	                        SELECT RecId, Lat, Lon FROM Valves WHERE Lat is not null and Lon is not null 

                        OPEN tableCursor 
	                        FETCH NEXT FROM tableCursor INTO @id, @latitude, @longitude; 
	                        WHILE @@FETCH_STATUS = 0 
	                        BEGIN 
		                        Insert Into Coordinates(latitude, longitude) values(@latitude, @longitude)
		                        update Valves set coordinateID = @@Identity where RecId = @id
		                        FETCH NEXT FROM tableCursor INTO @id, @latitude, @longitude; 
	                        END
                        CLOSE tableCursor; 
                        DEALLOCATE tableCursor;",
                ROLLBACK_VALVE_COORDINATES =
                    "Update Valves Set Lat = (SELECT Latitude from Coordinates where Coordinates.CoordinateID = Valves.CoordinateID);" +
                    "Update Valves Set Lon = (SELECT Longitude from Coordinates where Coordinates.CoordinateID = Valves.CoordinateID);",
                NEW_INDEXES_STATISTICS =
                    "CREATE CLUSTERED INDEX [_dta_index_ValveInspections_c_19_4911089__K22] ON [dbo].[ValveInspections]([ValveID] ASC)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY];" +
                    "CREATE STATISTICS [_dta_stat_1031010754_52_25] ON [dbo].[Valves]([SAPEquipmentID], [Id]);" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_ValveInspections_19_4911089__K22_K13_K9_2] ON [dbo].[ValveInspections] ( [ValveID] ASC, [Id] ASC, [Operated] ASC ) INCLUDE ([DateInspected]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]" +
                    "CREATE STATISTICS [_dta_stat_4911089_13_9] ON [dbo].[ValveInspections]([Id], [Operated])" +
                    "CREATE STATISTICS [_dta_stat_4911089_13_22_9] ON [dbo].[ValveInspections]([Id], [ValveID], [Operated])" +
                    "CREATE STATISTICS [_dta_stat_1031010754_57_61_3] ON [dbo].[Valves]([OperatingCenterId], [ValveControlsId], [BPUKPI])" +
                    "CREATE STATISTICS [_dta_stat_1031010754_25_57_61_66] ON [dbo].[Valves]([Id], [OperatingCenterId], [ValveControlsId], [ValveZoneId])" +
                    "CREATE STATISTICS [_dta_stat_1031010754_53_57_61_66] ON [dbo].[Valves]([ValveBillingId], [OperatingCenterId], [ValveControlsId], [ValveZoneId])" +
                    "CREATE STATISTICS [_dta_stat_1031010754_65_57_61_66_3] ON [dbo].[Valves]([ValveStatusId], [OperatingCenterId], [ValveControlsId], [ValveZoneId], [BPUKPI])" +
                    "CREATE STATISTICS [_dta_stat_1031010754_64_65_25_53_57] ON [dbo].[Valves]([ValveSizeId], [ValveStatusId], [Id], [ValveBillingId], [OperatingCenterId])" +
                    "CREATE STATISTICS [_dta_stat_1031010754_3_64_65_25_57_53] ON [dbo].[Valves]([BPUKPI], [ValveSizeId], [ValveStatusId], [Id], [OperatingCenterId], [ValveBillingId])" +
                    "CREATE STATISTICS [_dta_stat_1031010754_57_61_66_3_25_64] ON [dbo].[Valves]([OperatingCenterId], [ValveControlsId], [ValveZoneId], [BPUKPI], [Id], [ValveSizeId])" +
                    "CREATE STATISTICS [_dta_stat_1031010754_66_64_65_25_57_53_61] ON [dbo].[Valves]([ValveZoneId], [ValveSizeId], [ValveStatusId], [Id], [OperatingCenterId], [ValveBillingId], [ValveControlsId])" +
                    "CREATE STATISTICS [_dta_stat_1031010754_61_64_65_25_57_53_3] ON [dbo].[Valves]([ValveControlsId], [ValveSizeId], [ValveStatusId], [Id], [OperatingCenterId], [ValveBillingId], [BPUKPI])" +
                    "CREATE STATISTICS [_dta_stat_1031010754_57_61_66_3_53_64_65] ON [dbo].[Valves]([OperatingCenterId], [ValveControlsId], [ValveZoneId], [BPUKPI], [ValveBillingId], [ValveSizeId], [ValveStatusId])" +
                    "CREATE STATISTICS [_dta_stat_1031010754_64_57_61_66_3_65_25_53] ON [dbo].[Valves]([ValveSizeId], [OperatingCenterId], [ValveControlsId], [ValveZoneId], [BPUKPI], [ValveStatusId], [Id], [ValveBillingId])",
                REMOVE_NEW_INDEXES_STATISTICS =
                    "IF Exists(select 1 from SysIndexes where name = '_dta_index_ValveInspections_c_19_4911089__K22') DROP INDEX [ValveInspections].[_dta_index_ValveInspections_c_19_4911089__K22]" +
                    "IF Exists(select 1 from SysIndexes where name = '_dta_stat_1031010754_52_25') DROP STATISTICS [Valves].[_dta_stat_1031010754_52_25]" +
                    "IF EXISTS (SELECT 1 FROM sysindexes where name = '_dta_index_ValveInspections_19_4911089__K22_K13_K9_2') DROP INDEX [_dta_index_ValveInspections_19_4911089__K22_K13_K9_2] ON [dbo].[ValveInspections]" +
                    "IF EXISTS (SELECT 1 FROM sysindexes where name = '_dta_stat_4911089_13_9') DROP STATISTICS [ValveInspections].[_dta_stat_4911089_13_9] " +
                    "IF EXISTS (SELECT 1 FROM sysindexes where name = '_dta_stat_4911089_13_22_9') DROP STATISTICS [ValveInspections].[_dta_stat_4911089_13_22_9]" +
                    "IF EXISTS (SELECT 1 FROM sysindexes where name = '_dta_stat_1031010754_57_61_3') DROP STATISTICS [Valves].[_dta_stat_1031010754_57_61_3] " +
                    "IF EXISTS (SELECT 1 FROM sysindexes where name = '_dta_stat_1031010754_25_57_61_66') DROP STATISTICS [Valves].[_dta_stat_1031010754_25_57_61_66]" +
                    "IF EXISTS (SELECT 1 FROM sysindexes where name = '_dta_stat_1031010754_53_57_61_66') DROP STATISTICS [Valves].[_dta_stat_1031010754_53_57_61_66] " +
                    "IF EXISTS (SELECT 1 FROM sysindexes where name = '_dta_stat_1031010754_65_57_61_66_3') DROP STATISTICS [Valves].[_dta_stat_1031010754_65_57_61_66_3]" +
                    "IF EXISTS (SELECT 1 FROM sysindexes where name = '_dta_stat_1031010754_64_65_25_53_57') DROP STATISTICS [Valves].[_dta_stat_1031010754_64_65_25_53_57]" +
                    "IF EXISTS (SELECT 1 FROM sysindexes where name = '_dta_stat_1031010754_3_64_65_25_57_53') DROP STATISTICS [Valves].[_dta_stat_1031010754_3_64_65_25_57_53]" +
                    "IF EXISTS (SELECT 1 FROM sysindexes where name = '_dta_stat_1031010754_57_61_66_3_25_64') DROP STATISTICS [Valves].[_dta_stat_1031010754_57_61_66_3_25_64]" +
                    "IF EXISTS (SELECT 1 FROM sysindexes where name = '_dta_stat_1031010754_66_64_65_25_57_53_61') DROP STATISTICS [Valves].[_dta_stat_1031010754_66_64_65_25_57_53_61] " +
                    "IF EXISTS (SELECT 1 FROM sysindexes where name = '_dta_stat_1031010754_61_64_65_25_57_53_3') DROP STATISTICS [Valves].[_dta_stat_1031010754_61_64_65_25_57_53_3]" +
                    "IF EXISTS (SELECT 1 FROM sysindexes where name = '_dta_stat_1031010754_57_61_66_3_53_64_65') DROP STATISTICS [Valves].[_dta_stat_1031010754_57_61_66_3_53_64_65]" +
                    "IF EXISTS (SELECT 1 FROM sysindexes where name = '_dta_stat_1031010754_64_57_61_66_3_65_25_53') DROP STATISTICS [Valves].[_dta_stat_1031010754_64_57_61_66_3_65_25_53]";
        }

        #region SPs/Views

        public const string
            SQL_REMOVE_VALVE_SPS_AND_VIEWS = @"
                if exists (select 1 from sysobjects where name = 'selectValveInspectionInfo') drop procedure selectValveInspectionInfo
                if exists (select 1 from sysobjects where name = 'RptInspectionProductivity') drop procedure RptInspectionProductivity
                if exists (select 1 from sysobjects where name = 'StatisticsGeneral') drop procedure StatisticsGeneral
                if exists (select 1 from sysobjects where name = 'getValveInspectionData') drop procedure getValveInspectionData
                if exists (select 1 from sysobjects where name = 'getValveInspectionData2') drop procedure getValveInspectionData2
                if exists (select 1 from sysobjects where name = 'MapLayerComplaintsRecentValveActivity') drop procedure MapLayerComplaintsRecentValveActivity
                if exists (select 1 from sysobjects where name = 'MapLayerValveInspections') drop procedure MapLayerValveInspections
                if exists (select 1 from sysobjects where name = 'rptBlowOffInspectionsByTown') drop VIEW rptBlowOffInspectionsByTown
                if exists (select 1 from sysobjects where name = 'RptBPUValveCounts') drop procedure RptBPUValveCounts
                if exists (select 1 from sysobjects where name = 'rptHydrantInspectionsByTown') drop view rptHydrantInspectionsByTown
                if exists (select 1 from sysobjects where name = 'RptHydrantLog') drop procedure RptHydrantLog
                if exists (select 1 from sysobjects where name = 'RptHydrantLog') drop procedure RptHydrantLog
                if exists (select 1 from sysobjects where name = 'RptValveInspections') drop procedure RptValveInspections
                if exists (select 1 from sysobjects where name = 'rptValveInspections2') drop procedure rptValveInspections2
                if exists (select 1 from sysobjects where name = 'rptValveInspectionsByTown') drop view rptValveInspectionsByTown
                if exists (select 1 from sysobjects where name = 'RptValveInspectionsOperated') drop procedure RptValveInspectionsOperated
                if exists (select 1 from sysobjects where name = 'RptValveInspectionsReqOperated') drop procedure RptValveInspectionsReqOperated
                if exists (select 1 from sysobjects where name = 'RptValveLog') drop procedure RptValveLog
                if exists (select 1 from sysobjects where name = 'selectHydrantInspectionInfo') drop procedure selectHydrantInspectionInfo
                if exists (select 1 from sysobjects where name = 'sp_AddHydInspData') drop procedure sp_AddHydInspData
                if exists (select 1 from sysobjects where name = 'sp_AddValNew') drop procedure sp_AddValNew
                if exists (select 1 from sysobjects where name = 'sp_UD_ValCtrl') drop procedure sp_UD_ValCtrl
                if exists (select 1 from sysobjects where name = 'sp_UD_ValveRoute') drop procedure sp_UD_ValveRoute
                if exists (select 1 from sysobjects where name = 'sp_UD_VCrossSt') drop procedure sp_UD_VCrossSt
                if exists (select 1 from sysobjects where name = 'sp_UD_VDateInst') drop procedure sp_UD_VDateInst
                if exists (select 1 from sysobjects where name = 'sp_UD_VLateral') drop procedure sp_UD_VLateral
                if exists (select 1 from sysobjects where name = 'sp_UD_VMain') drop procedure sp_UD_VMain
                if exists (select 1 from sysobjects where name = 'sp_UD_VNorPos') drop procedure sp_UD_VNorPos
                if exists (select 1 from sysobjects where name = 'sp_UD_VOpens') drop procedure sp_UD_VOpens
                if exists (select 1 from sysobjects where name = 'sp_UD_VStatus') drop procedure sp_UD_VStatus
                if exists (select 1 from sysobjects where name = 'sp_UD_VValMake') drop procedure sp_UD_VValMake
                if exists (select 1 from sysobjects where name = 'sp_UD_VValType') drop procedure sp_UD_VValType
                if exists (select 1 from sysobjects where name = 'sp_UpDateValve') drop procedure sp_UpDateValve
                if exists (select 1 from sysobjects where name = 'tblNJAWBOInspDataInfo') drop view tblNJAWBOInspDataInfo
                if exists (select 1 from sysobjects where name = 'tblNJAWValInspDataInfo') drop view tblNJAWValInspDataInfo
                if exists (select 1 from sysobjects where name = 'tblNJAWValvesCounts') drop view tblNJAWValvesCounts
                if exists (select 1 from sysobjects where name = 'viewNJAWBOInspData') drop view viewNJAWBOInspData
                if exists (select 1 from sysobjects where name = 'viewNJAWValInspData') drop view viewNJAWValInspData
                if exists (select 1 from sysobjects where name = 'ViewNJAWValveList') drop view ViewNJAWValveList
                if exists (select 1 from sysobjects where name = 'viewValvesWO') drop view viewValvesWO",
            SQL_RESTORE_VALVE_SPS_AND_VIEWS = @"
CREATE PROCEDURE [dbo].[getValveInspectionData] @opCntr varchar(3), @year int
AS
	Select '<12-inch' as 'Size', 
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 1) as 'Jan',
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 2) as 'Feb',
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 3) as 'Mar',
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 4) as 'Apr',
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 5) as 'May',
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 6) as 'Jun',
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 7) as 'Jul',
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 8) as 'Aug',
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 9) as 'Sep',
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 10) as 'Oct',
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 11) as 'Nov',
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 12) as 'Dec'
		,(SELECT count(*) FROM tblNJAWValves	WHERE UPPER(isNull([ValCtrl],'')) <> 'BLOW OFF WITH FLUSHING' AND UPPER(isNull([ValveStatus],'')) = 'ACTIVE' AND opCntr = @opCntr AND isNull(ValveSize,0) <12.0
		AND YEAR(
			CASE 
							WHEN (isNull(DateInst,'01/01/1900')>COALESCE((Select max(dateInspect) from tblNJAWValInspData where year(dateInspect) < @year AND tblNJAWValInspData.ValNum = tblNJAWValves.ValNum and tblNJAWValInspData.OpCntr = tblNJAWValves.opCntr),'01/01/1900')) 
								THEN isNull(DateInst,'01/01/1900')
							ELSE
								COALESCE((Select max(dateInspect) from tblNJAWValInspData where year(dateInspect) < @year AND tblNJAWValInspData.ValNum = tblNJAWValves.ValNum and tblNJAWValInspData.OpCntr = tblNJAWValves.opCntr),'01/01/1900')		
							END
						) < @Year-3
			AND YEAR(isNull(DateInst,'01/01/1900')) <= @year) as 'All'
	UNION ALL
	Select '>= to 12-inch', 
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 1),
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 2),
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 3),
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 4),
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 5),
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 6),
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 7),
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 8),
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 9),
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 10),
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 11),
		(SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 12)
		,(SELECT count(*) FROM tblNJAWValves	WHERE UPPER(isNull([ValCtrl],'')) <> 'BLOW OFF WITH FLUSHING' AND UPPER(isNull([ValveStatus],'')) = 'ACTIVE' AND opCntr = @opCntr AND isNull(ValveSize,0) >=12.0
		AND YEAR(
			CASE 
							WHEN (isNull(DateInst,'01/01/1900')>COALESCE((Select max(dateInspect) from tblNJAWValInspData where year(dateInspect) < @year AND tblNJAWValInspData.ValNum = tblNJAWValves.ValNum and tblNJAWValInspData.OpCntr = tblNJAWValves.opCntr),'01/01/1900')) 
								THEN isNull(DateInst,'01/01/1900')
							ELSE
								COALESCE((Select max(dateInspect) from tblNJAWValInspData where year(dateInspect) < @year AND tblNJAWValInspData.ValNum = tblNJAWValves.ValNum and tblNJAWValInspData.OpCntr = tblNJAWValves.opCntr),'01/01/1900')		
							END
						) < @Year-1
			AND YEAR(isNull(DateInst,'01/01/1900')) <= @year) as 'All'
GO

CREATE PROCEDURE [dbo].[getValveInspectionData2] @opCntr varchar(3), @year int
AS
--Declare @year int
--Declare @opCntr varchar(3)
--Select @year = 2007
--Select @opCntr = 'NJ7'
Declare @tblTable TABLE(Size varchar(16), Total int, MonthNum int, [Month] varchar(10), [Year] int, reqinsp int)
Declare @tblLowValves Table (ValNum varchar(15), opcntr varchar(10))
Declare @tblHighValves Table (ValNum varchar(15), opcntr varchar(10))
Declare @LowCount int
Declare @HighCount int

Insert Into @tblLowValves
	SELECT ValNum, OpCntr FROM tblNJAWValves	WHERE UPPER(isNull([ValCtrl],'')) <> 'BLOW OFF WITH FLUSHING' AND UPPER(isNull([ValveStatus],'')) = 'ACTIVE' AND opCntr = @opCntr AND isNull(ValveSize,0) <12.0
		AND YEAR(
			CASE 
							WHEN (isNull(DateInst,'01/01/1900')>COALESCE((Select max(dateInspect) from tblNJAWValInspData where UPPER(isNull(Operated,'NO')) = 'YES' AND year(dateInspect) < @year AND tblNJAWValInspData.ValNum = tblNJAWValves.ValNum and tblNJAWValInspData.OpCntr = tblNJAWValves.opCntr),'01/01/1900')) 
								THEN isNull(DateInst,'01/01/1900')
							ELSE
								COALESCE((Select max(dateInspect) from tblNJAWValInspData where UPPER(isNull(Operated,'NO')) = 'YES' AND year(dateInspect) < @year AND tblNJAWValInspData.ValNum = tblNJAWValves.ValNum and tblNJAWValInspData.OpCntr = tblNJAWValves.opCntr),'01/01/1900')		
							END
						) < @Year-3
			AND YEAR(isNull(DateInst,'01/01/1900')) <= @year

Select @LowCount = (Select count(*) from @tblLowValves)

Insert Into @tblHighValves
	SELECT ValNum, OpCntr  FROM tblNJAWValves	WHERE UPPER(isNull([ValCtrl],'')) <> 'BLOW OFF WITH FLUSHING' AND UPPER(isNull([ValveStatus],'')) = 'ACTIVE' AND opCntr = @opCntr AND isNull(ValveSize,0) >=12.0
		AND YEAR(
			CASE 
							WHEN (isNull(DateInst,'01/01/1900')>COALESCE((Select max(dateInspect) from tblNJAWValInspData where UPPER(isNull(Operated,'NO')) = 'YES' AND year(dateInspect) < @year AND tblNJAWValInspData.ValNum = tblNJAWValves.ValNum and tblNJAWValInspData.OpCntr = tblNJAWValves.opCntr),'01/01/1900')) 
								THEN isNull(DateInst,'01/01/1900')
							ELSE
								COALESCE((Select max(dateInspect) from tblNJAWValInspData where UPPER(isNull(Operated,'NO')) = 'YES' AND year(dateInspect) < @year AND tblNJAWValInspData.ValNum = tblNJAWValves.ValNum and tblNJAWValInspData.OpCntr = tblNJAWValves.opCntr),'01/01/1900')		
							END
						) < @Year-1
			AND YEAR(isNull(DateInst,'01/01/1900')) <= @year

Select @HighCount = (Select count(*) from @tblHighValves)

Insert Into @tblTable 
	Select '<12""' as 'Size', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblLowValves) AND CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 1),1, 'Jan', @Year, @LowCount
Insert Into @tblTable 
	Select '<12""' as 'Size', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblLowValves) AND CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 2),2, 'Feb', @Year, @LowCount
Insert Into @tblTable 
	Select '<12""' as 'Size', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblLowValves) AND CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 3),3, 'Mar', @Year, @LowCount
Insert Into @tblTable 
	Select '<12""' as 'Size', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblLowValves) AND CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 4),4, 'Apr', @Year, @LowCount
Insert Into @tblTable 
	Select '<12""' as 'Size', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblLowValves) AND CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 5),5, 'May', @Year, @LowCount
Insert Into @tblTable 
	Select '<12""' as 'Size', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblLowValves) AND CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 6),6, 'Jun', @Year, @LowCount
Insert Into @tblTable 
	Select '<12""' as 'Size', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblLowValves) AND CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 7),7, 'Jul', @Year, @LowCount
Insert Into @tblTable 
	Select '<12""' as 'Size', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblLowValves) AND CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 8),8, 'Aug', @Year, @LowCount
Insert Into @tblTable 
	Select '<12""' as 'Size', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblLowValves) AND CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 9),9, 'Sep', @Year, @LowCount
Insert Into @tblTable 
	Select '<12""' as 'Size', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblLowValves) AND CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 10),10, 'Oct', @Year, @LowCount
Insert Into @tblTable 
	Select '<12""' as 'Size', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblLowValves) AND CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 11),11, 'Nov', @Year, @LowCount
Insert Into @tblTable 
	Select '<12""' as 'Size', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblLowValves) AND CAST([ValveSize] AS FLOAT)< '12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 12),12, 'Dec', @Year, @LowCount

Insert Into @tblTable 
	Select '>= 12""', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblHighValves) AND CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 1),1, 'Jan', @Year, @HighCount
Insert Into @tblTable 
	Select '>= 12""', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblHighValves) AND CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 2),2, 'Feb', @Year, @HighCount
Insert Into @tblTable 
	Select '>= 12""', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblHighValves) AND CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 3),3, 'Mar', @Year, @HighCount
Insert Into @tblTable 
	Select '>= 12""', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblHighValves) AND CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 4),4, 'Apr', @Year, @HighCount
Insert Into @tblTable 
	Select '>= 12""', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblHighValves) AND CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 5),5, 'May', @Year, @HighCount
Insert Into @tblTable 
	Select '>= 12""', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblHighValves) AND CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 6),6, 'Jun', @Year, @HighCount
Insert Into @tblTable 
	Select '>= 12""', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblHighValves) AND CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 7),7, 'Jul', @Year, @HighCount
Insert Into @tblTable 
	Select '>= 12""', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblHighValves) AND CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 8),8, 'Aug', @Year, @HighCount
Insert Into @tblTable 
	Select '>= 12""', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblHighValves) AND CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 9),9, 'Sep', @Year, @HighCount
Insert Into @tblTable 
	Select '>= 12""', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblHighValves) AND CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 10),10, 'Oct', @Year, @HighCount
Insert Into @tblTable 
	Select '>= 12""', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblHighValves) AND CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 11),11, 'Nov', @Year, @HighCount
Insert Into @tblTable 
	Select '>= 12""', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValves.ValNum in (SELECT ValNum from @tblHighValves) AND CAST([ValveSize] AS FLOAT)>='12' AND YEAR([DateInspect]) = @Year	AND [tblNJAWValInspData].[OpCntr] = @opCntr AND MONTH([DateInspect]) = 12),12, 'Dec', @Year, @HighCount

select * from @tblTable
GO

CREATE Procedure [dbo].[MapLayerComplaintsRecentValveActivity] (@swLat decimal(18,10), @neLat decimal(18,10), @swLon decimal(18,10), @neLon decimal(18,10), @range int, @complaintID int)
AS
select top 200 
	tblNJAWValves.recID, lat as Lat, lon as Lon, 'addData', 'icon15', tblNJAWValves.recID as description
    from tblNJAWValves, tblNJAWValInspData
    where 
				tblNJAWValves.ValNum = tblNJAWValInspData.ValNum 
				and tblNJAWValves.OpCntr = tblNJAWValInspData.OpCntr 
				and DateDiff(HH, DateInspect, (select dateAdd(DD, 1, date_complaint_received) from tblWQ_Complaints where complaint_number = @complaintID)) between 0 and @range 
				and Lat is not null 
        and lon is not null
        and cast(lat as decimal(18,10)) > @swLat
        and cast(lat as decimal(18,10)) < @neLat
        and cast(lon as decimal(18,10)) > @swLon
        and cast(lon as decimal(18,10)) < @neLon
order by tblNJAWValInspData.dateinspect desc

GO

CREATE Procedure [dbo].[MapLayerValveInspections] (@swLat decimal(18,10), @neLat decimal(18,10), @swLon decimal(18,10), @neLon decimal(18,10), @start datetime, @end datetime)
AS
select top 200 
	tblNJAWValves.recID, lat as Lat, lon as Lon, 'addData', 'icon15', tblNJAWValves.recID as description
from 
	tblNJAWValves, tblNJAWValInspData
where 
	tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.OpCntr = tblNJAWValInspData.OpCntr 
	and DateInspect between @start and @end
	and Lat is not null 
    and lon is not null
    and cast(lat as decimal(18,10)) > @swLat
    and cast(lat as decimal(18,10)) < @neLat
    and cast(lon as decimal(18,10)) > @swLon
    and cast(lon as decimal(18,10)) < @neLon
order by tblNJAWValInspData.dateinspect desc
GO

CREATE VIEW [dbo].[rptBlowOffInspectionsByTown]
AS
SELECT opCntr, (select #1.town from Towns #1 where #1.TownID = tblNJAWValves.town) as [twn], count(town) as [due] FROM tblNJAWValves 
	WHERE OpCntr = 'NJ7'  AND 
	DBO.RequiresInspectionBlowOff(ValCtrl, ValveStatus, getDate(), dateInst, (Select max(dateInspect) from tblNJAWHydInspData where tblNJAWHydInspData.HydNum = tblNJAWValves.ValNum and tblNJAWHydInspData.OpCntr = tblNJAWValves.opCntr and isNull(Inspect,'') in ('Inspect','INSPECT/FLUSH')), InspFreq, InspFreqUnit) = 1 AND UPPER(RTRIM(IsNull([ValCtrl],''))) = 'BLOW OFF WITH FLUSHING'  AND UPPER(RTRIM(IsNull(ValveStatus, ''))) = 'ACTIVE' 
	GROUP BY opCntr, town	

GO

CREATE Procedure [dbo].[RptBPUValveCounts] (@opCntr varchar(10), @town int)
AS
SELECT OpCntr, BillInfo, ValveStatus, Case When (cast(ValveSize as float) < 12) then '<12' else '>=12' end as [SizeRange], count(*) as [Count]
	FROM tblNJAWValves 
	WHERE Left(isNull(OpCntr, ''), 2) in ('NJ','EW','LW','NY') 
		AND upper(isNull(ValveStatus,'')) = 'ACTIVE'
		AND town = isNull(@town, town)
		AND opCntr = isNull(@opCntr, opCntr)
	group by OpCntr, BillInfo, ValveStatus, Case When (cast(ValveSize as float) < 12) then '<12' else '>=12' end
	order by OpCntr, BillInfo, ValveStatus, Case When (cast(ValveSize as float) < 12) then '<12' else '>=12' end
GO

--Number of Hydrants per town requiring inspection
CREATE VIEW [dbo].[rptHydrantInspectionsByTown]
AS
SELECT opCntr, (select #1.town from Towns #1 where #1.TownID = tblNJAWHydrant.town) as [twn], town, count(town) as [due] FROM tblNJAWHydrant 
WHERE 
	dbo.RequiresInspectionHydrant(
		tblNJAWHydrant.Town,
		ActRet, 
		BillInfo, 
		getDate(), 
		dateInst, 
		(Select max(dateInspect) from tblNJAWHydInspData where tblNJAWHydInspData.HydNum = tblNJAWHydrant.HydNum and tblNJAWHydInspData.OpCntr = tblNJAWHydrant.opCntr and isNull(Inspect,'') in ('Inspect','INSPECT/FLUSH','FLUSH')),
		InspFreq, 
		InspFreqUnit
	) = 1 
	AND UPPER(RTRIM(IsNull(ActRet, ''))) = 'ACTIVE' 
	AND UPPER(RTRIM(IsNull(BillInfo, ''))) = 'PUBLIC' 
GROUP BY opCntr, town
GO

                                        /****** Object:  StoredProcedure [dbo].[RptHydrantLog]    Script Date: 05/31/2011 11:27:29 ******/
                                         CREATE Procedure [dbo].[RptHydrantLog] (@opCntr varchar(4), @town int)
                                         AS
                                         IF (@town = 0 OR @town is null)
                                          BEGIN
                                           Select 
                                            tblNJAWHydrant.RecID, tblNJAWHydrant.OpCntr, 
                                            Towns.Town, tblNJAWHydrant.HydNum, 
                                            S.FullStName, tblNJAWHydrant.StName, 
                                            tblNJAWHydrant.Location, tblNJAWHydrant.CrossStreet, 
                                            tblNJAWHydrant.HydSize, M.Name as Manufacturer, 
                                            tblNJAWHydrant.SizeofMain, ValNum as LatValNum, 
                                            tblNJAWHydrant.WONum, 
                                            UPPER(tblNJAWHydrant.ActRet) as ActRet, 
                                            tblNJAWValves.RecID as 'ValRecID',
                                            Upper(tblNJAWHydrant.BillInfo) as 'BillInfo',
                                            IsNull((Select District_Name from FireDistrict where FireDistrict.FireDistrictID = tblNJAWHydrant.FireDistrictID), 'No fire district.') as [FireDistrict], 
                                            LatSize, 
                                            tblNJAWHydrant.Lat, 
                                            tblNJAWHydrant.Lon
                                           from tblNJAWHydrant
                                           LEFT JOIN Towns on Towns.TownID = tblNJAWHydrant.Town
                                           LEFT JOIN Streets S on S.StreetID = tblNJAWHydrant.StName
                                           LEFT JOIN tblNJAWValves on tblNJAWValves.RecID = tblNJAWHydrant.LatValNum
                                           LEFT JOIN Manufacturers M on M.ManufacturerID = tblNJAWHydrant.ManufacturerID
                                           where tblNJAWHydrant.OpCntr = @opCntr
                                           order by Towns.Town, tblNJAWHydrant.hydsuf
                                          END
                                         ELSE
                                          BEGIN
                                           Select 
                                            tblNJAWHydrant.RecID, tblNJAWHydrant.OpCntr, 
                                            Towns.Town, tblNJAWHydrant.HydNum, 
                                            S.FullStName, tblNJAWHydrant.StName, 
                                            tblNJAWHydrant.Location, tblNJAWHydrant.CrossStreet, 
                                            tblNJAWHydrant.HydSize, M.Name as Manufacturer, 
                                            tblNJAWHydrant.SizeofMain, ValNum as LatValNum, 
                                            tblNJAWHydrant.WONum, 
                                            UPPER(tblNJAWHydrant.ActRet) as ActRet, 
                                            tblNJAWValves.RecID as 'ValRecID',
                                            Upper(tblNJAWHydrant.BillInfo) as 'BillInfo', 
                                            IsNull((Select District_Name from FireDistrict where FireDistrict.FireDistrictID = tblNJAWHydrant.FireDistrictID), 'No fire district.') as [FireDistrict], 
                                            LatSize, 
                                            tblNJAWHydrant.Lat, 
                                            tblNJAWHydrant.Lon
                                           from tblNJAWHydrant
                                           LEFT JOIN Towns on Towns.TownID = tblNJAWHydrant.Town
                                           LEFT JOIN Streets S on S.StreetID = tblNJAWHydrant.StName
                                           LEFT JOIN tblNJAWValves on tblNJAWValves.RecID = tblNJAWHydrant.LatValNum 
                                           LEFT JOIN Manufacturers M on M.ManufacturerID = tblNJAWHydrant.ManufacturerID
                                           where tblNJAWHydrant.OpCntr = @opCntr and tblnjawhydrant.town = @town
                                           order by Towns.Town, tblNJAWHydrant.hydsuf
                                          END
GO
CREATE PROCEDURE [dbo].[RptInspectionProductivity] (@DateInspStart datetime, @DateInspEnd datetime, @inspector varchar(50), @opCntr varchar(10))
AS
Select @DateInspEnd = DateAdd(day, 1, @DateInspEnd)

Declare @table TABLE(inspName varchar(50), type varchar(10), size varchar(10), action varchar(20), inspDate varchar(10), inspCount int, opCntr varchar(10))

if (len(@inspector)>0)
	BEGIN
		Insert Into @Table
			select COALESCE(fullname, inspectedBy + '*'), 'BlowOff' as Type, '' as Size, Upper(Inspect), convert(varchar(10), DateInspect,101), Count(inspectedBy), tblNJAWHydInspData.opCntr
				from tblNJAWHydInspData LEFT JOIN tblPermissions on tblPermissions.username = inspectedBy
				where DateInspect >= @DateInspStart and DateInspect < @DateInspEnd and left(HydNum, 1) = 'V' and InspectedBy = @inspector
				group by COALESCE(fullname, inspectedBy + '*'), convert(varchar(10), DateInspect,101), Upper(Inspect), tblNJAWHydInspData.opCntr
				order by convert(varchar(10), DateInspect,101), Upper(Inspect)

		Insert Into @Table
			select COALESCE(fullname, inspectedBy + '*'), 'Hydrant' as Type, '' as Size, Upper(Inspect), convert(varchar(10), DateInspect,101), Count(inspectedBy), tblNJAWHydInspData.opCntr
				from tblNJAWHydInspData LEFT JOIN tblPermissions on tblPermissions.username = inspectedBy
				where DateInspect >= @DateInspStart and DateInspect < @DateInspEnd and left(HydNum, 1) = 'H' and InspectedBy = @inspector
				group by COALESCE(fullname, inspectedBy + '*'), convert(varchar(10), DateInspect,101), Upper(Inspect), tblNJAWHydInspData.opCntr
				order by convert(varchar(10), DateInspect,101), Upper(Inspect)

		Insert Into @Table
			select COALESCE(fullname, inspectBy + '*'), 'Valve' as Type, '<12' as Size, Upper(Operated), convert(varchar(10), DateInspect,101), Count(inspectBy), tblNJAWValInspData.opCntr
				from tblNJAWValInspData LEFT JOIN tblPermissions on tblPermissions.username = inspectBy
				Left Join tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum AND tblNJAWValves.OpCntr = tblNJAWValInspData.opCntr
				where DateInspect >= @DateInspStart and DateInspect < @DateInspEnd and InspectBy = @inspector
				and cast(ValveSize as float)< 12.0
				group by COALESCE(fullname, inspectBy + '*'), convert(varchar(10), DateInspect,101), Upper(Operated), tblNJAWValInspData.opCntr
				order by convert(varchar(10), DateInspect,101), Upper(Operated)

		Insert Into @Table
			select COALESCE(fullname, inspectBy + '*'), 'Valve' as Type, '>=12' as Size, Upper(Operated), convert(varchar(10), DateInspect,101), Count(inspectBy), tblNJAWValInspData.opCntr
				from tblNJAWValInspData LEFT JOIN tblPermissions on tblPermissions.username = inspectBy
				Left Join tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum AND tblNJAWValves.OpCntr = tblNJAWValInspData.opCntr
				where DateInspect >= @DateInspStart and DateInspect < @DateInspEnd and InspectBy = @inspector
				and cast(ValveSize as float)>= 12.0
				group by COALESCE(fullname, inspectBy + '*'), convert(varchar(10), DateInspect,101), Upper(Operated), tblNJAWValInspData.opCntr
				order by convert(varchar(10), DateInspect,101), Upper(Operated)
	END

ELSE
	BEGIN
		Insert Into @Table
			select COALESCE(fullname, inspectedBy + '*'), 'BlowOff' as Type, '' as Size, Upper(Inspect), convert(varchar(10), DateInspect,101), Count(inspectedBy), tblNJAWHydInspData.opCntr
				from tblNJAWHydInspData LEFT JOIN tblPermissions on tblPermissions.username = inspectedBy
				where DateInspect >= @DateInspStart and DateInspect < @DateInspEnd and left(HydNum, 1) = 'V' 
				group by COALESCE(fullname, inspectedBy + '*'), convert(varchar(10), DateInspect,101), Upper(Inspect), tblNJAWHydInspData.opCntr
				order by convert(varchar(10), DateInspect,101), Upper(Inspect)

		Insert Into @Table
			select COALESCE(fullname, inspectedBy + '*'), 'Hydrant' as Type, '' as Size, Upper(Inspect), convert(varchar(10), DateInspect,101), Count(inspectedBy), tblNJAWHydInspData.opCntr
				from tblNJAWHydInspData LEFT JOIN tblPermissions on tblPermissions.username = inspectedBy
				where DateInspect >= @DateInspStart and DateInspect < @DateInspEnd and left(HydNum, 1) = 'H' 
				group by COALESCE(fullname, inspectedBy + '*'), convert(varchar(10), DateInspect,101), Upper(Inspect), tblNJAWHydInspData.opCntr
				order by convert(varchar(10), DateInspect,101), Upper(Inspect)

		Insert Into @Table
			select COALESCE(fullname, inspectBy + '*'), 'Valve' as Type, '<12' as Size, Upper(Operated), convert(varchar(10), DateInspect,101), Count(inspectBy), tblNJAWValInspData.opCntr
				from tblNJAWValInspData LEFT JOIN tblPermissions on tblPermissions.username = inspectBy
				Left Join tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum AND tblNJAWValves.OpCntr = tblNJAWValInspData.opCntr
				where DateInspect >= @DateInspStart and DateInspect < @DateInspEnd 
				and cast(ValveSize as float)< 12.0
				group by COALESCE(fullname, inspectBy + '*'), convert(varchar(10), DateInspect,101), Upper(Operated), tblNJAWValInspData.opCntr
				order by convert(varchar(10), DateInspect,101), Upper(Operated)

		Insert Into @Table
			select COALESCE(fullname, inspectBy + '*'), 'Valve' as Type, '>=12' as Size, Upper(Operated), convert(varchar(10), DateInspect,101), Count(inspectBy), tblNJAWValInspData.opCntr
				from tblNJAWValInspData LEFT JOIN tblPermissions on tblPermissions.username = inspectBy
				Left Join tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum AND tblNJAWValves.OpCntr = tblNJAWValInspData.opCntr
				where DateInspect >= @DateInspStart and DateInspect < @DateInspEnd 
				and cast(ValveSize as float)>= 12.0
				group by COALESCE(fullname, inspectBy + '*'), convert(varchar(10), DateInspect,101), Upper(Operated), tblNJAWValInspData.opCntr
				order by convert(varchar(10), DateInspect,101), Upper(Operated)

	END	 

IF (Len(@OpCntr) > 0)
	BEGIN
		SELECT * FROM @table where opCntr = @opCntr order by InspDate
	END
ELSE
	BEGIN
		SELECT * FROM @table order by InspDate, opCntr
	END
GO
CREATE PROCEDURE [dbo].[RptValveInspections] @opCntr varchar(3), @year int
AS
--Declare @year int
--Declare @opCntr varchar(3)
--Select @year = 2011
--Select @opCntr = 'EW1'
DECLARE @tblValvesToBeInspected TABLE (Size varchar(16), ValNum varchar(15), opcntr varchar(10), valveID int)
DECLARE @LowCount varchar(55)
DECLARE @HighCount varchar(55)
DECLARE @LowTotalValves varchar(55)
DECLARE @HighTotalValves varchar(55)
--PLACE ALL THE VALVES TO BE INSPECTED IN TEMPORARY TABLE
Insert Into @tblValvesToBeInspected
	SELECT 
		CASE WHEN (Cast(ValveSize as float) >= 12.0) THEN '>= 12""' ELSE '<12""' END, ValNum, OpCntr, RecID
		FROM tblnjawValves 
			where OpCntr = @opCntr
			AND 
				DBO.RequiresInspectionValveByZone( ValveSize, ValCtrl, ValveStatus, BPUKPI, BillInfo, 
				(Select max(dateInspect) from tblNJAWValInspData where UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValInspData.valveID = tblNJAWValves.recID and year(dateInspect) < @year)
				, DateInst, '12/31/' + Cast(@year as varchar(4)), ValveZone, tblNJAWValves.Town
				) = 1
			ORDER BY 1
Select @LowCount = (Select count(1) from @tblValvesToBeInspected where size = '<12""')
Select @HighCount = (Select count(1) from @tblValvesToBeInspected where size = '>= 12""')
Select @LowTotalValves = (Select count(1) from tblNJAWValves where opCntr = @opCntr AND Cast(IsNull(ValveSize,0) as float) < 12.0 and upper(isNull(ValveStatus,'')) = 'ACTIVE' AND isNull(Valctrl,'') <> 'BLOW OFF WITH FLUSHING' AND not (valctrl = 'BLOW OFF' and cast(valvesize as float) < 2.0))
Select @HighTotalValves = (Select count(1) from tblNJAWValves where opCntr = @opCntr AND Cast(IsNull(ValveSize,0) as float) >= 12.0 and upper(isNull(ValveStatus,'')) = 'ACTIVE'  AND isNull(Valctrl,'') <> 'BLOW OFF WITH FLUSHING')
DECLARE @table TABLE(Size varchar(16), valveID int, [month] int)
/* 
	we don't want to count multiple inspections twice 
	store the latest in a temp table
*/
insert into @table
select 
	Distinct
	CASE WHEN (Cast(V.ValveSize as float) >= 12.0) THEN '>= 12""' ELSE '<12""' END, 
	V.RecID, 
	month(max(I.DateInspect))
from
	tblNJAWValves V
inner join
	tblNJAWValInspData I
ON
	I.valveID = V.recID
WHERE
	I.DateInspect is not null
AND
	V.RecID in (select ValveID from @tblValvesToBeInspected where ValveID is not null)
AND
	Year(I.DateInspect) = @Year
AND
	UPPER(isNull(I.Operated,'NO')) = 'YES' 
AND
	V.OpCntr = @OpCntr
GROUP BY 
	V.RecID, V.ValveSize
ORDER BY
	3 desc
	
select 
	Size, 
	Count(1) as Total,
	[Month] as MonthNum,
	left(datename(month, cast([month] as varchar(2)) + '/01/2000'),3) as [Month],
	@Year as [Year],
	CASE WHEN Size = '>= 12""' THEN @HighCount ELSE @LowCount END as reqinsp,
	CASE WHEN Size = '>= 12""' THEN @HighTotalValves ELSE @LowTotalValves END as totalValves
from 
	@table
group by
	Size, [month]
order by
	Size, [Month]

GO

CREATE Procedure [dbo].[rptValveInspections2] (@opCntr varchar(16), @town int, @startDate datetime, @endDate datetime)
AS
--exec [rptValveInspections2] 'NJ7', null, '01/01/2006', '02/01/2006'
SELECT 
	tblNJAWValves.OpCntr, 
	(Select Towns.Town from Towns where Towns.TownID = tblnJAWValves.town) as Town,
	tblNJAWValves.ValNum as [Valve #], 
	tblNJAWValves.Lat as [Latitude], 
	tblNJAWValves.Lon as [Longitude], 
	tblNJAWValInspData.DateInspect as [Inspected On], 
	tblNJAWValInspData.Operated, 
	tblNJAWValInspData.PosFound as [Position Found], 
	tblNJAWValInspData.PosLeft as [Position Left], 
	tblNJAWValInspData.Turns, 
	tblNJAWValInspData.Remarks
FROM 
	tblNJAWValves
LEFT JOIN 
	tblNJAWValInspData 
ON 
	tblNJAWValInspData.valNum = tblNJAWValves.valNum
	AND
	tblNJAWValInspData.opCntr = tblNJAWValves.opCntr
WHERE 
			tblNJAWValves.opCntr = isNull(@opCntr, tblNJAWValves.opCntr)
			and tblNJAWValves.Town = isNull(@town, tblNJAWValves.Town)
			and tblNJAWValInspData.dateInspect > @startDate
			and tblNJAWValInspData.dateInspect < @endDate
			AND upper(tblNJAWValInspData.Operated) = 'YES'

GO

CREATE VIEW 
	[dbo].[rptValveInspectionsByTown]
AS
SELECT 
	opCntr,
	(select #1.town from Towns #1 where #1.TownID = tblNJAWValves.town) as [twn],
	count(tblNJAWValves.town) as [due],
	tblNJAWValves.town,
	case when (valvesize>=12.0) then '>=12' else '<12' end as size
FROM 
	tblNJAWValves 
LEFT JOIN 
	Streets S ON S.StreetID = tblNJAWValves.StName 
WHERE 
	dbo.RequiresInspectionValveByZone(ValveSize, ValCtrl, ValveStatus, BPUKPI, BillInfo, (Select max(dateInspect) from tblNJAWValInspData where UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValInspData.ValNum = tblNJAWValves.ValNum and tblNJAWValInspData.OpCntr = tblNJAWValves.opCntr) , DateInst, GetDate(), ValveZone, tblNJAWValves.Town) = 1 
GROUP BY
	OpCntr, tblNJAWValves.town, case when (valvesize>=12.0) then '>=12' else '<12' end

GO

CREATE PROCEDURE [dbo].[RptValveInspectionsOperated] @opCntr varchar(3), @year int
AS

DECLARE @tblTable TABLE(Size varchar(16), Operated varchar(3), Total int, MonthNum int, [Month] varchar(10), [Year] int)

Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  1),1, 'Jan', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  2),2, 'Feb', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  3),3, 'Mar', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  4),4, 'Apr', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  5),5, 'May', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  6),6, 'Jun', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  7),7, 'Jul', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  8),8, 'Aug', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  9),9, 'Sep', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  10),10, 'Oct', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  11),11, 'Nov', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  12),12, 'Dec', @Year

Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  1),1, 'Jan', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  2),2, 'Feb', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  3),3, 'Mar', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  4),4, 'Apr', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  5),5, 'May', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  6),6, 'Jun', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  7),7, 'Jul', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  8),8, 'Aug', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  9),9, 'Sep', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  10),10, 'Oct', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  11),11, 'Nov', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  12),12, 'Dec', @Year

Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 1),1, 'Jan', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 2),2, 'Feb', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 3),3, 'Mar', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 4),4, 'Apr', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 5),5, 'May', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 6),6, 'Jun', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 7),7, 'Jul', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 8),8, 'Aug', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 9),9, 'Sep', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 10),10, 'Oct', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 11),11, 'Nov', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 12),12, 'Dec', @Year

Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 1),1, 'Jan', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 2),2, 'Feb', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 3),3, 'Mar', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 4),4, 'Apr', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 5),5, 'May', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 6),6, 'Jun', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 7),7, 'Jul', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 8),8, 'Aug', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 9),9, 'Sep', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 10),10, 'Oct', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 11),11, 'Nov', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 12),12, 'Dec', @Year

SELECT * from @tblTable

GO

CREATE PROCEDURE [dbo].[RptValveInspectionsReqOperated] @opCntr varchar(3), @year int
AS
DECLARE @tblTable TABLE(Size varchar(16), Operated varchar(3), Total int, MonthNum int, [Month] varchar(10), [Year] int)
DECLARE @tblValvesToBeInspected TABLE (Size varchar(16), ValNum varchar(15), opcntr varchar(10), recID int)
DECLARE @LowCount INT
DECLARE @HighCount INT
--PLACE ALL THE VALVES TO BE INSPECTED IN TEMPORARY TABLE
Insert Into @tblValvesToBeInspected
	SELECT 
		CASE WHEN (Cast(ValveSize as float) >= 12.0) THEN '>= 12""' ELSE '<12""' END, ValNum, OpCntr, RecID
		FROM tblnjawValves 
			where OpCntr = @opCntr
			AND 
				DBO.RequiresInspectionValveByZone( ValveSize, ValCtrl, ValveStatus, BPUKPI, BillInfo, 
				(Select max(dateInspect) from tblNJAWValInspData where UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValInspData.ValNum = tblNJAWValves.ValNum and tblNJAWValInspData.OpCntr = tblNJAWValves.opCntr and year(dateInspect) < @year)
				, DateInst, '12/31/' + Cast(@year as varchar(4)), ValveZone, tblNJAWValves.Town
				) = 1
			ORDER BY 1
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  1),1, 'Jan', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  2),2, 'Feb', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  3),3, 'Mar', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  4),4, 'Apr', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  5),5, 'May', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  6),6, 'Jun', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  7),7, 'Jul', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  8),8, 'Aug', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  9),9, 'Sep', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  10),10, 'Oct', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  11),11, 'Nov', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  12),12, 'Dec', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  1),1, 'Jan', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  2),2, 'Feb', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  3),3, 'Mar', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  4),4, 'Apr', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  5),5, 'May', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  6),6, 'Jun', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  7),7, 'Jul', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  8),8, 'Aug', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  9),9, 'Sep', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  10),10, 'Oct', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  11),11, 'Nov', @Year
Insert Into @tblTable 
	Select '<12""' as 'Size', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  12),12, 'Dec', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 1),1, 'Jan', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 2),2, 'Feb', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 3),3, 'Mar', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 4),4, 'Apr', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 5),5, 'May', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 6),6, 'Jun', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 7),7, 'Jul', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 8),8, 'Aug', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 9),9, 'Sep', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 10),10, 'Oct', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 11),11, 'Nov', @Year
Insert Into @tblTable 
	Select '>= 12""', 'YES', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'YES' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 12),12, 'Dec', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 1),1, 'Jan', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 2),2, 'Feb', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 3),3, 'Mar', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 4),4, 'Apr', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 5),5, 'May', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 6),6, 'Jun', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 7),7, 'Jul', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 8),8, 'Aug', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 9),9, 'Sep', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 10),10, 'Oct', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 11),11, 'Nov', @Year
Insert Into @tblTable 
	Select '>= 12""', 'NO', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,'NO')) = 'NO' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 12),12, 'Dec', @Year
SELECT * from @tblTable
GO

CREATE PROCEDURE [dbo].[RptValveLog] (@Town int) AS

Declare @townName as varchar(55)
select @townName = (Select town from Towns where TownID = @town)

SELECT 
	valnum, valsuf, #st.streetPrefix, #st.streetName, #st.streetsuffix, 
	crossstreet,valLoc, valMake, valvesize, valType, 
	valctrl, opens, turns, mappage, woNum, 
	CASE CONVERT(VARCHAR(10), dateinst, 101) WHEN '01/01/1900' THEN '' WHEN '01/01/0001' THEN '' ELSE CONVERT(VARCHAR(10), dateinst, 101) END AS [dtInst], 
	CASE CONVERT(VARCHAR(10), dateRetired, 101) WHEN '01/01/1900' THEN '' WHEN '01/01/0001' THEN '' ELSE CONVERT(VARCHAR(10), dateRetired, 101) END AS [dtRetired], 
	remarks, @townName as 'Town', ValveStatus, NorPos,
	TypeMain, Lat, Lon
	FROM tblNJAWValves
	LEFT JOIN Streets #st on #st.StreetID = tblNJAWValves.stName
	WHERE tblNJAWValves.[Town] = @Town
	order by valsuf
GO

CREATE Procedure [dbo].[selectHydrantInspectionInfo] (@pid varchar(50), @opCntr varchar(50), @username varchar(100))
AS
IF (CharIndex('-', @pid) > 0)
	BEGIN
		IF (left(@pid, 1) = 'V')
			BEGIN
				Select top 1 
					tblNJAWValves.Valnum as 'HydNum'
					, tblNJAWValves.valLoc as 'Location'
					, tblNJAWValves.ValSuf as 'HydSuf'
					, (Select top 1 EmpNum from tblPermissions where username = @username) as InspectorNum
					, tblNJAWValves.Critical, tblNJAWValves.CriticalNotes
					, tblNJAWValves.recID
					, '' as ManufacturerID
					, '' as [MyHydMake]
					, '' as [DirOpen]
					, '' as [YearManufactured]
					, '' as [ManufacturedUpdated]
					from tblNJAWValves 
					where valnum = @pid AND opCntr = @opcntr				
			END
		ELSE
			BEGIN
				Select top 1 
					tblNJAWHydrant.hydnum 
					, tblNJAWHydrant.Location
					, tblNJAWHydrant.HydSuf
					, (Select top 1 EmpNum from tblPermissions where username = @username) as InspectorNum
					, tblNJAWHydrant.Critical, tblNJAWHydrant.CriticalNotes
					, tblNJAWHydrant.recID
					, ManufacturerID
					, [DirOpen]
					, [YearManufactured] as [YearManufactured]
					, [ManufacturedUpdated]
				from tblNJAWHydrant 
					where hydnum = @pid AND opCntr = @opcntr
			END
	END	
ELSE

BEGIN
	Select top 1 
		tblNJAWHydrant.Location
		,tblNJAWHydInspData.recID		
		,tblNJAWHydInspData.[Chlorine]
		,cast(tblNJAWHydInspData.[Remarks] as varchar(4000)) as Remarks
		,tblNJAWHydInspData.[DateAdded]
		,tblNJAWHydInspData.[DateInspect]
		,tblNJAWHydInspData.[FullFlow]
		,tblNJAWHydInspData.[GalFlow]
		,tblNJAWHydInspData.[GPM]
		,tblNJAWHydInspData.[HydNum]
		,tblNJAWHydInspData.[HydSuf]
		,upper(tblNJAWHydInspData.[Inspect]) as Inspect
		,tblNJAWHydInspData.[InspectedBy]
		,tblNJAWHydInspData.[InspectorNum]
		,tblNJAWHydInspData.[MinFlow]
		,tblNJAWHydInspData.[OpCntr]
		,tblNJAWHydInspData.[PressStatic]
		,tblNJAWHydInspData.[RecID]
		,upper(tblNJAWHydInspData.[WOReq1]) as WOReq1
		,upper(tblNJAWHydInspData.[WOReq2]) as WOReq2
		,upper(tblNJAWHydInspData.[WOReq3]) as WOReq3
		,upper(tblNJAWHydInspData.[WOReq4]) as WOReq4
		,tblNJAWHydInspData.[SizeOpening]
		,'' as AddRemarks
		,tblNJAWHydrant.LatValNum
		,tblNJAWHydInspData.DateAdded
		,tblNJAWHydrant.Critical
		,tblNJAWHydrant.CriticalNotes
		,hp.Description as HydrantProblem, tblNJAWHydInspData.HydrantProblemID
		,hts.HydrantTagStatusID, hts.Description as HydrantTagStatus, ManufacturerID
	FROM tblNJAWHydInspData 
		left join tblNJAWHydrant on tblNJAWHydInspData.hydnum = tblNJAWHydrant.hydnum AND tblNJAWHydInspData.opCntr = tblNJAWHydrant.opCntr
		left join HydrantProblems hp on hp.HydrantProblemID = tblNJAWHydInspData.HydrantProblemID
		left join HydrantTagStatuses hts on hts.HydrantTagStatusID = tblNJAWHydInspData.HydrantTagStatusID
	where tblNJAWHydInspData.recID = @pid
END
GO

CREATE Procedure [dbo].[selectValveInspectionInfo] (@pid varchar(50), @opCntr varchar(50), @username varchar(100))
AS
IF (CharIndex('-', @pid) > 0)
BEGIN
	Select top 1 
		tblNJAWValves.valnum,
		tblNJAWValves.valloc, 
		tblNJAWValves.Opens, 
		tblNJAWValves.NorPos, 
		tblNJAWValves.Turns,		
		CASE WHEN (isNull(tblNJAWValves.turns,0) >1.0) then CEILING(isNull(tblNJAWValves.turns,0)*.2) else tblNJAWValves.turns end as 'MinTurns',
		(Select top 1 empNum from tblPermissions where username=@username) as 'InspectorNum',
		(Select top 1 town from tblNJAWValves where valNum=@pid and opCntr = @opCntr and town<>0) as 'Town'
		, tblNJAWValves.critical, tblNJAWValves.criticalNotes
		from tblNJAWValves 
		where valNum = @pid AND opCntr = @opcntr
END
	
ELSE

BEGIN
	Select top 1 
		tblNJAWValInspData.recID,		
		tblNJAWValves.valnum,
		tblNJAWValves.valloc,
		tblNJAWValInspData.DateInspect, 
		UPPER(tblNJAWValInspData.Operated) as 'Operated',
		UPPER(tblNJAWValInspData.PosFound) as 'PosFound', 
		UPPER(tblNJAWValInspData.PosLeft) as 'PosLeft', 
		tblNJAWValInspData.Turns as 'TurnsCompleted',
		tblNJAWValInspData.TurnsNotCompleted, 
		UPPER(tblNJAWValInspData.WOReq1) as 'WOReq1', 
		UPPER(tblNJAWValInspData.WOReq2) as 'WOReq2',
		UPPER(tblNJAWValInspData.WOReq3) as 'WOReq3',
		tblNJAWValInspData.Inaccessible,
		tblNJAWValInspData.Remarks,
		tblNJAWValInspData.InspectBy, tblNJAWValInspData.InspectorNum,
		tblNJAWValves.Opens, 
		tblNJAWValves.NorPos, 
		tblNJAWValves.Turns,		
		CASE WHEN (isNull(tblNJAWValves.turns,0) >1.0) then CEILING(isNull(tblNJAWValves.turns,0)*.2) else tblNJAWValves.turns end as 'MinTurns',
		tblNJAWValInspData.DateAdded
		, tblNJAWValves.critical, tblNJAWValves.criticalnotes
		FROM tblNJAWValInspData 
		left join tblNJAWValves on tblNJAWValInspData.valNum = tblNJAWValves.valNum AND tblNJAWValInspData.opCntr = tblNJAWValves.opCntr
		where tblNJAWValInspData.recID = @pid
END

GO

CREATE PROCEDURE [dbo].[sp_AddHydInspData] 

@Chlorine varchar(10),
@DateInspect varchar(25),
@FullFlow varchar(2),
@FullName varchar(25),
@GPM varchar(6),
@HydNum varchar(10),
@HydSuf varchar(7),
@Inspect varchar(20),
@MinFlow varchar(5),
@OpCntr varchar(4),
@PressStatic varchar(10),
@Remarks varchar(2000),
@WOReq1 varchar(25),
@WOReq2 varchar(25),
@WOReq3 varchar(25),
@WOReq4 varchar(25)

 AS

IF (@Inspect = 'INSPECT' AND @MinFlow = 0 and @GPM = 0)
	BEGIN
		insert into tblNJAWHydInspData(Chlorine, DateInspect, FullFlow, GalFlow, InspectedBy, GPM, HydNum, HydSuf, Inspect, MinFlow, OpCntr, PressStatic, Remarks, WOReq1, WOReq2, WOReq3, WOReq4 )
		values( @Chlorine, @DateInspect, @FullFlow, 200, @FullName, @GPM, @HydNum, @hydSuf, @Inspect, @Minflow, @OpCntr, @PressStatic, @Remarks, @WOReq1, @WOReq2, @WOReq3, @WOReq4 )
	END
ELSE
	BEGIN
		insert into tblNJAWHydInspData(Chlorine, DateInspect, FullFlow, InspectedBy, GPM, HydNum, HydSuf, Inspect, MinFlow, OpCntr, PressStatic, Remarks, WOReq1, WOReq2, WOReq3, WOReq4 )
		values( @Chlorine, @DateInspect, @FullFlow, @FullName, @GPM, @HydNum, @hydSuf, @Inspect, @Minflow, @OpCntr, @PressStatic, @Remarks, @WOReq1, @WOReq2, @WOReq3, @WOReq4 )
	END

-- IF THIS IS A BLOWOFF VALVE INSPECTION 
-- THEN CREATE A CORRESPONDING VALVE INSPECTION
IF (left(@HydNum,1) = 'V')
	BEGIN
		Declare @empNum varchar(50)
		Declare @town int
		Declare @minTurns float
		select @empNum = (Select top 1 empNum from tblPermissions where username = @fullname)
		select @town = (Select top 1 town from tblNJAWValves where left(valnum, 4) = left(@hydNum, 4) and opCntr = @opCntr and town is not null)
		select @minTurns = (select top 1 dbo.ValveMinTurns(turns) from tblNJAWValves where OpCntr = @opCntr and ValNum = @HydNum)
		INSERT INTO tblNJAWValInspData(
				DateInspect, 
				InspectBy, 
				InspectorNum, 
				OpCntr, 
				Operated, 
				Remarks, 
				Turns, 
				ValNum, 
				MinReq,
				Town) 
			VALUES(
				@DateInspect, 
				@FullName, 
				@empNum, 
				@OpCntr, 
				'YES', 
				'Generated from Blow Off', 
				@minTurns, 
				@HydNum,
				@minTurns,
				@town)
	END

GO

CREATE PROCEDURE [dbo].[sp_AddValNew] 

@Critical varchar(2),
@CriticalNotes varchar(150),
@CrossStreet varchar(30),
@DateInst varchar(10),
@DateRemoved varchar(10),
@Initiator varchar(25),
@InspFreq varchar(10), 
@InspFreqUnit varchar(50), 
@Lat varchar(15),
@Lon varchar(15),
@MapPage varchar(6),
@NorPos varchar(15),
@OpCntr varchar(4),
@Opens varchar(6),
@Remarks varchar(2000),
@Route varchar(9),
@SketchNum varchar(15),
@StNum varchar(10),
@StName varchar(7),
@Town varchar(5),
@Traffic varchar(2),
@Turns varchar(8),
@TwnSection varchar(30),
@TypeMain varchar(15),
@ValCtrl varchar(25),
@ValLoc varchar(150),
@ValMake varchar(30),
@ValNum varchar(10),
@ValSize varchar(6),
@ValveStatus varchar(10),
@ValSuf varchar(8),
@ValType varchar(25),
@WONum varchar(18),
@BillInfo varchar(16),
@BPUKPI varchar(2), 
@ValveZone int,
@ObjectID int
 AS
insert into tblNJAWValves( Critical, CriticalNotes, CrossStreet, DateInst, DateRetired, Initiator, Lat, Lon, MapPage, NorPos, OpCntr, Opens, Remarks, Route, SketchNum, StNum, StName, Town, Traffic, Turns, TwnSection, TypeMain, ValCtrl, ValLoc, ValMake, ValNum, ValveSize, ValveStatus, ValSuf, ValType, WONum, BillInfo, BPUKPI, InspFreq, InspFreqUnit,ObjectID,ValveZone )
values( @Critical, @CriticalNotes, @CrossStreet, @DateInst, @DateRemoved, @Initiator, @Lat, @Lon, @MapPage, @NorPos, @OpCntr, @Opens, @Remarks, @Route, @SketchNum, @StNum, @StName, @Town, @Traffic, @Turns, @TwnSection, @TypeMain, @ValCtrl, @ValLoc, @ValMake, @ValNum, @ValSize, @ValveStatus, @ValSuf, @ValType, @WONum, @BillInfo, @BPUKPI, @InspFreq, @InspFreqUnit, @ObjectID, @ValveZone)
GO

CREATE PROCEDURE [dbo].[sp_UD_ValCtrl] 
AS
Update tblNJAWValves SET
ValCtrl = UPPER(ValCtrl)
WHERE ValCtrl is Not Null
GO

CREATE PROCEDURE [dbo].[sp_UD_ValveRoute] 
AS
Update tblNJAWValves SET
Route = '0'
WHERE Route is Null
GO

CREATE PROCEDURE [dbo].[sp_UD_VCrossSt] 
AS
Update tblNJAWValves SET
CrossStreet = UPPER(CrossStreet)
WHERE CrossStreet is Not Null
GO

CREATE PROCEDURE [dbo].[sp_UD_VDateInst] 
AS
Update tblNJAWValves SET
DateInst = '1/1/1900'
WHERE DateInst is Null
GO

CREATE PROCEDURE [dbo].[sp_UD_VLateral] 
AS
Update tblNJAWValves SET
Lateral = UPPER(Lateral)
WHERE Lateral is Not Null
GO

CREATE PROCEDURE [dbo].[sp_UD_VMain] 
AS
Update tblNJAWValves SET
Main = UPPER(Main)
WHERE Main is Not Null
GO

CREATE PROCEDURE [dbo].[sp_UD_VNorPos] 
AS
Update tblNJAWValves SET
NorPos = UPPER(NorPos)
WHERE NorPos is Not Null
GO

CREATE PROCEDURE [dbo].[sp_UD_VOpens] 
AS
Update tblNJAWValves SET
Opens = UPPER(Opens)
WHERE Opens is Not Null
GO

CREATE PROCEDURE [dbo].[sp_UD_VStatus] 
AS
Update tblNJAWValves SET
ValveStatus = UPPER(ValveStatus)
WHERE ValveStatus is Not Null
GO

CREATE PROCEDURE [dbo].[sp_UD_VValMake] 
AS
Update tblNJAWValves SET
ValMake = UPPER(ValMake)
WHERE ValMake is Not Null
GO

CREATE PROCEDURE [dbo].[sp_UD_VValType] 
AS
Update tblNJAWValves SET
ValType = UPPER(ValType)
WHERE ValType is Not Null
GO

CREATE PROCEDURE [dbo].[sp_UpDateValve] 
	@Critical varchar(2),
	@CriticalNotes varchar(150),
	@CrossStreet varchar(30),
	@DateInst varchar(10),
	@DateRetired varchar(10),
	@InspFreq varchar(10),
	@InspFreqUnit varchar(50),
	@Lat varchar(15),
	@Lon varchar(15),
	@MapPage varchar(6),
	@NorPos varchar(25),
	@Opens varchar(6),
	@RecID int,
	@Remarks varchar(2000),
	@Route varchar(9),
	@SketchNum varchar(15),
	@StNum varchar(10),
	@StName varchar(7),
	@Traffic varchar(2),
	@Turns varchar(6),
	@TwnSection varchar(30),
	@TypeMain varchar(15),
	@ValCtrl varchar(25),
	@ValLoc varchar(150),
	@ValMake varchar(30),
	@ValNum varchar(15),
	@ValSuf int,
	@ValType varchar(25),
	@ValveSize varchar(10),
	@ValveStatus varchar(10),
	@WONum varchar(18),
	@BillInfo varchar(16),
	@BPUKPI varchar(2), 
	@ValveZone int,
	@ObjectID int
	
AS
DECLARE @abbreviation varchar(10)
SELECT @abbreviation = substring(@valNum, 2, charindex('-', @valNum)-2) 

DECLARE @townID int
DECLARE @town varchar(50)
DECLARE @county varchar(50)

-- need these because the database joins on string fields =/
SELECT @townID = (SELECT town FROM tblNJAWValves WHERE recID = @recID)
SELECT @town = (SELECT town FROM Towns WHERE TownID = @townID)
SELECT @county = (SELECT county FROM Towns WHERE TownID = @townID)

-- If the Abbreviation is not valid, throw an exception.
IF (NOT @abbreviation in (
							-- Get Valid Abbreviation Codes for the record's town
							select ab from Towns where TownID = @townID -- Neptune
							union all
							select distinct Abbreviation from TownSections where townID = @townID and isNull(abbreviation, '') <> ''
						 ))
	BEGIN
		SELECT isNull(@abbreviation,'')
		RAISERROR (N'ERROR: UNABLE TO SAVE THE VALVE WITH AN INVALID ABBREVIATION', 10, 1)
	END
ELSE  -- Else - Update It
BEGIN
	UPDATE 
		tblNJAWValves 
	SET
		Critical = @Critical,
		CriticalNotes = @CriticalNotes,
		CrossStreet = @CrossStreet,
		DateInst = @DateInst,
		DateRetired = @DateRetired,
		InspFreq = @InspFreq,
		InspFreqUnit = @InspFreqUnit,
		Lat = @Lat,
		Lon = @Lon,
		MapPage = @MapPage,
		NorPos = @NorPos,
		Opens = @Opens,
		Remarks = @Remarks,
		Route = @Route,
		SketchNum = @SketchNum,
		StNum = @StNum,
		StName = @StName,
		Traffic = @Traffic,
		Turns = @Turns,
		TwnSection = @TwnSection,
		TypeMain = @TypeMain,
		ValCtrl = @ValCtrl,
		ValLoc = @ValLoc,
		ValMake = @ValMake,
		ValNum = @ValNum,
		ValSuf = @ValSuf,
		ValType = @ValType,
		ValveSize = @ValveSize,
		ValveStatus = @ValveStatus,
		WONum = @WONum,
		BillInfo = @BillInfo,
		BPUKPI = @BPUKPI, 
		ValveZone = @ValveZone,
		ObjectID = @ObjectID,
		LastUpdated = getDate()
	WHERE 
		RecID = @RecID
	SELECT 1
END
GO

CREATE Procedure [dbo].[StatisticsGeneral]
AS

Declare @tbl TABLE([cnt] int, [descript] varchar(255))

Insert Into @tbl 
	select count(*), 'Services (Total)' from tblNJAWService
Insert Into @tbl 
	select count(*), 'Services Ordered this Month' from tblNJAWService where Year(OrdCreationDate) = Year(GetDate()) and Month(OrdCreationDate) = Month(GetDate())
Insert Into @tbl 
	select count(*), 'Services Installed this Month' from tblNJAWService where Year(DateInstalled) = Year(GetDate()) and Month(DateInstalled) = Month(GetDate())
--select top 10 * from tblNJAWService order by recID desc

Insert Into @tbl 
	select count(*), 'Hydrants (Total)' from tblNJAWHydrant
Insert Into @tbl 
	select count(*), 'Hydrants Installed this Month' from tblNJAWHydrant where Year(DateInst) = Year(GetDate()) and Month(DateInst) = Month(GetDate())
Insert Into @tbl 
	select count(*), 'Hydrants Added this Month' from tblNJAWHydrant  where Year(DateAdded) = Year(GetDate()) and Month(DateAdded) = Month(GetDate())
Insert Into @tbl 
	select count(*), 'Hydrant Inspections (Total)' from tblNJAWHydInspData 
Insert Into @tbl 
	select count(*), 'Hydrant Inspections (Last 30 days)' from tblNJAWHydInspData Where DateDiff(D, [DateInspect], GetDate()) < 31 and DateDiff(D, [DateInspect], GetDate()) > -1
Insert Into @tbl 
	select count(*), 'Hydrant Inspections (Today)' from tblNJAWHydInspData Where Year([DateInspect]) = Year(GetDate()) and Month(DateInspect) = Month(GetDate()) and Day(DateInspect) = Day(GetDate())

Insert Into @tbl 
	select count(*), 'Valves (Total)' from tblNJAWValves
Insert Into @tbl 
	select count(*), 'Valves Installed this Month' from tblNJAWValves where Year(DateInst) = Year(GetDate()) and Month(DateInst) = Month(GetDate())
Insert Into @tbl 
	select count(*), 'Valves Added this Month' from tblNJAWValves where Year(DateAdded) = Year(GetDate()) and Month(DateAdded) = Month(GetDate())
--select top 10 * from tblNJAWValves
Insert Into @tbl 
	select count(*), 'Valve Inspections (Total)' from tblNJAWValInspData 
Insert Into @tbl 
	select count(*), 'Valve Inspections (Last 30 days)' from tblNJAWValInspData Where DateDiff(D, [DateInspect], GetDate()) < 31 and DateDiff(D, [DateInspect], GetDate()) > -1
Insert Into @tbl 
	select count(*), 'Valve Inspections (Today)' from tblNJAWValInspData Where Year([DateInspect]) = Year(GetDate()) and Month(DateInspect) = Month(GetDate()) and Day(DateInspect) = Day(GetDate())

Insert Into @tbl
	select status/100, 'Database Size (MB)' from mcprod.dbo.sysfiles1 where name = 'MCProd_Data'

select descript as 'Description', cnt as 'Total' from @tbl

GO

CREATE view [dbo].[tblNJAWBOInspDataInfo] as 
                            select 
	                            tblNJAWValves.valnum, 
	                            tblNJAWValves.opCntr, 
	                            tblNJAWHydInspLastNonInsp.LastNonInspect, 
	                            tblNJAWHydInspLastInsp.LastInspect, 
	                            (Select WoReq1 from tblNJAWHydInspData X where X.HydNum = tblNJAWValves.ValNum and X.OpCntr = tblNJAWValves.OpCntr and x.DateInspect=tblNJAWHydInspLastInsp.LastInspect) as WoReq1,  
	                            dbo.RequiresInspectionHydrant(tblNJAWValves.Town, ValveStatus, BillInfo, getDate(), dateInst, (Select max(dateInspect) from tblNJAWHydInspData where tblNJAWHydInspData.HydNum = tblNJAWValves.ValNum and tblNJAWHydInspData.OpCntr = tblNJAWValves.opCntr and isNull(Inspect,'') in ('Inspect','INSPECT/FLUSH')),1, 'Y') as [required]
	                            from tblNJAWValves 
		                            left join tblNJAWHydInspLastNonInsp on tblNJAWHydInspLastNonInsp.HydNum = tblNJAWValves.ValNum and tblNJAWHydInspLastNonInsp.OpCntr = tblNJAWValves.OpCntr
		                            left join tblNJAWHydInspLastInsp on tblNJAWHydInspLastInsp.HydNum = tblNJAWValves.ValNum and tblNJAWHydInspLastInsp.OpCntr = tblNJAWValves.OpCntr
		                            left join tblNJAWHydInspData on tblNJAWHydInspData.HydNum = tblNJAWValves.ValNum and tblNJAWHydInspData.OpCntr = tblNJAWValves.OpCntr and tblNJAWHydInspLastNonInsp.LastNonInspect = tblNJAWHydInspData.DateInspect
   
GO

CREATE view [dbo].[tblNJAWValInspDataInfo] as
select 
	tblNJAWValves.valnum, 
	tblNJAWValves.opCntr, 
	tblNJAWValInspLastNonInsp.LastNonInspect, 
	tblNJAWValInspLastInsp.LastInspect, 
	case when (tblNJAWValInspLastNonInsp.LastNonInspect is null) then #2.WoReq1 else tblNJAWValInspData.WoReq1 end as WoReq1, 
	dbo.RequiresInspectionValveByZone( ValveSize, ValCtrl, ValveStatus, BPUKPI, BillInfo, (Select max(dateInspect) from tblNJAWValInspData where UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValInspData.ValNum = tblNJAWValves.ValNum and tblNJAWValInspData.OpCntr = tblNJAWValves.opCntr) , DateInst, getDate(), ValveZone, tblNJAWValves.Town) as [required]
	from tblNJAWValves 
		left join tblNJAWValInspLastNonInsp on tblNJAWValInspLastNonInsp.ValNum = tblNJAWValves.ValNum and tblNJAWValInspLastNonInsp.OpCntr = tblNJAWValves.OpCntr
		left join tblNJAWValInspLastInsp on tblNJAWValInspLastInsp.ValNum = tblNJAWValves.ValNum and tblNJAWValInspLastInsp.OpCntr = tblNJAWValves.OpCntr
		left join tblNJAWValInspData on tblNJAWValInspData.ValNum = tblNJAWValves.ValNum and tblNJAWValInspData.OpCntr = tblNJAWValves.OpCntr and tblNJAWValInspLastNonInsp.LastNonInspect = tblNJAWValInspData.DateInspect
		left join tblNJAWValInspData #2 on #2.ValNum = tblNJAWValves.ValNum and #2.OpCntr = tblNJAWValves.OpCntr and tblNJAWValInspLastInsp.LastInspect = #2.DateInspect

GO

Create View [dbo].[tblNJAWValvesCounts] AS
SELECT OpCntr, Town, BillInfo, ValveStatus, ValveSize, count(*) as [Total]
	FROM tblNJAWValves 
	WHERE Left(isNull(OpCntr, ''), 2) in ('NJ','EW','LW') and upper(isNull(ValveStatus,'')) = 'ACTIVE'
	group by OpCntr, Town, BillInfo, ValveStatus, ValveSize

GO

CREATE VIEW [dbo].[viewNJAWBOInspData]
AS
SELECT     dbo.tblNJAWHydInspData.Chlorine, dbo.tblNJAWValves.Critical, dbo.tblNJAWHydInspData.DateInspect, dbo.tblNJAWHydInspData.FullFlow, 
                      dbo.tblNJAWHydInspData.GPM, dbo.tblNJAWHydInspData.HydNum, dbo.tblNJAWHydInspData.HydSuf, dbo.tblNJAWHydInspData.Inspect, 
                      dbo.tblNJAWHydInspData.InspectedBy, dbo.tblNJAWHydInspData.MinFlow, dbo.tblNJAWHydInspData.PressStatic, dbo.tblNJAWHydInspData.RecID, 
                      dbo.tblNJAWHydInspData.Remarks, dbo.tblNJAWValves.Town, dbo.tblNJAWHydInspData.WOReq1, dbo.tblNJAWHydInspData.WOReq2, 
                      dbo.tblNJAWHydInspData.WOReq3, dbo.tblNJAWHydInspData.WOReq4,
											dbo.tblNJAWHydInspData.DateAdded
FROM         dbo.tblNJAWHydInspData INNER JOIN
                      dbo.tblNJAWValves ON dbo.tblNJAWHydInspData.HydNum = dbo.tblNJAWValves.ValNum
GO

CREATE VIEW [dbo].[viewNJAWValInspData]
AS
SELECT     dbo.tblNJAWValves.Critical, dbo.tblNJAWValInspData.DateInspect, dbo.tblNJAWValInspData.InspectBy, dbo.tblNJAWValInspData.MinReq, 
                      dbo.tblNJAWValInspData.NorPos, dbo.tblNJAWValInspData.OpCntr, dbo.tblNJAWValInspData.Operated, dbo.tblNJAWValInspData.PosFound, 
                      dbo.tblNJAWValInspData.PosLeft, dbo.tblNJAWValInspData.RecID, dbo.tblNJAWValves.Town, dbo.tblNJAWValInspData.ValNum, 
                      dbo.tblNJAWValves.ValSuf, dbo.tblNJAWValInspData.Remarks, dbo.tblNJAWValInspData.Turns, dbo.tblNJAWValInspData.WOReq1, 
                      dbo.tblNJAWValInspData.WOReq2, dbo.tblNJAWValInspData.WOReq3, tblNJAWValves.ValveSize, tblNJAWValInspData.DateAdded, 
											dbo.tblNJAWValves.Lat, dbo.tblNJAWValves.Lon
FROM         dbo.tblNJAWValInspData INNER JOIN
                      dbo.tblNJAWValves ON dbo.tblNJAWValInspData.ValNum = dbo.tblNJAWValves.ValNum AND 
                      dbo.tblNJAWValInspData.OpCntr = dbo.tblNJAWValves.OpCntr
GO

CREATE VIEW [dbo].[ViewNJAWValveList]
AS
SELECT     
	V.CrossStreet, 
	V.DateInst, 
	S.FullStName, 
	V.ValNum, 
    V.ValSuf, 
	V.OpCntr, 
	V.RecID, 
	V.StNum, 
	V.Town, 
	V.WONum, 
	V.TwnSection, 
	V.[MapPage],
	V.ValveStatus, 
	V.ValveSize, 
	V.traffic, 
	V.StName, 
	V.ValCtrl, 
	V.BillInfo
FROM
	tblNJAWValves V 
LEFT OUTER JOIN
	Streets S
ON 
	V.StName = S.StreetID
GO

CREATE VIEW [dbo].[viewValvesWO] 
AS
select 
	VID.ValNum, 
	V.ValveSize,
	VID.DateInspect, 
	V.sTNum, 
	S.FullStName as StreetName,
	V.CrossStreet,
	Towns.Town,
	VID.InspectBy,
	V.TwnSection,
	VID.WoReq1,
	VID.WoReq2,
	VID.WoReq3,
	VID.InAccessible,
	VID.Remarks,
	VID.OpCntr, 
	UPPER(IsNull(VID.Operated,'NO')) as Operated, 
	V.Town as TownID, 
	V.ValveStatus
	from tblNJAWValInspData VID 
	LEFT JOIN tblNJAWValves V on V.valNum = VID.valNum and V.opCntr = VID.opCntr
	LEFT JOIN Streets S on S.StreetID= V.stName
	LEFT JOIN Towns on Towns.TownID = V.Town
GO";

        #endregion

        public struct TableNames
        {
            public const string
                INACCESSIBLE_REASONS = "InaccessibleReasons",
                INSPECTION_FREQUENCY_UNITS = "RecurringFrequencyUnits",
                FUNCTIONAL_LOCATIONS = "FunctionalLocations",
                MAIN_TYPES = "MainTypes",
                OPERATING_CENTERS = "OperatingCenters",
                USERS = "tblPermissions",
                STREETS = "Streets",
                TOWNS = "Towns",
                TOWN_SECTIONS = "TownSections",
                VALVE_BILLINGS = "ValveBillings",
                VALVE_CONTROLS = "ValveControls",
                VALVE_INSPECTIONS_OLD = "tblNJAWValInspData",
                VALVE_INSPECTIONS_NEW = "ValveInspections",
                VALVE_MANUFACTURERS = "ValveManufacturers",
                VALVE_NORMAL_POSITIONS = "ValveNormalPositions",
                VALVE_OPEN_DIRECTIONS = "ValveOpenDirections",
                VALVE_SIZES = "ValveSizes",
                VALVE_STATUSES = "ValveStatuses",
                VALVE_TYPES = "ValveTypes",
                VALVE_WORK_ORDER_REQUESTS = "ValveWorkOrderRequests",
                VALVE_ZONES = "ValveZones",
                VALVES_OLD = "tblNJAWValves",
                VALVES_NEW = "Valves",
                WATER_SYSTEMS = "WaterSystems",
                VALVES_SAP = "ValvesSAP";
        }

        public struct OldColumnNames
        {
            public const string
                VAL_BILL_INFO = "BillInfo",
                VAL_BLOW_OFF = "BlowOff",
                VAL_BPUKPI = "BPUKPI",
                VAL_CRITICAL = "Critical",
                VAL_CRITICAL_NOTES = "CriticalNotes",
                VAL_CROSS_STREET = "CrossStreet",
                VAL_DATE_RETIRED = "DateRetired",
                VAL_DATE_TESTED = "DateTested",
                VAL_ELEVATION = "Elevation",
                VAL_INITIATOR = "Initiator",
                VAL_INSP_FREQ = "InspFreq",
                VAL_INSP_FREQ_UNIT = "InspFreqUnit",
                VAL_LAT = "Lat",
                VAL_LATERAL = "Lateral",
                VAL_LON = "Lon",
                VAL_MAIN = "Main",
                VAL_MAP_PAGE = "MapPage",
                VAL_NOR_POS = "NorPos",
                VAL_OP_CNTR = "OpCntr",
                VAL_OPENS = "Opens",
                VAL_OBJECT_ID = "ObjectID",
                VAL_PATH = "Path",
                VAL_PRINTED_LABEL = "PrintedLabel",
                VAL_REC_ID = "RecID",
                VAL_REMARKS = "Remarks",
                VAL_ROUTE = "Route",
                VAL_SKETCH_NUM = "SketchNum",
                VAL_ST_NUM = "StNum",
                VAL_ST_NAME = "StName",
                VAL_TASK_RETIRE = "TaskRetire",
                VAL_TOWN = "Town",
                VAL_TRAFFIC = "Traffic",
                VAL_TURNS = "Turns",
                VAL_TWN_SECTION = "TwnSection",
                VAL_TYPE_MAIN = "TypeMain",
                VAL_VAL_CTRL = "ValCtrl",
                VAL_VAL_LOC = "ValLoc",
                VAL_VAL_MAKE = "ValMake",
                VAL_VAL_NUM = "ValNum",
                VAL_VAL_SUF = "ValSuf",
                VAL_VAL_TYPE = "ValType",
                VAL_VALVE_SIZE = "ValveSize",
                VAL_VALVE_STATUS = "ValveStatus",
                VAL_WO_NUM = "WONum",
                VAL_DATE_ADDED = "DateAdded",
                VAL_VALVE_ZONE = "ValveZone",
                VAL_IMAGE_ACTION_ID = "ImageActionID",
                VAL_DATE_INST = "DateInst",
                VAL_CREATED_ON = "CreatedOn",
                VAL_LAST_UPDATED = "LastUpdated",
                VAL_SAP_EQUIPMENT_ID = "SAPEquipmentID",
                VAL_WATER_SYSTEM = "WaterSystem",
                VALVE_INSPECTION_CREATED_ON = "CreatedOn",
                VALVE_INSPECTION_INACCESSIBLE = "Inaccessible",
                VALVE_INSPECTION_INSPECTED_BY = "InspectBy",
                VALVE_INSPECTION_NOR_POS = "NorPos",
                VALVE_INSPECTION_DATE_ADDED = "",
                VALVE_INSPECTION_DATE_INSPECTED = "DateInspect",
                VALVE_INSPECTION_INSPECTOR_NUM = "InspectorNum",
                VALVE_INSPECTION_MINIMUM_REQUIRED_TURNS = "MinReq",
                VALVE_INSPECTION_OPERATING_CENTER = "OpCntr",
                VALVE_INSPECTION_OPERATED = "Operated",
                VALVE_INSPECTION_POSITION_FOUND = "PosFound",
                VALVE_INSPECTION_POSITION_LEFT = "PosLeft",
                VALVE_INSPECTION_RECORD_ADDED = "RecAdded",
                VALVE_INSPECTION_ID = "RecID",
                VALVE_INSPECTION_REMARKS = "Remarks",
                VALVE_INSPECTION_TOWN = "Town",
                VALVE_INSPECTION_TURNS = "Turns",
                VALVE_INSPECTION_TURNS_NOT_COMPLETED = "TurnsNotCompleted",
                VALVE_INSPECTION_VALVE_NUMBER = "ValNum",
                VALVE_INSPECTION_WO_REQ1 = "WOReq1",
                VALVE_INSPECTION_WO_REQ2 = "WOReq2",
                VALVE_INSPECTION_WO_REQ3 = "WOReq3",
                VALVE_INSPECTION_VALVE_ID = "ValveID",
                FUNCTIONAL_LOCATION = "FunctionalLocationID";
        }

        public struct NewColumnNames
        {
            public const string
                VAL_BILL_INFO = "ValveBillingId",
                VAL_BPUKPI = "BPUKPI",
                VAL_CRITICAL = "Critical",
                VAL_CRITICAL_NOTES = "CriticalNotes",
                VAL_CROSS_STREET = "CrossStreetId",
                VAL_DATE_RETIRED = "DateRetired",
                VAL_DATE_TESTED = "DateTested",
                VAL_ELEVATION = "Elevation",
                VAL_INITIATOR = "InitiatorId",
                VAL_INSP_FREQ = "InspectionFrequency",
                VAL_INSP_FREQ_UNIT = "InspectionFrequencyUnitId",
                VAL_LATERAL = "Lateral",
                VAL_MAIN = "Main",
                VAL_MAP_PAGE = "MapPage",
                VAL_NOR_POS = "NormalPositionId",
                VAL_OP_CNTR = "OperatingCenterId",
                VAL_OPENS = "OpensId",
                VAL_OBJECT_ID = "ObjectID",
                VAL_PATH = "Path",
                VAL_PRINTED_LABEL = "PrintedLabel",
                VAL_REC_ID = "Id",
                VAL_REMARKS = "Remarks",
                VAL_ROUTE = "Route",
                VAL_SKETCH_NUM = "SketchNumber",
                VAL_ST_NUM = "StreetNumber",
                VAL_ST_NAME = "StreetId",
                VAL_TASK_RETIRE = "TaskRetire",
                VAL_TOWN = "Town",
                VAL_TRAFFIC = "Traffic",
                VAL_TURNS = "TurnsId",
                VAL_TWN_SECTION = "TownSectionId",
                VAL_TYPE_MAIN = "MainTypeId",
                VAL_VAL_CTRL = "ValveControlsId",
                VAL_VAL_LOC = "ValveLocation",
                VAL_VAL_MAKE = "ValveMakeId",
                VAL_VAL_NUM = "ValveNumber",
                VAL_VAL_SUF = "ValveSuffix",
                VAL_VAL_TYPE = "ValveTypeId",
                VAL_VALVE_SIZE = "ValveSizeId",
                VAL_VALVE_STATUS = "ValveStatusId",
                VAL_WO_NUM = "WorkOrderNumber",
                VAL_DATE_ADDED = "DateAdded",
                VAL_VALVE_ZONE = "ValveZoneId",
                VAL_IMAGE_ACTION_ID = "ImageActionID",
                VAL_DATE_INST = "DateInstalled",
                VAL_CREATED_ON = "CreatedOn",
                VAL_LAST_UPDATED = "LastUpdated",
                VAL_SAP_EQUIPMENT_ID = "SAPEquipmentID",
                VAL_WATER_SYSTEM = "WaterSystemId",
                VALVE_INSPECTION_INACCESSIBLE = "InaccessibleId",
                VALVE_INSPECTION_INSPECTED_BY = "InspectedById",
                VALVE_INSPECTION_NOR_POS = "NormalPositionId",
                VALVE_INSPECTION_DATE_INSPECTED = "DateInspected",
                VALVE_INSPECTION_MINIMUM_REQUIRED_TURNS = "MinimumRequiredTurns",
                VALVE_INSPECTION_OPERATING_CENTER = "OperatingCenterId",
                VALVE_INSPECTION_OPERATED = "Operated",
                VALVE_INSPECTION_POSITION_FOUND = "PositionFoundId",
                VALVE_INSPECTION_POSITION_LEFT = "PositionLeftId",
                VALVE_INSPECTION_RECORD_ADDED = "CreatedOn",
                VALVE_INSPECTION_ID = "Id",
                VALVE_INSPECTION_REMARKS = "Remarks",
                VALVE_INSPECTION_TOWN = "Town",
                VALVE_INSPECTION_TURNS = "Turns",
                VALVE_INSPECTION_TURNS_NOT_COMPLETED = "TurnsNotCompleted",
                VALVE_INSPECTION_VALVE_NUMBER = "ValveNumber",
                VALVE_INSPECTION_WO_REQ1 = "WorkOrderRequest1Id",
                VALVE_INSPECTION_WO_REQ2 = "WorkOrderRequest2Id",
                VALVE_INSPECTION_WO_REQ3 = "WorkOrderRequest3Id",
                VALVE_INSPECTION_VALVE_ID = "ValveId",
                FUNCTIONAL_LOCATION = "FunctionalLocationId";
        }

        public struct StringLengths
        {
            public const int
                VAL_BILL_INFO = 16,
                VAL_BLOW_OFF = 3,
                VAL_BPUKPI = 2,
                VAL_CRITICAL = 2,
                VAL_CRITICAL_NOTES = 150,
                VAL_CROSS_STREET = 30,
                VAL_INITIATOR = 25,
                VAL_INSP_FREQ = 50,
                VAL_INSP_FREQ_UNIT = 50,
                VAL_LATERAL = 6,
                VAL_MAIN = 6,
                VAL_MAP_PAGE = 15,
                VAL_NOR_POS = 25,
                VAL_OP_CNTR = 4,
                VAL_OPENS = 6,
                VAL_PATH = 250,
                VAL_PRINTED_LABEL = 3,
                VAL_SKETCH_NUM = 15,
                VAL_ST_NUM = 10,
                VAL_TASK_RETIRE = 10,
                VAL_TRAFFIC = 2,
                VAL_TWN_SECTION = 30,
                VAL_TYPE_MAIN = 15,
                VAL_VAL_CTRL = 25,
                VAL_VAL_LOC = 150,
                VAL_VAL_MAKE = 30,
                VAL_VAL_NUM = 15,
                VAL_VAL_TYPE = 25,
                VAL_VALVE_SIZE = 10,
                VAL_VALVE_STATUS = 10,
                VAL_WO_NUM = 18,
                VALVE_INSPECTION_INACCESSIBLE = 50,
                VALVE_INSPECTION_INSPECT_BY = 25,
                VALVE_INSPECTION_INSPECTOR_NUM = 25,
                VALVE_INSPECTION_MIN_REQ = 10,
                VALVE_INSPECTION_NOR_POS = 15,
                VALVE_INSPECTION_OP_CNTR = 4,
                VALVE_INSPECTION_OPERATED = 3,
                VALVE_INSPECTION_POS_FOUND = 6,
                VALVE_INSPECTION_POS_LEFT = 6,
                VALVE_INSPECTION_TURNS = 10,
                VALVE_INSPECTION_VALNUM = 10,
                VALVE_INSPECTION_WO_REQ1 = 25,
                VALVE_INSPECTION_WO_REQ2 = 25,
                VALVE_INSPECTION_WO_REQ3 = 25;
        }

        #endregion

        public override void Up()
        {
            Execute.Sql(Sql.DROP_INDEXES_STATS);
            Execute.Sql(IndexesAndStatisticsForValvesForBug2413.ROLLBACK_INDEXES);

            #region Lookups

            this.CreateLookupTableFromQuery(TableNames.INACCESSIBLE_REASONS,
                "SELECT DISTINCT Inaccessible FROM tblNJAWValInspData WHERE IsNull(Inaccessible,'') <> '' ORDER BY 1");
            this.CreateLookupTableFromQuery(TableNames.VALVE_WORK_ORDER_REQUESTS,
                "SELECT DISTINCT WOReq FROM tblNJAWWOReq WHERE [Val] = 'ON' ORDER BY WOReq");
            this.CreateLookupTableFromQuery(TableNames.VALVE_BILLINGS,
                "SELECT DISTINCT BillInfo FROM tblNJAWValves WHERE isNull(BillInfo,'') <> '' ORDER BY 1");
            this.CreateLookupTableFromQuery(TableNames.VALVE_CONTROLS,
                "SELECT DISTINCT ValCtrl FROM tblNJAWValves WHERE isNull(valCtrl,'') <> '' ORDER by 1");
            this.CreateLookupTableFromQuery(TableNames.VALVE_MANUFACTURERS,
                "SELECT DISTINCT HVMake FROM tblNJAWHVMake WHERE isNull(HVMake,'') <> '' AND [Val] = 'ON' ORDER BY 1");
            this.CreateLookupTableFromQuery(TableNames.VALVE_TYPES,
                "SELECT [ValType] FROM [dbo].[tblNJAWValType] ORDER BY 1");
            this.CreateLookupTableFromQuery(TableNames.VALVE_SIZES,
                "SELECT Distinct Valve FROM tblNJAWSizeServ WHERE isNull([Valve],'') <> '' and RecId <> 29 ORDER BY Valve");
            this.CreateLookupTableWithValues(TableNames.VALVE_STATUSES, "ACTIVE", "CANCELLED", "PENDING", "RETIRED",
                "REMOVED", "INACTIVE", "PRIVATE");
            this.CreateLookupTableFromQuery(TableNames.VALVE_ZONES,
                "SELECT Distinct ValveZone from tblNJAWValves where isNull(ValveZone,'0') <> '0' and ValveZone is not null order by 1");

            #endregion

            #region Inspections

            Execute.Sql(Sql.UPDATE_VALVE_INSPECTION);
            Delete.ForeignKeyColumn(TableNames.VALVE_INSPECTIONS_OLD, OldColumnNames.VALVE_INSPECTION_TOWN,
                TableNames.TOWNS);
            Rename.Table(TableNames.VALVE_INSPECTIONS_OLD).To(TableNames.VALVE_INSPECTIONS_NEW);

            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.VALVE_INSPECTIONS_NEW,
                TableNames.INACCESSIBLE_REASONS, NewColumnNames.VALVE_INSPECTION_INACCESSIBLE,
                OldColumnNames.VALVE_INSPECTION_INACCESSIBLE);
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.VALVE_INSPECTIONS_NEW, TableNames.USERS,
                NewColumnNames.VALVE_INSPECTION_INSPECTED_BY, OldColumnNames.VALVE_INSPECTION_INSPECTED_BY, "RecID",
                "UserName");
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.VALVE_INSPECTIONS_NEW,
                TableNames.VALVE_NORMAL_POSITIONS, NewColumnNames.VALVE_INSPECTION_NOR_POS,
                OldColumnNames.VALVE_INSPECTION_NOR_POS);
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.VALVE_INSPECTIONS_NEW,
                TableNames.VALVE_NORMAL_POSITIONS, NewColumnNames.VALVE_INSPECTION_POSITION_FOUND,
                OldColumnNames.VALVE_INSPECTION_POSITION_FOUND);
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.VALVE_INSPECTIONS_NEW,
                TableNames.VALVE_NORMAL_POSITIONS, NewColumnNames.VALVE_INSPECTION_POSITION_LEFT,
                OldColumnNames.VALVE_INSPECTION_POSITION_LEFT);
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.VALVE_INSPECTIONS_NEW,
                TableNames.VALVE_WORK_ORDER_REQUESTS, NewColumnNames.VALVE_INSPECTION_WO_REQ1,
                OldColumnNames.VALVE_INSPECTION_WO_REQ1);
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.VALVE_INSPECTIONS_NEW,
                TableNames.VALVE_WORK_ORDER_REQUESTS, NewColumnNames.VALVE_INSPECTION_WO_REQ2,
                OldColumnNames.VALVE_INSPECTION_WO_REQ2);
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.VALVE_INSPECTIONS_NEW,
                TableNames.VALVE_WORK_ORDER_REQUESTS, NewColumnNames.VALVE_INSPECTION_WO_REQ3,
                OldColumnNames.VALVE_INSPECTION_WO_REQ3);

            Rename.Column(OldColumnNames.VALVE_INSPECTION_RECORD_ADDED).OnTable(TableNames.VALVE_INSPECTIONS_NEW)
                  .To(NewColumnNames.VALVE_INSPECTION_RECORD_ADDED);
            Rename.Column(OldColumnNames.VALVE_INSPECTION_ID).OnTable(TableNames.VALVE_INSPECTIONS_NEW)
                  .To(NewColumnNames.VALVE_INSPECTION_ID);
            Alter.Column(OldColumnNames.VALVE_INSPECTION_MINIMUM_REQUIRED_TURNS)
                 .OnTable(TableNames.VALVE_INSPECTIONS_NEW).AsDecimal(10, 5).Nullable();
            Rename.Column(OldColumnNames.VALVE_INSPECTION_MINIMUM_REQUIRED_TURNS)
                  .OnTable(TableNames.VALVE_INSPECTIONS_NEW).To(NewColumnNames.VALVE_INSPECTION_MINIMUM_REQUIRED_TURNS);
            Rename.Column(OldColumnNames.VALVE_INSPECTION_DATE_INSPECTED).OnTable(TableNames.VALVE_INSPECTIONS_NEW)
                  .To(NewColumnNames.VALVE_INSPECTION_DATE_INSPECTED);

            Alter.Column(OldColumnNames.VALVE_INSPECTION_TURNS).OnTable(TableNames.VALVE_INSPECTIONS_NEW)
                 .AsDecimal(10, 5).Nullable();
            Alter.Column(OldColumnNames.VALVE_INSPECTION_OPERATED).OnTable(TableNames.VALVE_INSPECTIONS_NEW).AsBoolean()
                 .NotNullable();
            Alter.Column(OldColumnNames.VALVE_INSPECTION_CREATED_ON).OnTable(TableNames.VALVE_INSPECTIONS_NEW)
                 .AsDateTime().Nullable();

            Delete.Column(OldColumnNames.VALVE_INSPECTION_INSPECTOR_NUM).FromTable(TableNames.VALVE_INSPECTIONS_NEW);
            Delete.Column(OldColumnNames.VALVE_INSPECTION_OPERATING_CENTER).FromTable(TableNames.VALVE_INSPECTIONS_NEW);
            Delete.Column(OldColumnNames.VALVE_INSPECTION_VALVE_NUMBER).FromTable(TableNames.VALVE_INSPECTIONS_NEW);

            #endregion

            #region Valves

            Execute.Sql(Sql.UPDATE_VALVES);
            Rename.Table(TableNames.VALVES_OLD).To(TableNames.VALVES_NEW);

            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.VALVES_NEW, TableNames.VALVE_BILLINGS,
                NewColumnNames.VAL_BILL_INFO, OldColumnNames.VAL_BILL_INFO);
            Alter.Column(NewColumnNames.VAL_BILL_INFO).OnTable(TableNames.VALVES_NEW).AsInt32().NotNullable();
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.VALVES_NEW, TableNames.STREETS,
                NewColumnNames.VAL_CROSS_STREET, OldColumnNames.VAL_CROSS_STREET, "StreetId", "FullStName",
                "AND " + TableNames.STREETS + ".TownID = " + TableNames.VALVES_NEW + ".Town");
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.VALVES_NEW,
                TableNames.INSPECTION_FREQUENCY_UNITS, NewColumnNames.VAL_INSP_FREQ_UNIT,
                OldColumnNames.VAL_INSP_FREQ_UNIT);
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.VALVES_NEW,
                TableNames.VALVE_NORMAL_POSITIONS, NewColumnNames.VALVE_INSPECTION_NOR_POS,
                OldColumnNames.VALVE_INSPECTION_NOR_POS);
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.VALVES_NEW, TableNames.OPERATING_CENTERS,
                NewColumnNames.VALVE_INSPECTION_OPERATING_CENTER, OldColumnNames.VALVE_INSPECTION_OPERATING_CENTER,
                "OperatingCenterId", "OperatingCenterCode");
            Alter.Column(NewColumnNames.VAL_OP_CNTR).OnTable(TableNames.VALVES_NEW).AsInt32().NotNullable();
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.VALVES_NEW, TableNames.VALVE_OPEN_DIRECTIONS,
                NewColumnNames.VAL_OPENS, OldColumnNames.VAL_OPENS);
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.VALVES_NEW, TableNames.TOWN_SECTIONS,
                NewColumnNames.VAL_TWN_SECTION, OldColumnNames.VAL_TWN_SECTION, "TownSectionID", "Name");
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.VALVES_NEW, TableNames.MAIN_TYPES,
                NewColumnNames.VAL_TYPE_MAIN, OldColumnNames.VAL_TYPE_MAIN);
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.VALVES_NEW, TableNames.VALVE_CONTROLS,
                NewColumnNames.VAL_VAL_CTRL, OldColumnNames.VAL_VAL_CTRL);
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.VALVES_NEW, TableNames.VALVE_MANUFACTURERS,
                NewColumnNames.VAL_VAL_MAKE, OldColumnNames.VAL_VAL_MAKE);
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.VALVES_NEW, TableNames.VALVE_TYPES,
                NewColumnNames.VAL_VAL_TYPE, OldColumnNames.VAL_VAL_TYPE);
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.VALVES_NEW, TableNames.VALVE_SIZES,
                NewColumnNames.VAL_VALVE_SIZE, OldColumnNames.VAL_VALVE_SIZE);
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.VALVES_NEW, TableNames.VALVE_STATUSES,
                NewColumnNames.VAL_VALVE_STATUS, OldColumnNames.VAL_VALVE_STATUS);
            Alter.Column(NewColumnNames.VAL_VALVE_STATUS).OnTable(TableNames.VALVES_NEW).AsInt32().NotNullable();

            Alter.Table(TableNames.VALVES_NEW)
                 .AddForeignKeyColumn(NewColumnNames.VAL_VALVE_ZONE, TableNames.VALVE_ZONES);
            Execute.Sql("UPDATE " + TableNames.VALVES_NEW +
                        " SET ValveZoneId = (SELECT Id from ValveZones where Description = ValveZone);");
            Delete.Column(OldColumnNames.VAL_VALVE_ZONE).FromTable(TableNames.VALVES_NEW);

            Execute.Sql(
                "INSERT INTO WaterSystems(Description) select Distinct WaterSystem from ValvesSAP where WaterSystem is not null and WaterSystem not in (Select Description from WaterSystems);");
            Alter.Table(TableNames.VALVES_NEW)
                 .AddForeignKeyColumn(NewColumnNames.VAL_WATER_SYSTEM, TableNames.WATER_SYSTEMS);
            Execute.Sql(
                "UPDATE Valves SET WaterSystemId = (SELECT Top 1 Id from WaterSystems where Description = (SELECT WaterSystem from ValvesSAP where ValvesSAP.ValveID = Valves.RecId))");
            Alter.Table(TableNames.VALVES_NEW).AddForeignKeyColumn(NewColumnNames.FUNCTIONAL_LOCATION,
                TableNames.FUNCTIONAL_LOCATIONS, "FunctionalLocationID");
            Execute.Sql(
                "UPDATE Valves SET FunctionalLocationId = (SELECT FunctionalLocationID from ValvesSAP WHERE ValvesSAP.ValveID = Valves.RecId)");
            Delete.Table(TableNames.VALVES_SAP);

            Alter.Table(TableNames.VALVES_NEW)
                 .AddForeignKeyColumn(NewColumnNames.VAL_INITIATOR, TableNames.USERS, "RecID");
            Execute.Sql("Update " + TableNames.VALVES_NEW + " SET " + NewColumnNames.VAL_INITIATOR +
                        " = (SELECT TOP 1 RecID from tblPermissions where FullName = " + OldColumnNames.VAL_INITIATOR +
                        ")");
            Delete.Column(OldColumnNames.VAL_INITIATOR).FromTable(TableNames.VALVES_NEW);

            Execute.Sql("UPDATE " + TableNames.VALVES_NEW + " SET BlowOff = 0 WHERE IsNull(BlowOff, '') = '';" +
                        "UPDATE " + TableNames.VALVES_NEW + " SET BlowOff = 1 WHERE IsNull(BlowOff, '') = 'YES';");

            Execute.Sql("UPDATE " + TableNames.VALVES_NEW + " SET BPUKPI = 0 WHERE IsNull(BPUKPI, '') = '';" +
                        "UPDATE " + TableNames.VALVES_NEW + " SET BPUKPI = 1 WHERE IsNull(BPUKPI, '') = 'ON';");
            Alter.Table(TableNames.VALVES_NEW).AlterColumn(OldColumnNames.VAL_BPUKPI).AsBoolean().NotNullable();

            Execute.Sql("UPDATE " + TableNames.VALVES_NEW +
                        " SET Critical = 0 WHERE IsNull(Critical, '') in ('','NO');" +
                        "UPDATE " + TableNames.VALVES_NEW + " SET Critical = 1 WHERE IsNull(Critical, '') = 'ON';");
            Alter.Table(TableNames.VALVES_NEW).AlterColumn(OldColumnNames.VAL_CRITICAL).AsBoolean().NotNullable();

            Execute.Sql("UPDATE " + TableNames.VALVES_NEW + " SET Traffic = 0 WHERE IsNull(Traffic, '') in ('','NO');" +
                        "UPDATE " + TableNames.VALVES_NEW + " SET Traffic = 1 WHERE IsNull(Traffic, '') = 'ON';");
            Alter.Table(TableNames.VALVES_NEW).AlterColumn(OldColumnNames.VAL_TRAFFIC).AsBoolean().NotNullable();

            Alter.Table(TableNames.VALVES_NEW).AlterColumn(OldColumnNames.VAL_INSP_FREQ).AsInt32().Nullable();
            Rename.Column(OldColumnNames.VAL_INSP_FREQ).OnTable(TableNames.VALVES_NEW).To(NewColumnNames.VAL_INSP_FREQ);

            Alter.Table(TableNames.VALVES_NEW).AlterColumn(OldColumnNames.VAL_VAL_SUF).AsInt32().Nullable();
            Rename.Column(OldColumnNames.VAL_VAL_SUF).OnTable(TableNames.VALVES_NEW).To(NewColumnNames.VAL_VAL_SUF);

            Execute.Sql("UPDATE " + TableNames.VALVES_NEW + " SET CriticalNotes = NULL where CriticalNotes = '';");
            Execute.Sql("UPDATE " + TableNames.VALVES_NEW + " SET MapPage = NULL where MapPage = '';");

            Rename.Column(OldColumnNames.VAL_SKETCH_NUM).OnTable(TableNames.VALVES_NEW)
                  .To(NewColumnNames.VAL_SKETCH_NUM);
            Rename.Column(OldColumnNames.VAL_ST_NUM).OnTable(TableNames.VALVES_NEW).To(NewColumnNames.VAL_ST_NUM);
            Rename.Column(OldColumnNames.VAL_VAL_LOC).OnTable(TableNames.VALVES_NEW).To(NewColumnNames.VAL_VAL_LOC);
            Rename.Column(OldColumnNames.VAL_WO_NUM).OnTable(TableNames.VALVES_NEW).To(NewColumnNames.VAL_WO_NUM);
            Rename.Column(OldColumnNames.VAL_DATE_INST).OnTable(TableNames.VALVES_NEW).To(NewColumnNames.VAL_DATE_INST);
            Rename.Column(OldColumnNames.VAL_VAL_NUM).OnTable(TableNames.VALVES_NEW).To(NewColumnNames.VAL_VAL_NUM);

            Delete.Column(OldColumnNames.VAL_LATERAL).FromTable(TableNames.VALVES_NEW);
            Delete.Column(OldColumnNames.VAL_MAIN).FromTable(TableNames.VALVES_NEW);
            Delete.Column(OldColumnNames.VAL_PATH).FromTable(TableNames.VALVES_NEW);
            Delete.Column(OldColumnNames.VAL_PRINTED_LABEL).FromTable(TableNames.VALVES_NEW);
            Delete.Column(OldColumnNames.VAL_TASK_RETIRE).FromTable(TableNames.VALVES_NEW);
            Delete.Column(OldColumnNames.VAL_CREATED_ON).FromTable(TableNames.VALVES_NEW);
            Delete.Column(OldColumnNames.VAL_BLOW_OFF).FromTable(TableNames.VALVES_NEW);

            Alter.Table(TableNames.VALVES_NEW).AddForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateID");
            Execute.Sql(Sql.UPDATE_VALVE_COORDINATES);
            Delete.Column(OldColumnNames.VAL_LAT).FromTable(TableNames.VALVES_NEW);
            Delete.Column(OldColumnNames.VAL_LON).FromTable(TableNames.VALVES_NEW);

            Rename.Column(OldColumnNames.VAL_REC_ID).OnTable(TableNames.VALVES_NEW).To(NewColumnNames.VAL_REC_ID);
            Rename.Column(OldColumnNames.VAL_ST_NAME).OnTable(TableNames.VALVES_NEW).To(NewColumnNames.VAL_ST_NAME);
            Rename.Column(OldColumnNames.VAL_TOWN).OnTable(TableNames.VALVES_NEW).To(NewColumnNames.VAL_TOWN);
            Alter.Column(NewColumnNames.VAL_TOWN).OnTable(TableNames.VALVES_NEW).AsInt32().NotNullable();
            /*
             * ImageActionID - investigate
             */

            Execute.Sql(@"
                declare @dataTypeId int
                insert into [DataType] (Data_Type, Table_Name) values('Valves', 'Valves')
                set @dataTypeId = (select @@IDENTITY)
                insert into [DocumentType] (Document_Type, DataTypeID) values('Document', @dataTypeId)
                insert into [DocumentType] (Document_Type, DataTypeID) values('Photo', @dataTypeId)");

            #endregion

            #region Sizes

            Delete.Index("IX_ValveSizes_Description").OnTable(TableNames.VALVE_SIZES);
            Alter.Table(TableNames.VALVE_SIZES).AlterColumn("Description").AsDecimal(5, 3).NotNullable();
            Rename.Column("Description").OnTable(TableNames.VALVE_SIZES).To("Size");

            #endregion

            Execute.Sql(Sql.NEW_INDEXES_STATISTICS);

            // This gets rolled back with the prior hydrant script
            Execute.Sql(SQL_REMOVE_VALVE_SPS_AND_VIEWS);
        }

        public override void Down()
        {
            Execute.Sql(Sql.REMOVE_NEW_INDEXES_STATISTICS);

            #region Valves

            Execute.Sql(
                "DELETE FROM DocumentLink WHERE DataTypeID = (SELECT DataTypeId FROM [DataType] WHERE Table_Name = 'Valves')");
            Execute.Sql(
                "DELETE FROM Document WHERE DocumentTypeId IN (SELECT DocumentTypeID FROM DocumentType WHERE DataTypeID = (SELECT DataTypeID FROM DataType WHERE Data_Type = 'Valves'))");

            Execute.Sql(@"
                delete from [DocumentType] where DataTypeID IN (select DataTypeId from [DataType] where Table_Name = 'Valves')
                delete from [DataType] where DataTypeId IN (select DataTypeId from [DataType] where Table_Name = 'Valves')");

            Alter.Column(NewColumnNames.VAL_TOWN).OnTable(TableNames.VALVES_NEW).AsInt32().Nullable();
            Rename.Column(NewColumnNames.VAL_TOWN).OnTable(TableNames.VALVES_NEW).To(OldColumnNames.VAL_TOWN);
            Rename.Column(NewColumnNames.VAL_ST_NAME).OnTable(TableNames.VALVES_NEW).To(OldColumnNames.VAL_ST_NAME);
            Rename.Column(NewColumnNames.VAL_REC_ID).OnTable(TableNames.VALVES_NEW).To(OldColumnNames.VAL_REC_ID);

            Alter.Table(TableNames.VALVES_NEW).AddColumn(OldColumnNames.VAL_LAT).AsFloat().Nullable();
            Alter.Table(TableNames.VALVES_NEW).AddColumn(OldColumnNames.VAL_LON).AsFloat().Nullable();
            Execute.Sql(Sql.ROLLBACK_VALVE_COORDINATES);
            Delete.ForeignKeyColumn(TableNames.VALVES_NEW, "CoordinateId", "Coordinates");

            Alter.Table(TableNames.VALVES_NEW).AddColumn(OldColumnNames.VAL_CREATED_ON).AsDateTime().Nullable();
            Alter.Table(TableNames.VALVES_NEW).AddColumn(OldColumnNames.VAL_TASK_RETIRE)
                 .AsAnsiString(StringLengths.VAL_TASK_RETIRE).Nullable();
            Alter.Table(TableNames.VALVES_NEW).AddColumn(OldColumnNames.VAL_PRINTED_LABEL)
                 .AsAnsiString(StringLengths.VAL_PRINTED_LABEL).Nullable();
            Alter.Table(TableNames.VALVES_NEW).AddColumn(OldColumnNames.VAL_PATH).AsAnsiString(StringLengths.VAL_PATH)
                 .Nullable();
            Alter.Table(TableNames.VALVES_NEW).AddColumn(OldColumnNames.VAL_MAIN).AsAnsiString(StringLengths.VAL_MAIN)
                 .Nullable();
            Alter.Table(TableNames.VALVES_NEW).AddColumn(OldColumnNames.VAL_LATERAL)
                 .AsAnsiString(StringLengths.VAL_LATERAL).Nullable();

            Rename.Column(NewColumnNames.VAL_SKETCH_NUM).OnTable(TableNames.VALVES_NEW)
                  .To(OldColumnNames.VAL_SKETCH_NUM);
            Rename.Column(NewColumnNames.VAL_ST_NUM).OnTable(TableNames.VALVES_NEW).To(OldColumnNames.VAL_ST_NUM);
            Rename.Column(NewColumnNames.VAL_VAL_LOC).OnTable(TableNames.VALVES_NEW).To(OldColumnNames.VAL_VAL_LOC);
            Rename.Column(NewColumnNames.VAL_WO_NUM).OnTable(TableNames.VALVES_NEW).To(OldColumnNames.VAL_WO_NUM);
            Rename.Column(NewColumnNames.VAL_DATE_INST).OnTable(TableNames.VALVES_NEW).To(OldColumnNames.VAL_DATE_INST);
            Rename.Column(NewColumnNames.VAL_VAL_NUM).OnTable(TableNames.VALVES_NEW).To(OldColumnNames.VAL_VAL_NUM);

            Rename.Column(NewColumnNames.VAL_VAL_SUF).OnTable(TableNames.VALVES_NEW).To(OldColumnNames.VAL_VAL_SUF);
            Alter.Table(TableNames.VALVES_NEW).AlterColumn(OldColumnNames.VAL_VAL_SUF).AsFloat().Nullable();

            Rename.Column(NewColumnNames.VAL_INSP_FREQ).OnTable(TableNames.VALVES_NEW).To(OldColumnNames.VAL_INSP_FREQ);
            Alter.Table(TableNames.VALVES_NEW).AlterColumn(OldColumnNames.VAL_INSP_FREQ)
                 .AsAnsiString(StringLengths.VAL_INSP_FREQ).Nullable();

            Alter.Table(TableNames.VALVES_NEW).AlterColumn(OldColumnNames.VAL_TRAFFIC)
                 .AsAnsiString(StringLengths.VAL_TRAFFIC).Nullable();
            Execute.Sql("Update " + TableNames.VALVES_NEW + " SET Traffic = 'ON' WHERE Traffic = '1';");
            Execute.Sql("Update " + TableNames.VALVES_NEW + " SET Traffic = NULL WHERE Traffic = '0';");

            Alter.Table(TableNames.VALVES_NEW).AlterColumn(OldColumnNames.VAL_CRITICAL)
                 .AsAnsiString(StringLengths.VAL_CRITICAL).Nullable();
            Execute.Sql("Update " + TableNames.VALVES_NEW + " SET Critical = 'ON' WHERE Critical = '1';");
            Execute.Sql("Update " + TableNames.VALVES_NEW + " SET Critical = NULL WHERE Critical = '0';");

            Alter.Table(TableNames.VALVES_NEW).AlterColumn(OldColumnNames.VAL_BPUKPI)
                 .AsAnsiString(StringLengths.VAL_BPUKPI).Nullable();
            Execute.Sql("UPDATE " + TableNames.VALVES_NEW + " SET BPUKPI = 'ON' WHERE BPUKPI = '1';");
            Execute.Sql("UPDATE " + TableNames.VALVES_NEW + " SET BPUKPI = NULL WHERE BPUKPI = '0';");

            Alter.Table(TableNames.VALVES_NEW).AddColumn(OldColumnNames.VAL_BLOW_OFF)
                 .AsAnsiString(StringLengths.VAL_BLOW_OFF).Nullable();

            Alter.Table(TableNames.VALVES_NEW).AddColumn(OldColumnNames.VAL_INITIATOR)
                 .AsAnsiString(StringLengths.VAL_INITIATOR).Nullable();
            Execute.Sql("Update " + TableNames.VALVES_NEW + " SET " + OldColumnNames.VAL_INITIATOR +
                        " = (SELECT FullName from tblPermissions where RecID = " + NewColumnNames.VAL_INITIATOR + ")");
            Delete.ForeignKeyColumn(TableNames.VALVES_NEW, NewColumnNames.VAL_INITIATOR, TableNames.USERS, "RecID");

            Alter.Table(TableNames.VALVES_NEW).AddColumn(OldColumnNames.VAL_VALVE_ZONE).AsInt32().Nullable();
            Execute.Sql("UPDATE " + TableNames.VALVES_NEW +
                        " SET ValveZone = (SELECT cast(Description as int) from ValveZones where ValveZones.Id = Valves.ValveZoneId)");
            Delete.ForeignKeyColumn(TableNames.VALVES_NEW, NewColumnNames.VAL_VALVE_ZONE, TableNames.VALVE_ZONES);

            this.RemoveLookupAndAdjustColumns(TableNames.VALVES_NEW, TableNames.VALVE_STATUSES,
                NewColumnNames.VAL_VALVE_STATUS, OldColumnNames.VAL_VALVE_STATUS, StringLengths.VAL_VALVE_STATUS);

            #region Sizes

            Rename.Column("Size").OnTable(TableNames.VALVE_SIZES).To("Description");
            Alter.Table(TableNames.VALVE_SIZES).AlterColumn("Description").AsAnsiString(50).NotNullable();

            #endregion

            this.RemoveLookupAndAdjustColumns(TableNames.VALVES_NEW, TableNames.VALVE_SIZES,
                NewColumnNames.VAL_VALVE_SIZE, OldColumnNames.VAL_VALVE_SIZE, StringLengths.VAL_VALVE_SIZE);
            this.RemoveLookupAndAdjustColumns(TableNames.VALVES_NEW, TableNames.VALVE_TYPES,
                NewColumnNames.VAL_VAL_TYPE, OldColumnNames.VAL_VAL_TYPE, StringLengths.VAL_VAL_TYPE);
            this.RemoveLookupAndAdjustColumns(TableNames.VALVES_NEW, TableNames.VALVE_MANUFACTURERS,
                NewColumnNames.VAL_VAL_MAKE, OldColumnNames.VAL_VAL_MAKE, StringLengths.VAL_VAL_MAKE);
            this.RemoveLookupAndAdjustColumns(TableNames.VALVES_NEW, TableNames.VALVE_CONTROLS,
                NewColumnNames.VAL_VAL_CTRL, OldColumnNames.VAL_VAL_CTRL, StringLengths.VAL_VAL_CTRL);
            this.RemoveLookupAndAdjustColumns(TableNames.VALVES_NEW, TableNames.MAIN_TYPES,
                NewColumnNames.VAL_TYPE_MAIN, OldColumnNames.VAL_TYPE_MAIN, StringLengths.VAL_TYPE_MAIN);
            this.RemoveLookupAndAdjustColumns(TableNames.VALVES_NEW, TableNames.TOWN_SECTIONS,
                NewColumnNames.VAL_TWN_SECTION, OldColumnNames.VAL_TWN_SECTION, StringLengths.VAL_TWN_SECTION,
                "TownSectionID", "Name");
            this.RemoveLookupAndAdjustColumns(TableNames.VALVES_NEW, TableNames.VALVE_OPEN_DIRECTIONS,
                NewColumnNames.VAL_OPENS, OldColumnNames.VAL_OPENS, StringLengths.VAL_OPENS);
            this.RemoveLookupAndAdjustColumns(TableNames.VALVES_NEW, TableNames.OPERATING_CENTERS,
                NewColumnNames.VALVE_INSPECTION_OPERATING_CENTER, OldColumnNames.VALVE_INSPECTION_OPERATING_CENTER,
                StringLengths.VAL_OP_CNTR, "OperatingCenterId", "OperatingCenterCode");
            this.RemoveLookupAndAdjustColumns(TableNames.VALVES_NEW, TableNames.VALVE_NORMAL_POSITIONS,
                NewColumnNames.VALVE_INSPECTION_NOR_POS, OldColumnNames.VALVE_INSPECTION_NOR_POS,
                StringLengths.VALVE_INSPECTION_NOR_POS);
            this.RemoveLookupAndAdjustColumns(TableNames.VALVES_NEW, TableNames.INSPECTION_FREQUENCY_UNITS,
                NewColumnNames.VAL_INSP_FREQ_UNIT, OldColumnNames.VAL_INSP_FREQ_UNIT, StringLengths.VAL_INSP_FREQ_UNIT);
            this.RemoveLookupAndAdjustColumns(TableNames.VALVES_NEW, TableNames.STREETS,
                NewColumnNames.VAL_CROSS_STREET, OldColumnNames.VAL_CROSS_STREET, StringLengths.VAL_CROSS_STREET,
                "StreetID", "FullStName");
            this.RemoveLookupAndAdjustColumns(TableNames.VALVES_NEW, TableNames.VALVE_BILLINGS,
                NewColumnNames.VAL_BILL_INFO, OldColumnNames.VAL_BILL_INFO, StringLengths.VAL_BILL_INFO);

            Create.Table(TableNames.VALVES_SAP).WithColumn("ValveID").AsInt32().NotNullable()
                  .WithColumn("FLRouteNumber").AsInt32().Nullable()
                  .WithColumn("WaterSystem").AsAnsiString(4).Nullable();
            Alter.Table(TableNames.VALVES_SAP).AddForeignKeyColumn(OldColumnNames.FUNCTIONAL_LOCATION,
                TableNames.FUNCTIONAL_LOCATIONS, "FunctionalLocationID");
            Execute.Sql("INSERT INTO " + TableNames.VALVES_SAP +
                        "(ValveID, WaterSystem, FunctionalLocationID) SELECT RecID, ws.Description as WaterSystem, FunctionalLocationID from Valves V LEFT JOIN WaterSystems ws on ws.Id = V.watersystemID");
            Delete.ForeignKeyColumn(TableNames.VALVES_NEW, "WaterSystemId", TableNames.WATER_SYSTEMS);
            Delete.ForeignKeyColumn(TableNames.VALVES_NEW, "FunctionalLocationId", TableNames.FUNCTIONAL_LOCATIONS);

            Rename.Table(TableNames.VALVES_NEW).To(TableNames.VALVES_OLD);
            Execute.Sql(Sql.ROLLBACK_VALVES);

            #endregion

            #region Inspections

            Alter.Table(TableNames.VALVE_INSPECTIONS_NEW).AddColumn(OldColumnNames.VALVE_INSPECTION_VALVE_NUMBER)
                 .AsAnsiString(StringLengths.VALVE_INSPECTION_VALNUM).Nullable();
            Execute.Sql(
                "UPDATE ValveInspections SET ValNum = (SELECT ValNum FROM tblNJAWValves where ValveInspections.ValveID = tblNJAWValves.RecID);");
            Alter.Table(TableNames.VALVE_INSPECTIONS_NEW).AddColumn(OldColumnNames.VALVE_INSPECTION_OPERATING_CENTER)
                 .AsAnsiString(StringLengths.VALVE_INSPECTION_OP_CNTR).Nullable();
            Execute.Sql(
                "UPDATE ValveInspections SET OpCntr = (SELECT OpCntr FROM tblNJAWValves where ValveInspections.ValveID = tblNJAWValves.RecID);");
            Alter.Table(TableNames.VALVE_INSPECTIONS_NEW).AddColumn(OldColumnNames.VALVE_INSPECTION_INSPECTOR_NUM)
                 .AsAnsiString(StringLengths.VALVE_INSPECTION_INSPECTOR_NUM).Nullable();
            Execute.Sql(
                "UPDATE ValveInspections SET InspectorNum = (select EmpNum from tblPermissions where ValveInspections.InspectedById = tblPermissions.RecID)");

            Alter.Column(OldColumnNames.VALVE_INSPECTION_CREATED_ON).OnTable(TableNames.VALVE_INSPECTIONS_NEW)
                 .AsCustom("smalldatetime").Nullable();
            Alter.Column(OldColumnNames.VALVE_INSPECTION_OPERATED).OnTable(TableNames.VALVE_INSPECTIONS_NEW)
                 .AsAnsiString(StringLengths.VALVE_INSPECTION_OPERATED).Nullable();
            Alter.Column(OldColumnNames.VALVE_INSPECTION_TURNS).OnTable(TableNames.VALVE_INSPECTIONS_NEW).AsFloat()
                 .Nullable();
            Alter.Column(OldColumnNames.VALVE_INSPECTION_TURNS).OnTable(TableNames.VALVE_INSPECTIONS_NEW)
                 .AsAnsiString(StringLengths.VALVE_INSPECTION_TURNS).Nullable();

            Alter.Column(NewColumnNames.VALVE_INSPECTION_MINIMUM_REQUIRED_TURNS)
                 .OnTable(TableNames.VALVE_INSPECTIONS_NEW).AsAnsiString(StringLengths.VALVE_INSPECTION_MIN_REQ)
                 .Nullable();
            Rename.Column(NewColumnNames.VALVE_INSPECTION_MINIMUM_REQUIRED_TURNS)
                  .OnTable(TableNames.VALVE_INSPECTIONS_NEW).To(OldColumnNames.VALVE_INSPECTION_MINIMUM_REQUIRED_TURNS);

            Rename.Column(NewColumnNames.VALVE_INSPECTION_ID).OnTable(TableNames.VALVE_INSPECTIONS_NEW)
                  .To(OldColumnNames.VALVE_INSPECTION_ID);
            Rename.Column(NewColumnNames.VALVE_INSPECTION_RECORD_ADDED).OnTable(TableNames.VALVE_INSPECTIONS_NEW)
                  .To(OldColumnNames.VALVE_INSPECTION_RECORD_ADDED);
            Rename.Column(NewColumnNames.VALVE_INSPECTION_DATE_INSPECTED).OnTable(TableNames.VALVE_INSPECTIONS_NEW)
                  .To(OldColumnNames.VALVE_INSPECTION_DATE_INSPECTED);

            this.RemoveLookupAndAdjustColumns(TableNames.VALVE_INSPECTIONS_NEW, TableNames.VALVE_WORK_ORDER_REQUESTS,
                NewColumnNames.VALVE_INSPECTION_WO_REQ3, OldColumnNames.VALVE_INSPECTION_WO_REQ3,
                StringLengths.VALVE_INSPECTION_WO_REQ3);
            this.RemoveLookupAndAdjustColumns(TableNames.VALVE_INSPECTIONS_NEW, TableNames.VALVE_WORK_ORDER_REQUESTS,
                NewColumnNames.VALVE_INSPECTION_WO_REQ2, OldColumnNames.VALVE_INSPECTION_WO_REQ2,
                StringLengths.VALVE_INSPECTION_WO_REQ2);
            this.RemoveLookupAndAdjustColumns(TableNames.VALVE_INSPECTIONS_NEW, TableNames.VALVE_WORK_ORDER_REQUESTS,
                NewColumnNames.VALVE_INSPECTION_WO_REQ1, OldColumnNames.VALVE_INSPECTION_WO_REQ1,
                StringLengths.VALVE_INSPECTION_WO_REQ1);
            this.RemoveLookupAndAdjustColumns(TableNames.VALVE_INSPECTIONS_NEW, TableNames.VALVE_NORMAL_POSITIONS,
                NewColumnNames.VALVE_INSPECTION_POSITION_FOUND, OldColumnNames.VALVE_INSPECTION_POSITION_FOUND,
                StringLengths.VALVE_INSPECTION_POS_FOUND);
            this.RemoveLookupAndAdjustColumns(TableNames.VALVE_INSPECTIONS_NEW, TableNames.VALVE_NORMAL_POSITIONS,
                NewColumnNames.VALVE_INSPECTION_POSITION_LEFT, OldColumnNames.VALVE_INSPECTION_POSITION_LEFT,
                StringLengths.VALVE_INSPECTION_POS_LEFT);
            this.RemoveLookupAndAdjustColumns(TableNames.VALVE_INSPECTIONS_NEW, TableNames.VALVE_NORMAL_POSITIONS,
                NewColumnNames.VALVE_INSPECTION_NOR_POS, OldColumnNames.VALVE_INSPECTION_NOR_POS,
                StringLengths.VALVE_INSPECTION_NOR_POS);
            this.RemoveLookupAndAdjustColumns(TableNames.VALVE_INSPECTIONS_NEW, TableNames.USERS,
                NewColumnNames.VALVE_INSPECTION_INSPECTED_BY, OldColumnNames.VALVE_INSPECTION_INSPECTED_BY,
                StringLengths.VALVE_INSPECTION_INSPECT_BY, "RecID", "UserName");
            this.RemoveLookupAndAdjustColumns(TableNames.VALVE_INSPECTIONS_NEW, TableNames.INACCESSIBLE_REASONS,
                NewColumnNames.VALVE_INSPECTION_INACCESSIBLE, OldColumnNames.VALVE_INSPECTION_INACCESSIBLE,
                StringLengths.VALVE_INSPECTION_INACCESSIBLE);

            Rename.Table(TableNames.VALVE_INSPECTIONS_NEW).To(TableNames.VALVE_INSPECTIONS_OLD);
            Alter.Table(TableNames.VALVE_INSPECTIONS_OLD)
                 .AddForeignKeyColumn(OldColumnNames.VALVE_INSPECTION_TOWN, TableNames.TOWNS, "TownId");
            Execute.Sql(
                "UPDATE tblNJAWValInspData SET Town = (SELECT Town from tblNJAWValves where tblNJAWValInspData.ValveID = tblNJAWValves.RecID);");

            Execute.Sql(Sql.ROLLBACK_VALVE_INSPECTIONS);

            #endregion

            #region Lookups

            Delete.Table(TableNames.INACCESSIBLE_REASONS);
            Delete.Table(TableNames.VALVE_WORK_ORDER_REQUESTS);
            Delete.Table(TableNames.VALVE_BILLINGS);
            Delete.Table(TableNames.VALVE_CONTROLS);
            Delete.Table(TableNames.VALVE_MANUFACTURERS);
            Delete.Table(TableNames.VALVE_TYPES);
            Delete.Table(TableNames.VALVE_SIZES);
            Delete.Table(TableNames.VALVE_STATUSES);
            Delete.Table(TableNames.VALVE_ZONES);

            #endregion

            Execute.Sql(Sql.ROLLBACK_INDEXES_STATS);
            Execute.Sql(IndexesAndStatisticsForValvesForBug2413.CREATE_INDEXES);
        }
    }
}
