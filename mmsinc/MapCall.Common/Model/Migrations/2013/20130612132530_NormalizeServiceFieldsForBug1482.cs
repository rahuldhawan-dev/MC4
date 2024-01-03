using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130612132530), Tags("Production")]
    public class NormalizeServiceFieldsForBug1482 : Migration
    {
        /*
         * TODO: Remove
         * spGetPurposesForSelects
         * spGetMainTypeForSelects
        */

        #region Constants

        public struct SQL
        {
            public const string
                UPDATE_SERVICE_MATERIALS =
                    "INSERT INTO [ServiceMaterials](Description) " +
                    "  SELECT DISTINCT ServMatl FROM tblNJAWServMatl ORDER BY 1;" +
                    "UPDATE [tblNJAWServMatl] SET " +
                    "  ServMatl = (Select ServiceMaterialID from ServiceMaterials where [Description] = servMatl)," +
                    "  OpCntr = (Select OperatingCenterID from OperatingCenters where OpCntr = OperatingCenterCode);" +
                    "UPDATE [tblNJAWService] SET " +
                    "  ServMatl = (Select ServiceMaterialID from ServiceMaterials where [Description] = servMatl)," +
                    "  PrevServiceMatl = (Select ServiceMaterialID from ServiceMaterials where [Description] = prevServiceMatl);",
                ROLLBACK_SERVICE_MATERIALS =
                    "UPDATE [tblNJAWService] SET" +
                    "  ServMatl = (Select [Description] from ServiceMaterials where ServiceMaterialID = ServMatl)," +
                    "  PrevServiceMatl = (Select [Description] from ServiceMaterials where ServiceMaterialID = prevServiceMatl);" +
                    "UPDATE [tblNJAWServMatl] SET " +
                    "  ServMatl = (Select [Description] from ServiceMaterials where ServiceMaterialID = servMatl)," +
                    "  OpCntr = (Select OperatingCenterCode from OperatingCenters where OpCntr = OperatingCenterID);",
                UPDATE_SERVICE_SIZES =
                    "UPDATE tblNJAWService SET " +
                    "  SizeofService = (Select Top 1 RecID from tblNJAWSizeServ SS WHERE SS.SizeServ = SizeofService)," +
                    "  PrevServiceSize = (Select Top 1 RecID from tblNJAWSizeServ SS where SS.SizeServ = PrevServiceSize)",
                ROLLBACK_SERVICE_SIZES =
                    "UPDATE tblNJAWService SET " +
                    "  SizeofService = (Select Top 1 SizeServ from tblNJAWSizeServ SS WHERE SS.RecID = SizeofService)," +
                    "  PrevServiceSize = (Select Top 1 SizeServ from tblNJAWSizeServ SS where SS.RecID = PrevServiceSize)",
                UPDATE_SERVICE_MAIN_SIZE =
                    "UPDATE tblNJAWService SET SizeOfMain = SS.RecID FROM tblNJAWService SE LEFT JOIN tblNJAWSizeServ SS ON SS.SizeServ = SE.SizeofMain",
                ROLLBACK_SERVICE_MAIN_SIZE =
                    "UPDATE tblNJAWService SET SizeOfMain = SS.SizeServ FROM tblNJAWService SE LEFT JOIN tblNJAWSizeServ SS ON SS.RecID = SE.SizeofMain",
                UPDATE_PURPOSE_INSTALL =
                    "IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWPurpInst]') AND name = N'PK_tblNJAWPurpInst')" +
                    "  BEGIN ALTER TABLE dbo.tblNJAWPurpInst ADD CONSTRAINT PK_tblNJAWPurpInst PRIMARY KEY CLUSTERED (RecID) END;" +
                    "UPDATE tblNJAWService SET PurpInstal = P.RecID   FROM tblNJAWService SE LEFT JOIN tblNJAWPurpInst P ON P.Purpose = SE.PurpInstal",
                ROLLBACK_PURPOSE_INSTALL =
                    "UPDATE tblNJAWService SET PurpInstal = P.Purpose FROM tblNJAWService SE LEFT JOIN tblNJAWPurpInst P ON P.RecID =   SE.PurpInstal",
                UPDATE_PRIORITY =
                    "IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWPriority]') AND name = N'PK_tblNJAWPriority')" +
                    "   BEGIN ALTER TABLE dbo.tblNJAWPriority ADD CONSTRAINT PK_tblNJAWPriority PRIMARY KEY CLUSTERED (RecID) END;" +
                    "UPDATE tblNJAWService SET [Priority] = PR.RecID FROM tblNJAWService SE LEFT JOIN tblNJAWPriority PR ON PR.[Priority] = SE.[Priority]",
                ROLLBACK_PRIORITY =
                    "UPDATE tblNJAWService SET [Priority] = PR.Priority FROM tblNJAWService SE LEFT JOIN tblNJAWPriority PR ON PR.RecID = SE.[Priority]",
                UPDATE_MAIN_TYPE =
                    "IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWTypeMain]') AND name = N'PK_tblNJAWTypeMain')" +
                    "  BEGIN ALTER TABLE dbo.tblNJAWTypeMain ADD CONSTRAINT PK_tblNJAWTypeMain PRIMARY KEY CLUSTERED (RecID) END;" +
                    "UPDATE tblNJAWService SET TypeMain = M.RecID FROM tblNJAWService SE LEFT JOIN tblNJAWTypeMain M ON M.TypeMain = SE.[TypeMain]",
                ROLLBACK_MAIN_TYPE =
                    "UPDATE tblNJAWService SET TypeMain = M.TypeMain FROM tblNJAWService SE LEFT JOIN tblNJAWTypeMain M ON M.RecID = SE.TypeMain";

            public const string
                DROP_CONSTRAINTS_STATISTICS =
                    "IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_ServMatl]') AND type = 'D')" +
                    "  BEGIN ALTER TABLE [tblNJAWService] DROP CONSTRAINT [DF_tblNJAWService_ServMatl] END;" +
                    "IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_15_1933965966__K78_K69_K63_K40_K14_K9')" +
                    "  DROP INDEX [_dta_index_tblNJAWService_15_1933965966__K78_K69_K63_K40_K14_K9] ON [tblNJAWService] WITH ( ONLINE = OFF );" +
                    "IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_15_1933965966__K78_K69_K63_K56_K65_K40_K14_K9')" +
                    "  DROP INDEX [_dta_index_tblNJAWService_15_1933965966__K78_K69_K63_K56_K65_K40_K14_K9] ON [tblNJAWService] WITH ( ONLINE = OFF );" +
                    "IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'IDX_tblNJAWService_SmartGrowth_ServMatl_SizeofService_Town_RecID_CatofService_OpCntr')" +
                    "  DROP INDEX [IDX_tblNJAWService_SmartGrowth_ServMatl_SizeofService_Town_RecID_CatofService_OpCntr] ON [tblNJAWService] WITH ( ONLINE = OFF );" +
                    "if  exists (select * from sys.stats where name = N'_dta_stat_1933965966_78_63' and object_id = object_id(N'[tblNJAWService]'))" +
                    "  DROP STATISTICS [dbo].[tblNJAWService].[_dta_stat_1933965966_78_63];" +
                    "if  exists (select * from sys.stats where name = N'_dta_stat_1933965966_65_78_63' and object_id = object_id(N'[tblNJAWService]'))" +
                    "  DROP STATISTICS [dbo].[tblNJAWService].[_dta_stat_1933965966_65_78_63];" +
                    "if  exists (select * from sys.stats where name = N'_dta_stat_1933965966_78_69_63_65' and object_id = object_id(N'[tblNJAWService]'))" +
                    "  DROP STATISTICS [dbo].[tblNJAWService].[_dta_stat_1933965966_78_69_63_65];" +
                    "if  exists (select * from sys.stats where name = N'_dta_stat_1933965966_78_69_63_56_14' and object_id = object_id(N'[tblNJAWService]'))" +
                    "  DROP STATISTICS [dbo].[tblNJAWService].[_dta_stat_1933965966_78_69_63_56_14];" +
                    "if  exists (select * from sys.stats where name = N'_dta_stat_1933965966_78_69_63_56_65_14' and object_id = object_id(N'[tblNJAWService]'))" +
                    "  DROP STATISTICS [dbo].[tblNJAWService].[_dta_stat_1933965966_78_69_63_56_65_14];" +
                    "IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'IDX_OpCntr_CatofService_PrevServiceMatl_PrevServiceSize')" +
                    "  DROP INDEX [IDX_OpCntr_CatofService_PrevServiceMatl_PrevServiceSize] ON [tblNJAWService] WITH ( ONLINE = OFF );" +
                    "IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_SizeofService]') AND type = 'D')" +
                    "  BEGIN ALTER TABLE [tblNJAWService] DROP CONSTRAINT [DF_tblNJAWService_SizeofService] END;" +
                    "IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_PrevServiceSize]') AND type = 'D')" +
                    "  BEGIN ALTER TABLE [tblNJAWService] DROP CONSTRAINT [DF_tblNJAWService_PrevServiceSize] END;" +
                    "IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_SizeofMain]') AND type = 'D')" +
                    "  BEGIN ALTER TABLE [tblNJAWService] DROP CONSTRAINT [DF_tblNJAWService_SizeofMain] END;" +
                    "IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_PurpInstal]') AND type = 'D')" +
                    "  BEGIN ALTER TABLE [tblNJAWService] DROP CONSTRAINT [DF_tblNJAWService_PurpInstal] END;" +
                    "IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_5_1933965966__K14_K9_K40_K56_K48_K47_K55_K20_K18_K15')" +
                    "  DROP INDEX [_dta_index_tblNJAWService_5_1933965966__K14_K9_K40_K56_K48_K47_K55_K20_K18_K15] ON [tblNJAWService] WITH ( ONLINE = OFF );" +
                    "IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_5_1933965966__K40_K9_K56_K48_K47_K14_K55_K20_K18_K15')" +
                    "  DROP INDEX [_dta_index_tblNJAWService_5_1933965966__K40_K9_K56_K48_K47_K14_K55_K20_K18_K15] ON [tblNJAWService] WITH ( ONLINE = OFF );" +
                    "IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_5_1933965966__K47_K18_K15_K48_K56_K9_K40_K14_K55_K20')" +
                    "  DROP INDEX [_dta_index_tblNJAWService_5_1933965966__K47_K18_K15_K48_K56_K9_K40_K14_K55_K20] ON [tblNJAWService] WITH ( ONLINE = OFF );" +
                    "IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_5_1933965966__K9_K40_K48_K47_K14_K55_K20_K18_K15_K56')" +
                    "  DROP INDEX [_dta_index_tblNJAWService_5_1933965966__K9_K40_K48_K47_K14_K55_K20_K18_K15_K56] ON [tblNJAWService] WITH ( ONLINE = OFF );" +
                    "if  exists (select * from sys.stats where name = N'STAT_tblNJAWService_1933965966_14_55' and object_id = object_id(N'[tblNJAWService]'))" +
                    "  DROP STATISTICS [dbo].[tblNJAWService].[STAT_tblNJAWService_1933965966_14_55];" +
                    "if  exists (select * from sys.stats where name = N'STAT_tblNJAWService_1933965966_48_47_14_55' and object_id = object_id(N'[tblNJAWService]'))" +
                    "  DROP STATISTICS [dbo].[tblNJAWService].[STAT_tblNJAWService_1933965966_48_47_14_55];" +
                    "if  exists (select * from sys.stats where name = N'STAT_tblNJAWService_1933965966_56_14_55_20' and object_id = object_id(N'[tblNJAWService]'))" +
                    "  DROP STATISTICS [dbo].[tblNJAWService].[STAT_tblNJAWService_1933965966_56_14_55_20];" +
                    "if  exists (select * from sys.stats where name = N'STAT_tblNJAWService_1933965966_55_20_14_47_48' and object_id = object_id(N'[tblNJAWService]'))" +
                    "  DROP STATISTICS [dbo].[tblNJAWService].[STAT_tblNJAWService_1933965966_55_20_14_47_48];" +
                    "IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_TypeMain]') AND type = 'D')" +
                    "  BEGIN ALTER TABLE [tblNJAWService] DROP CONSTRAINT [DF_tblNJAWService_TypeMain] END;" +
                    "IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_Priority]') AND type = 'D')" +
                    "   BEGIN ALTER TABLE [tblNJAWService] DROP CONSTRAINT [DF_tblNJAWService_Priority] END;",
                ROLLBACK_CONSTRAINTS_STATISTICS =
                    @"
                        IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_ServMatl]') AND type = 'D') 
                        BEGIN 
                            ALTER TABLE [tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_ServMatl]  DEFAULT ('') FOR [ServMatl] 
                        END;

                        IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_15_1933965966__K78_K69_K63_K40_K14_K9')
                        CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWService_15_1933965966__K78_K69_K63_K40_K14_K9] ON [tblNJAWService] 
                        (
	                        [Town] ASC,
	                        [SizeofService] ASC,
	                        [ServMatl] ASC,
	                        [OpCntr] ASC,
	                        [DateInstalled] ASC,
	                        [CatofService] ASC
                        )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON);
                        
                        IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'_dta_index_tblNJAWService_15_1933965966__K78_K69_K63_K56_K65_K40_K14_K9')
                        CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWService_15_1933965966__K78_K69_K63_K56_K65_K40_K14_K9] ON [tblNJAWService] 
                        (
	                        [Town] ASC,
	                        [SizeofService] ASC,
	                        [ServMatl] ASC,
	                        [RecID] ASC,
	                        [SmartGrowth] ASC,
	                        [OpCntr] ASC,
	                        [DateInstalled] ASC,
	                        [CatofService] ASC
                        )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)

                        IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'IDX_tblNJAWService_SmartGrowth_ServMatl_SizeofService_Town_RecID_CatofService_OpCntr')
                        CREATE NONCLUSTERED INDEX [IDX_tblNJAWService_SmartGrowth_ServMatl_SizeofService_Town_RecID_CatofService_OpCntr] ON [tblNJAWService] 
                        (
	                        [SmartGrowth] ASC,
	                        [ServMatl] ASC,
	                        [SizeofService] ASC,
	                        [Town] ASC,
	                        [RecID] ASC,
	                        [CatofService] ASC,
	                        [OpCntr] ASC
                        )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON);

                        IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'IDX_OpCntr_CatofService_PrevServiceMatl_PrevServiceSize')
                        CREATE NONCLUSTERED INDEX [IDX_OpCntr_CatofService_PrevServiceMatl_PrevServiceSize] ON [tblNJAWService] 
                        (
	                        [OpCntr] ASC,
	                        [CatofService] ASC,
	                        [PrevServiceMatl] ASC,
	                        [PrevServiceSize] ASC,
	                        [RetireDate] ASC,
	                        [Town] ASC
                        )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON);

                        IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_SizeofService]') AND type = 'D')
                        BEGIN
                        ALTER TABLE [tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_SizeofService]  DEFAULT ('') FOR [SizeofService]
                        END;

                        IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_PrevServiceSize]') AND type = 'D')
                        BEGIN
                        ALTER TABLE [tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_PrevServiceSize]  DEFAULT ('') FOR [PrevServiceSize]
                        END;

                        IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_SizeofMain]') AND type = 'D')
                        BEGIN
                        ALTER TABLE [tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_SizeofMain]  DEFAULT ('') FOR [SizeofMain]
                        END;

                        IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_PurpInstal]') AND type = 'D')
                        BEGIN
                        ALTER TABLE [tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_PurpInstal]  DEFAULT ('') FOR [PurpInstal]
                        END;

                        IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_TypeMain]') AND type = 'D')
                        BEGIN
                        ALTER TABLE [tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_TypeMain]  DEFAULT ('') FOR [TypeMain]
                        END;

                        IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_Priority]') AND type = 'D')
                        BEGIN
                        ALTER TABLE [tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_Priority]  DEFAULT ('') FOR [Priority]
                        END;",
                REMOVE_UPDATE_STORED_PROCEDURES =
                    @"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetServiceMaterialsForSelects]') AND type in (N'P', N'PC'))
                        DROP PROCEDURE [spGetServiceMaterialsForSelects];
                        IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_AddNSIA]') AND type in (N'P', N'PC'))
                        BEGIN
                        EXEC dbo.sp_executesql @statement = N'
                        -- Need to update the TaskNum1 parameter.
                        ALTER PROCEDURE [sp_AddNSIA] 

                        @Agreement varchar(3),
                        @AmntRcvd varchar(9),
                        @ApplApvd varchar(10),
                        @ApplRcvd varchar(10),
                        @ApplSent varchar(10),
                        @Apt varchar(15),
                        @Block varchar(9),
                        @BSDWPermit varchar(5),
                        @CatOfService varchar(40),
                        @ContactDate varchar(10),
                        @CrossStreet varchar(30),
                        @DateClosed varchar(10),
                        @DateIssuedToField varchar(10),
                        @DateInstalled varchar(10),
                        @Development varchar(30),
                        @DevServD varchar(3),
                        @Fax varchar(12),
                        @Initiator varchar(25),
                        @InspDate varchar(10),
                        @InstCost varchar(9),
                        @InstInv varchar(10),
                        @InstInvDate varchar(10),
                        @InspSignOffReady varchar(25),
                        @JobNotes varchar(2000),
                        @LengthService varchar(10),
                        @Lot varchar(9),
                        @MailStNum varchar(10),
                        @MailStName varchar(30),
                        @MailPhoneNum varchar(12),
                        @MailTown varchar(30),
                        @MailState varchar(2),
                        @MailZip varchar(10),
                        @MeterSetReq varchar(17),
                        @Name varchar(40),
                        @OpCntr varchar(4),
                        @ParentTaskNum varchar(10),
                        @PayRefNum varchar(15),
                        @PermitExpDate varchar(12),
                        @PermitNum varchar(12),
                        @PermitRcvdDate varchar(12),
                        @PermitSentDate varchar(12),
                        @PermitType varchar(6),
                        @PhoneNum varchar(12),
                        @PurpInstal int,
                        @PremNum varchar(9),
                        @Priority int,
                        @RoadOpenFee varchar(9),
                        @ServInstFee varchar(9),
                        @ServMatl int,
                        @ServNum varchar(10),
                        @SGCost varchar(9),
                        @SGMethodUsed varchar(15),
                        @SizeOfMain int,
                        @SizeOfService int,
                        @SizeOfTap varchar(10),
                        @SmartGrowth varchar(3),
                        @State varchar(2),
                        @StNum varchar(10),
                        @StName varchar(6),
                        @TapOrdNote varchar(75),
                        @TaskNum1 varchar(18),
                        @TaskNum2 varchar(10),
                        @Town varchar(4),
                        @TwnSection varchar(30),
                        @TypeMain int,
                        @WorkIssuedTo varchar(30),
                        @Zip varchar(10), 
                        @QuesSentDate datetime, 
                        @QuesRecvDate datetime, 
                        @BackflowDevice varchar(40), 
                        @Lat varchar(15),
                        @Lon varchar(15),
                        @ObjectID int 

                         AS

                        insert into tblNJAWService
	                        (Agreement, AmntRcvd, ApplApvd, ApplRcvd, ApplSent, Apt, Block, BSDWPermit, CatOfService, ContactDate,
	                         CrossStreet, DateClosed, DateIssuedToField, DateInstalled, Development, DevServD, Fax, Initiator, InspDate,
	                         InstCost, InstInv, InstInvDate, InspSignOffReady, JobNotes, LengthService, Lot, MailStNum, MailStName,
	                         MailPhoneNum, MailTown, MailState, MailZip, MeterSetReq, Name, OpCntr, ParentTaskNum, PayRefNum,
	                         PermitExpDate, PermitNum, PermitRcvdDate, PermitSentDate, PermitType, PhoneNum, PurpInstal, PremNum,
	                         Priority, RoadOpenFee, ServInstFee, ServMatl, ServNum, SGCost, SGMethodUsed, SizeOfMain, SizeOfService,
	                         SizeOfTap, SmartGrowth, State, StNum, StName, TapOrdNote, TaskNum1, TaskNum2, Town, TwnSection,
	                         TypeMain, WorkIssuedTo, Zip, QuesSentDate, QuesRecvDate, FlowbackDevice, Lat, Lon, ObjectID)
                        values
	                        (@Agreement, @AmntRcvd, @ApplApvd, @ApplRcvd, @ApplSent, @Apt, @Block, @BSDWPermit, @CatOfService, @ContactDate,
	                         @CrossStreet, @DateClosed, @DateIssuedToField, @DateInstalled, @Development, @DevServD, @Fax, @Initiator, @InspDate,
	                         @InstCost, @InstInv, @InstInvDate, @InspSignOffReady, @JobNotes, @LengthService, @Lot, @MailStNum, @MailStName,
	                         @MailPhoneNum, @MailTown, @MailState, @MailZip, @MeterSetReq, @Name, @OpCntr, @ParentTaskNum, @PayRefNum,
	                         @PermitExpDate, @PermitNum, @PermitRcvdDate, @PermitSentDate, @PermitType, @PhoneNum, @PurpInstal, @PremNum,
	                         @Priority, @RoadOpenFee, @ServInstFee, @ServMatl, @ServNum, @SGCost, @SGMethodUsed, @SizeOfMain, @SizeOfService,
	                         @SizeOfTap, @SmartGrowth, @State, @StNum, @StName, @TapOrdNote, @TaskNum1, @TaskNum2, @Town, @TwnSection,
	                         @TypeMain, @WorkIssuedTo, @Zip, @QuesSentDate, @QuesRecvDate, @BackflowDevice, @Lat, @Lon, @ObjectID)

                        ' 
                        END;
                        IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetDevServicesInstalled]') AND type in (N'P', N'PC'))
                        BEGIN
                        EXEC dbo.sp_executesql @statement = N'--exec [spGetDevServicesNotInstalled] ''NJ7'', 60

                        /****** Object:  StoredProcedure [dbo].[spGetDevServicesInstalled]    Script Date: 06/28/2011 09:59:32 ******/

                        /*	Gets developer services installed between the start and end dates.
	                        03 October 2006, cmm	*/

                        ALTER PROCEDURE [spGetDevServicesInstalled]
	                        @StartDate smalldatetime,
	                        @EndDate smalldatetime,
	                        @OpCtr varchar(3),
	                        @Town varchar(30)

                        AS

	                        SELECT SV.Development, CAST(SV.ServNum AS int) AS ''Service Number'',
	  	                         SV.PremNum ''Premise Number'', SV.DateInstalled ''Date Installed'',
		                         SV.StNum + ' ' +  ST.FullStName AS CompleteStAddress

	                        FROM tblNJAWService SV, Streets ST, Towns T
	                        WHERE [DateInstalled]
	                        BETWEEN @StartDate AND @EndDate
	                        AND SV.StName = ST.StreetID
	                        AND SV.Town = T.TownID
	                        AND SV.Town = @Town
	                        AND SV.OpCntr = @OpCtr
	                        AND SV.DevServD = ''Yes''
	                        AND NOT EXISTS (Select 1 from ServiceCategories where Description like ''%Measurement Only%'' and ServiceCategoryID = CatofService)

	                        ORDER BY SV.Development, SV.StName, SV.StNum
                        ' 
                        END;

                        IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetDevServicesNotInstalled]') AND type in (N'P', N'PC'))
                        BEGIN
                        EXEC dbo.sp_executesql @statement = N'/*	Gets developer services installed between the start and end dates.
	                        03 October 2006, cmm	*/

                        ALTER PROCEDURE [spGetDevServicesNotInstalled]
	                        @OpCtr varchar(3),
	                        @Town int

                        AS
	                        SELECT	 SV.Development, CAST(SV.ServNum AS int) AS ''Service Number'',
		                         SV.PremNum ''Premise Number'',
		                         SV.StNum + '' '' + ST.FullStName AS CompleteStAddress
	
	                        FROM tblNJAWService SV, Streets ST, Towns T
	                        WHERE [DateInstalled] = ''''
	                        AND SV.StName = ST.StreetID
	                        AND SV.Town = T.TownID
	                        AND SV.Town = @Town
	                        AND SV.OpCntr = @OpCtr
	                        AND SV.DevServD = ''Yes''
	                        AND NOT EXISTS (Select 1 from ServiceCategories where Description like ''%Measurement Only%'' and ServiceCategoryID = CatofService)
	                        ORDER BY SV.Development, SV.StName, SV.StNum
                        ' 
                        END;

                        IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetSvcNumsIssued]') AND type in (N'P', N'PC'))
                            DROP PROCEDURE [spGetSvcNumsIssued];

                        IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RptServicesInstalled]') AND type in (N'P', N'PC'))
                        BEGIN
                        EXEC dbo.sp_executesql @statement = N'
                        /****** Object:  StoredProcedure [dbo].[RptServicesInstalled]    Script Date: 05/31/2011 11:23:26 ******/
                        ALTER PROCEDURE [RptServicesInstalled] (@startDate datetime, @endDate dateTime, @opCntr varchar(4),  @devdriv varchar(10))
                        AS

                        IF (Len(@opCntr) > 0)
	                        BEGIN
		                        select OperatingCenterCode as OpCntr, Towns.town, SC.Description as CatOfService, isNull(SS.SizeServ,0) as SizeOfService, Sum(isNull(LengthService ,0)) as ''LengthService'', Count(SizeOfService) as ''Total''
			                        from tblNJAWService
			                        LEFT JOIN OperatingCenters OC on OC.OperatingCenterID = tblNJAWservice.OpCntr
			                        LEFT JOIN Towns on Towns.TownID = tblNJAWservice.town
			                        LEFT JOIN ServiceCategories SC on SC.ServiceCategoryID = tblNJAWService.CatOfService
			                        LEFT JOIN tblNJAWSizeServ SS ON SS.RecID = tblNJAWService.SizeofService
			                        where (charindex(''INSTALLATION'', upper(isNull(SC.Description,''''))) > 0 OR charindex('' NEW'', upper(isNull(SC.Description,''''))) > 0)
				                        and OpCntr = @opCntr
				                        and DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
				                        and devServD = isNull(@devdriv, devServD)
			                        group by OperatingCenterCode, Towns.town, SC.Description, SS.SizeServ
			                        order by OperatingCenterCode, Towns.town, SC.Description, SS.SizeServ
	                        END
                        ELSE
	                        BEGIN
		                        select OperatingCenterCode as OpCntr, Towns.town, SC.Description as CatOfService, isNull(SS.SizeServ,0) as SizeOfService, Sum(isNull(LengthService ,0)) as ''LengthService'', Count(SizeOfService) as ''Total''
			                        from tblNJAWService
			                        LEFT JOIN OperatingCenters OC on OC.OperatingCenterID = tblNJAWservice.OpCntr
			                        LEFT JOIN Towns on Towns.TownID = tblNJAWservice.town
			                        LEFT JOIN ServiceCategories SC on SC.ServiceCategoryID = tblNJAWService.CatOfService
			                        LEFT JOIN tblNJAWSizeServ SS ON SS.RecID = tblNJAWService.SizeOfService
			                        where (charindex(''INSTALLATION'', upper(isNull(SC.Description,''''))) > 0 OR charindex('' NEW'', upper(isNull(SC.Description,''''))) > 0)
				                        and DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
				                        and devServD = isNull(@devdriv, devServD)
			                        group by OperatingCenterCode, Towns.town, SC.Description, SS.SizeServ
			                        order by OperatingCenterCode, Towns.town, SC.Description, SS.SizeServ
	                        END
                        ' 
                        END;

                        IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RptServicesRenewed]') AND type in (N'P', N'PC'))
                        BEGIN
                        EXEC dbo.sp_executesql @statement = N'
                        /****** Object:  StoredProcedure [dbo].[RptServicesRenewed]    Script Date: 05/31/2011 11:16:45 ******/
                        ALTER PROCEDURE [RptServicesRenewed] (@startDate datetime, @endDate dateTime, @opCntr varchar(4), @devdriv varchar(10))
                        AS
                        IF (Len(@opCntr) > 0)
	                        BEGIN
		                        select OC.OperatingCenterCode as OpCntr, Towns.town, Towns.TownID as RecID, SC.Description as CatOfService, isNull(SS.SizeServ,0) as SizeOfService, Sum(isNull(LengthService ,0)) as ''LengthService'', Count(SS.SizeServ) as ''Total'', 
			                        Count(case when (smartgrowth=''Yes'') then 1 else null end) as SmartGrowth
			                        from tblNJAWService
			                        LEFT JOIN Towns on Towns.TownID = tblNJAWservice.town
			                        LEFT JOIN OperatingCenters OC on OC.OperatingCenterID = tblNJAWService.OpCntr
			                        LEFT JOIN ServiceCategories SC ON SC.ServiceCategoryID = tblNJAWService.CatofService
			                        LEFT JOIN tblNJAWSizeServ SS ON SS.RecID = tblNJAWService.SizeOfService 
		                        where 
			                        (charindex(''RENEW'', upper(isNull(SC.Description,''''))) > 0 
			                        OR 
			                        charindex(''INSTALL METER SET'', upper(isNull(SC.Description,''''))) > 0)
		                        and OperatingCenterID = @opCntr
		                        and DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
		                        and devServD = isNull(@devdriv, devServD)
			                        group by OC.OperatingCenterCode, Towns.town, Towns.TownID, SC.Description, SS.SizeServ
			                        order by OC.OperatingCenterCode, Towns.town, SC.Description, SS.SizeServ	
	                        END
                        ELSE
	                        BEGIN
		                        select OC.OperatingCenterCode as OpCntr, Towns.town, Towns.TownID as RecID, SC.Description as CatOfService, isNull(SS.SizeServ,0) as SizeOfService, Sum(isNull(LengthService ,0)) as ''LengthService'', Count(SS.SizeServ) as ''Total'', 
			                        Count(case when (smartgrowth=''Yes'') then 1 else null end) as SmartGrowth
			                        from tblNJAWService
			                        LEFT JOIN Towns on Towns.TownID = tblNJAWservice.town
			                        LEFT JOIN OperatingCenters OC on OC.OperatingCenterID = tblNJAWService.OpCntr
			                        LEFT JOIN ServiceCategories SC ON SC.ServiceCategoryID = tblNJAWService.CatofService
			                        LEFT JOIN tblNJAWSizeServ SS ON SS.RecID = tblNJAWService.SizeOfService 
			                        where
				                        (charindex(''RENEW'', upper(isNull(SC.Description,''''))) > 0 
				                        OR 
				                        charindex(''INSTALL METER SET'', upper(isNull(SC.Description,''''))) > 0)
			                        and DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
			                        and devServD = isNull(@devdriv, devServD)
			                        group by OC.OperatingCenterCode, Towns.town, Towns.TownID, SC.Description, SS.SizeServ
			                        order by OC.OperatingCenterCode, Towns.town, SC.Description, SS.SizeServ
	                        END ' 
                        END;

                        IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RptServicesRenewedRetirementsDetail]') AND type in (N'P', N'PC'))
                        BEGIN
                        EXEC dbo.sp_executesql @statement = N'
                                    ALTER Procedure [RptServicesRenewedRetirementsDetail] (@startDate datetime, @endDate datetime, @town int, @CatOfService varchar(40), @SizeOfService varchar(10))
                                    AS
                                    select 
                                    Year(OrigInstDate) as ''Year Installed'', 
                                    SM.Description as PrevServiceMatl,  
                                    LengthService as [Length], --Sum(LengthService) as [Length], 
                                    servnum,--Count(OrigInstDate) as ''Total''
                                    TaskNum1 as TaskNumber	
                                    from 
                                    tblNJAWService
                                    left join 
                                        ServiceCategories SC on SC.ServiceCategoryID = tblNJAWService.CatOfService
                                    left join
										tblNJAWSizeServ SS on SS.RecID = tblNJAWService.SizeofService
									left join
										ServiceMaterials SM on SM.ServiceMaterialID = tblNJAWService.PrevServiceMatl
                                    where 
                                    year(OrigInstDate) > 1900
                                    and 
                                    DateInstalled >= @startDate and DateInstalled <= @endDate
                                    and 
                                    SC.Description = @CatOfService
                                    and 
                                    SS.SizeServ = @SizeOfService
                                    and 
                                    Town = @Town
                                    --group by 
                                    --	year(OrigInstDate), PrevServiceMatl
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
		                                    select OperatingCenterCode as OpCntr, Towns.town, SC.Description as CatOfService, isNull(SS.SizeServ,0) as SizeOfService, LengthService as ''LengthService'', ServNum, case when (isnull(smartgrowth,'''')=''YES'') then ''Yes'' end as [SmartGrowth]
			                                    from tblNJAWService S
			                                    LEFT JOIN Towns on Towns.TownID = S.town
			                                    LEFT JOIN OperatingCenters OC on OC.OperatingCenterID = S.OpCntr
			                                    LEFT JOIN ServiceCategories SC on SC.ServiceCategoryID = S.CatOfService
			                                    LEFT JOIN tblNJAWSizeServ SS ON SS.RecID = S.SizeOfService 
			                                    where (charindex(''INSTALLATION'', SC.Description) > 0 OR charindex('' NEW'', SC.Description) > 0)
				                                    and oc.OperatingCenterID = @opCntr
				                                    and DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
				                                    and devServD = isNull(@devdriv, devServD)
			                                    order by OpCntr, Towns.town, SC.Description, SS.SizeServ	
	                                    END
                                    ELSE
	                                    BEGIN
		                                    select OperatingCenterCode as OpCntr, Towns.town, SC.Description as CatOfService, isNull(SS.SizeServ,0) as SizeOfService, LengthService as ''LengthService'', ServNum, case when (isnull(smartgrowth,'''')=''YES'') then ''Yes'' end as [SmartGrowth] 
			                                    from tblNJAWService S
			                                    LEFT JOIN Towns on Towns.TownID = S.town
			                                    LEFT JOIN OperatingCenters OC on OC.OperatingCenterID = S.OpCntr
			                                    LEFT JOIN ServiceCategories SC on SC.ServiceCategoryID = S.CatOfService
			                                    LEFT JOIN tblNJAWSizeServ SS ON SS.RecID = S.SizeOfService 
			                                    where (charindex(''INSTALLATION'', SC.Description) > 0 OR charindex('' NEW'', SC.Description) > 0)
				                                    and DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
				                                    and devServD = isNull(@devdriv, devServD)
			                                    order by OpCntr, Towns.town, SC.Description, SS.SizeServ
	                                    END
                                    ' 
                                END;",
                RESTORE_STORED_PROCEDURES =
                    @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetServiceMaterialsForSelects]') AND type in (N'P', N'PC'))
                        BEGIN
                        EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [spGetServiceMaterialsForSelects]
	                        @OpCntr varchar(3),
	                        @NewOnly varchar(3)

                        AS
	                        IF ( UPPER(@NewOnly) = ''YES'' )

		                        SELECT ServMatl
		                        FROM tblNJAWServMatl
		                        WHERE OpCntr = @OpCntr
		                        AND NSR = ''ON''
		                        ORDER BY ServMatl

	                        ELSE

		                        SELECT ServMatl
		                        FROM tblNJAWServMatl
		                        WHERE OpCntr = @OpCntr
		                        ORDER BY ServMatl' 
                        END;
                        IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_AddNSIA]') AND type in (N'P', N'PC'))
                        BEGIN
                        EXEC dbo.sp_executesql @statement = N'
                        -- Need to update the TaskNum1 parameter.
                        ALTER PROCEDURE [sp_AddNSIA] 

                        @Agreement varchar(3),
                        @AmntRcvd varchar(9),
                        @ApplApvd varchar(10),
                        @ApplRcvd varchar(10),
                        @ApplSent varchar(10),
                        @Apt varchar(15),
                        @Block varchar(9),
                        @BSDWPermit varchar(5),
                        @CatOfService varchar(40),
                        @ContactDate varchar(10),
                        @CrossStreet varchar(30),
                        @DateClosed varchar(10),
                        @DateIssuedToField varchar(10),
                        @DateInstalled varchar(10),
                        @Development varchar(30),
                        @DevServD varchar(3),
                        @Fax varchar(12),
                        @Initiator varchar(25),
                        @InspDate varchar(10),
                        @InstCost varchar(9),
                        @InstInv varchar(10),
                        @InstInvDate varchar(10),
                        @InspSignOffReady varchar(25),
                        @JobNotes varchar(2000),
                        @LengthService varchar(10),
                        @Lot varchar(9),
                        @MailStNum varchar(10),
                        @MailStName varchar(30),
                        @MailPhoneNum varchar(12),
                        @MailTown varchar(30),
                        @MailState varchar(2),
                        @MailZip varchar(10),
                        @MeterSetReq varchar(17),
                        @Name varchar(40),
                        @OpCntr varchar(4),
                        @ParentTaskNum varchar(10),
                        @PayRefNum varchar(15),
                        @PermitExpDate varchar(12),
                        @PermitNum varchar(12),
                        @PermitRcvdDate varchar(12),
                        @PermitSentDate varchar(12),
                        @PermitType varchar(6),
                        @PhoneNum varchar(12),
                        @PurpInstal varchar(30),
                        @PremNum varchar(9),
                        @Priority varchar(25),
                        @RoadOpenFee varchar(9),
                        @ServInstFee varchar(9),
                        @ServMatl varchar(20),
                        @ServNum varchar(10),
                        @SGCost varchar(9),
                        @SGMethodUsed varchar(15),
                        @SizeOfMain varchar(10),
                        @SizeOfService varchar(10),
                        @SizeOfTap varchar(10),
                        @SmartGrowth varchar(3),
                        @State varchar(2),
                        @StNum varchar(10),
                        @StName varchar(6),
                        @TapOrdNote varchar(75),
                        @TaskNum1 varchar(18),
                        @TaskNum2 varchar(10),
                        @Town varchar(4),
                        @TwnSection varchar(30),
                        @TypeMain varchar(15),
                        @WorkIssuedTo varchar(30),
                        @Zip varchar(10), 
                        @QuesSentDate datetime, 
                        @QuesRecvDate datetime, 
                        @BackflowDevice varchar(40), 
                        @Lat varchar(15),
                        @Lon varchar(15),
                        @ObjectID int 

                            AS

                        insert into tblNJAWService
	                        (Agreement, AmntRcvd, ApplApvd, ApplRcvd, ApplSent, Apt, Block, BSDWPermit, CatOfService, ContactDate,
	                            CrossStreet, DateClosed, DateIssuedToField, DateInstalled, Development, DevServD, Fax, Initiator, InspDate,
	                            InstCost, InstInv, InstInvDate, InspSignOffReady, JobNotes, LengthService, Lot, MailStNum, MailStName,
	                            MailPhoneNum, MailTown, MailState, MailZip, MeterSetReq, Name, OpCntr, ParentTaskNum, PayRefNum,
	                            PermitExpDate, PermitNum, PermitRcvdDate, PermitSentDate, PermitType, PhoneNum, PurpInstal, PremNum,
	                            Priority, RoadOpenFee, ServInstFee, ServMatl, ServNum, SGCost, SGMethodUsed, SizeOfMain, SizeOfService,
	                            SizeOfTap, SmartGrowth, State, StNum, StName, TapOrdNote, TaskNum1, TaskNum2, Town, TwnSection,
	                            TypeMain, WorkIssuedTo, Zip, QuesSentDate, QuesRecvDate, FlowbackDevice, Lat, Lon, ObjectID)
                        values
	                        (@Agreement, @AmntRcvd, @ApplApvd, @ApplRcvd, @ApplSent, @Apt, @Block, @BSDWPermit, @CatOfService, @ContactDate,
	                            @CrossStreet, @DateClosed, @DateIssuedToField, @DateInstalled, @Development, @DevServD, @Fax, @Initiator, @InspDate,
	                            @InstCost, @InstInv, @InstInvDate, @InspSignOffReady, @JobNotes, @LengthService, @Lot, @MailStNum, @MailStName,
	                            @MailPhoneNum, @MailTown, @MailState, @MailZip, @MeterSetReq, @Name, @OpCntr, @ParentTaskNum, @PayRefNum,
	                            @PermitExpDate, @PermitNum, @PermitRcvdDate, @PermitSentDate, @PermitType, @PhoneNum, @PurpInstal, @PremNum,
	                            @Priority, @RoadOpenFee, @ServInstFee, @ServMatl, @ServNum, @SGCost, @SGMethodUsed, @SizeOfMain, @SizeOfService,
	                            @SizeOfTap, @SmartGrowth, @State, @StNum, @StName, @TapOrdNote, @TaskNum1, @TaskNum2, @Town, @TwnSection,
	                            @TypeMain, @WorkIssuedTo, @Zip, @QuesSentDate, @QuesRecvDate, @BackflowDevice, @Lat, @Lon, @ObjectID)

                        ' 
                        END;
                        IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetDevServicesInstalled]') AND type in (N'P', N'PC'))
                        BEGIN
                        EXEC dbo.sp_executesql @statement = N'--exec [spGetDevServicesNotInstalled] ''NJ7'', 60

                        /****** Object:  StoredProcedure [dbo].[spGetDevServicesInstalled]    Script Date: 06/28/2011 09:59:32 ******/

                        /*	Gets developer services installed between the start and end dates.
	                        03 October 2006, cmm	*/

                        ALTER PROCEDURE [spGetDevServicesInstalled]
	                        @StartDate smalldatetime,
	                        @EndDate smalldatetime,
	                        @OpCtr varchar(3),
	                        @Town varchar(30)

                        AS

	                        SELECT SV.Development, CAST(SV.ServNum AS int) AS ''Service Number'',
	  	                         SV.PremNum ''Premise Number'', SV.DateInstalled ''Date Installed'',
		                         SV.StNum + ' ' +  ST.FullStName AS CompleteStAddress

	                        FROM tblNJAWService SV, Streets ST, Towns T
	                        WHERE [DateInstalled]
	                        BETWEEN @StartDate AND @EndDate
	                        AND SV.StName = ST.StreetID
	                        AND SV.Town = T.TownID
	                        AND SV.Town = @Town
	                        AND SV.OpCntr = @OpCtr
	                        AND SV.DevServD = ''Yes''
	                        AND CatOfService NOT LIKE ''%Measurement Only%''

	                        ORDER BY SV.Development, SV.StName, SV.StNum
                        ' 
                        END;

                        IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetDevServicesNotInstalled]') AND type in (N'P', N'PC'))
                        BEGIN
                        EXEC dbo.sp_executesql @statement = N'/*	Gets developer services installed between the start and end dates.
	                        03 October 2006, cmm	*/

                        ALTER PROCEDURE [spGetDevServicesNotInstalled]
	                        @OpCtr varchar(3),
	                        @Town int

                        AS
	                        SELECT	 SV.Development, CAST(SV.ServNum AS int) AS ''Service Number'',
		                         SV.PremNum ''Premise Number'',
		                         SV.StNum + '' '' + ST.FullStName AS CompleteStAddress
	
	                        FROM tblNJAWService SV, Streets ST, Towns T
	                        WHERE [DateInstalled] = ''''
	                        AND SV.StName = ST.StreetID
	                        AND SV.Town = T.TownID
	                        AND SV.Town = @Town
	                        AND SV.OpCntr = @OpCtr
	                        AND SV.DevServD = ''Yes''
	                        AND CatOfService NOT LIKE ''%Measurement Only%''

	                        ORDER BY SV.Development, SV.StName, SV.StNum
                        ' 
                        END;

                            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[spGetSvcNumsIssued]') AND type in (N'P', N'PC'))
                            BEGIN
                            EXEC dbo.sp_executesql @statement = N'
                            /****** Object:  StoredProcedure [dbo].[spGetSvcNumsIssued]    Script Date: 05/31/2011 11:06:37 ******/
                            CREATE PROCEDURE [spGetSvcNumsIssued]
	                            @StartDate smalldatetime,
	                            @EndDate smalldatetime,
	                            @OpCtr varchar(3)
                            AS

                            SET NOCOUNT ON;

	                            SELECT CAST(SV.ServNum AS int) ''Service Number'', SV.DateInstalled, 
	                            SV.StNum + '' '' +  ST.FullStName AS CompleteStAddress,
	                            TN.Town, SV.ContactDate, SV.CatOfService
	
	                            FROM tblNJAWService SV, Streets ST, Towns TN
	                            JOIN OperatingCentersTowns OCT
	                            ON OCT.TownID = TN.TownID
	                            JOIN OperatingCenters OC
	                            ON OC.OperatingCenterID = OCT.OperatingCenterID		
	                            WHERE SV.ContactDate
	                            BETWEEN @StartDate  AND @EndDate
	                            AND SV.StName = ST.StreetID
	                            AND SV.Town = TN.TownID
	                            AND SV.PurpInstal = ''New Service'' 
	                            AND OC.OperatingCenterCode = @OpCtr

	                            ORDER BY SV.ServNum

                            ' END;

                            IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RptServicesInstalled]') AND type in (N'P', N'PC'))
                            BEGIN
                            EXEC dbo.sp_executesql @statement = N'
                            /****** Object:  StoredProcedure [dbo].[RptServicesInstalled]    Script Date: 05/31/2011 11:23:26 ******/
                            ALTER PROCEDURE [RptServicesInstalled] (@startDate datetime, @endDate dateTime, @opCntr varchar(4),  @devdriv varchar(10))
                            AS

                            IF (Len(@opCntr) > 0)
	                            BEGIN
		                            select OpCntr, Towns.town, CatOfService, isNull(SizeOfService,0) as SizeOfService, Sum(isNull(LengthService ,0)) as ''LengthService'', Count(SizeOfService) as ''Total''
			                            from tblNJAWService
			                            LEFT JOIN Towns on Towns.TownID = tblNJAWservice.town
			                            where (charindex(''INSTALLATION'', upper(isNull(catofservice,''''))) > 0 OR charindex('' NEW'', upper(isNull(catofservice,''''))) > 0)
				                            and tblNJAWService.OpCntr = @opCntr
				                            and DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
				                            and devServD = isNull(@devdriv, devServD)
			                            group by OpCntr, Towns.town, CatofService, SizeofService
			                            order by OpCntr, Towns.town, CatofService, SizeofService	
	                            END
                            ELSE
	                            BEGIN
		                            select OpCntr, Towns.town, CatOfService, isNull(SizeOfService,0) as SizeOfService, Sum(isNull(LengthService ,0)) as ''LengthService'', Count(SizeOfService) as ''Total''
			                            from tblNJAWService
			                            LEFT JOIN Towns on Towns.TownID = tblNJAWservice.town
			                            where (charindex(''INSTALLATION'', upper(isNull(catofservice,''''))) > 0 OR charindex('' NEW'', upper(isNull(catofservice,''''))) > 0)
				                            and DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
				                            and devServD = isNull(@devdriv, devServD)
			                            group by OpCntr, Towns.town, CatofService, SizeofService
			                            order by OpCntr, Towns.town, CatofService, SizeofService
	                            END
                            ' 
                            END;

                            IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RptServicesRenewed]') AND type in (N'P', N'PC'))
                            BEGIN
                            EXEC dbo.sp_executesql @statement = N'
                                    /****** Object:  StoredProcedure [dbo].[RptServicesRenewed]    Script Date: 05/31/2011 11:16:45 ******/
                                    ALTER PROCEDURE [RptServicesRenewed] (@startDate datetime, @endDate dateTime, @opCntr varchar(4), @devdriv varchar(10))
                                    AS
                                    IF (Len(@opCntr) > 0)
	                                    BEGIN
		                                    select OC.OperatingCenterCode as OpCntr, Towns.town, Towns.TownID as RecID, SC.Description as CatOfService, isNull(SS.SizeServ,0) as SizeOfService, Sum(isNull(LengthService ,0)) as ''LengthService'', Count(SS.SizeServ) as ''Total'', 
			                                    Count(case when (smartgrowth=''Yes'') then 1 else null end) as SmartGrowth
			                                    from tblNJAWService
			                                    LEFT JOIN Towns on Towns.TownID = tblNJAWservice.town
			                                    LEFT JOIN OperatingCenters OC on OC.OperatingCenterID = tblNJAWService.OpCntr
			                                    LEFT JOIN ServiceCategories SC ON SC.ServiceCategoryID = tblNJAWService.CatofService
			                                    LEFT JOIN tblNJAWSizeServ SS ON SS.RecID = tblNJAWService.SizeOfService 
		                                    where 
			                                    (charindex(''RENEW'', upper(isNull(SC.Description,''''))) > 0 
			                                    OR 
			                                    charindex(''INSTALL METER SET'', upper(isNull(SC.Description,''''))) > 0)
		                                    and OperatingCenterID = @opCntr
		                                    and DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
		                                    and devServD = isNull(@devdriv, devServD)
			                                    group by OC.OperatingCenterCode, Towns.town, Towns.TownID, SC.Description, SS.SizeServ
			                                    order by OC.OperatingCenterCode, Towns.town, SC.Description, SS.SizeServ	
	                                    END
                                    ELSE
	                                    BEGIN
		                                    select OC.OperatingCenterCode as OpCntr, Towns.town, Towns.TownID as RecID, SC.Description as CatOfService, isNull(SS.SizeServ,0) as SizeOfService, Sum(isNull(LengthService ,0)) as ''LengthService'', Count(SS.SizeServ) as ''Total'', 
			                                    Count(case when (smartgrowth=''Yes'') then 1 else null end) as SmartGrowth
			                                    from tblNJAWService
			                                    LEFT JOIN Towns on Towns.TownID = tblNJAWservice.town
			                                    LEFT JOIN OperatingCenters OC on OC.OperatingCenterID = tblNJAWService.OpCntr
			                                    LEFT JOIN ServiceCategories SC ON SC.ServiceCategoryID = tblNJAWService.CatofService
			                                    LEFT JOIN tblNJAWSizeServ SS ON SS.RecID = tblNJAWService.SizeOfService 
			                                    where
				                                    (charindex(''RENEW'', upper(isNull(SC.Description,''''))) > 0 
				                                    OR 
				                                    charindex(''INSTALL METER SET'', upper(isNull(SC.Description,''''))) > 0)
			                                    and DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
			                                    and devServD = isNull(@devdriv, devServD)
			                                    group by OC.OperatingCenterCode, Towns.town, Towns.TownID, SC.Description, SS.SizeServ
			                                    order by OC.OperatingCenterCode, Towns.town, SC.Description, SS.SizeServ
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
                                END;";
        }

        public struct Tables
        {
            public const string OPERATING_CENTERS = "OperatingCenters",
                                SERVICES = "tblNJAWService",
                                SERVICE_MATERIALS = "ServiceMaterials",
                                SERVICE_SIZES = "tblNJAWSizeServ",
                                OPERATING_CENTERS_SERVICE_MATERIALS = "tblNJAWServMatl",
                                INSTALLATION_PURPOSES = "tblNJAWPurpInst",
                                MAIN_TYPES = "tblNJAWTypeMain",
                                PRIORITIES = "tblNJAWPriority";
        }

        public struct Columns
        {
            public const string SERVICE_ID = "RecID",
                                OPERATING_CENTER = "OpCntr",
                                OPERATING_CENTER_ID = "OperatingCenterID",
                                DESCRIPTION = "Description",
                                SERVICE_MATERIAL = "ServMatl",
                                SERVICE_MATERIAL_ID = "ServiceMaterialID",
                                PREVIOUS_SERVICE_MATERIAL = "PrevServiceMatl",
                                MAIN_SIZE = "SizeOfMain",
                                PURPOSE_OF_INSTALL = "PurpInstal",
                                PURPOSE_OF_INSTALL_ID = "RecID",
                                PRIORITY = "Priority",
                                MAIN_TYPE = "TypeMain",
                                MAIN_TYPE_ID = "RecID",
                                SERVICE_SIZE = "SizeOfService",
                                PREVIOUS_SERVICE_SIZE = "PrevServiceSize",
                                SERVICE_SIZE_ID = "RecID",
                                MAIN_SIZE_ID = "RecID",
                                PRIORITY_ID = "RecID";
        }

        public struct ForeignKeys
        {
            public const string FK_SERVICES_SERVICE_MATERIALS = "FK_tblNJAWService_ServiceMaterials_ServMatl",
                                FK_SERVICES_PREVIOUS_SERVICE_MATERIALS =
                                    "FK_tblNJAWService_ServiceMaterials_PrevServiceMatl",
                                FK_OPERATING_CENTERS_SERVICE_MATERIALS_OPERATING_CENTERS =
                                    "FK_tblNJAWServMatl_OperatingCenters_OpCntr",
                                FK_OPERATING_CENTERS_SERVICE_MATERIALS_SERVICE_MATERIALS =
                                    "FK_tblNJAWServMatl_ServiceMaterials_ServMatl",
                                FK_SERVICES_SERVICE_SIZE = "FK_tblNJAWService_tblNJAWSizeServ_SizeofService",
                                FK_SERVICES_PREVIOUS_SERVICE_SIZE = "FK_tblNJAWService_tblNJAWSizeServ_PrevServiceSize",
                                FK_SERVICES_MAIN_SIZE_MAIN_SIZE_ID = "FK_tblNJAWService_tblNJAWSizeServ_SizeOfMain",
                                FK_SERVICES_INSTALLATION_PURPOSE = "FK_tblNJAWService_tblNJAWPurpInst_PurpInstal",
                                FK_SERVICES_MAIN_TYPES = "FK_tblNJAWService_tblNJAWTypeMain_TypeMain",
                                FK_SERVICES_PRIORITIES = "FK_tblNJAWService_tblNJAWPriority_Priority";
        }

        public struct StringLengths
        {
            public const int OPERATING_CENTER = 4,
                             DESCRIPTION = 50,
                             SERVICE_MATERIAL = 20,
                             SERVICE_SIZE = 10,
                             MAIN_SIZE = 10,
                             MAIN_TYPE = 15,
                             INSTALLATION_PURPOSE = 50,
                             PRIORITY = 25;
        }

        #endregion

        public override void Up()
        {
            #region Create Tables

            Create.Table(Tables.SERVICE_MATERIALS)
                  .WithColumn(Columns.SERVICE_MATERIAL_ID).AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();

            #endregion

            #region Constraints/Indexes

            Execute.Sql(SQL.DROP_CONSTRAINTS_STATISTICS);

            #endregion

            #region Transform Data

            Execute.Sql(SQL.UPDATE_SERVICE_MATERIALS);
            Execute.Sql(SQL.UPDATE_SERVICE_SIZES);
            Execute.Sql(SQL.UPDATE_SERVICE_MAIN_SIZE);
            Execute.Sql(SQL.UPDATE_PURPOSE_INSTALL);
            Execute.Sql(SQL.UPDATE_PRIORITY);
            Execute.Sql(SQL.UPDATE_MAIN_TYPE);

            #endregion

            #region Columns

            // Size of Main
            // Purpose of Install
            // Priority
            // Main Type
            //* State: not necessary, it is defined for the town already

            Alter.Table(Tables.SERVICES)
                 .AlterColumn(Columns.SERVICE_MATERIAL).AsInt32().Nullable()
                 .AlterColumn(Columns.PREVIOUS_SERVICE_MATERIAL).AsInt32().Nullable()
                 .AlterColumn(Columns.SERVICE_SIZE).AsInt32().Nullable()
                 .AlterColumn(Columns.PREVIOUS_SERVICE_SIZE).AsInt32().Nullable()
                 .AlterColumn(Columns.MAIN_SIZE).AsInt32().Nullable()
                 .AlterColumn(Columns.PURPOSE_OF_INSTALL).AsInt32().Nullable()
                 .AlterColumn(Columns.MAIN_TYPE).AsInt32().Nullable()
                 .AlterColumn(Columns.PRIORITY).AsInt32().Nullable();
            Alter.Table(Tables.OPERATING_CENTERS_SERVICE_MATERIALS)
                 .AlterColumn(Columns.SERVICE_MATERIAL).AsInt32().Nullable()
                 .AlterColumn(Columns.OPERATING_CENTER).AsInt32().Nullable();

            #endregion

            #region Foreign Keys

            Create.ForeignKey(ForeignKeys.FK_OPERATING_CENTERS_SERVICE_MATERIALS_OPERATING_CENTERS)
                  .FromTable(Tables.OPERATING_CENTERS_SERVICE_MATERIALS).ForeignColumn(Columns.OPERATING_CENTER)
                  .ToTable(Tables.OPERATING_CENTERS).PrimaryColumn(Columns.OPERATING_CENTER_ID);
            Create.ForeignKey(ForeignKeys.FK_OPERATING_CENTERS_SERVICE_MATERIALS_SERVICE_MATERIALS)
                  .FromTable(Tables.OPERATING_CENTERS_SERVICE_MATERIALS).ForeignColumn(Columns.SERVICE_MATERIAL)
                  .ToTable(Tables.SERVICE_MATERIALS).PrimaryColumn(Columns.SERVICE_MATERIAL_ID);

            Create.ForeignKey(ForeignKeys.FK_SERVICES_SERVICE_MATERIALS)
                  .FromTable(Tables.SERVICES).ForeignColumn(Columns.SERVICE_MATERIAL)
                  .ToTable(Tables.SERVICE_MATERIALS).PrimaryColumn(Columns.SERVICE_MATERIAL_ID);
            Create.ForeignKey(ForeignKeys.FK_SERVICES_PREVIOUS_SERVICE_MATERIALS)
                  .FromTable(Tables.SERVICES).ForeignColumn(Columns.PREVIOUS_SERVICE_MATERIAL)
                  .ToTable(Tables.SERVICE_MATERIALS).PrimaryColumn(Columns.SERVICE_MATERIAL_ID);
            Create.ForeignKey(ForeignKeys.FK_SERVICES_SERVICE_SIZE)
                  .FromTable(Tables.SERVICES).ForeignColumn(Columns.SERVICE_SIZE)
                  .ToTable(Tables.SERVICE_SIZES).PrimaryColumn(Columns.SERVICE_SIZE_ID);
            Create.ForeignKey(ForeignKeys.FK_SERVICES_PREVIOUS_SERVICE_SIZE)
                  .FromTable(Tables.SERVICES).ForeignColumn(Columns.PREVIOUS_SERVICE_SIZE)
                  .ToTable(Tables.SERVICE_SIZES).PrimaryColumn(Columns.SERVICE_SIZE_ID);
            Create.ForeignKey(ForeignKeys.FK_SERVICES_MAIN_SIZE_MAIN_SIZE_ID)
                  .FromTable(Tables.SERVICES).ForeignColumn(Columns.MAIN_SIZE)
                  .ToTable(Tables.SERVICE_SIZES).PrimaryColumn(Columns.MAIN_SIZE_ID);
            Create.ForeignKey(ForeignKeys.FK_SERVICES_INSTALLATION_PURPOSE)
                  .FromTable(Tables.SERVICES).ForeignColumn(Columns.PURPOSE_OF_INSTALL)
                  .ToTable(Tables.INSTALLATION_PURPOSES).PrimaryColumn(Columns.PURPOSE_OF_INSTALL_ID);
            Create.ForeignKey(ForeignKeys.FK_SERVICES_MAIN_TYPES)
                  .FromTable(Tables.SERVICES).ForeignColumn(Columns.MAIN_TYPE)
                  .ToTable(Tables.MAIN_TYPES).PrimaryColumn(Columns.MAIN_TYPE_ID);
            Create.ForeignKey(ForeignKeys.FK_SERVICES_PRIORITIES)
                  .FromTable(Tables.SERVICES).ForeignColumn(Columns.PRIORITY)
                  .ToTable(Tables.PRIORITIES).PrimaryColumn(Columns.PRIORITY_ID);

            #endregion
        }

        public override void Down()
        {
            #region Indexes/Constraints/Statistics

            Execute.Sql(SQL.DROP_CONSTRAINTS_STATISTICS);

            #endregion

            #region Foreign Keys

            Delete.ForeignKey(ForeignKeys.FK_SERVICES_SERVICE_MATERIALS).OnTable(Tables.SERVICES);
            Delete.ForeignKey(ForeignKeys.FK_SERVICES_PREVIOUS_SERVICE_MATERIALS).OnTable(Tables.SERVICES);
            Delete.ForeignKey(ForeignKeys.FK_SERVICES_SERVICE_SIZE).OnTable(Tables.SERVICES);
            Delete.ForeignKey(ForeignKeys.FK_SERVICES_PREVIOUS_SERVICE_SIZE).OnTable(Tables.SERVICES);
            Delete.ForeignKey(ForeignKeys.FK_SERVICES_MAIN_SIZE_MAIN_SIZE_ID).OnTable(Tables.SERVICES);
            Delete.ForeignKey(ForeignKeys.FK_SERVICES_INSTALLATION_PURPOSE).OnTable(Tables.SERVICES);
            Delete.ForeignKey(ForeignKeys.FK_SERVICES_MAIN_TYPES).OnTable(Tables.SERVICES);
            Delete.ForeignKey(ForeignKeys.FK_SERVICES_PRIORITIES).OnTable(Tables.SERVICES);

            Delete.ForeignKey(ForeignKeys.FK_OPERATING_CENTERS_SERVICE_MATERIALS_OPERATING_CENTERS)
                  .OnTable(Tables.OPERATING_CENTERS_SERVICE_MATERIALS);
            Delete.ForeignKey(ForeignKeys.FK_OPERATING_CENTERS_SERVICE_MATERIALS_SERVICE_MATERIALS)
                  .OnTable(Tables.OPERATING_CENTERS_SERVICE_MATERIALS);

            #endregion

            #region Columns

            Alter.Table(Tables.SERVICES)
                 .AlterColumn(Columns.SERVICE_MATERIAL).AsAnsiString(StringLengths.SERVICE_MATERIAL).Nullable()
                 .AlterColumn(Columns.PREVIOUS_SERVICE_MATERIAL).AsAnsiString(StringLengths.SERVICE_MATERIAL).Nullable()
                 .AlterColumn(Columns.SERVICE_SIZE).AsAnsiString(StringLengths.SERVICE_SIZE).Nullable()
                 .AlterColumn(Columns.PREVIOUS_SERVICE_SIZE).AsAnsiString(StringLengths.SERVICE_SIZE).Nullable()
                 .AlterColumn(Columns.MAIN_SIZE).AsAnsiString(StringLengths.MAIN_SIZE).Nullable()
                 .AlterColumn(Columns.PURPOSE_OF_INSTALL).AsAnsiString(StringLengths.INSTALLATION_PURPOSE).Nullable()
                 .AlterColumn(Columns.MAIN_TYPE).AsAnsiString(StringLengths.MAIN_TYPE).Nullable()
                 .AlterColumn(Columns.PRIORITY).AsAnsiString(StringLengths.PRIORITY).Nullable();
            Alter.Table(Tables.OPERATING_CENTERS_SERVICE_MATERIALS)
                 .AlterColumn(Columns.SERVICE_MATERIAL).AsAnsiString(StringLengths.SERVICE_MATERIAL).Nullable()
                 .AlterColumn(Columns.OPERATING_CENTER).AsAnsiString(StringLengths.OPERATING_CENTER).Nullable();

            #endregion

            #region Transform Data

            Execute.Sql(SQL.ROLLBACK_SERVICE_MATERIALS);
            Execute.Sql(SQL.ROLLBACK_SERVICE_SIZES);
            Execute.Sql(SQL.ROLLBACK_SERVICE_MAIN_SIZE);
            Execute.Sql(SQL.ROLLBACK_PURPOSE_INSTALL);
            Execute.Sql(SQL.ROLLBACK_PRIORITY);
            Execute.Sql(SQL.ROLLBACK_MAIN_TYPE);

            #endregion

            #region Constraints/Indexes

            Execute.Sql(SQL.ROLLBACK_CONSTRAINTS_STATISTICS);

            #endregion

            #region Delete Tables

            Delete.Table(Tables.SERVICE_MATERIALS);

            #endregion
        }
    }
}
