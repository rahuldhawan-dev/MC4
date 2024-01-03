using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150519082225001), Tags("Production")]
    public class UpdateHydrantsForBug2223 : Migration
    {
        #region Constants

        public struct Sql
        {
            public const string
                UPDATE_HYDRANTS =
                    "UPDATE Towns set AbbreviationTypeID = (SELECT AbbreviationTypeID FROM AbbreviationTypes WHERE DESCRIPTION = 'Town') WHERE isNUll(AbbreviationTypeId,'') = '';" +
                    "UPDATE Towns set Ab = LEFT(Town, 2) WHERE isNull(Ab , '') = '';" +
                    "UPDATE tblNJAWValves SET TwnSection = null where TwnSection in ('BIANCO', 'GUNTHER', 'NUGENT', 'SCHREIBER', 'GRABOWSKI', 'LEWANDOWSKI');" +
                    "UPDATE WorkOrders SET TownSectionID = NULL WHERE TownSectionID in (SELECT TownSectionID FROM TownSections WHERE UPPER(Name) IN ('BIANCO', 'GUNTHER', 'NUGENT', 'SCHREIBER', 'GRABOWSKI', 'LEWANDOWSKI'));" +
                    "DELETE FROM TownSections WHERE UPPER(Name) IN ('BIANCO', 'GUNTHER', 'NUGENT', 'SCHREIBER', 'GRABOWSKI', 'LEWANDOWSKI');" +
                    "UPDATE " + TableNames.HYDRANTS_NEW +
                    " SET BPUKPI = CASE WHEN (RTRIM(LTRIM(IsNull(BPUKPI, ''))) = 'ON') THEN 1 ELSE 0 END;" +
                    "UPDATE " + TableNames.HYDRANTS_NEW +
                    " SET Critical = CASE WHEN (RTRIM(LTRIM(IsNull(Critical, ''))) = 'ON') THEN 1 ELSE 0 END;" +
                    "UPDATE " + TableNames.HYDRANTS_NEW +
                    " SET DEM = CASE WHEN (RTRIM(LTRIM(IsNull(DEM, ''))) = 'YES') THEN 1 ELSE 0 END;" +
                    "UPDATE " + TableNames.HYDRANTS_NEW +
                    " SET OutOfServ = CASE WHEN (RTRIM(LTRIM(IsNull(OutOfServ, ''))) = 'ON') THEN 1 ELSE 0 END;" +
                    "UPDATE " + TableNames.HYDRANTS_NEW + " SET ClowTagged = 0 where ClowTagged is null;" +
                    "UPDATE " + TableNames.HYDRANTS_NEW + " SET Elevation = null where isNumeric(elevation) = 0;" +
                    "UPDATE " + TableNames.HYDRANTS_NEW + " SET DateInst = NULL where DateInst  = '01/01/1900';" +
                    "UPDATE " + TableNames.HYDRANTS_NEW + " SET DateRemoved = NULL where DateRemoved = '01/01/1900';" +
                    "UPDATE " + TableNames.HYDRANTS_NEW + " SET DateTested = NULL where DateTested = '01/01/1900';" +
                    "UPDATE " + TableNames.HYDRANTS_NEW + " SET BillingDate = NULL where BillingDate = '01/01/1900';" +
                    "UPDATE " + TableNames.HYDRANTS_NEW + " SET CriticalNotes = NULL where CriticalNotes = '';" +
                    "UPDATE " + TableNames.HYDRANTS_NEW + " SET Location = NULL where Location = '';" +
                    "UPDATE " + TableNames.HYDRANTS_NEW + " SET MapPage = NULL where MapPage = '';" +
                    "UPDATE " + TableNames.HYDRANTS_NEW + " SET StNum = NULL where StNum = '';" +
                    "UPDATE " + TableNames.HYDRANTS_NEW + " SET ValLoc = NULL where ValLoc = '';" +
                    "UPDATE " + TableNames.HYDRANTS_NEW + " SET WONum = NULL where WONum = '';" +
                    "UPDATE " + TableNames.HYDRANTS_NEW + " SET PremiseNumber = NULL where PremiseNumber = '';" +
                    "IF EXISTS (select 1 from sysobjects where name = 'DF_tblNJAWHydrant_InspFreq') ALTER TABLE " +
                    TableNames.HYDRANTS_NEW + " DROP CONSTRAINT [DF_tblNJAWHydrant_InspFreq];",
                ROLLBACK_HYDRANTS =
                    "UPDATE " + TableNames.HYDRANTS_NEW +
                    " SET BPUKPI = CASE WHEN (BPUKPI = 1) THEN 'ON' ELSE NULL END;" +
                    "UPDATE " + TableNames.HYDRANTS_NEW +
                    " SET Critical = CASE WHEN (Critical= 1) THEN 'ON' ELSE NULL END;" +
                    "UPDATE " + TableNames.HYDRANTS_NEW + " SET DEM = CASE WHEN (DEM = 1) THEN 'YES' ELSE NULL END;" +
                    "UPDATE " + TableNames.HYDRANTS_NEW +
                    " SET OutOfServ = CASE WHEN (OutOfServ= 1) THEN 'ON' ELSE NULL END;" +
                    "ALTER TABLE " + TableNames.HYDRANTS_NEW +
                    " ADD  CONSTRAINT [DF_tblNJAWHydrant_InspFreq]  DEFAULT (1) FOR [InspFreq]",
                UPDATE_HYDRANT_COORDINATES = @"
                        SET NOCOUNT ON 
                        DECLARE @latitude float
                        DECLARE @longitude float
                        DECLARE @id int
                        DECLARE @coordinateID int

                        DECLARE	tableCursor 
                        CURSOR FOR 
	                        SELECT RecId, Lat, Lon FROM Hydrants WHERE Lat is not null and Lon is not null 

                        OPEN tableCursor 
	                        FETCH NEXT FROM tableCursor INTO @id, @latitude, @longitude; 
	                        WHILE @@FETCH_STATUS = 0 
	                        BEGIN 
		                        Insert Into Coordinates(latitude, longitude) values(@latitude, @longitude)
		                        update Hydrants set coordinateID = @@Identity where RecId = @id
		                        FETCH NEXT FROM tableCursor INTO @id, @latitude, @longitude; 
	                        END
                        CLOSE tableCursor; 
                        DEALLOCATE tableCursor;",
                ROLLBACK_HYDRANT_COORDINATES =
                    "Update Hydrants Set Lat = (SELECT Latitude from Coordinates where Coordinates.CoordinateID = Hydrants.CoordinateID);" +
                    "Update Hydrants Set Lon = (SELECT Longitude from Coordinates where Coordinates.CoordinateID = Hydrants.CoordinateID);",
                DROP_HYDRANT_INDEXES =
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_index_tblNJAWHydrant_15_580913141__K38_K41_K1_K32_K7_K30_K25_K21_K18_K34') DROP INDEX [_dta_index_tblNJAWHydrant_15_580913141__K38_K41_K1_K32_K7_K30_K25_K21_K18_K34] ON [dbo].[tblNJAWHydrant];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_index_tblNJAWHydrant_15_936390405__K40_K18_K31_K17_K28_K6_K24_K29_K33_K5_K8') DROP INDEX [_dta_index_tblNJAWHydrant_15_936390405__K40_K18_K31_K17_K28_K6_K24_K29_K33_K5_K8] ON [dbo].[tblNJAWHydrant];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_index_tblNJAWHydrant_5_580913141__K1_K38_K41_K18_K32_7_17_39_42') DROP INDEX [_dta_index_tblNJAWHydrant_5_580913141__K1_K38_K41_K18_K32_7_17_39_42] ON [dbo].[tblNJAWHydrant];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_index_tblNJAWHydrant_5_580913141__K38_K18_K32_K41_1_7_17_39_42') DROP INDEX [_dta_index_tblNJAWHydrant_5_580913141__K38_K18_K32_K41_1_7_17_39_42] ON [dbo].[tblNJAWHydrant];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_index_tblNJAWHydrant_7_1131203130__K18_K32_K41_K25_K30_K34') DROP INDEX [_dta_index_tblNJAWHydrant_7_1131203130__K18_K32_K41_K25_K30_K34] ON [dbo].[tblNJAWHydrant];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_index_tblNJAWHydrant_7_1131203130__K32_K41_K18_K25_K30_K34') DROP INDEX [_dta_index_tblNJAWHydrant_7_1131203130__K32_K41_K18_K25_K30_K34] ON [dbo].[tblNJAWHydrant];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_index_tblNJAWHydrant_8_580913141__K34_K21') DROP INDEX [_dta_index_tblNJAWHydrant_8_580913141__K34_K21] ON [dbo].[tblNJAWHydrant];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_1131203130_41_32') DROP STATISTICS [tblNJAWHydrant].[_dta_stat_1131203130_41_32]" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_580913141_1_32_38_34_47') DROP STATISTICS [tblNJAWHydrant].[_dta_stat_580913141_1_32_38_34_47]" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_580913141_1_32_57_34_47_38_58_41') DROP STATISTICS [tblNJAWHydrant].[_dta_stat_580913141_1_32_57_34_47_38_58_41]" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_580913141_1_32_58_34_47_38') DROP STATISTICS [tblNJAWHydrant].[_dta_stat_580913141_1_32_58_34_47_38]" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_580913141_32_18_25_30_1_2_8_23_24') DROP STATISTICS [tblNJAWHydrant].[_dta_stat_580913141_32_18_25_30_1_2_8_23_24]" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_580913141_32_18_29_21_5_6_34_17_13_48_49') DROP STATISTICS [tblNJAWHydrant].[_dta_stat_580913141_32_18_29_21_5_6_34_17_13_48_49]" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_580913141_32_21_41_38_18_47_7_8_34_39_45_42_17_15_31_1') DROP STATISTICS [tblNJAWHydrant].[_dta_stat_580913141_32_21_41_38_18_47_7_8_34_39_45_42_17_15_31_1]" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_580913141_32_34_47_38_58_57_41') DROP STATISTICS [tblNJAWHydrant].[_dta_stat_580913141_32_34_47_38_58_57_41]" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_580913141_34_1_32_47') DROP STATISTICS [tblNJAWHydrant].[_dta_stat_580913141_34_1_32_47]" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_580913141_34_47_38_58_57_41_1') DROP STATISTICS [tblNJAWHydrant].[_dta_stat_580913141_34_47_38_58_57_41_1]" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_580913141_38_18_32_21') DROP STATISTICS [tblNJAWHydrant].[_dta_stat_580913141_38_18_32_21]" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_580913141_41_1_32_34_47_38_58') DROP STATISTICS [tblNJAWHydrant].[_dta_stat_580913141_41_1_32_34_47_38_58]" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_580913141_47_1_32') DROP STATISTICS [tblNJAWHydrant].[_dta_stat_580913141_47_1_32]" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_580913141_57_1') DROP STATISTICS [tblNJAWHydrant].[_dta_stat_580913141_57_1]" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_580913141_58_1') DROP STATISTICS [tblNJAWHydrant].[_dta_stat_580913141_58_1]" +
                    "ALTER TABLE [dbo].[tblNJAWHydrant] DROP CONSTRAINT [DF_tblNJAWHydrant_Critical];" +
                    "ALTER TABLE [dbo].[tblNJAWHydrant] DROP CONSTRAINT [DF_tblNJAWHydrant_OutOfServ];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = 'IDX_HydSuf') DROP INDEX [IDX_HydSuf] ON [dbo].[tblNJAWHydrant];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = 'IDX_LatSize') DROP INDEX [IDX_LatSize] ON [dbo].[tblNJAWHydrant];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = 'Town') DROP INDEX [Town] ON [dbo].[tblNJAWHydrant];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = 'IDX_OpCntr') DROP INDEX [IDX_OpCntr] ON [dbo].[tblNJAWHydrant];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = 'STAT_HydNum_OpCntr_ActRet_Town') DROP STATISTICS [dbo].[tblNJAWHydrant].[STAT_HydNum_OpCntr_ActRet_Town];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = 'STAT_ActRet_Town') DROP STATISTICS [dbo].[tblNJAWHydrant].[STAT_ActRet_Town];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = 'STAT_ActRet_HydNum') DROP STATISTICS [dbo].[tblNJAWHydrant].[STAT_ActRet_HydNum];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = 'STAT_Town_StName_HydNum_OpCntr') DROP STATISTICS [dbo].[tblNJAWHydrant].[STAT_Town_StName_HydNum_OpCntr]",
                RESTORE_HYDRANT_INDEXES =
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWHydrant_15_580913141__K38_K41_K1_K32_K7_K30_K25_K21_K18_K34] ON [dbo].[tblNJAWHydrant]" +
                    "([StName] ASC,[Town] ASC,[ActRet] ASC,[OpCntr] ASC,[CrossStreet] ASC,[Lon] ASC,[Lat] ASC,[HydSuf] ASC,[HydNum] ASC,[RecID] ASC) ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWHydrant_15_936390405__K40_K18_K31_K17_K28_K6_K24_K29_K33_K5_K8] ON [dbo].[tblNJAWHydrant]" +
                    "([Town] ASC,[HydNum] ASC,[OpCntr] ASC,[HydMake] ASC,[Location] ASC,[CriticalNotes] ASC,[Lat] ASC,[Lon] ASC,[RecID] ASC,[Critical] ASC,[DateInst] ASC)  ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWHydrant_5_580913141__K1_K38_K41_K18_K32_7_17_39_42] ON [dbo].[tblNJAWHydrant]" +
                    "([ActRet] ASC,[StName] ASC,[Town] ASC,[HydNum] ASC,[OpCntr] ASC,[CrossStreet] ASC,[HydMake] ASC,[StNum] ASC,[TwnSection] ASC) ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWHydrant_5_580913141__K38_K18_K32_K41_1_7_17_39_42] ON [dbo].[tblNJAWHydrant]" +
                    "([StName] ASC,[HydNum] ASC,[OpCntr] ASC,[Town] ASC) ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWHydrant_7_1131203130__K18_K32_K41_K25_K30_K34] ON [dbo].[tblNJAWHydrant]" +
                    "([HydNum] ASC,[OpCntr] ASC,[Town] ASC,[Lat] ASC,[Lon] ASC,[RecID] ASC) ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWHydrant_7_1131203130__K32_K41_K18_K25_K30_K34] ON [dbo].[tblNJAWHydrant]" +
                    "([OpCntr] ASC,[Town] ASC,[HydNum] ASC,[Lat] ASC,[Lon] ASC,[RecID] ASC) ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWHydrant_8_580913141__K34_K21] ON [dbo].[tblNJAWHydrant]" +
                    "([RecID] ASC,[HydSuf] ASC) ON [PRIMARY];" +
                    "ALTER TABLE [dbo].[tblNJAWHydrant] ADD  CONSTRAINT [DF_tblNJAWHydrant_Critical]  DEFAULT ('') FOR [Critical];" +
                    "ALTER TABLE [dbo].[tblNJAWHydrant] ADD  CONSTRAINT [DF_tblNJAWHydrant_OutOfServ]  DEFAULT ('') FOR [OutOfServ];" +
                    "CREATE NONCLUSTERED INDEX [IDX_HydSuf] ON [dbo].[tblNJAWHydrant]([RecID] ASC,[HydSuf] ASC)  ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [IDX_LatSize] ON [dbo].[tblNJAWHydrant]([LatSize] ASC) ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [IDX_OpCntr] ON [dbo].[tblNJAWHydrant] ( [OpCntr] ASC ) ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [Town] ON [dbo].[tblNJAWHydrant] ( [Town] ASC ) ON [PRIMARY];" +
                    "CREATE STATISTICS [_dta_stat_1131203130_41_32] ON [dbo].[tblNJAWHydrant]([Town], [OpCntr], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_580913141_1_32_38_34_47] ON [dbo].[tblNJAWHydrant]([ActRet], [OpCntr], [StName], [RecID], [FireDistrictID]);" +
                    "CREATE STATISTICS [_dta_stat_580913141_1_32_57_34_47_38_58_41] ON [dbo].[tblNJAWHydrant]([ActRet], [OpCntr], [ManufacturerID], [RecID], [FireDistrictID], [StName], [HydrantModelID], [Town]);" +
                    "CREATE STATISTICS [_dta_stat_580913141_1_32_58_34_47_38] ON [dbo].[tblNJAWHydrant]([ActRet], [OpCntr], [HydrantModelID], [RecID], [FireDistrictID], [StName]);" +
                    "CREATE STATISTICS [_dta_stat_580913141_32_18_25_30_1_2_8_23_24] ON [dbo].[tblNJAWHydrant]([OpCntr], [HydNum], [Lat], [Lon], [ActRet], [BillInfo], [DateInst], [InspFreq], [InspFreqUnit], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_580913141_32_18_29_21_5_6_34_17_13_48_49] ON [dbo].[tblNJAWHydrant]([OpCntr], [HydNum], [Location], [HydSuf], [Critical], [CriticalNotes], [RecID], [HydMake], [DirOpen], [YearManufactured], [ManufacturedUpdated]);" +
                    "CREATE STATISTICS [_dta_stat_580913141_32_21_41_38_18_47_7_8_34_39_45_42_17_15_31_1] ON [dbo].[tblNJAWHydrant]([OpCntr], [HydSuf], [Town], [StName], [HydNum], [FireDistrictID], [CrossStreet], [DateInst], [RecID], [StNum], [WONum], [TwnSection], [HydMake], [FireD], [MapPage], [ActRet]);" +
                    "CREATE STATISTICS [_dta_stat_580913141_32_34_47_38_58_57_41] ON [dbo].[tblNJAWHydrant]([OpCntr], [RecID], [FireDistrictID], [StName], [HydrantModelID], [ManufacturerID], [Town]);" +
                    "CREATE STATISTICS [_dta_stat_580913141_34_1_32_47] ON [dbo].[tblNJAWHydrant]([RecID], [ActRet], [OpCntr], [FireDistrictID]);" +
                    "CREATE STATISTICS [_dta_stat_580913141_34_47_38_58_57_41_1] ON [dbo].[tblNJAWHydrant]([RecID], [FireDistrictID], [StName], [HydrantModelID], [ManufacturerID], [Town], [ActRet]);" +
                    "CREATE STATISTICS [_dta_stat_580913141_38_18_32_21] ON [dbo].[tblNJAWHydrant]([StName], [HydNum], [OpCntr], [HydSuf], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_580913141_41_1_32_34_47_38_58] ON [dbo].[tblNJAWHydrant]([Town], [ActRet], [OpCntr], [RecID], [FireDistrictID], [StName], [HydrantModelID]);" +
                    "CREATE STATISTICS [_dta_stat_580913141_47_1_32] ON [dbo].[tblNJAWHydrant]([FireDistrictID], [ActRet], [OpCntr]);" +
                    "CREATE STATISTICS [_dta_stat_580913141_57_1] ON [dbo].[tblNJAWHydrant]([ManufacturerID], [ActRet]);" +
                    "CREATE STATISTICS [_dta_stat_580913141_58_1] ON [dbo].[tblNJAWHydrant]([HydrantModelID], [ActRet]);" +
                    "CREATE STATISTICS [STAT_HydNum_OpCntr_ActRet_Town] ON [dbo].[tblNJAWHydrant]([HydNum], [OpCntr], [ActRet], [Town], [RecID]);" +
                    "CREATE STATISTICS [STAT_ActRet_Town] ON [dbo].[tblNJAWHydrant]([ActRet], [Town], [RecID]);" +
                    "CREATE STATISTICS [STAT_ActRet_HydNum] ON [dbo].[tblNJAWHydrant]([ActRet], [HydNum], [RecID]);" +
                    "CREATE STATISTICS [STAT_Town_StName_HydNum_OpCntr] ON [dbo].[tblNJAWHydrant]([Town], [StName], [HydNum], [OpCntr], [RecID]);",
                UPDATE_HYDRANT_INSPECTIONS =
                    "UPDATE " + TableNames.HYDRANT_INSPECTIONS_NEW + " SET " + NewColumnNames.INSPECTION_TYPE +
                    " = (SELECT Id from HydrantInspectionTypes where Description = Inspect);" +
                    "UPDATE " + TableNames.HYDRANT_INSPECTIONS_NEW + " SET " + NewColumnNames.WORK_ORDER_REQUEST_1 +
                    " = (SELECT Id from " + TableNames.WORKORDER_REQUESTS + " WHERE DESCRIPTION = WoReq1);" +
                    "UPDATE " + TableNames.HYDRANT_INSPECTIONS_NEW + " SET " + NewColumnNames.WORK_ORDER_REQUEST_2 +
                    " = (SELECT Id from " + TableNames.WORKORDER_REQUESTS + " WHERE DESCRIPTION = WoReq2);" +
                    "UPDATE " + TableNames.HYDRANT_INSPECTIONS_NEW + " SET " + NewColumnNames.WORK_ORDER_REQUEST_3 +
                    " = (SELECT Id from " + TableNames.WORKORDER_REQUESTS + " WHERE DESCRIPTION = WoReq3);" +
                    "UPDATE " + TableNames.HYDRANT_INSPECTIONS_NEW + " SET " + NewColumnNames.WORK_ORDER_REQUEST_4 +
                    " = (SELECT Id from " + TableNames.WORKORDER_REQUESTS + " WHERE DESCRIPTION = WoReq4);" +
                    "UPDATE " + TableNames.HYDRANT_INSPECTIONS_NEW + " SET " + NewColumnNames.HYDRANT +
                    " = (SELECT TOP 1 RecID from " + TableNames.HYDRANTS_OLD + " H WHERE H.HydNum = [" +
                    TableNames.HYDRANT_INSPECTIONS_NEW + "].HydNum and H.OpCntr = [" +
                    TableNames.HYDRANT_INSPECTIONS_NEW + "].OpCntr)" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'mortara' where inspectedBy = 'Anthony Mortarulo';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'bertbaker' where inspectedBy = 'bakerb';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'diskinc' where inspectedBy = 'Charles Diskin';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'bruenod' where inspectedBy = 'David Brueno';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'williamsd' where inspectedBy = 'Dock Williams';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'jonesdo' where inspectedBy = 'Douglas Jones';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'dougthorn' where inspectedBy = 'Douglas Thorn';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'eilbacher' where inspectedBy = 'eilbachg';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'picconef' where inspectedBy = 'Frank Piccone';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'amatog' where inspectedBy = 'Gerald Amato';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'joegreen' where inspectedBy = 'greenjo';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'joshgwyn' where inspectedBy = 'gwynj';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'davisj' where inspectedBy = 'James Davis';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'straskoj' where inspectedBy = 'James Strasko';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'viladej' where inspectedBy = 'James Vilade';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'perezj' where inspectedBy = 'Jorge Parez';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'muhajo' where inspectedBy = 'Joseph Muha';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'pierjo' where inspectedBy = 'Joseph Pier';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'smithka' where inspectedBy = 'karlsm';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'mobleyl' where inspectedBy = 'Leroy Mobley';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'kevinmaloney' where inspectedBy = 'maloneyk';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'oeckinm' where inspectedBy = 'Mark Oeckinghaus';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'pannellam' where inspectedBy = 'Mark Pannella';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'stevemason' where inspectedBy = 'masonst';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'bormanmw' where inspectedBy = 'Mike Bormann';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'amermanp' where inspectedBy = 'Paul Amerman';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'baceri' where inspectedBy = 'Richard Bace';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'allainr' where inspectedBy = 'Robert Allain';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'sullivanr' where inspectedBy = 'Robert Sullivan';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'bibbor' where inspectedBy = 'Rosario Bibbo';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'hubertsingley' where inspectedBy = 'singleyh';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'joestankiewicz' where inspectedBy = 'stankiej';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'sablacks' where inspectedBy = 'Steve Sablack';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'spaint' where inspectedBy = 'Tod Spain';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'boburban' where inspectedBy = 'urbanr';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'williamsd' where inspectedBy = 'willimasd';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedBy = 'sutulaz' where inspectedBy = 'Ziggy Sutula';" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedById = (select top 1 RecId from tblPermissions where username = InspectedBy);" +
                    "update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " set InspectedById = (select top 1 RecId from tblPermissions where username = 'mcadmin') where InspectedById is null;" +
                    "DELETE FROM " + TableNames.HYDRANT_INSPECTIONS_NEW + " WHERE DateInspected IS NULL",
                // InspectedById/InspectorNum
                UPDATE_BLOWOFF_INSPECTIONS = "INSERT INTO [dbo].[BlowOffInspections] " +
                                             "  ([ResidualChlorine], [DateAdded], [DateInspected], [FullFlow], [GallonsFlowed], [GPM], [MinutesFlowed], [StaticPressure], [Remarks], [TotalChlorine], [HydrantProblemId], [HydrantTagStatusId], " +
                                             "  [InspectionTypeId], [WorkOrderRequest1], [WorkOrderRequest2], [WorkOrderRequest3], [WorkOrderRequest4], [ValveId], [InspectedById])" +
                                             "SELECT " +
                                             "  Chlorine, DateAdded, DateInspect, FullFlow, GalFlow, GPM, MinFlow, PressStatic, Remarks, TotalChlorine, HydrantProblemId, HydrantTagStatusId," +
                                             "  (Select Id from HydrantInspectionTypes where Description = Inspect)," +
                                             "  (Select Id from WorkOrderRequests where Description = WoReq1)," +
                                             "  (Select Id from WorkOrderRequests where Description = WoReq2)," +
                                             "  (Select Id from WorkOrderRequests where Description = WoReq3)," +
                                             "  (Select Id from WorkOrderRequests where Description = WoReq4)," +
                                             "  (SELECT top 1 RecID from tblNJAWValves V where V.ValNum = tblNJAWHydInspData.HydNum AND V.OpCntr = tblNJAWHydInspData.OpCntr)," +
                                             "  COALESCE((Select RecID from tblPermissions where UserName = inspectedBy), (Select RecID from tblPermissions where userName = 'mcadmin'))" +
                                             "FROM " +
                                             "  tblNJAWHydInspData WHERE HydNum LIKE 'V%' AND DateInspect is not null;" +
                                             "DELETE FROM [tblNJAWHydInspData] WHERE HydNum LIKE 'V%';",
                DROP_INDEXES =
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_index_tblNJAWHydInspData_7_1057438841__K13_K7_K3_K17_K9') DROP INDEX [_dta_index_tblNJAWHydInspData_7_1057438841__K13_K7_K3_K17_K9] ON [tblNJAWHydInspData];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_index_tblNJAWHydInspData_7_1057438841__K13_K7_K3_K9') DROP INDEX [_dta_index_tblNJAWHydInspData_7_1057438841__K13_K7_K3_K9] ON [tblNJAWHydInspData]" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_index_tblNJAWHydInspData_7_1057438841__K3_K9_K13') DROP INDEX [_dta_index_tblNJAWHydInspData_7_1057438841__K3_K9_K13] ON [tblNJAWHydInspData]" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = 'IDX_OpCntr_RecID') DROP INDEX [IDX_OpCntr_RecID] ON [tblNJAWHydInspData];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_1057438841_3_13_7_17_9') DROP STATISTICS [tblNJAWHydInspData].[_dta_stat_1057438841_3_13_7_17_9];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_237959924_3_7') DROP STATISTICS [tblNJAWHydInspData].[_dta_stat_237959924_3_7];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_237959924_7_13_3_15') DROP STATISTICS [tblNJAWHydInspData].[_dta_stat_237959924_7_13_3_15];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_237959924_7_13_3_17_15') DROP STATISTICS [tblNJAWHydInspData].[_dta_stat_237959924_7_13_3_17_15];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_237959924_7_13_3_17_15') DROP STATISTICS [tblNJAWHydInspData].[_dta_stat_237959924_7_13_3_17_15]" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_237959924_7_13_17_15') DROP STATISTICS [tblNJAWHydInspData].[_dta_stat_237959924_7_13_17_15];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_237959924_7_13_23') DROP STATISTICS [dbo].[tblNJAWHydInspData].[_dta_stat_237959924_7_13_23];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_stat_237959924_3_26') DROP STATISTICS [dbo].[tblNJAWHydInspData].[_dta_stat_237959924_3_26];" +
                    "IF EXISTS (SELECT 1 from sysindexes where Name = '_dta_index_tblNJAWHydInspData_19_237959924__K26_K3_9_10') DROP INDEX [tblNJAWHydInspData].[_dta_index_tblNJAWHydInspData_19_237959924__K26_K3_9_10];",
                ROLLBACK_HYDRANT_INSPECTIONS =
                    "UPDATE " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " SET Inspect = (Select Description from HydrantInspectionTypes hit where hit.Id = InspectionTypeId);" +
                    "UPDATE " + TableNames.HYDRANT_INSPECTIONS_NEW + " SET WoReq1 = (SELECT Description from " +
                    TableNames.WORKORDER_REQUESTS + " WHERE Id = WoReq1);" +
                    "UPDATE " + TableNames.HYDRANT_INSPECTIONS_NEW + " SET WoReq2 = (SELECT Description from " +
                    TableNames.WORKORDER_REQUESTS + " WHERE Id = WoReq2);" +
                    "UPDATE " + TableNames.HYDRANT_INSPECTIONS_NEW + " SET WoReq3 = (SELECT Description from " +
                    TableNames.WORKORDER_REQUESTS + " WHERE Id = WoReq3);" +
                    "UPDATE " + TableNames.HYDRANT_INSPECTIONS_NEW + " SET WoReq4 = (SELECT Description from " +
                    TableNames.WORKORDER_REQUESTS + " WHERE Id = WoReq4);" +
                    "UPDATE " + TableNames.HYDRANT_INSPECTIONS_NEW + " SET HydNum = (SELECT HydNum from " +
                    TableNames.HYDRANTS_OLD + " WHERE RecID = HydrantId);" +
                    "UPDATE " + TableNames.HYDRANT_INSPECTIONS_NEW + " SET HydSuf = (SELECT HydSuf from " +
                    TableNames.HYDRANTS_OLD + " WHERE RecID = HydrantId);" +
                    "UPDATE " + TableNames.HYDRANT_INSPECTIONS_NEW + " SET OpCntr = (SELECT OpCntr from " +
                    TableNames.HYDRANTS_OLD + " WHERE RecID = HydrantId);" +
                    "UPDATE " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " SET InspectedBy = (select top 1 username from tblPermissions where RecID = inspectedById);" +
                    "UPDATE " + TableNames.HYDRANT_INSPECTIONS_NEW +
                    " SET InspectorNum = (select top 1 empNum from tblPermissions where RecID = inspectedById);",
                ROLLBACK_BLOWOFF_INSPECTIONS = "INSERT INTO [dbo].[tblNJAWHydInspData]" +
                                               " ([Chlorine],[DateAdded],[DateInspect],[FullFlow],[GalFlow],[GPM],[HydNum],[HydSuf],[Inspect],[InspectedBy],[InspectorNum],[MinFlow]" +
                                               " ,[OpCntr],[PressStatic],[Remarks],[WOReq1],[WOReq2],[WOReq3],[WOReq4],[HydrantProblemID],[HydrantTagStatusID],[TotalChlorine])" +
                                               "SELECT" +
                                               " [ResidualChlorine],[DateAdded],[DateInspected],[FullFlow],[GallonsFlowed],[GPM]," +
                                               " (Select ValNum from tblNJAWValves where RecID = ValveId) as [HydNum]," +
                                               " (Select ValSuf from tblNJAWValves where RecID = ValveId) as [HydSuf]," +
                                               " (Select Description from HydrantInspectionTypes where ID = InspectionTypeId) as [Inspect]," +
                                               " (Select username from tblPermissions where RecID = InspectedById) as [InspectedBy]," +
                                               " (Select empnum from tblPermissions where RecID = InspectedById) as [InspectorNum]," +
                                               " [MinutesFlowed]," +
                                               " (Select OpCntr from tblNJAWValves where RecID = ValveId) as [OpCntr]," +
                                               " [StaticPressure],[Remarks]," +
                                               " (Select Description from WorkOrderRequests where Id = WorkOrderRequest1) as [WOReq1]," +
                                               " (Select Description from WorkOrderRequests where Id = WorkOrderRequest2) as [WOReq2]," +
                                               " (Select Description from WorkOrderRequests where Id = WorkOrderRequest3) as [WOReq3]," +
                                               " (Select Description from WorkOrderRequests where Id = WorkOrderRequest4) as [WOReq4]," +
                                               " [HydrantProblemID]," +
                                               " [HydrantTagStatusID]," +
                                               " [TotalChlorine] " +
                                               "FROM " +
                                               " [BlowOffInspections];",
                RESTORE_INDEXES =
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWHydInspData_7_1057438841__K13_K7_K3_K17_K9] ON [tblNJAWHydInspData]([OpCntr] ASC, [HydNum] ASC, [DateInspect] ASC, [WOReq1] ASC, [Inspect] ASC) ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWHydInspData_7_1057438841__K13_K7_K3_K9] ON [tblNJAWHydInspData] ([OpCntr] ASC, [HydNum] ASC, [DateInspect] ASC, [Inspect] ASC) ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWHydInspData_7_1057438841__K3_K9_K13] ON [tblNJAWHydInspData] ([DateInspect] ASC, [Inspect] ASC, [OpCntr] ASC, [HydNum] ASC) ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [IDX_OpCntr_RecID] ON [tblNJAWHydInspData] ([OpCntr] ASC, [RecID] ASC, [GalFlow] ASC, [GPM] ASC, [MinFlow] ASC) ON [PRIMARY];" +
                    "CREATE STATISTICS [_dta_stat_1057438841_3_13_7_17_9] ON [tblNJAWHydInspData]([DateInspect], [OpCntr], [HydNum], [WOReq1], [Inspect], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_237959924_3_7] ON [tblNJAWHydInspData]([DateInspect], [HydNum], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_237959924_7_13_3_15] ON [tblNJAWHydInspData]([HydNum], [OpCntr], [DateInspect], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_237959924_7_13_3_17_15] ON [dbo].[tblNJAWHydInspData]([HydNum], [OpCntr], [DateInspect], [WOReq1], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_237959924_7_13_17_15] ON [dbo].[tblNJAWHydInspData]([HydNum], [OpCntr], [WOReq1], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_237959924_7_13_23] ON [dbo].[tblNJAWHydInspData]([HydNum], [OpCntr], [HydrantTagStatusID], [RecID]);" +
                    "CREATE STATISTICS [_dta_stat_237959924_3_26] ON [dbo].[tblNJAWHydInspData]([DateInspect], [HydrantId]);" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWHydInspData_19_237959924__K26_K3_9_10] ON [dbo].[tblNJAWHydInspData]([HydrantId] ASC,[DateInspect] ASC) INCLUDE ([Inspect], [InspectedBy]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]";
        }

        public struct TableNames
        {
            public const string BLOW_OFF_INSPECTIONS = "BlowOffInspections",
                                COORDINATES = "Coordinates",
                                GRADIENTS = "Gradients",
                                HYDRANT_BILLING = "HydrantBillings",
                                HYDRANT_DIRECTIONS = "HydrantDirections",
                                HYDRANT_INSPECTION_TYPES = "HydrantInspectionTypes",
                                HYDRANT_INSPECTIONS_OLD = "tblNJAWHydInspData",
                                HYDRANT_INSPECTIONS_NEW = "HydrantInspections",
                                HYDRANT_MAIN_SIZES = "HydrantMainSizes",
                                HYDRANT_MODELS = "HydrantModels",
                                HYDRANT_PROBLEMS = "HydrantProblems",
                                HYDRANT_SIZES = "HydrantSizes", // MAIN WASHER SIZE
                                HYDRANT_STATUSES = "HydrantStatuses",
                                HYDRANT_THREAD_TYPES = "HydrantThreadTypes",
                                HYDRANT_TAG_STATUSES = "HydrantTagStatuses",
                                HYDRANTS_OLD = "tblNJAWHydrant",
                                HYDRANTS_NEW = "Hydrants",
                                MANUFACTURERS_NEW = "HydrantManufacturers",
                                MANUFACTURERS_OLD = "Manufacturers",
                                INSPECTION_FREQUENCY_UNITS = "RecurringFrequencyUnits",
                                LATERAL_SIZES = "LateralSizes",
                                MAIN_TYPES = "MainTypes",
                                OPERATING_CENTERS = "OperatingCenters",
                                STREETS = "Streets",
                                TOWN_SECTIONS = "TownSections",
                                USERS = "tblPermissions",
                                VALVES = "tblNJAWValves",
                                WORKORDER_REQUESTS = "WorkOrderRequests",
                                WATER_SYSTEMS = "WaterSystems",
                                FUNCTIONAL_LOCATIONS = "FunctionalLocations",
                                HYDRANTS_SAP = "tblNJAWHydrantSAP";
        }

        public struct OldColumnNames
        {
            public const string
                HYD_BILL_INFO = "BillInfo",
                HYD_BPU_KPI = "BPUKPI",
                HYD_BRANCH_LENGTH_FEET = "BranchLnFt",
                HYD_BRANCH_LENGTH_INCHES = "BranchLnIn",
                HYD_BRANCH_LN = "BranchLn",
                HYD_CLOW_TAGGED = "ClowTagged",
                HYD_CRITICAL = "Critical",
                HYD_CROSS_STREET = "CrossStreet",
                HYD_DATE_INSTALLED = "DateInst",
                HYD_DATE_RETIRED = "DateRemoved",
                HYD_DATE_TESTED = "DateTested",
                HYD_DIRECTION = "DirOpen",
                HYD_DEAD_END_MAIN = "DEM",
                HYD_DEPTH_BURY = "DepthBury",
                HYD_DEPTH_BURY_FEET = "DepthBuryFt",
                HYD_DEPTH_BURY_INCHES = "DepthBuryIn",
                HYD_ELEVATION = "Elevation",
                HYD_FIRE_D = "FireD",
                HYD_FL_ROUTE_NUMBER = "FLRouteNumber",
                HYD_FL_SEQUENCE = "FLRouteSequence",
                HYD_GRADIENT = "Gradiant",
                HYD_HAS_NO_LATERAL_VALVE = "HasNoLateralValve",
                HYD_HISTORICAL_HYDRANT_LOCATION = "HistoricalHydrantLocation",
                HYD_HISTORICAL_VALVE_LOCATION = "HistoricalValveLocation",
                HYD_HYD_MAKE = "HydMake",
                HYD_HYD_STYLE = "HydStyle",
                HYD_HYDRANT_NUMBER = "HydNum",
                HYD_HYDRANT_SIZE = "HydSize",
                HYD_HYDRANT_STATUS = "ActRet",
                HYD_HYDRANT_SUFFIX = "HydSuf",
                HYD_ID = "RecID",
                HYD_INITIATOR = "Initiator",
                HYD_INSPECTION_FREQUENCY = "InspFreq",
                HYD_INSPECTION_FREQUENCY_UNIT = "InspFreqUnit",
                HYD_LATERAL_SIZE = "LatSize",
                HYD_LATERAL_VALVE = "LatValNum",
                HYD_LATITUDE = "Lat",
                HYD_LONGITUDE = "Lon",
                HYD_LINK_NUM = "LinkNum",
                HYD_OPERATING_CENTER = "OpCntr",
                HYD_OUT_OF_SERVICE = "OutOfServ",
                HYD_MAIN_SIZE = "SizeofMain",
                HYD_MAIN_TYPE = "TypeMain",
                HYD_MANUFACTURER_UPDATED = "ManufacturedUpdated",
                HYD_MANUFACTURER_UPDATED_BY = "ManufacturedUpdatedBy",
                HYD_STREET = "StName",
                HYD_STREET_NUMBER = "StNum",
                HYD_THREAD = "Thread",
                HYD_TOWN = "Town",
                HYD_TOWN_SECTION = "TwnSection",
                HYD_VALVE_LOCATION = "ValLoc",
                HYD_WORK_ORDER_NUMBER = "WONum",
                MAN_DESCRIPTION = "Name",
                MAN_ID = "ManufacturerID",
                HYDRANT_MODEL_ID = "HydrantModelId",
                HYDRANT_MODEL_DESCRIPTION = "Name",
                DATE_INSPECTED = "DateInspect",
                GALLONS_FLOWED = "GalFlow",
                GPM = "GPM",
                HYD_INSP_NUM = "HydNum",
                HYD_SUF = "HydSuf",
                INSPECTION_TYPE = "Inspect",
                INSPECTED_BY = "InspectedBy",
                INSPECTOR_NUM = "InspectorNum",
                MIN_FLOW = "MinFlow",
                OPERATING_CENTER = "OpCntr",
                RESIDUAL_CHLORINE = "Chlorine",
                STATIC_PRESSURE = "PressStatic",
                REMARKS = "Remarks",
                WORK_ORDER_REQUEST_1 = "WoReq1",
                WORK_ORDER_REQUEST_2 = "WoReq2",
                WORK_ORDER_REQUEST_3 = "WoReq3",
                WORK_ORDER_REQUEST_4 = "WoReq4",
                SIZE_OPENING = "SizeOpening",
                HYDRANT_PROBLEM = "HydrantProblemID",
                HYDRANT_TAG_STATUS = "HydrantTagStatusID",
                ID = "RecID",
                HYD_WATER_SYSTEM = "WaterSystem",
                FUNCTIONAL_LOCATION = "FunctionalLocationID";
        }

        public struct NewColumnNames
        {
            public const string
                HYD_BILL_INFO = "HydrantBillingId",
                HYD_BPU_KPI = "IsNonBPUKPI",
                HYD_BRANCH_LENGTH_FEET = "BranchLengthFeet",
                HYD_BRANCH_LENGTH_INCHES = "BranchLengthInches",
                HYD_COORDINATE_ID = "CoordinateId",
                HYD_CROSS_STREET = "CrossStreetId",
                HYD_DATE_INSTALLED = "DateInstalled",
                HYD_DATE_RETIRED = "DateRetired",
                HYD_DEAD_END_MAIN = "IsDeadEndMain",
                HYD_DEPTH_BURY_FEET = "DepthBuryFeet",
                HYD_DEPTH_BURY_INCHES = "DepthBuryInches",
                HYD_DIRECTION = "OpensDirectionId",
                HYD_GRADIENT = "GradientId",
                HYD_HYDRANT_NUMBER = "HydrantNumber",
                HYD_HYDRANT_SIZE = "HydrantSizeId",
                HYD_HYDRANT_STATUS = "HydrantStatusId",
                HYD_HYDRANT_SUFFIX = "HydrantSuffix",
                HYD_ID = "Id",
                HYD_INITIATOR = "InitiatorId",
                HYD_INSPECTION_FREQUENCY = "InspectionFrequency",
                HYD_INSPECTION_FREQUENCY_UNIT = "InspectionFrequencyUnitId",
                HYD_LATERAL_SIZE = "LateralSizeId",
                HYD_LATERAL_VALVE = "LateralValveId",
                HYD_MAIN_SIZE = "HydrantMainSizeId",
                HYD_MAIN_TYPE = "MainTypeId",
                HYD_OUT_OF_SERVICE = "OutOfService",
                HYD_OPERATING_CENTER = "OperatingCenterId",
                HYD_STREET = "StreetId",
                HYD_STREET_NUMBER = "StreetNumber",
                HYD_THREAD = "HydrantThreadTypeId",
                HYD_TOWN_SECTION = "TownSectionId",
                HYD_VALVE_LOCATION = "ValveLocation",
                HYD_WORK_ORDER_NUMBER = "WorkOrderNumber",
                HYDRANT_MODEL_ID = "Id",
                HYDRANT_MODEL_DESCRIPTION = "Description",
                MAN_DESCRIPTION = "Description",
                MAN_ID = "Id",
                RESIDUAL_CHLORINE = "ResidualChlorine",
                DATE_ADDED = "DateAdded",
                DATE_INSPECTED = "DateInspected",
                FULL_FLOW = "FullFlow",
                GALLONS_FLOWED = "GallonsFlowed",
                GPM = "GPM",
                HYDRANT = "HydrantId",
                INSPECTION_TYPE = "InspectionTypeId",
                INSPECTED_BY = "InspectedById",
                MIN_FLOW = "MinutesFlowed",
                STATIC_PRESSURE = "StaticPressure",
                REMARKS = "Remarks",
                WORK_ORDER_REQUEST_1 = "WorkOrderRequest1",
                WORK_ORDER_REQUEST_2 = "WorkOrderRequest2",
                WORK_ORDER_REQUEST_3 = "WorkOrderRequest3",
                WORK_ORDER_REQUEST_4 = "WorkOrderRequest4",
                HYDRANT_PROBLEM = "HydrantProblemId",
                HYDRANT_TAG_STATUS = "HydrantTagStatusId",
                TOTAL_CHLORINE = "TotalChlorine",
                ID = "Id",
                VALVE = "ValveId",
                HYD_WATER_SYSTEM = "WaterSystemId",
                FUNCTIONAL_LOCATION = "FunctionalLocationId";
        }

        public struct StringLengths
        {
            public const int HYD_HYDRANT_STATUS = 10,
                             HYD_BILL_INFO = 15,
                             HYD_BPU_KPI = 2,
                             HYD_BRANCH_LN = 10,
                             HYD_CRITICAL = 2,
                             HYD_CRITICAL_NOTES = 150,
                             HYD_CROSS_STREET = 30,
                             HYD_DEAD_END_MAIN = 3,
                             HYD_DEPTH_BURY = 10,
                             HYD_DIR_OPEN = 7,
                             HYD_ELEVATION = 10,
                             HYD_FIRE_D = 1,
                             HYD_GRADIENT = 25,
                             HYD_HISTORICAL_HYDRANT_LOCATION = 150,
                             HYD_HISTORICAL_VALVE_LOCATION = 150,
                             HYD_HYD_MAKE = 30,
                             HYD_HYD_NUMBER = 12,
                             HYD_HYD_SIZE = 5,
                             HYD_HYD_STYLE = 15,
                             HYD_INITIATOR = 25,
                             HYD_INSPECTION_FREQUENCY = 10,
                             HYD_INSPECTION_FREQUENCY_UNIT = 50,
                             HYD_LAT_SIZE = 10,
                             HYD_LINK_NUM = 10,
                             HYD_LOCATION = 150,
                             HYD_MANUFACTURED_UPDATED_BY = 50,
                             HYD_MAP_PAGE = 15,
                             HYD_OP_CNTR = 4,
                             HYD_OUT_OF_SERVICE = 2,
                             HYD_PREMISE_NUMBER = 10,
                             HYD_SIZEOF_MAIN = 10,
                             HYD_ST_NUM = 10,
                             HYD_THREAD = 15,
                             HYD_TWN_SECTION = 30,
                             HYD_TYPE_MAIN = 15,
                             HYD_VALVE_LOCATION = 150,
                             HYD_WORKORDER = 18,
                             INSPECTED_BY = 25,
                             WORK_ORDER_REQUEST = 25,
                             HYD_INSP_NUM = 55,
                             OPERATING_CENTER = 4,
                             SIZE_OPENING = 20,
                             INSPECTOR_NUM = 25,
                             WATER_SYSTEM = 4,
                             ROUTE = 255;
        }

        #endregion

        public override void Up()
        {
            #region Lookups

            Execute.Sql(@"UPDATE tblNJAWSizeServ set Valve = .5 where SizeServ = '1/2' and isNull(Valve, '') = '';
                            UPDATE tblNJAWSizeServ set Valve = 4.5 where SizeServ = '4 1/2' and isNull(Valve, '') = '';
                            UPDATE tblNJAWSizeServ set Valve = 4.25 where SizeServ = '4 1/4' and isNull(Valve, '') = '';
                            UPDATE tblNJAWSizeServ set Valve = 5.25 where SizeServ = '5 1/4' and isNull(Valve, '') = '';
                            UPDATE tblNJAWSizeServ set Valve = 5.5 where SizeServ = '5 1/2' and isNull(Valve, '') = '';
                            UPDATE tblNJAWSizeServ set Valve = 4.75 where SizeServ = '4 3/4' and isNull(Valve, '') = '';
                            UPDATE tblNJAWSizeServ set Valve = 5 where SizeServ = '5' and isNull(Valve, '') = '';
                            UPDATE tblNJAWSizeServ set Valve = 15 where SizeServ = '15' and isNull(Valve, '') = '';");

            this.CreateLookupTableWithValues(TableNames.WORKORDER_REQUESTS, "BROKEN FLANGE", "BROKEN STEM", "FROZEN",
                "HARD TO OPERATE", "LEAKS", "LOWER TO GRADE", "MISSING", "NO DRIP", "OFFICE REVIEW", "RAISE TO GRADE",
                "REPLACE NOZZLE", "RISER PIPE CROOKED", "RISER PIPE THREADS BAD", "SPUN", "STRAIGHTEN", "VALVE COVERED",
                "STORM - LEANING", "STORM - REQUIRES REPLACEMENT", "STORM-CANNOT FIND", "STORM - TEMPORARILY OOS");
            Alter.Table(TableNames.WORKORDER_REQUESTS).AddColumn("IsActive").AsBoolean().Nullable();
            Execute.Sql("UPDATE " + TableNames.WORKORDER_REQUESTS + " SET IsActive = 1");
            Alter.Column("IsActive").OnTable(TableNames.WORKORDER_REQUESTS).AsBoolean().NotNullable();
            Execute.Sql("INSERT INTO WorkOrderRequests(Description, IsActive) SELECT 'BOX CROOKED/BROKEN', 0;" +
                        "INSERT INTO WorkOrderRequests(Description, IsActive) SELECT 'BOX FILLED', 0;" +
                        "INSERT INTO WorkOrderRequests(Description, IsActive) SELECT 'BOXED FILLED', 0;" +
                        "INSERT INTO WorkOrderRequests(Description, IsActive) SELECT 'CANNOT LOCATE', 0;" +
                        "INSERT INTO WorkOrderRequests(Description, IsActive) SELECT 'COVERED', 0;" +
                        "INSERT INTO WorkOrderRequests(Description, IsActive) SELECT 'HARD TO OPERATE.', 0;" +
                        "INSERT INTO WorkOrderRequests(Description, IsActive) SELECT 'HYDRANT LEAKING', 0;" +
                        "INSERT INTO WorkOrderRequests(Description, IsActive) SELECT 'HYDRANT NEEDS TO BE TURNE', 0;" +
                        "INSERT INTO WorkOrderRequests(Description, IsActive) SELECT 'NUT ROUNDED', 0;" +
                        "INSERT INTO WorkOrderRequests(Description, IsActive) SELECT 'RAISE HYDRANT', 0;" +
                        "INSERT INTO WorkOrderRequests(Description, IsActive) SELECT 'SLOW DRIP', 0;" +
                        "INSERT INTO WorkOrderRequests(Description, IsActive) SELECT 'TURNS HARD', 0;" +
                        "INSERT INTO WorkOrderRequests(Description, IsActive) SELECT 'VALVE BROKEN', 0;" +
                        "INSERT INTO WorkOrderRequests(Description, IsActive) SELECT 'VALVE NOT TO GRADE', 0;");
            this.CreateLookupTableWithValues(TableNames.HYDRANT_INSPECTION_TYPES, "FLUSH", "INSPECT", "INSPECT/FLUSH",
                "WATER QUALITY");
            this.CreateLookupTableWithValues(TableNames.HYDRANT_STATUSES, "ACTIVE", "CANCELLED", "PENDING", "RETIRED");
            this.CreateLookupTableFromQuery(TableNames.HYDRANT_DIRECTIONS,
                "SELECT DISTINCT DirOpen FROM tblNJAWHydrant WHERE ISNULL(DirOpen, '') <> '' ORDER BY 1");
            this.CreateLookupTableFromQuery(TableNames.GRADIENTS,
                "SELECT Distinct Gradiant from tblNJAWHydrant where isNull(Gradiant, '') <> '' order by 1 ");
            this.CreateLookupTableFromQuery(TableNames.HYDRANT_THREAD_TYPES,
                "SELECT Distinct Thread from tblNJAWThreadType");
            this.CreateLookupTableFromQuery(TableNames.MAIN_TYPES,
                "SELECT TypeMain FROM tblNJAWTypeMain ORDER BY TypeMain");
            this.CreateLookupTableFromQuery(TableNames.HYDRANT_BILLING,
                "SELECT DISTINCT BillInfo FROM tblNJAWHydrant WHERE isNull(BillInfo, '') <> '' ");
            this.CreateLookupTableFromQuery(TableNames.WATER_SYSTEMS,
                "SELECT Distinct WaterSystem from tblNJAWHydrantSAP where IsNull(WaterSystem, '') <> '' ORDER BY 1");

            // Lateral Sizes - *RecOrder
            this.CreateLookupTableFromQuery(TableNames.LATERAL_SIZES,
                "SELECT SizeServ As Value FROM tblNJAWSizeServ WHERE [Lat] = 'ON' ORDER BY RecOrd"); // verified
            Alter.Table(TableNames.LATERAL_SIZES)
                 .AddColumn("Size").AsDecimal(5, 3).Nullable()
                 .AddColumn("SortOrder").AsInt32().Nullable();
            Execute.Sql("UPDATE " + TableNames.LATERAL_SIZES +
                        " SET [SortOrder] = (Select Top 1 RecOrd from tblNJAWSizeServ where Description = SizeServ)");
            Execute.Sql("UPDATE " + TableNames.LATERAL_SIZES +
                        " SET [Size] = (Select Top 1 Valve from tblNJAWSizeServ where Description = SizeServ)");
            Alter.Column("Size").OnTable(TableNames.LATERAL_SIZES).AsDecimal(5, 3).NotNullable();
            Alter.Column("SortOrder").OnTable(TableNames.LATERAL_SIZES).AsInt32().NotNullable();

            // Hydrant Sizes - *RecOrder
            this.CreateLookupTableFromQuery(TableNames.HYDRANT_SIZES,
                "select Distinct SizeServ from tblNJAWSizeServ WHERE [Hyd] = 'ON' ORDER BY 1");
            Alter.Table(TableNames.HYDRANT_SIZES)
                 .AddColumn("Size").AsDecimal(5, 3).Nullable()
                 .AddColumn("SortOrder").AsInt32().Nullable();
            Execute.Sql("UPDATE " + TableNames.HYDRANT_SIZES +
                        " SET [SortOrder] = (Select Top 1 RecOrd from tblNJAWSizeServ where Description = SizeServ)");
            Execute.Sql("UPDATE " + TableNames.HYDRANT_SIZES +
                        " SET [Size] = (Select Top 1 Valve from tblNJAWSizeServ where Description = SizeServ)");
            Alter.Column("Size").OnTable(TableNames.HYDRANT_SIZES).AsDecimal(5, 3).NotNullable();
            Alter.Column("SortOrder").OnTable(TableNames.HYDRANT_SIZES).AsInt32().NotNullable();

            //MainSizes - *RecOrder
            this.CreateLookupTableFromQuery(TableNames.HYDRANT_MAIN_SIZES,
                "SELECT Distinct SizeServ FROM tblNJAWSizeServ WHERE [Main] = 'ON'");
            Alter.Table(TableNames.HYDRANT_MAIN_SIZES)
                 .AddColumn("Size").AsDecimal(5, 3).Nullable()
                 .AddColumn("SortOrder").AsInt32().Nullable();
            Execute.Sql("UPDATE " + TableNames.HYDRANT_MAIN_SIZES +
                        " SET [SortOrder] = (Select Top 1 RecOrd from tblNJAWSizeServ where Description = SizeServ)");
            Execute.Sql("UPDATE " + TableNames.HYDRANT_MAIN_SIZES +
                        " SET [Size] = (Select Top 1 Valve from tblNJAWSizeServ where Description = SizeServ)");
            Alter.Column("Size").OnTable(TableNames.HYDRANT_MAIN_SIZES).AsDecimal(5, 3).NotNullable();
            Alter.Column("SortOrder").OnTable(TableNames.HYDRANT_MAIN_SIZES).AsInt32().NotNullable();

            #endregion

            #region BlowOff Inspections

            // DUMP BLOW OFF INSPECTIONS TO 
            Create.Table(TableNames.BLOW_OFF_INSPECTIONS)
                  .WithIdentityColumn()
                  .WithColumn(NewColumnNames.RESIDUAL_CHLORINE).AsDecimal(3, 2).Nullable()
                  .WithColumn(NewColumnNames.DATE_ADDED).AsDateTime().Nullable()
                  .WithColumn(NewColumnNames.DATE_INSPECTED).AsDateTime().NotNullable()
                  .WithColumn(NewColumnNames.FULL_FLOW).AsBoolean().Nullable()
                  .WithColumn(NewColumnNames.GALLONS_FLOWED).AsInt32().Nullable()
                  .WithColumn(NewColumnNames.GPM).AsDecimal(18, 0).Nullable()
                  .WithColumn(NewColumnNames.MIN_FLOW).AsDecimal(18, 2).Nullable()
                  .WithColumn(NewColumnNames.STATIC_PRESSURE).AsDecimal(5, 2).Nullable()
                  .WithColumn(NewColumnNames.REMARKS).AsCustom("text").Nullable()
                  .WithColumn(NewColumnNames.TOTAL_CHLORINE).AsDecimal(3, 2).Nullable();
            Alter.Table(TableNames.BLOW_OFF_INSPECTIONS)
                 .AddForeignKeyColumn(NewColumnNames.HYDRANT_PROBLEM, TableNames.HYDRANT_PROBLEMS, "HydrantProblemID");
            Alter.Table(TableNames.BLOW_OFF_INSPECTIONS)
                 .AddForeignKeyColumn(NewColumnNames.HYDRANT_TAG_STATUS, TableNames.HYDRANT_TAG_STATUSES,
                      "HydrantTagStatusID");
            Alter.Table(TableNames.BLOW_OFF_INSPECTIONS)
                 .AddForeignKeyColumn(NewColumnNames.INSPECTION_TYPE, TableNames.HYDRANT_INSPECTION_TYPES);
            Alter.Table(TableNames.BLOW_OFF_INSPECTIONS)
                 .AddForeignKeyColumn(NewColumnNames.WORK_ORDER_REQUEST_1, TableNames.WORKORDER_REQUESTS);
            Alter.Table(TableNames.BLOW_OFF_INSPECTIONS)
                 .AddForeignKeyColumn(NewColumnNames.WORK_ORDER_REQUEST_2, TableNames.WORKORDER_REQUESTS);
            Alter.Table(TableNames.BLOW_OFF_INSPECTIONS)
                 .AddForeignKeyColumn(NewColumnNames.WORK_ORDER_REQUEST_3, TableNames.WORKORDER_REQUESTS);
            Alter.Table(TableNames.BLOW_OFF_INSPECTIONS)
                 .AddForeignKeyColumn(NewColumnNames.WORK_ORDER_REQUEST_4, TableNames.WORKORDER_REQUESTS);
            Alter.Table(TableNames.BLOW_OFF_INSPECTIONS)
                 .AddForeignKeyColumn(NewColumnNames.VALVE, TableNames.VALVES, "RecID");
            Alter.Table(TableNames.BLOW_OFF_INSPECTIONS)
                 .AddForeignKeyColumn(NewColumnNames.INSPECTED_BY, TableNames.USERS, "RecID");

            Execute.Sql(Sql.UPDATE_BLOWOFF_INSPECTIONS);

            #endregion

            #region Hydrant Inspections

            Execute.Sql(Sql.DROP_INDEXES);

            // RENAME TABLES
            Rename.Table(TableNames.HYDRANT_INSPECTIONS_OLD).To(TableNames.HYDRANT_INSPECTIONS_NEW);

            // RENAME_COLUMNS
            Rename.Column(OldColumnNames.RESIDUAL_CHLORINE).OnTable(TableNames.HYDRANT_INSPECTIONS_NEW)
                  .To(NewColumnNames.RESIDUAL_CHLORINE);
            Rename.Column(OldColumnNames.DATE_INSPECTED).OnTable(TableNames.HYDRANT_INSPECTIONS_NEW)
                  .To(NewColumnNames.DATE_INSPECTED);
            Rename.Column(OldColumnNames.GALLONS_FLOWED).OnTable(TableNames.HYDRANT_INSPECTIONS_NEW)
                  .To(NewColumnNames.GALLONS_FLOWED);
            Rename.Column(OldColumnNames.MIN_FLOW).OnTable(TableNames.HYDRANT_INSPECTIONS_NEW)
                  .To(NewColumnNames.MIN_FLOW);
            Rename.Column(OldColumnNames.STATIC_PRESSURE).OnTable(TableNames.HYDRANT_INSPECTIONS_NEW)
                  .To(NewColumnNames.STATIC_PRESSURE);
            Rename.Column(OldColumnNames.HYDRANT_PROBLEM).OnTable(TableNames.HYDRANT_INSPECTIONS_NEW)
                  .To(NewColumnNames.HYDRANT_PROBLEM);
            Rename.Column(OldColumnNames.HYDRANT_TAG_STATUS).OnTable(TableNames.HYDRANT_INSPECTIONS_NEW)
                  .To(NewColumnNames.HYDRANT_TAG_STATUS);
            Rename.Column(OldColumnNames.ID).OnTable(TableNames.HYDRANT_INSPECTIONS_NEW).To(NewColumnNames.ID);

            //ADD NEW COLUMNS FOR FKs
            Alter.Table(TableNames.HYDRANT_INSPECTIONS_NEW).AddForeignKeyColumn(NewColumnNames.INSPECTION_TYPE,
                TableNames.HYDRANT_INSPECTION_TYPES);
            Alter.Table(TableNames.HYDRANT_INSPECTIONS_NEW).AddForeignKeyColumn(NewColumnNames.WORK_ORDER_REQUEST_1,
                TableNames.WORKORDER_REQUESTS);
            Alter.Table(TableNames.HYDRANT_INSPECTIONS_NEW).AddForeignKeyColumn(NewColumnNames.WORK_ORDER_REQUEST_2,
                TableNames.WORKORDER_REQUESTS);
            Alter.Table(TableNames.HYDRANT_INSPECTIONS_NEW).AddForeignKeyColumn(NewColumnNames.WORK_ORDER_REQUEST_3,
                TableNames.WORKORDER_REQUESTS);
            Alter.Table(TableNames.HYDRANT_INSPECTIONS_NEW).AddForeignKeyColumn(NewColumnNames.WORK_ORDER_REQUEST_4,
                TableNames.WORKORDER_REQUESTS);
            Delete.Column(NewColumnNames.HYDRANT).FromTable(TableNames.HYDRANT_INSPECTIONS_NEW);
            Alter.Table(TableNames.HYDRANT_INSPECTIONS_NEW)
                 .AddForeignKeyColumn(NewColumnNames.HYDRANT, TableNames.HYDRANTS_OLD, "RecId");
            Alter.Table(TableNames.HYDRANT_INSPECTIONS_NEW)
                 .AddForeignKeyColumn(NewColumnNames.INSPECTED_BY, TableNames.USERS, "RecID");

            //UPDATE COLUMNS WITH FK/LOOKUP VALUES
            Execute.Sql(Sql.UPDATE_HYDRANT_INSPECTIONS);

            //ALTER COLUMNS
            Alter.Column(NewColumnNames.DATE_INSPECTED).OnTable(TableNames.HYDRANT_INSPECTIONS_NEW).AsDateTime()
                 .NotNullable();

            //REMOVE OLD FK COLUMNS / UPDATE COLUMNS TO PROPER DATA TYPES
            Delete.Column(OldColumnNames.INSPECTION_TYPE).FromTable(TableNames.HYDRANT_INSPECTIONS_NEW);
            Delete.Column(OldColumnNames.WORK_ORDER_REQUEST_1).FromTable(TableNames.HYDRANT_INSPECTIONS_NEW);
            Delete.Column(OldColumnNames.WORK_ORDER_REQUEST_2).FromTable(TableNames.HYDRANT_INSPECTIONS_NEW);
            Delete.Column(OldColumnNames.WORK_ORDER_REQUEST_3).FromTable(TableNames.HYDRANT_INSPECTIONS_NEW);
            Delete.Column(OldColumnNames.WORK_ORDER_REQUEST_4).FromTable(TableNames.HYDRANT_INSPECTIONS_NEW);
            Delete.Column(OldColumnNames.HYD_INSP_NUM).FromTable(TableNames.HYDRANT_INSPECTIONS_NEW);
            Delete.Column(OldColumnNames.HYD_SUF).FromTable(TableNames.HYDRANT_INSPECTIONS_NEW);
            Delete.Column(OldColumnNames.OPERATING_CENTER).FromTable(TableNames.HYDRANT_INSPECTIONS_NEW);
            Delete.Column(OldColumnNames.SIZE_OPENING).FromTable(TableNames.HYDRANT_INSPECTIONS_NEW);
            Delete.Column(OldColumnNames.INSPECTED_BY).FromTable(TableNames.HYDRANT_INSPECTIONS_NEW);
            Delete.Column(OldColumnNames.INSPECTOR_NUM).FromTable(TableNames.HYDRANT_INSPECTIONS_NEW);

            // ADD AN INDEX FOR PERFORMANCE REASONS
            // These are all suggested indexes from the database tuning advisor thing.
            Create.Index("IDX_HydrantId").OnTable(TableNames.HYDRANT_INSPECTIONS_NEW)
                  .OnColumn(NewColumnNames.HYDRANT);

            Create.Index("IDX_HydrantId_Id").OnTable(TableNames.HYDRANT_INSPECTIONS_NEW)
                  .OnColumn(NewColumnNames.HYDRANT).Ascending()
                  .OnColumn(NewColumnNames.ID).Ascending();

            Create.Index("IDX_HydrantId_DateInspected").OnTable(TableNames.HYDRANT_INSPECTIONS_NEW)
                  .OnColumn(NewColumnNames.HYDRANT).Ascending()
                  .OnColumn(NewColumnNames.DATE_INSPECTED).Descending();

            Create.Index("IDX_HydrantId_Id_WorkOrderRequest1_DateInspected").OnTable(TableNames.HYDRANT_INSPECTIONS_NEW)
                  .OnColumn(NewColumnNames.HYDRANT).Ascending()
                  .OnColumn(NewColumnNames.ID).Ascending()
                  .OnColumn(NewColumnNames.WORK_ORDER_REQUEST_1).Ascending()
                  .OnColumn(NewColumnNames.DATE_INSPECTED).Ascending();

            //REMOVE UNNEEDED COLUMNS
            //Delete.Column(OldColumnNames.SIZE_OPENING).FromTable(TableNames.HYDRANT_INSPECTIONS_NEW);
            //Delete.Column(OldColumnNames.OPERATING_CENTER).FromTable(TableNames.HYDRANT_INSPECTIONS_NEW);
            //Delete.Column(OldColumnNames.HYD_SUF).FromTable(TableNames.HYDRANT_INSPECTIONS_NEW);
            //Delete.Column(OldColumnNames.OPERATING_CENTER).FromTable(TableNames.HYDRANT_INSPECTIONS_NEW);
            //Delete.Column(OldColumnNames.OPERATING_CENTER).FromTable(TableNames.HYDRANT_INSPECTIONS_NEW);
            //Delete.Column(OldColumnNames.OPERATING_CENTER).FromTable(TableNames.HYDRANT_INSPECTIONS_NEW);

            //ALTER DATA

            #endregion

            #region Hydrants

            #region Rename Table/Drop Indexes

            Execute.Sql(Sql.DROP_HYDRANT_INDEXES);
            Rename.Table(TableNames.HYDRANTS_OLD).To(TableNames.HYDRANTS_NEW);
            Rename.Table(TableNames.MANUFACTURERS_OLD).To(TableNames.MANUFACTURERS_NEW);

            #endregion

            #region FK Columns

            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.HYDRANTS_NEW, TableNames.HYDRANT_STATUSES,
                NewColumnNames.HYD_HYDRANT_STATUS, OldColumnNames.HYD_HYDRANT_STATUS);
            Alter.Column(NewColumnNames.HYD_HYDRANT_STATUS).OnTable(TableNames.HYDRANTS_NEW).AsInt32().NotNullable();

            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.HYDRANTS_NEW, TableNames.LATERAL_SIZES,
                NewColumnNames.HYD_LATERAL_SIZE, OldColumnNames.HYD_LATERAL_SIZE);
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.HYDRANTS_NEW, TableNames.STREETS,
                NewColumnNames.HYD_CROSS_STREET, OldColumnNames.HYD_CROSS_STREET, "StreetID", "FullStName",
                "AND " + TableNames.STREETS + ".TownID = " + TableNames.HYDRANTS_NEW + ".Town");
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.HYDRANTS_NEW, TableNames.HYDRANT_DIRECTIONS,
                NewColumnNames.HYD_DIRECTION, OldColumnNames.HYD_DIRECTION);
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.HYDRANTS_NEW, TableNames.GRADIENTS,
                NewColumnNames.HYD_GRADIENT, OldColumnNames.HYD_GRADIENT);
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.HYDRANTS_NEW, TableNames.HYDRANT_SIZES,
                NewColumnNames.HYD_HYDRANT_SIZE, OldColumnNames.HYD_HYDRANT_SIZE);
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.HYDRANTS_NEW, TableNames.USERS,
                NewColumnNames.HYD_INITIATOR, OldColumnNames.HYD_INITIATOR, "RecId", "UserName");
            Execute.Sql("UPDATE " + TableNames.HYDRANTS_NEW + " SET " + OldColumnNames.HYD_INSPECTION_FREQUENCY_UNIT +
                        " = 'Year' WHERE  " + OldColumnNames.HYD_INSPECTION_FREQUENCY_UNIT + " = 'Y'");
            Execute.Sql("UPDATE " + TableNames.HYDRANTS_NEW + " SET " + OldColumnNames.HYD_INSPECTION_FREQUENCY_UNIT +
                        " = 'Month' WHERE  " + OldColumnNames.HYD_INSPECTION_FREQUENCY_UNIT + " = 'M'");
            Execute.Sql("UPDATE " + TableNames.HYDRANTS_NEW + " SET " + OldColumnNames.HYD_INSPECTION_FREQUENCY_UNIT +
                        " = 'Day' WHERE  " + OldColumnNames.HYD_INSPECTION_FREQUENCY_UNIT + " = 'D'");
            Execute.Sql("UPDATE " + TableNames.HYDRANTS_NEW + " SET " + OldColumnNames.HYD_INSPECTION_FREQUENCY_UNIT +
                        " = 'Week' WHERE  " + OldColumnNames.HYD_INSPECTION_FREQUENCY_UNIT + " = 'W'");
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.HYDRANTS_NEW,
                TableNames.INSPECTION_FREQUENCY_UNITS, NewColumnNames.HYD_INSPECTION_FREQUENCY_UNIT,
                OldColumnNames.HYD_INSPECTION_FREQUENCY_UNIT);
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.HYDRANTS_NEW, TableNames.OPERATING_CENTERS,
                NewColumnNames.HYD_OPERATING_CENTER, OldColumnNames.HYD_OPERATING_CENTER, "OperatingCenterID",
                "OperatingCenterCode");
            Alter.Column(NewColumnNames.HYD_OPERATING_CENTER).OnTable(TableNames.HYDRANTS_NEW).AsInt32().NotNullable();
            Alter.Column(OldColumnNames.HYD_TOWN).OnTable(TableNames.HYDRANTS_NEW).AsInt32().NotNullable();

            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.HYDRANTS_NEW, TableNames.HYDRANT_MAIN_SIZES,
                NewColumnNames.HYD_MAIN_SIZE, OldColumnNames.HYD_MAIN_SIZE);
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.HYDRANTS_NEW,
                TableNames.HYDRANT_THREAD_TYPES, NewColumnNames.HYD_THREAD, OldColumnNames.HYD_THREAD);
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.HYDRANTS_NEW, TableNames.MAIN_TYPES,
                NewColumnNames.HYD_MAIN_TYPE, OldColumnNames.HYD_MAIN_TYPE);
            Execute.Sql("UPDATE " + TableNames.HYDRANTS_NEW +
                        " SET TwnSection = null WHERE TwnSection IN ('BIANCO', 'GUNTHER', 'NUGENT','SCHREIBER', 'GRABOWSKI', 'LEWANDOWSKI')");
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.HYDRANTS_NEW, TableNames.TOWN_SECTIONS,
                NewColumnNames.HYD_TOWN_SECTION, OldColumnNames.HYD_TOWN_SECTION, "TownSectionID", "Name",
                " AND TownID = Town");
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.HYDRANTS_NEW, TableNames.HYDRANT_BILLING,
                NewColumnNames.HYD_BILL_INFO, OldColumnNames.HYD_BILL_INFO);

            // This will fail until there are no longer nulls in your local database. These values are updated on the live site so there are no nulls.
            Update.Table(TableNames.HYDRANTS_NEW).Set(new {HydrantBillingId = 1})
                  .Where(new {HydrantBillingId = (object)null});
            Alter.Column(NewColumnNames.HYD_BILL_INFO).OnTable(TableNames.HYDRANTS_NEW).AsInt32().NotNullable();

            Alter.Table(TableNames.HYDRANTS_NEW).AddForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateID");

            Alter.Table(TableNames.HYDRANTS_NEW)
                 .AddForeignKeyColumn(NewColumnNames.HYD_WATER_SYSTEM, TableNames.WATER_SYSTEMS);
            Execute.Sql(
                "UPDATE Hydrants SET WaterSystemId = (SELECT Top 1 Id from WaterSystems where Description = (SELECT WaterSystem from tblNJAWHydrantSAP where tblNJAWHydrantSAP.RecID = Hydrants.RecId))");
            Alter.Table(TableNames.HYDRANTS_NEW).AddForeignKeyColumn(NewColumnNames.FUNCTIONAL_LOCATION,
                TableNames.FUNCTIONAL_LOCATIONS, "FunctionalLocationID");
            Execute.Sql(
                "UPDATE Hydrants SET FunctionalLocationId = (SELECT FunctionalLocationID from tblNJAWHydrantSAP WHERE tblNJAWHydrantSAP.RecID = Hydrants.RecId)");
            Delete.Table(TableNames.HYDRANTS_SAP);

            // This has bad data that needs to be cleaned before the FK can be made, only reason this update isn't part of the other updates.
            Execute.Sql("UPDATE " + TableNames.HYDRANTS_NEW + " SET FireDistrictID = NULL where FireDistrictID = 0;");
            Create.ForeignKey("FK_Hydrants_FireDistrict_FireDistrictID").FromTable("Hydrants")
                  .ForeignColumn("FireDistrictID").ToTable("FireDistrict").PrimaryColumn("FireDistrictID");
            Execute.Sql(Sql.UPDATE_HYDRANT_COORDINATES);
            Delete.Column(OldColumnNames.HYD_LATITUDE).FromTable(TableNames.HYDRANTS_NEW);
            Delete.Column(OldColumnNames.HYD_LONGITUDE).FromTable(TableNames.HYDRANTS_NEW);

            #endregion

            #region Update

            Execute.Sql(Sql.UPDATE_HYDRANTS);

            #endregion

            #region Alter/Rename Columns

            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(OldColumnNames.HYD_BPU_KPI).AsBoolean().NotNullable();
            Rename.Column(OldColumnNames.HYD_BPU_KPI).OnTable(TableNames.HYDRANTS_NEW).To(NewColumnNames.HYD_BPU_KPI);
            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(OldColumnNames.HYD_CRITICAL).AsBoolean().NotNullable();
            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(OldColumnNames.HYD_DEAD_END_MAIN).AsBoolean()
                 .NotNullable();
            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(OldColumnNames.HYD_CLOW_TAGGED).AsBoolean().NotNullable();
            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(OldColumnNames.HYD_HYDRANT_SUFFIX).AsInt32().NotNullable();
            Rename.Column(OldColumnNames.HYD_DEAD_END_MAIN).OnTable(TableNames.HYDRANTS_NEW)
                  .To(NewColumnNames.HYD_DEAD_END_MAIN);
            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(OldColumnNames.HYD_OUT_OF_SERVICE).AsBoolean()
                 .NotNullable();
            Rename.Column(OldColumnNames.HYD_OUT_OF_SERVICE).OnTable(TableNames.HYDRANTS_NEW)
                  .To(NewColumnNames.HYD_OUT_OF_SERVICE);
            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(OldColumnNames.HYD_ELEVATION).AsDecimal(11, 6).Nullable();
            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(OldColumnNames.HYD_INSPECTION_FREQUENCY).AsInt32()
                 .Nullable();
            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(OldColumnNames.HYD_FL_ROUTE_NUMBER).AsInt32().Nullable();
            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(OldColumnNames.HYD_FL_SEQUENCE).AsInt32().Nullable();
            Rename.Column(OldColumnNames.HYD_DATE_RETIRED).OnTable(TableNames.HYDRANTS_NEW)
                  .To(NewColumnNames.HYD_DATE_RETIRED);

            Rename.Column(OldColumnNames.HYD_HYDRANT_SUFFIX).OnTable(TableNames.HYDRANTS_NEW)
                  .To(NewColumnNames.HYD_HYDRANT_SUFFIX);
            Rename.Column(OldColumnNames.HYD_INSPECTION_FREQUENCY).OnTable(TableNames.HYDRANTS_NEW)
                  .To(NewColumnNames.HYD_INSPECTION_FREQUENCY);
            Rename.Column(OldColumnNames.HYD_ID).OnTable(TableNames.HYDRANTS_NEW).To(NewColumnNames.HYD_ID);
            Rename.Column(OldColumnNames.HYD_STREET).OnTable(TableNames.HYDRANTS_NEW).To(NewColumnNames.HYD_STREET);
            Rename.Column(OldColumnNames.HYD_STREET_NUMBER).OnTable(TableNames.HYDRANTS_NEW)
                  .To(NewColumnNames.HYD_STREET_NUMBER);
            Rename.Column(OldColumnNames.HYD_HYDRANT_NUMBER).OnTable(TableNames.HYDRANTS_NEW)
                  .To(NewColumnNames.HYD_HYDRANT_NUMBER);
            Rename.Column(OldColumnNames.HYD_VALVE_LOCATION).OnTable(TableNames.HYDRANTS_NEW)
                  .To(NewColumnNames.HYD_VALVE_LOCATION);
            Rename.Column(OldColumnNames.HYD_WORK_ORDER_NUMBER).OnTable(TableNames.HYDRANTS_NEW)
                  .To(NewColumnNames.HYD_WORK_ORDER_NUMBER);
            Rename.Column(OldColumnNames.HYD_BRANCH_LENGTH_FEET).OnTable(TableNames.HYDRANTS_NEW)
                  .To(NewColumnNames.HYD_BRANCH_LENGTH_FEET);
            Rename.Column(OldColumnNames.HYD_BRANCH_LENGTH_INCHES).OnTable(TableNames.HYDRANTS_NEW)
                  .To(NewColumnNames.HYD_BRANCH_LENGTH_INCHES);
            Rename.Column(OldColumnNames.HYD_DEPTH_BURY_FEET).OnTable(TableNames.HYDRANTS_NEW)
                  .To(NewColumnNames.HYD_DEPTH_BURY_FEET);
            Rename.Column(OldColumnNames.HYD_DEPTH_BURY_INCHES).OnTable(TableNames.HYDRANTS_NEW)
                  .To(NewColumnNames.HYD_DEPTH_BURY_INCHES);
            Rename.Column(OldColumnNames.HYD_LATERAL_VALVE).OnTable(TableNames.HYDRANTS_NEW)
                  .To(NewColumnNames.HYD_LATERAL_VALVE);

            Rename.Column(OldColumnNames.HYD_DATE_INSTALLED).OnTable(TableNames.HYDRANTS_NEW)
                  .To(NewColumnNames.HYD_DATE_INSTALLED);
            Rename.Column(OldColumnNames.MAN_ID).OnTable(TableNames.MANUFACTURERS_NEW).To(NewColumnNames.MAN_ID);
            Rename.Column(OldColumnNames.MAN_DESCRIPTION).OnTable(TableNames.MANUFACTURERS_NEW)
                  .To(NewColumnNames.MAN_DESCRIPTION);
            Rename.Column(OldColumnNames.HYDRANT_MODEL_ID).OnTable(TableNames.HYDRANT_MODELS)
                  .To(NewColumnNames.HYDRANT_MODEL_ID);
            Rename.Column(OldColumnNames.HYDRANT_MODEL_DESCRIPTION).OnTable(TableNames.HYDRANT_MODELS)
                  .To(NewColumnNames.HYDRANT_MODEL_DESCRIPTION);

            // These were smalldatetimes and should be datetime instead. This isn't rolled back.

            Alter.Column(NewColumnNames.HYD_DATE_INSTALLED).OnTable(TableNames.HYDRANTS_NEW).AsDateTime().Nullable();
            Alter.Column(NewColumnNames.HYD_DATE_RETIRED).OnTable(TableNames.HYDRANTS_NEW).AsDateTime().Nullable();
            Alter.Column(OldColumnNames.HYD_DATE_TESTED).OnTable(TableNames.HYDRANTS_NEW).AsDateTime().Nullable();

            #endregion

            #region Remove Old Columns

            Delete.Column(OldColumnNames.HYD_BRANCH_LN).FromTable(TableNames.HYDRANTS_NEW);
            Delete.Column(OldColumnNames.HYD_DEPTH_BURY).FromTable(TableNames.HYDRANTS_NEW);
            Delete.Column(OldColumnNames.HYD_FIRE_D).FromTable(TableNames.HYDRANTS_NEW);
            Delete.Column(OldColumnNames.HYD_HYD_MAKE).FromTable(TableNames.HYDRANTS_NEW);
            Delete.Column(OldColumnNames.HYD_HYD_STYLE).FromTable(TableNames.HYDRANTS_NEW);
            Delete.Column(OldColumnNames.HYD_LINK_NUM).FromTable(TableNames.HYDRANTS_NEW);
            Delete.Column(OldColumnNames.HYD_MANUFACTURER_UPDATED).FromTable(TableNames.HYDRANTS_NEW);
            Delete.Column(OldColumnNames.HYD_MANUFACTURER_UPDATED_BY).FromTable(TableNames.HYDRANTS_NEW);
            Delete.Column(OldColumnNames.HYD_HISTORICAL_HYDRANT_LOCATION).FromTable(TableNames.HYDRANTS_NEW);
            Delete.Column(OldColumnNames.HYD_HISTORICAL_VALVE_LOCATION).FromTable(TableNames.HYDRANTS_NEW);
            Delete.Column(OldColumnNames.HYD_HAS_NO_LATERAL_VALVE).FromTable(TableNames.HYDRANTS_NEW);

            #endregion

            #endregion

            Execute.Sql(@"
                declare @dataTypeId int
                insert into [DataType] (Data_Type, Table_Name) values('Hydrants', 'Hydrants')
                set @dataTypeId = (select @@IDENTITY)
                insert into [DocumentType] (Document_Type, DataTypeID) values('Document', @dataTypeId)
                insert into [DocumentType] (Document_Type, DataTypeID) values('Photo', @dataTypeId)");
        }

        public override void Down()
        {
            Execute.Sql(
                "DELETE FROM DocumentLink WHERE DataTypeID = (SELECT DataTypeId FROM [DataType] WHERE Table_Name = 'Hydrants')");
            Execute.Sql(
                "DELETE FROM Document WHERE DocumentTypeId IN (SELECT DocumentTypeID FROM DocumentType WHERE DataTypeID = (SELECT DataTypeID FROM DataType WHERE Data_Type = 'Hydrants'))");

            Execute.Sql(@"
                delete from [DocumentType] where DataTypeID IN (select DataTypeId from [DataType] where Table_Name = 'Hydrants')
                delete from [DataType] where DataTypeId IN (select DataTypeId from [DataType] where Table_Name = 'Hydrants')");

            #region Hydrants

            // delete an index
            Delete.Index("IDX_HydrantId").OnTable(TableNames.HYDRANT_INSPECTIONS_NEW);
            Delete.Index("IDX_HydrantId_Id").OnTable(TableNames.HYDRANT_INSPECTIONS_NEW);
            Delete.Index("IDX_HydrantId_Id_WorkOrderRequest1_DateInspected")
                  .OnTable(TableNames.HYDRANT_INSPECTIONS_NEW);
            Delete.Index("IDX_HydrantId_DateInspected").OnTable(TableNames.HYDRANT_INSPECTIONS_NEW);

            #region Add Old Columns back

            Alter.Table(TableNames.HYDRANTS_NEW)
                 .AddColumn(OldColumnNames.HYD_BRANCH_LN).AsAnsiString(StringLengths.HYD_BRANCH_LN).Nullable()
                 .AddColumn(OldColumnNames.HYD_DEPTH_BURY).AsAnsiString(StringLengths.HYD_DEPTH_BURY).Nullable()
                 .AddColumn(OldColumnNames.HYD_FIRE_D).AsAnsiString(StringLengths.HYD_FIRE_D).Nullable()
                 .AddColumn(OldColumnNames.HYD_HYD_MAKE).AsAnsiString(StringLengths.HYD_HYD_MAKE).Nullable()
                 .AddColumn(OldColumnNames.HYD_HYD_STYLE).AsAnsiString(StringLengths.HYD_HYD_STYLE).Nullable()
                 .AddColumn(OldColumnNames.HYD_LINK_NUM).AsAnsiString(StringLengths.HYD_LINK_NUM).Nullable()
                 .AddColumn(OldColumnNames.HYD_MANUFACTURER_UPDATED).AsDateTime().Nullable()
                 .AddColumn(OldColumnNames.HYD_MANUFACTURER_UPDATED_BY)
                 .AsAnsiString(StringLengths.HYD_MANUFACTURED_UPDATED_BY).Nullable()
                 .AddColumn(OldColumnNames.HYD_HISTORICAL_HYDRANT_LOCATION)
                 .AsAnsiString(StringLengths.HYD_HISTORICAL_HYDRANT_LOCATION).Nullable()
                 .AddColumn(OldColumnNames.HYD_HISTORICAL_VALVE_LOCATION)
                 .AsAnsiString(StringLengths.HYD_HISTORICAL_VALVE_LOCATION).Nullable()
                 .AddColumn(OldColumnNames.HYD_HAS_NO_LATERAL_VALVE).AsBoolean().Nullable();

            #endregion

            #region Alter/Rename Columns

            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(NewColumnNames.HYD_INSPECTION_FREQUENCY)
                 .AsAnsiString(StringLengths.HYD_INSPECTION_FREQUENCY).Nullable();
            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(NewColumnNames.HYD_HYDRANT_SUFFIX).AsFloat().Nullable();
            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(NewColumnNames.HYD_BPU_KPI)
                 .AsAnsiString(StringLengths.HYD_BPU_KPI).Nullable();
            Rename.Column(NewColumnNames.HYD_BPU_KPI).OnTable(TableNames.HYDRANTS_NEW).To(OldColumnNames.HYD_BPU_KPI);
            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(OldColumnNames.HYD_CLOW_TAGGED).AsBoolean().Nullable();
            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(OldColumnNames.HYD_CRITICAL)
                 .AsAnsiString(StringLengths.HYD_CRITICAL).Nullable();
            Rename.Column(NewColumnNames.HYD_DEAD_END_MAIN).OnTable(TableNames.HYDRANTS_NEW)
                  .To(OldColumnNames.HYD_DEAD_END_MAIN);
            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(OldColumnNames.HYD_DEAD_END_MAIN)
                 .AsAnsiString(StringLengths.HYD_DEAD_END_MAIN).Nullable();
            Rename.Column(NewColumnNames.HYD_OUT_OF_SERVICE).OnTable(TableNames.HYDRANTS_NEW)
                  .To(OldColumnNames.HYD_OUT_OF_SERVICE);
            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(OldColumnNames.HYD_OUT_OF_SERVICE)
                 .AsAnsiString(StringLengths.HYD_OUT_OF_SERVICE).Nullable();
            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(OldColumnNames.HYD_ELEVATION)
                 .AsAnsiString(StringLengths.HYD_ELEVATION).Nullable();
            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(OldColumnNames.HYD_FL_ROUTE_NUMBER).AsFloat().Nullable();
            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(OldColumnNames.HYD_FL_SEQUENCE).AsFloat().Nullable();
            Rename.Column(NewColumnNames.HYD_DATE_RETIRED).OnTable(TableNames.HYDRANTS_NEW)
                  .To(OldColumnNames.HYD_DATE_RETIRED);

            Rename.Column(NewColumnNames.HYD_HYDRANT_SUFFIX).OnTable(TableNames.HYDRANTS_NEW)
                  .To(OldColumnNames.HYD_HYDRANT_SUFFIX);

            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(OldColumnNames.HYD_HYDRANT_SUFFIX).AsFloat().Nullable();
            Rename.Column(NewColumnNames.HYD_INSPECTION_FREQUENCY).OnTable(TableNames.HYDRANTS_NEW)
                  .To(OldColumnNames.HYD_INSPECTION_FREQUENCY);
            Alter.Table(TableNames.HYDRANTS_NEW).AlterColumn(OldColumnNames.HYD_INSPECTION_FREQUENCY)
                 .AsAnsiString(StringLengths.HYD_INSPECTION_FREQUENCY).Nullable();
            Rename.Column(NewColumnNames.HYD_ID).OnTable(TableNames.HYDRANTS_NEW).To(OldColumnNames.HYD_ID);
            Rename.Column(NewColumnNames.HYD_STREET).OnTable(TableNames.HYDRANTS_NEW).To(OldColumnNames.HYD_STREET);
            Rename.Column(NewColumnNames.HYD_STREET_NUMBER).OnTable(TableNames.HYDRANTS_NEW)
                  .To(OldColumnNames.HYD_STREET_NUMBER);

            Rename.Column(NewColumnNames.HYD_HYDRANT_NUMBER).OnTable(TableNames.HYDRANTS_NEW)
                  .To(OldColumnNames.HYD_HYDRANT_NUMBER);
            Rename.Column(NewColumnNames.HYD_VALVE_LOCATION).OnTable(TableNames.HYDRANTS_NEW)
                  .To(OldColumnNames.HYD_VALVE_LOCATION);
            Rename.Column(NewColumnNames.HYD_WORK_ORDER_NUMBER).OnTable(TableNames.HYDRANTS_NEW)
                  .To(OldColumnNames.HYD_WORK_ORDER_NUMBER);
            Rename.Column(NewColumnNames.HYD_BRANCH_LENGTH_FEET).OnTable(TableNames.HYDRANTS_NEW)
                  .To(OldColumnNames.HYD_BRANCH_LENGTH_FEET);
            Rename.Column(NewColumnNames.HYD_BRANCH_LENGTH_INCHES).OnTable(TableNames.HYDRANTS_NEW)
                  .To(OldColumnNames.HYD_BRANCH_LENGTH_INCHES);
            Rename.Column(NewColumnNames.HYD_DEPTH_BURY_FEET).OnTable(TableNames.HYDRANTS_NEW)
                  .To(OldColumnNames.HYD_DEPTH_BURY_FEET);
            Rename.Column(NewColumnNames.HYD_DEPTH_BURY_INCHES).OnTable(TableNames.HYDRANTS_NEW)
                  .To(OldColumnNames.HYD_DEPTH_BURY_INCHES);
            Rename.Column(NewColumnNames.HYD_LATERAL_VALVE).OnTable(TableNames.HYDRANTS_NEW)
                  .To(OldColumnNames.HYD_LATERAL_VALVE);
            Rename.Column(NewColumnNames.HYD_DATE_INSTALLED).OnTable(TableNames.HYDRANTS_NEW)
                  .To(OldColumnNames.HYD_DATE_INSTALLED);

            Rename.Column(NewColumnNames.MAN_ID).OnTable(TableNames.MANUFACTURERS_NEW).To(OldColumnNames.MAN_ID);
            Rename.Column(NewColumnNames.MAN_DESCRIPTION).OnTable(TableNames.MANUFACTURERS_NEW)
                  .To(OldColumnNames.MAN_DESCRIPTION);
            Rename.Column(NewColumnNames.HYDRANT_MODEL_ID).OnTable(TableNames.HYDRANT_MODELS)
                  .To(OldColumnNames.HYDRANT_MODEL_ID);
            Rename.Column(NewColumnNames.HYDRANT_MODEL_DESCRIPTION).OnTable(TableNames.HYDRANT_MODELS)
                  .To(OldColumnNames.HYDRANT_MODEL_DESCRIPTION);

            #endregion

            #region Rollback

            Execute.Sql(Sql.ROLLBACK_HYDRANTS);

            #endregion

            #region FK Columns

            this.RemoveLookupAndAdjustColumns(TableNames.HYDRANTS_NEW, TableNames.STREETS,
                NewColumnNames.HYD_CROSS_STREET, OldColumnNames.HYD_CROSS_STREET, StringLengths.HYD_CROSS_STREET,
                "StreetID", "FullStName");
            this.RemoveLookupAndAdjustColumns(TableNames.HYDRANTS_NEW, TableNames.LATERAL_SIZES,
                NewColumnNames.HYD_LATERAL_SIZE, OldColumnNames.HYD_LATERAL_SIZE, StringLengths.HYD_LAT_SIZE);
            this.RemoveLookupAndAdjustColumns(TableNames.HYDRANTS_NEW, TableNames.HYDRANT_STATUSES,
                NewColumnNames.HYD_HYDRANT_STATUS, OldColumnNames.HYD_HYDRANT_STATUS, StringLengths.HYD_HYDRANT_STATUS);
            this.RemoveLookupAndAdjustColumns(TableNames.HYDRANTS_NEW, TableNames.HYDRANT_DIRECTIONS,
                NewColumnNames.HYD_DIRECTION, OldColumnNames.HYD_DIRECTION, StringLengths.HYD_DIR_OPEN);
            this.RemoveLookupAndAdjustColumns(TableNames.HYDRANTS_NEW, TableNames.GRADIENTS,
                NewColumnNames.HYD_GRADIENT, OldColumnNames.HYD_GRADIENT, StringLengths.HYD_GRADIENT);
            this.RemoveLookupAndAdjustColumns(TableNames.HYDRANTS_NEW, TableNames.HYDRANT_SIZES,
                NewColumnNames.HYD_HYDRANT_SIZE, OldColumnNames.HYD_HYDRANT_SIZE, StringLengths.HYD_HYD_SIZE);
            this.RemoveLookupAndAdjustColumns(TableNames.HYDRANTS_NEW, TableNames.USERS, NewColumnNames.HYD_INITIATOR,
                OldColumnNames.HYD_INITIATOR, StringLengths.HYD_INITIATOR, "RecId", "UserName");
            this.RemoveLookupAndAdjustColumns(TableNames.HYDRANTS_NEW, TableNames.INSPECTION_FREQUENCY_UNITS,
                NewColumnNames.HYD_INSPECTION_FREQUENCY_UNIT, OldColumnNames.HYD_INSPECTION_FREQUENCY_UNIT,
                StringLengths.HYD_INSPECTION_FREQUENCY_UNIT);
            Execute.Sql("UPDATE " + TableNames.HYDRANTS_NEW + " SET " + OldColumnNames.HYD_INSPECTION_FREQUENCY_UNIT +
                        " = LEFT(" + OldColumnNames.HYD_INSPECTION_FREQUENCY_UNIT + ", 1)");
            this.RemoveLookupAndAdjustColumns(TableNames.HYDRANTS_NEW, TableNames.OPERATING_CENTERS,
                NewColumnNames.HYD_OPERATING_CENTER, OldColumnNames.HYD_OPERATING_CENTER, StringLengths.HYD_OP_CNTR,
                "OperatingCenterID", "OperatingCenterCode");
            this.RemoveLookupAndAdjustColumns(TableNames.HYDRANTS_NEW, TableNames.HYDRANT_MAIN_SIZES,
                NewColumnNames.HYD_MAIN_SIZE, OldColumnNames.HYD_MAIN_SIZE, StringLengths.HYD_SIZEOF_MAIN);
            this.RemoveLookupAndAdjustColumns(TableNames.HYDRANTS_NEW, TableNames.HYDRANT_THREAD_TYPES,
                NewColumnNames.HYD_THREAD, OldColumnNames.HYD_THREAD, StringLengths.HYD_THREAD);
            this.RemoveLookupAndAdjustColumns(TableNames.HYDRANTS_NEW, TableNames.MAIN_TYPES,
                NewColumnNames.HYD_MAIN_TYPE, OldColumnNames.HYD_MAIN_TYPE, StringLengths.HYD_TYPE_MAIN);
            this.RemoveLookupAndAdjustColumns(TableNames.HYDRANTS_NEW, TableNames.TOWN_SECTIONS,
                NewColumnNames.HYD_TOWN_SECTION, OldColumnNames.HYD_TOWN_SECTION, StringLengths.HYD_TWN_SECTION,
                "TownSectionID", "Name");
            this.RemoveLookupAndAdjustColumns(TableNames.HYDRANTS_NEW, TableNames.HYDRANT_BILLING,
                NewColumnNames.HYD_BILL_INFO, OldColumnNames.HYD_BILL_INFO, StringLengths.HYD_BILL_INFO);
            Alter.Table(TableNames.HYDRANTS_NEW).AddColumn(OldColumnNames.HYD_LATITUDE).AsFloat().Nullable();
            Alter.Table(TableNames.HYDRANTS_NEW).AddColumn(OldColumnNames.HYD_LONGITUDE).AsFloat().Nullable();
            Execute.Sql(Sql.ROLLBACK_HYDRANT_COORDINATES);
            Delete.ForeignKeyColumn(TableNames.HYDRANTS_NEW, "CoordinateId", "Coordinates");
            Create.Table(TableNames.HYDRANTS_SAP)
                  .WithColumn("RecID").AsInt32().NotNullable()
                  .WithColumn("Route").AsAnsiString(StringLengths.ROUTE).Nullable()
                  .WithColumn("Sequence").AsFloat().Nullable()
                  .WithColumn("WaterSystem").AsAnsiString(StringLengths.WATER_SYSTEM).Nullable();
            Alter.Table(TableNames.HYDRANTS_SAP).AddForeignKeyColumn(OldColumnNames.FUNCTIONAL_LOCATION,
                TableNames.FUNCTIONAL_LOCATIONS, "FunctionalLocationID");
            Execute.Sql("INSERT INTO " + TableNames.HYDRANTS_SAP +
                        "(RecID, WaterSystem, FunctionalLocationID) SELECT RecID, ws.Description as WaterSystem, FunctionalLocationID from Hydrants H LEFT JOIN WaterSystems ws on ws.Id = h.watersystemID");
            Delete.ForeignKeyColumn(TableNames.HYDRANTS_NEW, "WaterSystemId", TableNames.WATER_SYSTEMS);
            Delete.ForeignKeyColumn(TableNames.HYDRANTS_NEW, "FunctionalLocationId", TableNames.FUNCTIONAL_LOCATIONS);
            Delete.ForeignKey("FK_Hydrants_FireDistrict_FireDistrictID").OnTable(TableNames.HYDRANTS_NEW);

            #endregion

            #region Rename / Add Indexes Back

            Rename.Table(TableNames.MANUFACTURERS_NEW).To(TableNames.MANUFACTURERS_OLD);
            Rename.Table(TableNames.HYDRANTS_NEW).To(TableNames.HYDRANTS_OLD);
            Execute.Sql(Sql.RESTORE_HYDRANT_INDEXES);

            #endregion

            #endregion

            #region Hydrant Inspections

            // RENAME COLUMNS
            Rename.Column(NewColumnNames.RESIDUAL_CHLORINE).OnTable(TableNames.HYDRANT_INSPECTIONS_NEW)
                  .To(OldColumnNames.RESIDUAL_CHLORINE);
            Rename.Column(NewColumnNames.DATE_INSPECTED).OnTable(TableNames.HYDRANT_INSPECTIONS_NEW)
                  .To(OldColumnNames.DATE_INSPECTED);
            Alter.Column(OldColumnNames.DATE_INSPECTED).OnTable(TableNames.HYDRANT_INSPECTIONS_NEW).AsDateTime()
                 .Nullable();
            Rename.Column(NewColumnNames.GALLONS_FLOWED).OnTable(TableNames.HYDRANT_INSPECTIONS_NEW)
                  .To(OldColumnNames.GALLONS_FLOWED);
            Rename.Column(NewColumnNames.MIN_FLOW).OnTable(TableNames.HYDRANT_INSPECTIONS_NEW)
                  .To(OldColumnNames.MIN_FLOW);
            Rename.Column(NewColumnNames.STATIC_PRESSURE).OnTable(TableNames.HYDRANT_INSPECTIONS_NEW)
                  .To(OldColumnNames.STATIC_PRESSURE);
            Rename.Column(NewColumnNames.HYDRANT_PROBLEM).OnTable(TableNames.HYDRANT_INSPECTIONS_NEW)
                  .To(OldColumnNames.HYDRANT_PROBLEM);
            Rename.Column(NewColumnNames.HYDRANT_TAG_STATUS).OnTable(TableNames.HYDRANT_INSPECTIONS_NEW)
                  .To(OldColumnNames.HYDRANT_TAG_STATUS);
            Rename.Column(NewColumnNames.ID).OnTable(TableNames.HYDRANT_INSPECTIONS_NEW).To(OldColumnNames.ID);

            // ADD BACK OLD FK TEXT COLUMNS
            Alter.Table(TableNames.HYDRANT_INSPECTIONS_NEW)
                 .AddColumn(OldColumnNames.INSPECTION_TYPE)
                 .AsAnsiString(StringLengths.INSPECTED_BY).Nullable();
            Alter.Table(TableNames.HYDRANT_INSPECTIONS_NEW)
                 .AddColumn(OldColumnNames.WORK_ORDER_REQUEST_1)
                 .AsAnsiString(StringLengths.WORK_ORDER_REQUEST).Nullable();
            Alter.Table(TableNames.HYDRANT_INSPECTIONS_NEW)
                 .AddColumn(OldColumnNames.WORK_ORDER_REQUEST_2)
                 .AsAnsiString(StringLengths.WORK_ORDER_REQUEST).Nullable();
            Alter.Table(TableNames.HYDRANT_INSPECTIONS_NEW)
                 .AddColumn(OldColumnNames.WORK_ORDER_REQUEST_3)
                 .AsAnsiString(StringLengths.WORK_ORDER_REQUEST).Nullable();
            Alter.Table(TableNames.HYDRANT_INSPECTIONS_NEW)
                 .AddColumn(OldColumnNames.WORK_ORDER_REQUEST_4)
                 .AsAnsiString(StringLengths.WORK_ORDER_REQUEST).Nullable();
            Alter.Table(TableNames.HYDRANT_INSPECTIONS_NEW)
                 .AddColumn(OldColumnNames.HYD_INSP_NUM)
                 .AsAnsiString(StringLengths.HYD_INSP_NUM).Nullable();
            Alter.Table(TableNames.HYDRANT_INSPECTIONS_NEW)
                 .AddColumn(OldColumnNames.HYD_SUF)
                 .AsFloat().Nullable();
            Alter.Table(TableNames.HYDRANT_INSPECTIONS_NEW)
                 .AddColumn(OldColumnNames.OPERATING_CENTER)
                 .AsAnsiString(StringLengths.OPERATING_CENTER).Nullable();
            Alter.Table(TableNames.HYDRANT_INSPECTIONS_NEW)
                 .AddColumn(OldColumnNames.SIZE_OPENING)
                 .AsAnsiString(StringLengths.SIZE_OPENING).Nullable();
            Alter.Table(TableNames.HYDRANT_INSPECTIONS_NEW)
                 .AddColumn(OldColumnNames.INSPECTED_BY)
                 .AsAnsiString(StringLengths.INSPECTED_BY).Nullable();
            Alter.Table(TableNames.HYDRANT_INSPECTIONS_NEW)
                 .AddColumn(OldColumnNames.INSPECTOR_NUM)
                 .AsAnsiString(StringLengths.INSPECTOR_NUM).Nullable();

            // ROLLBACK DATA            
            Execute.Sql(Sql.ROLLBACK_HYDRANT_INSPECTIONS);

            // REMOVE NEW FK COLUMNS
            Delete.ForeignKeyColumn(TableNames.HYDRANT_INSPECTIONS_NEW, NewColumnNames.INSPECTION_TYPE,
                TableNames.HYDRANT_INSPECTION_TYPES);
            Delete.ForeignKeyColumn(TableNames.HYDRANT_INSPECTIONS_NEW, NewColumnNames.WORK_ORDER_REQUEST_1,
                TableNames.WORKORDER_REQUESTS);
            Delete.ForeignKeyColumn(TableNames.HYDRANT_INSPECTIONS_NEW, NewColumnNames.WORK_ORDER_REQUEST_2,
                TableNames.WORKORDER_REQUESTS);
            Delete.ForeignKeyColumn(TableNames.HYDRANT_INSPECTIONS_NEW, NewColumnNames.WORK_ORDER_REQUEST_3,
                TableNames.WORKORDER_REQUESTS);
            Delete.ForeignKeyColumn(TableNames.HYDRANT_INSPECTIONS_NEW, NewColumnNames.WORK_ORDER_REQUEST_4,
                TableNames.WORKORDER_REQUESTS);
            Delete.ForeignKeyColumn(TableNames.HYDRANT_INSPECTIONS_NEW, NewColumnNames.HYDRANT,
                TableNames.HYDRANTS_OLD);
            Delete.ForeignKeyColumn(TableNames.HYDRANT_INSPECTIONS_NEW, NewColumnNames.INSPECTED_BY, TableNames.USERS);

            Alter.Table(TableNames.HYDRANT_INSPECTIONS_NEW).AddColumn("HydrantId").AsInt32().Nullable();
            Execute.Sql("update " + TableNames.HYDRANT_INSPECTIONS_NEW +
                        " set HydrantId = (select Top 1 RecID from tblNJAWHydrant H where H.HydNum = hid.HydNum and H.OpCntr = hid.OpCntr) from " +
                        TableNames.HYDRANT_INSPECTIONS_NEW + " hid");

            // RENAME TABLES
            Rename.Table(TableNames.HYDRANT_INSPECTIONS_NEW).To(TableNames.HYDRANT_INSPECTIONS_OLD);

            Execute.Sql(Sql.RESTORE_INDEXES);

            #endregion

            #region BlowOff Inspections

            Execute.Sql(Sql.ROLLBACK_BLOWOFF_INSPECTIONS);
            Delete.Table(TableNames.BLOW_OFF_INSPECTIONS);

            #endregion

            #region Lookups

            Delete.Table(TableNames.HYDRANT_BILLING);
            Delete.Table(TableNames.MAIN_TYPES);
            Delete.Table(TableNames.HYDRANT_THREAD_TYPES);
            Delete.Table(TableNames.HYDRANT_MAIN_SIZES);
            Delete.Table(TableNames.GRADIENTS);
            Delete.Table(TableNames.HYDRANT_SIZES);
            Delete.Table(TableNames.WORKORDER_REQUESTS);
            Delete.Table(TableNames.HYDRANT_INSPECTION_TYPES);
            Delete.Table(TableNames.HYDRANT_STATUSES);
            Delete.Table(TableNames.LATERAL_SIZES);
            Delete.Table(TableNames.HYDRANT_DIRECTIONS);
            Delete.Table(TableNames.WATER_SYSTEMS);

            #endregion

            Execute.Sql(UpdateValvesForBug2224.SQL_RESTORE_VALVE_SPS_AND_VIEWS);
        }
    }
}
