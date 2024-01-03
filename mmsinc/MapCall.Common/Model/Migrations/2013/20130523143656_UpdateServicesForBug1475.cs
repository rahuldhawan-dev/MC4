using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130523143656), Tags("Production")]
    public class UpdateServicesForBug1475 : Migration
    {
        // TODO:
        // DROP ViewNJAWServList, spGetMaxServNumLimited, spGetCategoriesByOpCntr

        #region Constants

        public struct Sql
        {
            public const string UPDATE_COLUMNS =
                                    "INSERT INTO " + Tables.SERVICE_CATEGORIES +
                                    " SELECT Distinct CatOfService FROM tblNJAWService WHERE ISNULL(CatofService,'') <> '' ORDER BY 1;" +
                                    "UPDATE " + Tables.SERVICES + " SET OpCntr = OperatingCenterID FROM " +
                                    Tables.SERVICES +
                                    " S LEFT JOIN OperatingCenters oc ON oc.OperatingCenterCode = S.OpCntr;" +
                                    "UPDATE " + Tables.SERVICES + " SET CatOfService = ServiceCategoryID FROM " +
                                    Tables.SERVICES +
                                    " S LEFT JOIN ServiceCategories sc ON sc.Description = S.CatOfService;" +
                                    "UPDATE " + Tables.OPERATING_CENTERS_SERVICE_CATEGORIES +
                                    " SET CatOfService = ServiceCategoryID FROM " +
                                    Tables.OPERATING_CENTERS_SERVICE_CATEGORIES +
                                    " ocsc LEFT JOIN ServiceCategories sc ON ocsc.CatOfService = sc.Description;" +
                                    "UPDATE " + Tables.OPERATING_CENTERS_SERVICE_CATEGORIES +
                                    " SET OpCntr = OperatingCenterID FROM " +
                                    Tables.OPERATING_CENTERS_SERVICE_CATEGORIES +
                                    " ocsc LEFT JOIN OperatingCenters oc ON ocsc.OpCntr = oc.OperatingCenterCode;",
                                ROLLBACK_COLUMNS =
                                    "UPDATE " + Tables.SERVICES + " SET OpCntr = OperatingCenterCode FROM " +
                                    Tables.SERVICES +
                                    " S LEFT JOIN OperatingCenters oc ON oc.OperatingCenterID = S.OpCntr;" +
                                    "UPDATE " + Tables.SERVICES + " SET CatOfService = Description FROM " +
                                    Tables.SERVICES +
                                    " S LEFT JOIN ServiceCategories sc ON sc.ServiceCategoryID = S.CatOfService;" +
                                    "UPDATE " + Tables.OPERATING_CENTERS_SERVICE_CATEGORIES +
                                    " SET CatOfService = Description FROM " +
                                    Tables.OPERATING_CENTERS_SERVICE_CATEGORIES +
                                    " ocsc LEFT JOIN ServiceCategories sc ON ocsc.CatOfService = sc.ServiceCategoryID;" +
                                    "UPDATE " + Tables.OPERATING_CENTERS_SERVICE_CATEGORIES +
                                    " SET OpCntr = OperatingCenterCode FROM " +
                                    Tables.OPERATING_CENTERS_SERVICE_CATEGORIES +
                                    " ocsc LEFT JOIN OperatingCenters oc ON ocsc.OpCntr = oc.OperatingCenterID;",
                                SERVICES_CLEANUP =
                                    "DELETE FROM " + Tables.SERVICES + " WHERE ISNULL(OpCntr,'') = ''",
                                SERVICES_DROP_CONSTRAINTS =
                                    "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_OpCntr]') AND type = 'D') ALTER TABLE " +
                                    Tables.SERVICES + " DROP CONSTRAINT [DF_tblNJAWService_OpCntr];" +
                                    "IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_CatofService]') AND type = 'D') ALTER TABLE " +
                                    Tables.SERVICES + " DROP CONSTRAINT [DF_tblNJAWService_CatofService]",
                                SERVICES_DROP_INDEXES =
                                    "IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_15_1933965966__K40_K64') DROP INDEX [_dta_index_tblNJAWService_15_1933965966__K40_K64] ON [tblNJAWService];" +
                                    "IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_15_1933965966__K78_K69_K63_K40_K14_K9') DROP INDEX [_dta_index_tblNJAWService_15_1933965966__K78_K69_K63_K40_K14_K9] ON [dbo].[tblNJAWService];" +
                                    "IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_15_1933965966__K78_K69_K63_K56_K65_K40_K14_K9') DROP INDEX [_dta_index_tblNJAWService_15_1933965966__K78_K69_K63_K56_K65_K40_K14_K9] ON [dbo].[tblNJAWService];" +
                                    "IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_5_1933965966__K14_K9_K40_K56_K48_K47_K55_K20_K18_K15') DROP INDEX [_dta_index_tblNJAWService_5_1933965966__K14_K9_K40_K56_K48_K47_K55_K20_K18_K15] ON [dbo].[tblNJAWService];" +
                                    "IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_5_1933965966__K40_K9_K56_K48_K47_K14_K55_K20_K18_K15') DROP INDEX [_dta_index_tblNJAWService_5_1933965966__K40_K9_K56_K48_K47_K14_K55_K20_K18_K15] ON [dbo].[tblNJAWService];" +
                                    "IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_5_1933965966__K47_K18_K15_K48_K56_K9_K40_K14_K55_K20') DROP INDEX [_dta_index_tblNJAWService_5_1933965966__K47_K18_K15_K48_K56_K9_K40_K14_K55_K20] ON [dbo].[tblNJAWService];" +
                                    "IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_5_1933965966__K9_K40_K48_K47_K14_K55_K20_K18_K15_K56') DROP INDEX [_dta_index_tblNJAWService_5_1933965966__K9_K40_K48_K47_K14_K55_K20_K18_K15_K56] ON [dbo].[tblNJAWService];" +
                                    "IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'IDX_CatofService_OpCntr_RecID') DROP INDEX [IDX_CatofService_OpCntr_RecID] ON [tblNJAWService];" +
                                    "IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'IDX_OpCntr_CatofService_PrevServiceMatl_PrevServiceSize') DROP INDEX [IDX_OpCntr_CatofService_PrevServiceMatl_PrevServiceSize] ON [tblNJAWService];" +
                                    "IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'IDX_RetireDate') DROP INDEX [IDX_RetireDate] ON [tblNJAWService];" +
                                    "IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'IDX_tblNJAWService_SmartGrowth_ServMatl_SizeofService_Town_RecID_CatofService_OpCntr') DROP INDEX [IDX_tblNJAWService_SmartGrowth_ServMatl_SizeofService_Town_RecID_CatofService_OpCntr] ON [tblNJAWService];" +
                                    "IF EXISTS (SELECT 1 FROM sysindexes where [name] = '_dta_stat_1933965966_9_78_69_63_40') DROP STATISTICS tblNJAWService._dta_stat_1933965966_9_78_69_63_40;" +
                                    "IF EXISTS (SELECT 1 FROM sysindexes where [name] = '_dta_stat_1933965966_69_9_78_65_63_56_40') DROP STATISTICS tblNJAWService._dta_stat_1933965966_69_9_78_65_63_56_40;" +
                                    "IF EXISTS (SELECT 1 FROM sysindexes where [name] = '_dta_stat_1933965966_78_69_63_40_14_9_65_56') DROP STATISTICS tblNJAWService._dta_stat_1933965966_78_69_63_40_14_9_65_56;" +
                                    "IF EXISTS (SELECT 1 FROM sysindexes where [name] = '_dta_stat_1933965966_64_40') DROP STATISTICS tblNJAWService._dta_stat_1933965966_64_40;" +
                                    "IF EXISTS (SELECT 1 FROM sysindexes where [name] = 'STAT_tblNJAWService_RecID_OpCntr_CatofService') DROP STATISTICS tblNJAWService.STAT_tblNJAWService_RecID_OpCntr_CatofService;" +
                                    "IF EXISTS (SELECT 1 FROM sysindexes where [name] = 'STAT_tblNJAWService_Town_OpCntr_CatofService_PrevServiceMatl_PrevServiceSize') DROP STATISTICS tblNJAWService.STAT_tblNJAWService_Town_OpCntr_CatofService_PrevServiceMatl_PrevServiceSize;" +
                                    "IF EXISTS (SELECT 1 FROM sysindexes where [name] = 'STAT_tblNJAWService_1933965966_20_9_40_56_48_47_14') DROP STATISTICS tblNJAWService.STAT_tblNJAWService_1933965966_20_9_40_56_48_47_14;" +
                                    "IF EXISTS (SELECT 1 FROM sysindexes where [name] = 'STAT_tblNJAWService_1933965966_48_9_40_56_47_14_55_20') DROP STATISTICS tblNJAWService.STAT_tblNJAWService_1933965966_48_9_40_56_47_14_55_20;" +
                                    "IF EXISTS (SELECT 1 FROM sysindexes where [name] = 'STAT_tblNJAWService_1933965966_55_20_14_15_56_9_40_48_47') DROP STATISTICS tblNJAWService.STAT_tblNJAWService_1933965966_55_20_14_15_56_9_40_48_47;" +
                                    "IF EXISTS (SELECT 1 FROM sysindexes where [name] = 'STAT_tblNJAWService_1933965966_15_18_56_9_40_48_47_14_55') DROP STATISTICS tblNJAWService.STAT_tblNJAWService_1933965966_15_18_56_9_40_48_47_14_55;" +
                                    "IF EXISTS (SELECT 1 FROM sysindexes where [name] = 'STAT_tblNJAWService_1933965966_18_56_9_40_48_47_14_55_20_15') DROP STATISTICS tblNJAWService.STAT_tblNJAWService_1933965966_18_56_9_40_48_47_14_55_20_15;" +
                                    "IF EXISTS (SELECT 1 FROM sysindexes where [name] = 'STAT_tblNJAWService_OpCntr_Town_ServMatl') DROP STATISTICS tblNJAWService.STAT_tblNJAWService_OpCntr_Town_ServMatl;" +
                                    "IF EXISTS (SELECT 1 FROM sysindexes where [name] = 'STAT_tblNJAWService_OpCntr_SizeofService_ServMatl_Town_RecID') DROP STATISTICS tblNJAWService.STAT_tblNJAWService_OpCntr_SizeofService_ServMatl_Town_RecID;" +
                                    "IF EXISTS (SELECT 1 FROM sysindexes where [name] = '_dta_stat_1933965966_9_78_69_63_56_14') DROP STATISTICS tblNJAWService._dta_stat_1933965966_9_78_69_63_56_14;" +
                                    "IF EXISTS (SELECT 1 FROM sysindexes where [name] = '_dta_stat_1933965966_65_9_78_69_63_56_14') DROP STATISTICS tblNJAWService._dta_stat_1933965966_65_9_78_69_63_56_14;" +
                                    "IF EXISTS (SELECT 1 FROM sysindexes where [name] = 'STAT_tblNJAWService_1933965966_48_47_18_15_56_9') DROP STATISTICS tblNJAWService.STAT_tblNJAWService_1933965966_48_47_18_15_56_9;" +
                                    "IF EXISTS (SELECT 1 FROM sysindexes where [name] = 'STAT_tblNJAWService_1933965966_56_48_47_14_55_20_9') DROP STATISTICS tblNJAWService.STAT_tblNJAWService_1933965966_56_48_47_14_55_20_9;" +
                                    "IF EXISTS (SELECT 1 FROM sysindexes where [name] = 'STAT_tblNJAWService_RecID_CatofService_ServMatl_SizeofService') DROP STATISTICS tblNJAWService.STAT_tblNJAWService_RecID_CatofService_ServMatl_SizeofService;" +
                                    "IF EXISTS (SELECT 1 FROM sysindexes where [name] = 'STAT_tblNJAWService_CatofService_Town_ServMatl_RecID') DROP STATISTICS tblNJAWService.STAT_tblNJAWService_CatofService_Town_ServMatl_RecID;" +
                                    "IF EXISTS (SELECT 1 FROM sysindexes where [name] = '_dta_stat_1933965966_63_9_78_65') DROP STATISTICS tblNJAWService._dta_stat_1933965966_63_9_78_65;",
                                SERVICES_ADD_CONSTRAINTS =
                                    "IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_OpCntr]') AND type = 'D') ALTER TABLE [tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_OpCntr]  DEFAULT ('') FOR [OpCntr];" +
                                    "IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_CatofService]') AND type = 'D') ALTER TABLE [tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_CatofService]  DEFAULT ('') FOR [CatofService];",
                                SERVICES_ADD_INDEXES =
                                    "IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_15_1933965966__K40_K64') CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWService_15_1933965966__K40_K64] ON [tblNJAWService]([OpCntr] ASC,[ServNum] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON);" +
                                    "IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_15_1933965966__K78_K69_K63_K40_K14_K9') CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWService_15_1933965966__K78_K69_K63_K40_K14_K9] ON [tblNJAWService] ([Town] ASC,[SizeofService] ASC,[ServMatl] ASC,[OpCntr] ASC,[DateInstalled] ASC,[CatofService] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON);" +
                                    "IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_15_1933965966__K78_K69_K63_K56_K65_K40_K14_K9') CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWService_15_1933965966__K78_K69_K63_K56_K65_K40_K14_K9] ON [tblNJAWService] ([Town] ASC,[SizeofService] ASC,[ServMatl] ASC,[RecID] ASC,[SmartGrowth] ASC,[OpCntr] ASC,[DateInstalled] ASC,[CatofService] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON);" +
                                    "IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_5_1933965966__K14_K9_K40_K56_K48_K47_K55_K20_K18_K15') CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWService_5_1933965966__K14_K9_K40_K56_K48_K47_K55_K20_K18_K15] ON [tblNJAWService] ( [DateInstalled] ASC, [CatofService] ASC, [OpCntr] ASC, [RecID] ASC, [PermitSentDate] ASC, [PermitRcvdDate] ASC, [PurpInstal] ASC, [InActSrv] ASC, [DevServD] ASC, [DateIssuedtoField] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON);" +
                                    "IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_5_1933965966__K40_K9_K56_K48_K47_K14_K55_K20_K18_K15') CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWService_5_1933965966__K40_K9_K56_K48_K47_K14_K55_K20_K18_K15] ON [tblNJAWService] ( [OpCntr] ASC, [CatofService] ASC, [RecID] ASC, [PermitSentDate] ASC, [PermitRcvdDate] ASC, [DateInstalled] ASC, [PurpInstal] ASC, [InActSrv] ASC, [DevServD] ASC, [DateIssuedtoField] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON);" +
                                    "IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_5_1933965966__K47_K18_K15_K48_K56_K9_K40_K14_K55_K20') CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWService_5_1933965966__K47_K18_K15_K48_K56_K9_K40_K14_K55_K20] ON [tblNJAWService] ( [PermitRcvdDate] ASC, [DevServD] ASC, [DateIssuedtoField] ASC, [PermitSentDate] ASC, [RecID] ASC, [CatofService] ASC, [OpCntr] ASC, [DateInstalled] ASC, [PurpInstal] ASC, [InActSrv] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON);" +
                                    "IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_5_1933965966__K9_K40_K48_K47_K14_K55_K20_K18_K15_K56') CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWService_5_1933965966__K9_K40_K48_K47_K14_K55_K20_K18_K15_K56] ON [tblNJAWService] ( [CatofService] ASC, [OpCntr] ASC, [PermitSentDate] ASC, [PermitRcvdDate] ASC, [DateInstalled] ASC, [PurpInstal] ASC, [InActSrv] ASC, [DevServD] ASC, [DateIssuedtoField] ASC, [RecID] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON);" +
                                    "IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'IDX_CatofService_OpCntr_RecID') CREATE NONCLUSTERED INDEX [IDX_CatofService_OpCntr_RecID] ON [tblNJAWService] ( [CatofService] ASC, [OpCntr] ASC, [RecID] ASC, [SmartGrowth] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON);" +
                                    "IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'IDX_OpCntr_CatofService_PrevServiceMatl_PrevServiceSize') CREATE NONCLUSTERED INDEX [IDX_OpCntr_CatofService_PrevServiceMatl_PrevServiceSize] ON [tblNJAWService] ( [OpCntr] ASC, [CatofService] ASC, [PrevServiceMatl] ASC, [PrevServiceSize] ASC, [RetireDate] ASC, [Town] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON);" +
                                    "IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'IDX_RetireDate') CREATE NONCLUSTERED INDEX [IDX_RetireDate] ON [tblNJAWService] ( [RetireDate] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON);" +
                                    "IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'IDX_tblNJAWService_SmartGrowth_ServMatl_SizeofService_Town_RecID_CatofService_OpCntr') CREATE NONCLUSTERED INDEX [IDX_tblNJAWService_SmartGrowth_ServMatl_SizeofService_Town_RecID_CatofService_OpCntr] ON [tblNJAWService] ( [SmartGrowth] ASC, [ServMatl] ASC, [SizeofService] ASC, [Town] ASC, [RecID] ASC, [CatofService] ASC, [OpCntr] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)",
                                REMOVE_STORED_PROCEDURES =
                                    "IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetMaxServNumLimited]') AND type in (N'P', N'PC')) BEGIN DROP PROCEDURE [spGetMaxServNumLimited] END;" +
                                    "IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetCompletedServiceCounts]') AND type in (N'P', N'PC')) BEGIN DROP PROCEDURE [spGetCompletedServiceCounts] END;" +
                                    "IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetOpenContractorSvcs]') AND type in (N'P', N'PC')) BEGIN DROP PROCEDURE [spGetOpenContractorSvcs] END;" +
                                    "IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetPendingSvcsByTown]') AND type in (N'P', N'PC')) BEGIN DROP PROCEDURE [spGetPendingSvcsByTown] END;" +
                                    "IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetBilling]') AND type in (N'P', N'PC')) BEGIN DROP PROCEDURE [spGetBilling] END;" +
                                    "IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetServiceHistory]') AND type in (N'P', N'PC')) BEGIN DROP PROCEDURE [spGetServiceHistory] END;" +
                                    "IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetCategoriesByServNum]') AND type in (N'P', N'PC')) BEGIN DROP PROCEDURE [spGetCategoriesByServNum] END;" +
                                    "IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetTDCounts]') AND type in (N'P', N'PC')) BEGIN DROP PROCEDURE [spGetTDCounts] END;" +
                                    "IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetSewerTDCount]') AND type in (N'P', N'PC')) BEGIN DROP PROCEDURE [spGetSewerTDCount] END;" +
                                    "IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetContractorLoading]') AND type in (N'P', N'PC')) BEGIN DROP PROCEDURE [spGetContractorLoading] END;",
                                GRANT_PERMISSIONS =
                                    "GRANT ALL ON spGetMaxServNumLimited TO MCUser;" +
                                    "GRANT ALL ON spGetCompletedServiceCounts TO MCUser;" +
                                    "GRANT ALL ON spGetOpenContractorSvcs TO MCUser;" +
                                    "GRANT ALL ON spGetPendingSvcsByTown TO MCUser;" +
                                    "GRANT ALL ON spGetBilling TO MCUser;" +
                                    "GRANT ALL ON spGetServiceHistory TO MCUser;" +
                                    "GRANT ALL ON spGetCategoriesByServNum TO MCUser;" +
                                    "GRANT ALL ON spGetTDCounts TO MCUser;" +
                                    "GRANT ALL ON spGetSewerTDCount TO MCUser;" +
                                    "GRANT ALL ON spGetContractorLoading TO MCUser",
                                FIX_STORED_PROCEDURES = @"
                                    IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[getRptServicesInstalled2]') AND type in (N'P', N'PC'))
                                    BEGIN
                                    EXEC dbo.sp_executesql @statement = N'
                                    ALTER PROCEDURE [getRptServicesInstalled2] (@year int)
                                    AS
                                    Declare @tblTable TABLE(catofservice varchar(40), smartgrowth varchar(3), Total int, MonthNum int, [Month] varchar(10), [Year] int)
                                    Declare @tblOpCntrs TABLE(opcntr varchar(4))
                                    Declare @tblServices TABLE(catofservice varchar(40))
                                    Declare @tblSmartGrowth TABLE(smartgrowth varchar(3))

                                    Insert into @tblOpcntrs select distinct opcntr from tblnjawservice where isNull(opCntr,'''') <> '''' order by opcntr
                                    Insert into @tblServices select distinct #1.catofservice from tblnjawservice #1 where isNull(CatofService,'''') in (Select ServiceCategoryID from ServiceCategories where Description in (''Fire Service Installation'',''Irrigation New'',''Sewer Service New'',''Water Service New Commercial'', ''Water Service New Domestic'')) order by 1
                                    Insert Into @tblSmartGrowth select distinct CASE WHEN upper(isNull(smartgrowth, '''')) = ''YES'' then ''YES'' else ''NO'' end from tblNJAWService where isNull(smartgrowth,'''') <> '''' order by CASE WHEN upper(isNull(smartgrowth, '''')) = ''YES'' then ''YES'' else ''NO'' end desc

                                    select (select OperatingCenterCode from OperatingCenters where OperatingCenterID = #oc.opcntr) as [opCntr], (select [Description] from ServiceCategories where ServiceCategoryID = #s.catofservice) as [catofservice], #sg.smartgrowth, left(DATENAME(M,Cast(Dnum as varchar(2)) + ''/01/2000''),3) as ''MonthName''
	                                    , (
			                                    Select count(*) from tblnjawservice #1 
				                                    where #1.opcntr = #oc.opcntr 
					                                    and #1.catofservice = #s.catofservice 
					                                    and CASE WHEN upper(isNull(#1.smartgrowth, '''')) = ''YES'' then ''YES'' else ''NO'' end = #sg.smartgrowth
					                                    and month(#1.dateinstalled) = Dnum 
					                                    and year(#1.dateinstalled) = @year
					                                    and upper(IsNull(DevServD,'''')) <> ''YES''
		                                    ) as ''total''
	                                    FROM @tblOpCntrs #oc
	                                    CROSS JOIN @tblServices #s
	                                    CROSS JOIN @tblSmartGrowth #sg
	                                    CROSS JOIN dnum 
	                                    where (
			                                    Select count(*) from tblnjawservice #1 
				                                    where #1.opcntr = #oc.opcntr 
					                                    and #1.catofservice = #s.catofservice 
					                                    and CASE WHEN upper(isNull(#1.smartgrowth, '''')) = ''YES'' then ''YES'' else ''NO'' end = #sg.smartgrowth
					                                    and year(#1.dateinstalled) = @year
					                                    and upper(IsNull(DevServD,'''')) <> ''YES''
		                                    ) > 0
	                                    order by 1,dnum' 
                                    END;
                                    IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RptServicesRenewed]') AND type in (N'P', N'PC'))
                                    BEGIN
                                    EXEC dbo.sp_executesql @statement = N'
                                    /****** Object:  StoredProcedure [dbo].[RptServicesRenewed]    Script Date: 05/31/2011 11:16:45 ******/
                                    ALTER PROCEDURE [RptServicesRenewed] (@startDate datetime, @endDate dateTime, @opCntr varchar(4), @devdriv varchar(10))
                                    AS

                                    IF (Len(@opCntr) > 0)
	                                    BEGIN
		                                    select OC.OperatingCenterCode as OpCntr, Towns.town, Towns.TownID as RecID, SC.Description as CatOfService, isNull(SizeOfService,0) as SizeOfService, Sum(isNull(LengthService ,0)) as ''LengthService'', Count(SizeOfService) as ''Total'', 
			                                    Count(case when (smartgrowth=''Yes'') then 1 else null end) as SmartGrowth
			                                    from tblNJAWService
			                                    LEFT JOIN Towns on Towns.TownID = tblNJAWservice.town
			                                    LEFT JOIN OperatingCenters OC on OC.OperatingCenterID = tblNJAWService.OpCntr
			                                    LEFT JOIN ServiceCategories SC ON SC.ServiceCategoryID = tblNJAWService.CatofService
		                                    where 
			                                    (charindex(''RENEW'', upper(isNull(SC.Description,''''))) > 0 
			                                    OR 
			                                    charindex(''INSTALL METER SET'', upper(isNull(SC.Description,''''))) > 0)
		                                    and OperatingCenterID = @opCntr
		                                    and DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
		                                    and devServD = isNull(@devdriv, devServD)
			                                    group by OC.OperatingCenterCode, Towns.town, Towns.TownID, SC.Description, SizeofService
			                                    order by OC.OperatingCenterCode, Towns.town, SC.Description, SizeofService	
	                                    END
                                    ELSE
	                                    BEGIN
		                                    select OC.OperatingCenterCode as OpCntr, Towns.town, Towns.TownID as RecID, SC.Description as CatOfService, isNull(SizeOfService,0) as SizeOfService, Sum(isNull(LengthService ,0)) as ''LengthService'', Count(SizeOfService) as ''Total'', 
			                                    Count(case when (smartgrowth=''Yes'') then 1 else null end) as SmartGrowth
			                                    from tblNJAWService
			                                    LEFT JOIN Towns on Towns.TownID = tblNJAWservice.town
			                                    LEFT JOIN OperatingCenters OC on OC.OperatingCenterID = tblNJAWService.OpCntr
			                                    LEFT JOIN ServiceCategories SC ON SC.ServiceCategoryID = tblNJAWService.CatofService
			                                    where
				                                    (charindex(''RENEW'', upper(isNull(SC.Description,''''))) > 0 
				                                    OR 
				                                    charindex(''INSTALL METER SET'', upper(isNull(SC.Description,''''))) > 0)
			                                    and DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
			                                    and devServD = isNull(@devdriv, devServD)
			                                    group by OC.OperatingCenterCode, Towns.town, Towns.TownID, SC.Description, SizeofService
			                                    order by OC.OperatingCenterCode, Towns.town, SC.Description, SizeofService
	                                    END
                                    ' 
                                    END;
                                    IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RptServicesInstalledDetail]') AND type in (N'P', N'PC'))
                                    BEGIN
                                    EXEC dbo.sp_executesql @statement = N'
                                    /****** Object:  StoredProcedure [dbo].[RptServicesInstalledDetail]    Script Date: 05/31/2011 11:25:30 ******/
                                    ALTER PROCEDURE [RptServicesInstalledDetail] (@startDate datetime, @endDate dateTime, @opCntr varchar(4),  @devdriv varchar(10))
                                    AS
                                    IF (Len(@opCntr) > 0)
	                                    BEGIN
		                                    select OperatingCenterCode as OpCntr, Towns.town, SC.Description as CatOfService, isNull(SizeOfService,0) as SizeOfService, LengthService as ''LengthService'', ServNum, case when (isnull(smartgrowth,'''')=''YES'') then ''Yes'' end as [SmartGrowth]
			                                    from tblNJAWService S
			                                    LEFT JOIN Towns on Towns.TownID = S.town
			                                    LEFT JOIN OperatingCenters OC on OC.OperatingCenterID = S.OpCntr
			                                    LEFT JOIN ServiceCategories SC on SC.ServiceCategoryID = S.CatOfService
			                                    where (charindex(''INSTALLATION'', SC.Description) > 0 OR charindex('' NEW'', SC.Description) > 0)
				                                    and oc.OperatingCenterID = @opCntr
				                                    and DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
				                                    and devServD = isNull(@devdriv, devServD)
			                                    order by OpCntr, Towns.town, SC.Description, SizeofService	
	                                    END
                                    ELSE
	                                    BEGIN
		                                    select OperatingCenterCode as OpCntr, Towns.town, SC.Description as CatOfService, isNull(SizeOfService,0) as SizeOfService, LengthService as ''LengthService'', ServNum, case when (isnull(smartgrowth,'''')=''YES'') then ''Yes'' end as [SmartGrowth] 
			                                    from tblNJAWService S
			                                    LEFT JOIN Towns on Towns.TownID = S.town
			                                    LEFT JOIN OperatingCenters OC on OC.OperatingCenterID = S.OpCntr
			                                    LEFT JOIN ServiceCategories SC on SC.ServiceCategoryID = S.CatOfService
			                                    where (charindex(''INSTALLATION'', SC.Description) > 0 OR charindex('' NEW'', SC.Description) > 0)
				                                    and DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
				                                    and devServD = isNull(@devdriv, devServD)
			                                    order by OpCntr, Towns.town, SC.Description, SizeofService
	                                    END
                                    ' 
                                    END;
                                    IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RptServicesRenewedRetirementsDetail]') AND type in (N'P', N'PC'))
                                    BEGIN
                                    EXEC dbo.sp_executesql @statement = N'
                                    ALTER Procedure [RptServicesRenewedRetirementsDetail] (@startDate datetime, @endDate datetime, @town int, @CatOfService varchar(40), @SizeOfService varchar(10))
                                    AS
                                    select 
                                    Year(OrigInstDate) as ''Year Installed'', 
                                    PrevServiceMatl, 
                                    LengthService as [Length], --Sum(LengthService) as [Length], 
                                    servnum,--Count(OrigInstDate) as ''Total''
                                    TaskNum1 as TaskNumber	
                                    from 
                                    tblNJAWService
                                    left join 
                                        ServiceCategories SC on SC.ServiceCategoryID = tblNJAWService.CatOfService
                                    where 
                                    year(OrigInstDate) > 1900
                                    and 
                                    DateInstalled >= @startDate and DateInstalled <= @endDate
                                    and 
                                    SC.Description = @CatOfService
                                    and 
                                    SizeOfService = @SizeOfService
                                    and 
                                    Town = @Town
                                    --group by 
                                    --	year(OrigInstDate), PrevServiceMatl

                                    ' 
                                    END;
                                    EXEC dbo.sp_executesql @statement = N'
                                    /****** Object:  StoredProcedure [dbo].[RptServicesRenewedByYear]    Script Date: 05/31/2011 11:15:04 ******/
                                    ALTER PROCEDURE [RptServicesRenewedByYear] (@startYear int, @endYear int, @opCntr varchar(4))
                                    AS
                                    --Declare @startYear int
                                    --Declare @endYear int
                                    --Declare @opCntr varchar(3)
                                    --Select @startYear = 1920
                                    --Select @endYear = 2007
                                    --Select @opCntr = ''NJ7''

                                    if (Len(@opCntr) > 0)
	                                    BEGIN
		                                    Select OperatingCenterCode as OpCntr, Towns.town, Year(DateInstalled) as [Year], Count(*) as [Total]
			                                    from tblNJAWService S
			                                    LEFT JOIN Towns on Towns.TownID = S.town
                                                LEFT JOIN OperatingCenters OC on OC.OperatingCenterID = S.OpCntr
                                                LEFT JOIN ServiceCategories SC on SC.ServiceCategoryID = S.CatOfService

			                                    where 
					                                    S.OpCntr = @opCntr AND
					                                    charindex(''RENEW'', SC.Description) > 0 AND
				                                    year(DateInstalled) >= @startYear and year(DateInstalled) <= @endYear
			                                    group by OperatingCenterCode, Towns.town, Year(DateInstalled)
			                                    order by OperatingCenterCode, Towns.town, Year(DateInstalled)
	                                    END
                                    ELSE
	                                    BEGIN
		                                    Select OperatingCenterCode as OpCntr, Towns.town, Year(DateInstalled) as [Year], Count(*) as [Total]
			                                    from tblNJAWService S
			                                    LEFT JOIN Towns on Towns.TownID = S.town
                                                LEFT JOIN OperatingCenters OC on OC.OperatingCenterID = S.OpCntr
                                                LEFT JOIN ServiceCategories SC on SC.ServiceCategoryID = S.CatOfService
			                                    where 
				                                    charindex(''RENEW'', sc.Description) > 0 AND	
				                                    year(DateInstalled) >= @startYear and year(DateInstalled) <= @endYear
			                                    group by OperatingCenterCode, Towns.town, Year(DateInstalled)
			                                    order by OperatingCenterCode, Towns.town, Year(DateInstalled)
	                                    END
                                    ';
                                    IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RptFormNewWaterServiceInquiry]') AND type in (N'P', N'PC'))
                                    BEGIN
                                    EXEC dbo.sp_executesql @statement = N'
                                    /****** Object:  StoredProcedure [dbo].[RptFormNewWaterServiceInquiry]    Script Date: 05/31/2011 11:32:40 ******/
                                    ALTER Procedure [RptFormNewWaterServiceInquiry] (@RecID int)
                                    AS
                                    SELECT 
	                                    S.Agreement,
	                                    S.AmntRcvd,
	                                    S.ApplApvd,
	                                    S.ApplRcvd,
	                                    S.ApplSent,
	                                    S.Apt,
	                                    S.Block,
	                                    S.BSDWPermit,
	                                    SC.Description as CatofService,
	                                    S.ContactDate,
	                                    S.CrossStreet,
	                                    S.DateClosed, 
	                                    CASE WHEN S.DateInstalled < ''1/1/1901'' THEN NULL ELSE S.DateInstalled END As DateInstalled,
	                                    S.DateIssuedtoField,
	                                    S.Development,
	                                    S.DevServD,
	                                    S.Initiator,
	                                    S.InspDate,
	                                    S.InspSignoffReady,
	                                    S.InstCost, 
	                                    S.InstInv,
	                                    S.InstInvDate,
	                                    S.JobNotes,
	                                    S.Lat,
	                                    S.Lon,
	                                    S.Lot,
	                                    S.MailPhoneNum,
	                                    S.MailStName,
	                                    S.MailStNum,
	                                    S.MailTown,
	                                    S.MailState,
	                                    S.MailZip,
	                                    S.Name,
	                                    S.MeterSetReq,
	                                    S.OpCntr,
	                                    S.OrdCreationDate, 
	                                    CASE WHEN S.OrigInstDate < ''1/1/1901'' THEN NULL ELSE S.OrigInstDate END As OrigInstDate,
	                                    S.ParentTaskNum,
	                                    S.PayRefNum,
	                                    S.PermitExpDate,
	                                    S.PermitNum,
	                                    S.PermitRcvdDate,
	                                    S.PermitSentDate,
	                                    S.PermitType,
	                                    S.PhoneNum,
	                                    S.PremNum,
	                                    S.PrevServiceSize,
	                                    S.Priority,
	                                    S.PurpInstal, 
	                                    S.RecID,
	                                    S.RetireAcct,
	                                    S.RetireDate,
	                                    S.RoadOpenFee,
	                                    S.ServInstFee,
	                                    S.ServMatl,
	                                    S.ServNum,
	                                    Cat.ServType, 
	                                    CASE WHEN isNull(S.SmartGrowth,'''') = ''YES'' THEN ''X'' ELSE '''' END as ''SmartGrowth'',
	                                    S.SGMethodUsed,
	                                    S.SGCost,
	                                    S.SizeofMain,
	                                    S.SizeofService, 
	                                    S.SizeofTap,
	                                    S.State,
	                                    S.StreetMatl,
	                                    S.StName,
	                                    S.StNum,
	                                    S.TapOrdNote,
	                                    S.TaskNum1,
	                                    S.TaskNum2,
	                                    S.Town,
	                                    S.TwnSection,
	                                    S.TypeMain,
	                                    S.WorkIssuedto,
	                                    S.Zip,
	                                    UPPER(SC.Description) As UCatOfService, 
	                                    T.DistrictID As TDistrictID,
	                                    T.County As TCounty,
	                                    T.Town As TTown, 
	                                    St.StreetPrefix As StStreetPrefix,
	                                    St.StreetName  As StStreetName,
	                                    St.StreetSuffix As StStreetSuffix,
	                                    (Select Town from Towns where Towns.TownID = St.TownID) As StTown,
	                                    States.Abbreviation As StSt,
	                                    St.FullStName As StFullStName, 
	                                    Cntr.ServContactNum As CServContactNum,
	                                    Cntr.CSNum As CCsNum,
	                                    Cntr.CoInfo As CCoInfo,
	                                    Cntr.OperatingCenterName AS COpCntrName, 
	                                    Cntr.MailCo AS CMailCo,
	                                    Cntr.MailAdd AS CMailAdd,
	                                    Cntr.MailCSZ AS CMailCSZ,
	                                    Cntr.FaxNum AS CFaxNum,
	                                    S.Fax
	                                    ,isNull(Cntr.MailCo,'''') + '', '' + isNull(Cntr.MailAdd, '''') + '', '' + isNull(Cntr.MailCSZ,'''') as cMailFullAdd
	                                    FROM tblNJAWService S 
		                                    LEFT JOIN Towns T ON S.Town = T.TownID
		                                    LEFT JOIN States ON States.StateID = T.StateID
		                                    LEFT JOIN Streets St ON St.StreetID = S.StName 
		                                    LEFT JOIN OperatingCenters Cntr ON S.OpCntr = Cntr.OperatingCenterID
		                                    LEFT JOIN tblNJAWCategoryService Cat ON S.CatOfService = Cat.CatOfService AND S.OpCntr = Cat.OpCntr
                                            LEFT JOIN ServiceCategories SC on SC.ServiceCategoryID = S.CatOfService
	                                    WHERE S.RecID = @RecID
                                    ' 
                                    END;",
                                RESTORE_STORED_PROCEDURES =
                                    @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetMaxServNumLimited]') AND type in (N'P', N'PC'))
                                        BEGIN
                                        EXEC dbo.sp_executesql @statement = N'/*	Gets max service number in the
	                                        database based on the Op Center
	                                        and limiting factors specific to the Op Center.	*/

                                        CREATE PROCEDURE [spGetMaxServNumLimited] @OpCntr varchar(3) AS
	                                        IF ( @OpCntr = ''NJ3'' ) 
		                                        SELECT CAST(MAX(ServNum) AS int) AS MaxServNum
		                                        FROM tblNJAWService
		                                        WHERE  (ServNum < 87000000) AND (OpCntr = ''NJ3'')
	                                        ELSE IF ( @OpCntr = ''NJ4'' ) 
		                                        SELECT CAST(MAX(ServNum) AS int) AS MaxServNum
		                                        FROM tblNJAWService
		                                        WHERE (ServNum < 64000) AND (OpCntr = ''NJ4'')
	                                        ELSE IF (@OpCntr = ''NJ5'' ) 
		                                        SELECT CAST(MAX(ServNum) AS int) AS MaxServNum
		                                        FROM tblNJAWService
		                                        WHERE (ServNum < 499999) AND (OpCntr = ''NJ5'')
	                                        ELSE
		                                        SELECT CAST(MAX(ServNum) AS int) AS MaxServNum
		                                        FROM tblNJAWService
		                                        WHERE OpCntr = @OpCntr' 
                                        END;
                                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetCompletedServiceCounts]') AND type in (N'P', N'PC'))
                                    BEGIN
                                    EXEC dbo.sp_executesql @statement = N'/*	Retrieves service counts from the tables.
	                                    Replace * with appropriate field name.
	                                    last update
	                                    11-22-2006 cmm		*/

                                    CREATE PROCEDURE [spGetCompletedServiceCounts]
	                                    @StartDate smalldatetime,
	                                    @EndDate smalldatetime,
	                                    @OpCntr varchar(3)

                                    AS

                                    SELECT	FireServiceCount = 
	                                    (
		                                    SELECT COUNT(CatOfService)
		                                    FROM tblNJAWService
		                                    WHERE [CatOfService] LIKE ''Fire Service%''
		                                    AND [DateInstalled] >= @StartDate
		                                    AND [DateInstalled] <= @EndDate
		                                    AND [OpCntr] = @OpCntr
		                                    AND [InActSrv] <> ''ON'' 
	                                    ),
	                                    IncreaseCount =
	                                    (
		                                    SELECT COUNT(CatOfService)
		                                    FROM tblNJAWService
		                                    WHERE [CatOfService] = ''Water Service Increase Size''
		                                    AND [DateInstalled] >= @StartDate
		                                    AND [DateInstalled] <= @EndDate
		                                    AND [OpCntr] = @OpCntr
		                                    AND [InActSrv] <> ''ON'' 
	                                    ),
	                                    IrrigationCount =
	                                    (
		                                    SELECT COUNT(CatOfService)
		                                    FROM tblNJAWService
		                                    WHERE [CatOfService] LIKE ''Irrigation%''
		                                    AND [DateInstalled] >= @StartDate
		                                    AND [DateInstalled] <= @EndDate
		                                    AND [OpCntr] = @OpCntr
		                                    AND [InActSrv] <> ''ON'' 
	                                    ),
	                                    MeterSetCount =
	                                    (
		                                    SELECT COUNT(CatOfService)
		                                    FROM tblNJAWService
		                                    WHERE [CatOfService] LIKE ''%Meter Set%''
		                                    AND [DateInstalled] >= @StartDate
		                                    AND [DateInstalled] <= @EndDate
		                                    AND [OpCntr] = @OpCntr
		                                    AND [InActSrv] <> ''ON'' 
	                                    ),
	                                    CommercialCount =
	                                    (
		                                    SELECT COUNT(CatOfService)
		                                    FROM tblNJAWService
		                                    WHERE [CatOfService] = ''Water Service New Commercial''
		                                    AND [DateInstalled] >= @StartDate
		                                    AND [DateInstalled] <= @EndDate
		                                    AND [OpCntr] = @OpCntr
		                                    AND [InActSrv] <> ''ON''
	                                    ),
	                                    DomesticCount = 
	                                    (
		                                    SELECT COUNT(CatOfService)
		                                    FROM tblNJAWService
		                                    WHERE [CatOfService] = ''Water Service New Domestic''
		                                    AND [DateInstalled] >= @StartDate
		                                    AND [DateInstalled] <= @EndDate
		                                    AND [OpCntr] = @OpCntr
		                                    AND [InActSrv] <> ''ON'' 
	                                    ),
	                                    OnePointCount = 
	                                    (
		                                    SELECT COUNT(CatOfService)
		                                    FROM tblNJAWService
		                                    WHERE [CatOfService] = ''Water Measurement Only''
		                                    AND [DateInstalled]>= @StartDate
		                                    AND [DateInstalled] <= @EndDate
		                                    AND [OpCntr] = @OpCntr
		                                    AND [InActSrv]<>''ON''
	                                    ),
	                                    ReconnectCount = 
	                                    (
		                                    SELECT COUNT(CatOfService)
		                                    FROM tblNJAWService
		                                    WHERE [CatOfService] = ''Water Reconnect''
		                                    AND [DateInstalled] >= @StartDate
		                                    AND [DateInstalled] <= @EndDate
		                                    AND [OpCntr] = @OpCntr
		                                    AND [InActSrv] <> ''ON''
	                                    ),
	                                    RetireServiceCount = 
	                                    (
		                                    SELECT COUNT(CatOfService)
		                                    FROM tblNJAWService
		                                    WHERE [CatOfService] = ''Water Retire Service Only''
		                                    AND [DateInstalled] >= @StartDate
		                                    AND [DateInstalled] <= @EndDate
		                                    AND [OpCntr] = @OpCntr
		                                    AND [InActSrv] <> ''ON''
	                                    ),
	                                    ServiceRenewalCount =
	                                    (
		                                    SELECT COUNT(CatOfService)
		                                    FROM tblNJAWService
		                                    WHERE [CatOfService] = ''Water Service Renewal''
		                                    AND [DateInstalled] >= @StartDate
		                                    AND [DateInstalled] <= @EndDate
		                                    AND [OpCntr] = @OpCntr
		                                    AND [InActSrv]<>''ON''
	                                    ),
	                                    SewerNewCount = 
	                                    (
		                                    SELECT COUNT(CatOfService)
		                                    FROM tblNJAWService
		                                    WHERE [CatOfService] = ''Sewer Service New''
		                                    AND [DateInstalled] >= @StartDate
		                                    AND [DateInstalled] <= @EndDate
		                                    AND [OpCntr] = @OpCntr
		                                    AND [InActSrv]<>''ON''
	                                    ),
	                                    SewerRenewCount = 
	                                    (
		                                    SELECT COUNT(CatOfService)
		                                    FROM tblNJAWService
		                                    WHERE [CatOfService] = ''Sewer Service Renewal''
		                                    AND [DateInstalled] >= @StartDate
		                                    AND [DateInstalled] <= @EndDate
		                                    AND [OpCntr] = @OpCntr
		                                    AND [InActSrv]<>''ON''
	                                    ),
	                                    SewerReconnectCount = 
	                                    (
		                                    SELECT COUNT(CatOfService)
		                                    FROM tblNJAWService
		                                    WHERE [CatOfService] = ''Sewer Reconnect''
		                                    AND [DateInstalled] >= @StartDate
		                                    AND [DateInstalled] <= @EndDate
		                                    AND [OpCntr] = @OpCntr
		                                    AND [InActSrv]<>''ON''
	                                    ),
	                                    SewerRetireCount = 
	                                    (
		                                    SELECT COUNT(CatOfService)
		                                    FROM tblNJAWService
		                                    WHERE [CatOfService] = ''Sewer Retire Service Only''
		                                    AND [DateInstalled] >= @StartDate
		                                    AND [DateInstalled] <= @EndDate
		                                    AND [OpCntr] = @OpCntr
		                                    AND [InActSrv]<>''ON''
	                                    )' 
                                    END;
                                    IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[getRptServicesInstalled2]') AND type in (N'P', N'PC'))
                                    BEGIN
                                    EXEC dbo.sp_executesql @statement = N'
                                    ALTER PROCEDURE [getRptServicesInstalled2] (@year int)
                                    AS
                                    Declare @tblTable TABLE(catofservice varchar(40), smartgrowth varchar(3), Total int, MonthNum int, [Month] varchar(10), [Year] int)
                                    Declare @tblOpCntrs TABLE(opcntr varchar(4))
                                    Declare @tblServices TABLE(catofservice varchar(40))
                                    Declare @tblSmartGrowth TABLE(smartgrowth varchar(3))

                                    Insert into @tblOpcntrs select distinct opcntr from tblnjawservice where isNull(opCntr,'''') <> '''' order by opcntr
                                    Insert into @tblServices select distinct #1.catofservice from tblnjawservice #1 where isNull(CatofService,'''') in (''Fire Service Installation'',''Irrigation New'',''Sewer Service New'',''Water Service New Commercial'', ''Water Service New Domestic'') order by #1.catofservice
                                    Insert Into @tblSmartGrowth select distinct CASE WHEN upper(isNull(smartgrowth, '''')) = ''YES'' then ''YES'' else ''NO'' end from tblNJAWService where isNull(smartgrowth,'''') <> '''' order by CASE WHEN upper(isNull(smartgrowth, '''')) = ''YES'' then ''YES'' else ''NO'' end desc

                                    select #oc.opcntr,  #s.catofservice, #sg.smartgrowth, left(DATENAME(M,Cast(Dnum as varchar(2)) + ''/01/2000''),3) as ''MonthName''
	                                    , (
			                                    Select count(*) from tblnjawservice #1 
				                                    where #1.opcntr = #oc.opcntr 
					                                    and #1.catofservice = #s.catofservice 
					                                    and CASE WHEN upper(isNull(#1.smartgrowth, '''')) = ''YES'' then ''YES'' else ''NO'' end = #sg.smartgrowth
					                                    and month(#1.dateinstalled) = Dnum 
					                                    and year(#1.dateinstalled) = @year
					                                    and upper(IsNull(DevServD,'''')) <> ''YES''
		                                    ) as ''total''
	                                    FROM @tblOpCntrs #oc
	                                    CROSS JOIN @tblServices #s
	                                    CROSS JOIN @tblSmartGrowth #sg
	                                    CROSS JOIN dnum 
	                                    where (
			                                    Select count(*) from tblnjawservice #1 
				                                    where #1.opcntr = #oc.opcntr 
					                                    and #1.catofservice = #s.catofservice 
					                                    and CASE WHEN upper(isNull(#1.smartgrowth, '''')) = ''YES'' then ''YES'' else ''NO'' end = #sg.smartgrowth
					                                    and year(#1.dateinstalled) = @year
					                                    and upper(IsNull(DevServD,'''')) <> ''YES''
		                                    ) > 0
	                                    order by 1,dnum' 
                                    END;
                                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetOpenContractorSvcs]') AND type in (N'P', N'PC'))
                                    BEGIN
                                    EXEC dbo.sp_executesql @statement = N'/*	Returns data related to Open Contractor Services.	*/
                                    CREATE  PROCEDURE [spGetOpenContractorSvcs]
	                                    @OpCntr varchar(3)
	                                    ,@Contractor varchar(30)
	                                    ,@OrderBy int	
                                    AS

                                    Declare @sql varchar(2000)
                                    select @sql = ''SELECT SV.Priority, SV.WorkIssuedTo, SV.DateIssuedToField, '' + 
							                                    '' CAST(SV.ServNum AS int) AS ServNum,'' + 
							                                    '' SV.StNum + '''' '''' +  ST.FullStName AS CompleteStAddress,'' + 
							                                    '' T.Town, SV.CatofService, SV.PurpInstal,'' + 
							                                    '' SV.CrossStreet, SV.PermitNum, SV.TapOrdNote as [JobNotes]'' + 
							                                    '' FROM tblNJAWService SV, Streets ST, Towns T'' + 
							                                    '' WHERE SV.CatOfService <> ''''Stub Service'''''' + 
							                                    '' AND SV.Town = T.TownID '' +
							                                    '' AND SV.CatOfService NOT LIKE ''''%Measurement Only%'''''' + 
							                                    '' AND SV.OpCntr = '''''' + @OpCntr + '''''''' + 
							                                    '' AND SV.WorkIssuedTo = '''''' + @Contractor + '''''''' + 
							                                    '' AND SV.InActSrv<>''''ON'''''' + 
							                                    '' AND SV.PurpInstal <> ''''Main Replacement'''''' + 
							                                    '' AND SV.DateIssuedToField > ''''1/1/1900'''''' + 
							                                    '' AND SV.DateInstalled = ''''1/1/1900'''''' + 
							                                    '' AND SV.StName = ST.StreetID'' 

							                                    IF (@OrderBy = 1) 
							                                      BEGIN
									                                    Select @Sql = @Sql + '' ORDER BY T.Town, SV.DateIssuedToField, ST.FullStName, SV.StNum'' 
								                                    END
							                                    ELSE
								                                    BEGIN
									                                    Select @Sql = @Sql + '' ORDER BY SV.Priority, SV.DateIssuedToField, ST.FullStName, SV.StNum''
								                                    END
                                    Exec(@sql)
                                    ' 
                                    END;
                                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetPendingSvcsByTown]') AND type in (N'P', N'PC'))
                                    BEGIN
                                    EXEC dbo.sp_executesql @statement = N'
                                    CREATE PROCEDURE [spGetPendingSvcsByTown]
	                                    @OpCntr varchar(3),
	                                    @Town int
	
                                    AS
	                                    SELECT T.Town, CAST(SV.ServNum AS int) AS ServNum,
	                                    SV.StNum + '' '' +  ST.FullStName AS CompleteStAddress,
	                                    SV.ContactDate, SV.ApplSent, SV.ApplRcvd, SV.PermitSentDate, 
	                                    SV.PermitRcvdDate, SV.DateIssuedtoField, SV.WorkIssuedto, SV.CatofService,
	                                    SV.PurpInstal, SV.SizeofService, SV.TaskNum1, SV.TaskNum2, SV.PremNum

	                                    FROM  tblNJAWService SV, Streets ST, Towns T
	                                    WHERE T.TownID = @Town
	                                    AND	SV.Town = T.TownID
	                                    AND SV.OpCntr = @OpCntr
	                                    AND SV.InActSrv <> ''ON''
	                                    AND SV.DateInstalled = ''1/1/1900''
	                                    AND SV.CatofService <> ''Stub Service''
	                                    AND SV.CatofService NOT LIKE ''%Measurement Only%''
	                                    AND SV.StName = ST.StreetID
	                                    ORDER BY T.Town, ST.FullStName, SV.StNum

                                    ' 
                                    END;
                                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetBilling]') AND type in (N'P', N'PC'))
                                    BEGIN
                                    EXEC dbo.sp_executesql @statement = N'/*	Returns Billing information based on 
	                                    Op Center, Contractor, and date interval.	*/

                                    CREATE PROCEDURE [spGetBilling]
	                                    @OpCntr varchar(3),
	                                    @Contractor varchar(30),
	                                    @BegDate varchar(30),
	                                    @EndDate varchar(30), 
	                                    @invoiced varchar(10)
	
                                    AS
                                    Declare @sql varchar(2000)

                                    select @Sql = ''SELECT CAST(SV.ServNum AS int) AS ServNum,'' +
							                                    '' SV.StNum + '''' '''' +  ST.FullStName AS CompleteStAddress,'' +
							                                    '' T.Town, SV.CatofService, SV.PurpInstal, SV.SizeOfService,'' +
							                                    '' SV.LengthService, SV.TaskNum1, SV.DateInstalled,'' +
							                                    '' SV.SizeOfMain, SV.TypeMain'' + 
							                                    '' FROM tblNJAWService SV, Streets ST, Towns T'' +
							                                    '' WHERE SV.OpCntr = '''''' + @opCntr + '''''''' + 
							                                    '' AND T.TownID = SV.Town '' + 
							                                    '' AND SV.InActSrv <> ''''ON'''''' +
							                                    '' AND SV.DateInstalled > ''''1/1/1900'''''' +
							                                    '' AND SV.WorkIssuedTo = '''''' + @Contractor + '''''''' + 
							                                    '' AND SV.DateInstalled >= '''''' + @BegDate + '''''''' + 
							                                    '' AND SV.DateInstalled <= '''''' + @EndDate + '''''''' + 
							                                    '' AND SV.CatOfService <> ''''Stub Service'''''' +
							                                    '' AND SV.CatOfService NOT LIKE ''''%Measurement Only%'''''' +
							                                    '' AND SV.StName = ST.StreetID''
                                    if (@invoiced = ''YES'')
                                    BEGIN
	                                    select @Sql = @sql + '' AND SV.InstInvDate <> ''''1/1/1900''''''
                                    END
                                    if (@invoiced = ''NO'')
                                    BEGIN
	                                    select @Sql = @sql + '' AND SV.InstInvDate = ''''1/1/1900''''''
                                    END
                                    select @Sql = @sql + '' ORDER BY SV.WorkIssuedTo, SV.DateInstalled'' 
                                    Exec(@Sql)
                                    ' 
                                    END;
                                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetServiceHistory]') AND type in (N'P', N'PC'))
                                    BEGIN
                                    EXEC dbo.sp_executesql @statement = N'/*	Returns Site Information and Dates
	                                    for Op Center and Service Number provided.	*/

                                    CREATE PROCEDURE [spGetServiceHistory]
	                                    @OpCtr varchar(3),
	                                    @ServNum float,
	                                    @Category varchar(40)

                                    AS
	                                    IF (@Category <> '''' )
		                                    SELECT RecID, PremNum, CatofService, Initiator, [Name],
		                                    PhoneNum, StNum, Apt, StName, Town, State, Zip,
		                                    CrossStreet, TwnSection, Development, Lot, Block,
		                                    WorkIssuedTo, ContactDate,
		                                    PermitSentDate, PermitRcvdDate, PermitExpDate,
		                                    ApplSent, ApplRcvd, ApplApvd,
		                                    InspDate, DateIssuedToField, DateInstalled,
		                                    OrigInstDate, RetireDate, InstInvDate
		
		                                    FROM tblNJAWService
		                                    WHERE OpCntr = @OpCtr
		                                    AND ServNum = @ServNum
		                                    AND CatOfService = @Category

	                                    ELSE
		                                    SELECT RecID, PremNum, CatofService, Initiator, [Name],
		                                    PhoneNum, StNum, Apt, StName, Town, State, Zip,
		                                    CrossStreet, TwnSection, Development, Lot, Block,
		                                    WorkIssuedTo, ContactDate,
		                                    PermitSentDate, PermitRcvdDate, PermitExpDate,
		                                    ApplSent, ApplRcvd, ApplApvd,
		                                    InspDate, DateIssuedToField, DateInstalled,
		                                    OrigInstDate, RetireDate, InstInvDate
		
		                                    FROM tblNJAWService
		                                    WHERE OpCntr = @OpCtr
		                                    AND ServNum = @ServNum' 
                                    END;
                                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetCategoriesByServNum]') AND type in (N'P', N'PC'))
                                    BEGIN
                                    EXEC dbo.sp_executesql @statement = N'/*	Returns the Categories of Service
	                                    for the Service Number provided.	*/

                                    CREATE PROCEDURE [spGetCategoriesByServNum]
	                                    @ServNum float

                                    AS
	                                    SELECT CatofService
	                                    FROM tblNJAWService
	                                    WHERE ServNum = @ServNum' 
                                    END;
                                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetTDCounts]') AND type in (N'P', N'PC'))
                                    BEGIN
                                    EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [spGetTDCounts]
	                                    @OpCntr varchar(3)

                                    AS

                                    SELECT	RenewPermitsPending = 
	                                    (
		                                    SELECT COUNT(CatOfService)
		                                    FROM tblNJAWService
		                                    WHERE [CatOfService] = ''Water Service Renewal''
		                                    AND [OpCntr] = @OpCntr
		                                    AND [PermitSentDate] > ''1/1/1900''
		                                    AND [PermitRcvdDate] = ''1/1/1900''
		                                    AND [PurpInstal] <> ''Main Replacement''
		                                    AND [DateInstalled] = ''1/1/1900''
		                                    AND [InActSrv]<>''ON''
	                                    ),
	                                    RenewIssuedToField =
	                                    (
		                                    SELECT COUNT(CatOfService)
		                                    FROM tblNJAWService
		                                    WHERE [CatOfService] = ''Water Service Renewal''
		                                    AND [OpCntr] = @OpCntr
		                                    AND [DateIssuedtoField] > ''1/1/1900''
		                                    AND [DateInstalled] = ''1/1/1900''
		                                    AND [PurpInstal] <> ''Main Replacement''
		                                    AND [InActSrv]<>''ON''
	                                    ),
	                                    NewApprovedApplications = 
	                                    (
		                                    SELECT COUNT(CatOfService)
		                                    FROM tblNJAWService
		                                    WHERE [CatOfService] LIKE ''Water Service New%'' AND isNull([PurpInstal],'''') <> ''Main Replacement'' AND DevServD = ''NO'' AND isNull([InActSrv],'''')<>''ON'' AND Year([DateInstalled]) = 1900
			                                    AND Year([ApplApvd]) > 1900
			                                    AND [OpCntr] = @OpCntr
			                                    AND [PermitSentDate] = ''1/1/1900''
			                                    AND [PermitRcvdDate] = ''1/1/1900''
			                                    AND [DateIssuedToField] = ''1/1/1900''
			                                    AND isNull(InspSignOffReady,'''') not like ''%Site Not Ready%''
	                                    ),
	                                    NewPermitsPending = 
	                                    (
		                                    SELECT COUNT(CatOfService)
		                                    FROM tblNJAWService
		                                    WHERE [CatOfService] LIKE ''Water Service New%'' AND isNull([PurpInstal],'''') <> ''Main Replacement'' AND DevServD = ''NO'' AND isNull([InActSrv],'''')<>''ON'' AND Year([DateInstalled]) = 1900
			                                    AND [OpCntr] = @OpCntr			
			                                    AND [PermitSentDate] > ''1/1/1900''
			                                    AND [PermitRcvdDate] = ''1/1/1900''
			                                    AND [DateIssuedToField] = ''1/1/1900''
			                                    AND isNull(InspSignOffReady,'''') not like ''%Site Not Ready%''
	                                    ),
	                                    NewServiceIssuedToField = 
	                                    (
		                                    SELECT COUNT(CatOfService)
		                                    FROM tblNJAWService
		                                    WHERE [CatOfService] LIKE ''Water Service New%'' AND isNull([PurpInstal],'''') <> ''Main Replacement'' AND DevServD = ''NO'' AND isNull([InActSrv],'''')<>''ON'' AND Year([DateInstalled]) = 1900
			                                    AND [OpCntr] = @OpCntr
			                                    AND [DateIssuedToField] > ''1/1/1900''
			                                    AND isNull(InspSignOffReady,'''') not like ''%Site Not Ready%''
	                                    ),
	                                    NewSiteNotReady = 
	                                    (
		                                    SELECT COUNT(CatOfService)
		                                    FROM tblNJAWService
		                                    WHERE [CatOfService] LIKE ''Water Service New%'' AND isNull([PurpInstal],'''') <> ''Main Replacement'' AND DevServD = ''NO'' AND isNull([InActSrv],'''')<>''ON'' AND Year([DateInstalled]) = 1900
			                                    AND [OpCntr] = @OpCntr
			                                    AND isNull(InspSignOffReady,'''') like ''%Site Not Ready%''
	                                    )
                                    /*

                                    -- ALL 
                                    select InspSignOffReady, [DateInstalled], [DateIssuedToField], year([DateInstalled]), DevServD, * from tblNJAWService
		                                    WHERE [CatOfService] LIKE ''Water Service New%'' AND isNull([PurpInstal],'''') <> ''Main Replacement'' AND DevServD = ''NO'' AND isNull([InActSrv],'''')<>''ON'' AND Year([DateInstalled]) = 1900 
			                                    AND Year([ApplApvd]) > 1900
			                                    AND [OpCntr] = ''NJ7''
			
                                    -- APPROVED
                                    select InspSignOffReady, [DateInstalled], [DateIssuedToField], year([DateInstalled]), DevServD, * from tblNJAWService
		                                    WHERE [CatOfService] LIKE ''Water Service New%'' AND isNull([PurpInstal],'''') <> ''Main Replacement'' AND DevServD = ''NO'' AND isNull([InActSrv],'''')<>''ON'' AND Year([DateInstalled]) = 1900
			                                    AND Year([ApplApvd]) > 1900
			                                    AND [PermitSentDate] = ''1/1/1900''
			                                    AND [PermitRcvdDate] = ''1/1/1900''
			                                    AND [DateIssuedToField] = ''1/1/1900''
			                                    AND isNull(InspSignOffReady,'''') not like ''%Site Not Ready%''
                                    -- PERMITS PENDING
                                    select InspSignOffReady, [DateInstalled], [DateIssuedToField], year([DateInstalled]), DevServD, * from tblNJAWService
		                                    WHERE [CatOfService] LIKE ''Water Service New%'' AND isNull([PurpInstal],'''') <> ''Main Replacement'' AND DevServD = ''NO'' AND isNull([InActSrv],'''')<>''ON'' AND Year([DateInstalled]) = 1900
			                                    AND [PermitSentDate] > ''1/1/1900''
			                                    AND [PermitRcvdDate] = ''1/1/1900''
			                                    AND [DateIssuedToField] = ''1/1/1900''
			                                    AND isNull(InspSignOffReady,'''') not like ''%Site Not Ready%''
                                    -- ISSUED TO FIELD
                                    select InspSignOffReady, [DateInstalled], [DateIssuedToField], year([DateInstalled]), DevServD, * from tblNJAWService
		                                    WHERE [CatOfService] LIKE ''Water Service New%'' AND isNull([PurpInstal],'''') <> ''Main Replacement'' AND DevServD = ''NO'' AND isNull([InActSrv],'''')<>''ON'' AND Year([DateInstalled]) = 1900
			                                    AND [DateIssuedToField] > ''1/1/1900''
			                                    AND isNull(InspSignOffReady,'''') not like ''%Site Not Ready%''
                                    -- SITE NOT READY
                                    select InspSignOffReady, [DateInstalled], [DateIssuedToField], year([DateInstalled]), DevServD, * from tblNJAWService
		                                    WHERE [CatOfService] LIKE ''Water Service New%'' AND isNull([PurpInstal],'''') <> ''Main Replacement'' AND DevServD = ''NO'' AND isNull([InActSrv],'''')<>''ON'' AND Year([DateInstalled]) = 1900
			                                    AND isNull(InspSignOffReady,'''') like ''%Site Not Ready%''
                                    */' 
                                    END;
                                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetSewerTDCount]') AND type in (N'P', N'PC'))
                                    BEGIN
                                    EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [spGetSewerTDCount]
	                                    @OpCntr varchar(3)

                                    AS

                                    SELECT Sewers = 
                                    (
	                                    SELECT COUNT(CatOfService)
	                                    FROM tblNJAWService
	                                    WHERE [CatOfService] LIKE ''Sewer%''
	                                    AND [CatOfService] NOT LIKE ''%Reconnect''
	                                    AND [OpCntr] = @OpCntr
	                                    AND [DateIssuedToField] > ''1/1/1900''
	                                    AND [DateInstalled] = ''1/1/1900''
	                                    AND DevServD = ''NO''
	                                    AND [InActSrv]<>''ON''
                                    )' 
                                    END;
                                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetContractorLoading]') AND type in (N'P', N'PC'))
                                    BEGIN
                                    EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [spGetContractorLoading]
	                                    @OpCntr varchar(3)

                                    AS
	                                    SELECT WorkIssuedto, SUM(CntRec) AS LOADC
	                                    FROM tblNJAWService
	                                    WHERE CatOfService LIKE ''%New%''
	                                    AND CatOfService NOT LIKE ''&Renew%''
	                                    AND OpCntr = @OpCntr
	                                    AND DateIssuedToField > ''1/1/1900''
	                                    AND DateInstalled = ''1/1/1900''
	                                    AND PurpInstal <> ''Main Replacement''
	                                    AND DevServD = ''NO''
	                                    AND WorkIssuedto <> ''''
	                                    GROUP BY WorkIssuedTo' 
                                    END;
                                    IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RptServicesRenewed]') AND type in (N'P', N'PC'))
                                    BEGIN
                                    EXEC dbo.sp_executesql @statement = N'
                                    /****** Object:  StoredProcedure [dbo].[RptServicesRenewed]    Script Date: 05/31/2011 11:16:45 ******/
                                    ALTER PROCEDURE [RptServicesRenewed] (@startDate datetime, @endDate dateTime, @opCntr varchar(4), @devdriv varchar(10))
                                    AS

                                    IF (Len(@opCntr) > 0)
	                                    BEGIN
		                                    select OpCntr, Towns.town, Towns.TownID as RecID, CatOfService, isNull(SizeOfService,0) as SizeOfService, Sum(isNull(LengthService ,0)) as ''LengthService'', Count(SizeOfService) as ''Total'', 
			                                    Count(case when (smartgrowth=''Yes'') then 1 else null end) as SmartGrowth
			                                    from tblNJAWService
			                                    LEFT JOIN Towns on Towns.TownID = tblNJAWservice.town
			                                    where (charindex(''RENEW'', upper(isNull(catofservice,''''))) > 0 OR charindex(''INSTALL METER SET'', upper(isNull(catofservice,''''))) > 0)
				                                    and tblNJAWService.OpCntr = @opCntr
				                                    and DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
				                                    and devServD = isNull(@devdriv, devServD)
			                                    group by OpCntr, Towns.town, Towns.TownID, CatofService, SizeofService
			                                    order by OpCntr, Towns.town, CatofService, SizeofService	
	                                    END
                                    ELSE
	                                    BEGIN
		                                    select OpCntr, Towns.town, Towns.TownID as RecID, CatOfService, isNull(SizeOfService,0) as SizeOfService, Sum(isNull(LengthService ,0)) as ''LengthService'', Count(SizeOfService) as ''Total'', 
			                                    Count(case when (smartgrowth=''Yes'') then 1 else null end) as SmartGrowth
			                                    from tblNJAWService
			                                    LEFT JOIN Towns on Towns.TownID = tblNJAWservice.town
			                                    where (charindex(''RENEW'', upper(isNull(catofservice,''''))) > 0 OR charindex(''INSTALL METER SET'', upper(isNull(catofservice,''''))) > 0)
				                                    and DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
				                                    and devServD = isNull(@devdriv, devServD)
			                                    group by OpCntr, Towns.town, Towns.TownID, CatofService, SizeofService
			                                    order by OpCntr, Towns.town, CatofService, SizeofService
	                                    END
                                    ' 
                                    END;
                                    IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RptServicesInstalledDetail]') AND type in (N'P', N'PC'))
                                    BEGIN
                                    EXEC dbo.sp_executesql @statement = N'
                                    /****** Object:  StoredProcedure [dbo].[RptServicesInstalledDetail]    Script Date: 05/31/2011 11:25:30 ******/
                                    ALTER PROCEDURE [RptServicesInstalledDetail] (@startDate datetime, @endDate dateTime, @opCntr varchar(4),  @devdriv varchar(10))
                                    AS
                                    IF (Len(@opCntr) > 0)
	                                    BEGIN
		                                    select OpCntr, Towns.town, CatOfService, isNull(SizeOfService,0) as SizeOfService, LengthService as ''LengthService'', ServNum, case when (isnull(smartgrowth,'''')=''YES'') then ''Yes'' end as [SmartGrowth]
			                                    from tblNJAWService
			                                    LEFT JOIN Towns on Towns.TownID = tblNJAWservice.town
			                                    where (charindex(''INSTALLATION'', upper(isNull(catofservice,''''))) > 0 OR charindex('' NEW'', upper(isNull(catofservice,''''))) > 0)
				                                    and tblNJAWService.OpCntr = @opCntr
				                                    and DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
				                                    and devServD = isNull(@devdriv, devServD)
			                                    order by OpCntr, Towns.town, CatofService, SizeofService	
	                                    END
                                    ELSE
	                                    BEGIN
		                                    select OpCntr, Towns.town, CatOfService, isNull(SizeOfService,0) as SizeOfService, LengthService as ''LengthService'', ServNum, case when (isnull(smartgrowth,'''')=''YES'') then ''Yes'' end as [SmartGrowth] 
			                                    from tblNJAWService
			                                    LEFT JOIN Towns on Towns.TownID = tblNJAWservice.town
			                                    where (charindex(''INSTALLATION'', upper(isNull(catofservice,''''))) > 0 OR charindex('' NEW'', upper(isNull(catofservice,''''))) > 0)
				                                    and DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
				                                    and devServD = isNull(@devdriv, devServD)
			                                    order by OpCntr, Towns.town, CatofService, SizeofService
	                                    END
                                    ' 
                                    END;
                                    IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RptServicesRenewedRetirementsDetail]') AND type in (N'P', N'PC'))
                                    BEGIN
                                    EXEC dbo.sp_executesql @statement = N'
                                    ALTER Procedure [RptServicesRenewedRetirementsDetail] (@startDate datetime, @endDate datetime, @town int, @CatOfService varchar(40), @SizeOfService varchar(10))
                                    AS
                                    select 
	                                    Year(OrigInstDate) as ''Year Installed'', 
	                                    PrevServiceMatl, 
	                                    LengthService as [Length], --Sum(LengthService) as [Length], 
	                                    servnum,--Count(OrigInstDate) as ''Total''
	                                    TaskNum1 as TaskNumber	
                                    from 
	                                    tblNJAWService
                                    where 
	                                    year(OrigInstDate) > 1900
                                    and 
	                                    DateInstalled >= @startDate and DateInstalled <= @endDate
                                    and 
	                                    CatOfService = @CatOfService
                                    and 
	                                    SizeOfService = @SizeOfService
                                    and 
	                                    Town = @Town
                                    ' 
                                    END;
                                    BEGIN
                                    EXEC dbo.sp_executesql @statement = N'
                                    /****** Object:  StoredProcedure [dbo].[RptServicesRenewedByYear]    Script Date: 05/31/2011 11:15:04 ******/
                                    ALTER PROCEDURE [RptServicesRenewedByYear] (@startYear int, @endYear int, @opCntr varchar(4))
                                    AS
                                    --Declare @startYear int
                                    --Declare @endYear int
                                    --Declare @opCntr varchar(3)
                                    --Select @startYear = 1920
                                    --Select @endYear = 2007
                                    --Select @opCntr = ''NJ7''

                                    if (Len(@opCntr) > 0)
	                                    BEGIN
		                                    Select OpCntr, Towns.town, Year(DateInstalled) as [Year], Count(*) as [Total]
			                                    from tblNJAWService
			                                    LEFT JOIN Towns on Towns.TownID = tblNJAWservice.town
			                                    where 
					                                    tblNJAWService.OpCntr = @opCntr AND
					                                    charindex(''RENEW'', upper(isNull(catofservice,''''))) > 0 AND
				                                    year(DateInstalled) >= @startYear and year(DateInstalled) <= @endYear
			                                    group by OpCntr, Towns.town, Year(DateInstalled)
			                                    order by OpCntr, Towns.town, Year(DateInstalled)
	                                    END
                                    ELSE
	                                    BEGIN
		                                    Select OpCntr, Towns.town, Year(DateInstalled) as [Year], Count(*) as [Total]
			                                    from tblNJAWService
			                                    LEFT JOIN Towns on Towns.TownID = tblNJAWservice.town
			                                    where 
				                                    charindex(''RENEW'', upper(isNull(catofservice,''''))) > 0 AND	
				                                    year(DateInstalled) >= @startYear and year(DateInstalled) <= @endYear
			                                    group by OpCntr, Towns.town, Year(DateInstalled)
			                                    order by OpCntr, Towns.town, Year(DateInstalled)
	                                    END
                                    ' 
                                    END;
                                    IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RptFormNewWaterServiceInquiry]') AND type in (N'P', N'PC'))
                                    BEGIN
                                    EXEC dbo.sp_executesql @statement = N'
                                    /****** Object:  StoredProcedure [dbo].[RptFormNewWaterServiceInquiry]    Script Date: 05/31/2011 11:32:40 ******/
                                    ALTER Procedure [RptFormNewWaterServiceInquiry] (@RecID int)
                                    AS
                                    SELECT 
	                                    S.Agreement,
	                                    S.AmntRcvd,
	                                    S.ApplApvd,
	                                    S.ApplRcvd,
	                                    S.ApplSent,
	                                    S.Apt,
	                                    S.Block,
	                                    S.BSDWPermit,
	                                    S.CatofService,
	                                    S.ContactDate,
	                                    S.CrossStreet,
	                                    S.DateClosed, 
	                                    CASE WHEN S.DateInstalled < ''1/1/1901'' THEN NULL ELSE S.DateInstalled END As DateInstalled,
	                                    S.DateIssuedtoField,
	                                    S.Development,
	                                    S.DevServD,
	                                    S.Initiator,
	                                    S.InspDate,
	                                    S.InspSignoffReady,
	                                    S.InstCost, 
	                                    S.InstInv,
	                                    S.InstInvDate,
	                                    S.JobNotes,
	                                    S.Lat,
	                                    S.Lon,
	                                    S.Lot,
	                                    S.MailPhoneNum,
	                                    S.MailStName,
	                                    S.MailStNum,
	                                    S.MailTown,
	                                    S.MailState,
	                                    S.MailZip,
	                                    S.Name,
	                                    S.MeterSetReq,
	                                    S.OpCntr,
	                                    S.OrdCreationDate, 
	                                    CASE WHEN S.OrigInstDate < ''1/1/1901'' THEN NULL ELSE S.OrigInstDate END As OrigInstDate,
	                                    S.ParentTaskNum,
	                                    S.PayRefNum,
	                                    S.PermitExpDate,
	                                    S.PermitNum,
	                                    S.PermitRcvdDate,
	                                    S.PermitSentDate,
	                                    S.PermitType,
	                                    S.PhoneNum,
	                                    S.PremNum,
	                                    S.PrevServiceSize,
	                                    S.Priority,
	                                    S.PurpInstal, 
	                                    S.RecID,
	                                    S.RetireAcct,
	                                    S.RetireDate,
	                                    S.RoadOpenFee,
	                                    S.ServInstFee,
	                                    S.ServMatl,
	                                    S.ServNum,
	                                    Cat.ServType, 
	                                    CASE WHEN isNull(S.SmartGrowth,'''') = ''YES'' THEN ''X'' ELSE '''' END as ''SmartGrowth'',
	                                    S.SGMethodUsed,
	                                    S.SGCost,
	                                    S.SizeofMain,
	                                    S.SizeofService, 
	                                    S.SizeofTap,
	                                    S.State,
	                                    S.StreetMatl,
	                                    S.StName,
	                                    S.StNum,
	                                    S.TapOrdNote,
	                                    S.TaskNum1,
	                                    S.TaskNum2,
	                                    S.Town,
	                                    S.TwnSection,
	                                    S.TypeMain,
	                                    S.WorkIssuedto,
	                                    S.Zip,
	                                    UPPER(S.CatOfService) As UCatOfService, 
	                                    T.DistrictID As TDistrictID,
	                                    T.County As TCounty,
	                                    T.Town As TTown, 
	                                    St.StreetPrefix As StStreetPrefix,
	                                    St.StreetName  As StStreetName,
	                                    St.StreetSuffix As StStreetSuffix,
	                                    (Select Town from Towns where Towns.TownID = St.TownID) As StTown,
	                                    States.Abbreviation As StSt,
	                                    St.FullStName As StFullStName, 
	                                    Cntr.ServContactNum As CServContactNum,
	                                    Cntr.CSNum As CCsNum,
	                                    Cntr.CoInfo As CCoInfo,
	                                    Cntr.OperatingCenterName AS COpCntrName, 
	                                    Cntr.MailCo AS CMailCo,
	                                    Cntr.MailAdd AS CMailAdd,
	                                    Cntr.MailCSZ AS CMailCSZ,
	                                    Cntr.FaxNum AS CFaxNum,
	                                    S.Fax
	                                    ,isNull(Cntr.MailCo,'''') + '', '' + isNull(Cntr.MailAdd, '''') + '', '' + isNull(Cntr.MailCSZ,'''') as cMailFullAdd
	                                    FROM tblNJAWService S 
		                                    LEFT JOIN Towns T ON S.Town = T.TownID
		                                    LEFT JOIN States ON States.StateID = T.StateID
		                                    LEFT JOIN Streets St ON St.StreetID = S.StName 
		                                    LEFT JOIN OperatingCenters Cntr ON S.OpCntr = Cntr.OperatingCenterCode 
		                                    LEFT JOIN tblNJAWCategoryService Cat ON S.CatOfService = Cat.CatOfService AND S.OpCntr = Cat.OpCntr
	                                    WHERE S.RecID = @RecID
                                    ' 
                                    END";
        }

        public struct Tables
        {
            public const string SERVICES = "tblNJAWService",
                                SERVICE_CATEGORIES = "ServiceCategories",
                                OPERATING_CENTERS = "OperatingCenters",
                                OPERATING_CENTERS_SERVICE_CATEGORIES = "tblNJAWCategoryService";
        }

        public struct Columns
        {
            public const string OPERATING_CENTER = "OpCntr",
                                OPERATING_CENTER_ID = "OperatingCenterID",
                                SERVICE_CATEGORY_ID = "ServiceCategoryID",
                                SERVICE_CATEGORY = "CatOfService",
                                DESCRIPTION = "Description";
        }

        public struct ForeignKeys
        {
            public const string FK_SERVICES_SERVICE_CATEGORIES = "FK_tblNJAWService_ServiceCategories_CatOfService",
                                FK_SERVICES_OPERATING_CENTERS = "FK_tblNJAWService_OperatingCenters_OpCntr",
                                FK_OPERATING_CENTERS_SERVICE_CATEGORIES_OPERATING_CENTERS =
                                    "FK_tblNJAWCategoryService_OperatingCenters_OpCntr",
                                FK_OPERATING_CENTERS_SERVICE_CATEGORIES_SERVICE_CATEGORIES =
                                    "FK_tblNJAWCategoryService_ServiceCategories_CatOfService";
        }

        public struct StringLengths
        {
            public const int OPERATING_CENTER = 4,
                             DESCRIPTION = 50,
                             SERVICE_CATEGORY = 40;
        }

        #endregion

        public override void Up()
        {
            #region CLEANUP

            Execute.Sql(Sql.SERVICES_CLEANUP);
            Execute.Sql(Sql.SERVICES_DROP_CONSTRAINTS);
            Execute.Sql(Sql.SERVICES_DROP_INDEXES);

            #endregion

            #region SUPPORT

            Create.Table(Tables.SERVICE_CATEGORIES)
                  .WithColumn(Columns.SERVICE_CATEGORY_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();

            #endregion

            #region UPDATE THE TABLE

            Execute.Sql(Sql.UPDATE_COLUMNS);
            Alter.Table(Tables.SERVICES)
                 .AlterColumn(Columns.OPERATING_CENTER).AsInt32().NotNullable()
                 .AlterColumn(Columns.SERVICE_CATEGORY).AsInt32().Nullable();
            Alter.Table(Tables.OPERATING_CENTERS_SERVICE_CATEGORIES)
                 .AlterColumn(Columns.OPERATING_CENTER).AsInt32().NotNullable()
                 .AlterColumn(Columns.SERVICE_CATEGORY).AsInt32().NotNullable();
            Execute.Sql(Sql.SERVICES_ADD_INDEXES);

            #endregion

            #region FOREIGN KEYS

            Create.ForeignKey(ForeignKeys.FK_SERVICES_SERVICE_CATEGORIES)
                  .FromTable(Tables.SERVICES).ForeignColumn(Columns.SERVICE_CATEGORY)
                  .ToTable(Tables.SERVICE_CATEGORIES).PrimaryColumn(Columns.SERVICE_CATEGORY_ID);
            Create.ForeignKey(ForeignKeys.FK_SERVICES_OPERATING_CENTERS)
                  .FromTable(Tables.SERVICES).ForeignColumn(Columns.OPERATING_CENTER)
                  .ToTable(Tables.OPERATING_CENTERS).PrimaryColumn(Columns.OPERATING_CENTER_ID);
            Create.ForeignKey(ForeignKeys.FK_OPERATING_CENTERS_SERVICE_CATEGORIES_OPERATING_CENTERS)
                  .FromTable(Tables.OPERATING_CENTERS_SERVICE_CATEGORIES).ForeignColumn(Columns.OPERATING_CENTER)
                  .ToTable(Tables.OPERATING_CENTERS).PrimaryColumn(Columns.OPERATING_CENTER_ID);
            Create.ForeignKey(ForeignKeys.FK_OPERATING_CENTERS_SERVICE_CATEGORIES_SERVICE_CATEGORIES)
                  .FromTable(Tables.OPERATING_CENTERS_SERVICE_CATEGORIES).ForeignColumn(Columns.SERVICE_CATEGORY)
                  .ToTable(Tables.SERVICE_CATEGORIES).PrimaryColumn(Columns.SERVICE_CATEGORY_ID);

            #endregion

            #region STORED PROCEDURES

            Execute.Sql(Sql.REMOVE_STORED_PROCEDURES);
            Execute.Sql(Sql.FIX_STORED_PROCEDURES);

            #endregion
        }

        public override void Down()
        {
            Execute.Sql(Sql.SERVICES_DROP_INDEXES);
            Delete.ForeignKey(ForeignKeys.FK_SERVICES_SERVICE_CATEGORIES).OnTable(Tables.SERVICES);
            Delete.ForeignKey(ForeignKeys.FK_SERVICES_OPERATING_CENTERS).OnTable(Tables.SERVICES);
            Delete.ForeignKey(ForeignKeys.FK_OPERATING_CENTERS_SERVICE_CATEGORIES_OPERATING_CENTERS)
                  .OnTable(Tables.OPERATING_CENTERS_SERVICE_CATEGORIES);
            Delete.ForeignKey(ForeignKeys.FK_OPERATING_CENTERS_SERVICE_CATEGORIES_SERVICE_CATEGORIES)
                  .OnTable(Tables.OPERATING_CENTERS_SERVICE_CATEGORIES);

            Alter.Table(Tables.SERVICES)
                 .AlterColumn(Columns.OPERATING_CENTER).AsAnsiString(StringLengths.OPERATING_CENTER).Nullable()
                 .AlterColumn(Columns.SERVICE_CATEGORY).AsAnsiString(StringLengths.SERVICE_CATEGORY).Nullable();
            Alter.Table(Tables.OPERATING_CENTERS_SERVICE_CATEGORIES)
                 .AlterColumn(Columns.OPERATING_CENTER).AsAnsiString(StringLengths.OPERATING_CENTER).Nullable()
                 .AlterColumn(Columns.SERVICE_CATEGORY).AsAnsiString(StringLengths.SERVICE_CATEGORY).NotNullable();
            Execute.Sql(Sql.ROLLBACK_COLUMNS);

            Execute.Sql(Sql.SERVICES_ADD_INDEXES);
            Execute.Sql(Sql.SERVICES_ADD_CONSTRAINTS);
            Execute.Sql(Sql.RESTORE_STORED_PROCEDURES);
            Execute.Sql(Sql.GRANT_PERMISSIONS);

            Delete.Table(Tables.SERVICE_CATEGORIES);
        }
    }
}
