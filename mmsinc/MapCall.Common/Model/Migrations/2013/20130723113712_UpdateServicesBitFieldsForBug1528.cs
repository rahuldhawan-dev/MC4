using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130723113712), Tags("Production")]
    public class UpdateServicesBitFieldsForBug1528 : Migration
    {
        #region Constants

        public struct Sql
        {
            public const string UPDATE_AGREEMENT =
                                    "UPDATE tblNJAWService SET Agreement = 1    where UPPER(ISNULL(Agreement, '')) = 'YES';" +
                                    "UPDATE tblNJAWService SET Agreement = 0    where ISNULL(Agreement, '') = 'NO';" +
                                    "UPDATE tblNJAWService SET Agreement = null where ISNULL(Agreement, '') = '';",
                                UPDATE_BSDM_PERMIT =
                                    "UPDATE tblNJAWService SET BSDWPermit = 1    where UPPER(ISNULL(BSDWPermit, '')) = 'YES';" +
                                    "UPDATE tblNJAWService SET BSDWPermit = 0    where ISNULL(BSDWPermit, '') = 'NO';" +
                                    "UPDATE tblNJAWService SET BSDWPermit = null where ISNULL(BSDWPermit, '') = '';",
                                UPDATE_DEV_SERV_D =
                                    "UPDATE tblNJAWService SET DevServD = 1    where UPPER(ISNULL(DevServD, '')) = 'YES';" +
                                    "UPDATE tblNJAWService SET DevServD = 0    where ISNULL(DevServD, '') = 'NO';" +
                                    "UPDATE tblNJAWService SET DevServD = null where ISNULL(DevServD, '') = '';",
                                UPDATE_METER_SET_REQUIRED =
                                    "UPDATE tblNJAWService SET MeterSetReq = 1    where UPPER(ISNULL(MeterSetReq, '')) = 'YES';" +
                                    "UPDATE tblNJAWService SET MeterSetReq = 0    where ISNULL(MeterSetReq, '') = 'NO';" +
                                    "UPDATE tblNJAWService SET MeterSetReq = null where ISNULL(MeterSetReq, '') = '';",
                                UPDATE_SMART_GROWTH =
                                    "UPDATE tblNJAWService SET SmartGrowth = 1    where UPPER(ISNULL(SmartGrowth, '')) = 'YES';" +
                                    "UPDATE tblNJAWService SET SmartGrowth = 0    where ISNULL(SmartGrowth, '') = 'NO';" +
                                    "UPDATE tblNJAWService SET SmartGrowth = null where ISNULL(SmartGrowth, '') in ('','9/6');",
                                UPDATE_STORED_PROCEDURES = @"
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
	    CASE WHEN isNull(S.SmartGrowth,'''') = ''1'' THEN ''X'' ELSE '''' END as ''SmartGrowth'',
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
END;
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RptServicesRenewed]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
                                    /****** Object:  StoredProcedure [dbo].[RptServicesRenewed]    Script Date: 05/31/2011 11:16:45 ******/
                                    ALTER PROCEDURE [RptServicesRenewed] (@startDate datetime, @endDate dateTime, @opCntr varchar(4), @devdriv varchar(10))
                                    AS
                                    IF (Len(@opCntr) > 0)
	                                    BEGIN
		                                    select 
												OC.OperatingCenterCode as OpCntr,
												Towns.town, 
												Towns.TownID as RecID, 
												SC.Description as CatOfService, 
												isNull(SS.SizeServ,'''') as SizeOfService, 
												Sum(isNull(LengthService ,0)) as ''LengthService'', 
												Count(SizeOfService) as ''Total'', 
												Count(case when (smartgrowth=1) then 1 else null end) as SmartGrowth
			                                from 
												tblNJAWService
											LEFT JOIN
												tblNJAWSizeServ SS ON SS.RecID = tblNJAWService.SizeOfService
			                                LEFT JOIN 
												Towns on Towns.TownID = tblNJAWservice.town
			                                LEFT JOIN 
												OperatingCenters OC on OC.OperatingCenterID = tblNJAWService.OpCntr
			                                LEFT JOIN 
												ServiceCategories SC ON SC.ServiceCategoryID = tblNJAWService.CatofService
		                                    where 
			                                    (charindex(''RENEW'', upper(isNull(SC.Description,''''))) > 0 
			                                    OR 
			                                    charindex(''INSTALL METER SET'', upper(isNull(SC.Description,''''))) > 0)
		                                    and 
												OperatingCenterID = @opCntr
		                                    and 
												DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
		                                    and 
												devServD = isNull(@devdriv, devServD)
			                                group by 
												OC.OperatingCenterCode, Towns.town, Towns.TownID, SC.Description, SS.SizeServ
			                                order by 
												OC.OperatingCenterCode, Towns.town, SC.Description, SS.SizeServ	
	                                    END
                                    ELSE
	                                    BEGIN
		                                    select 
												OC.OperatingCenterCode as OpCntr, 
												Towns.town, 
												Towns.TownID as RecID, 
												SC.Description as CatOfService, 
												isNull(SS.SizeServ,'''') as SizeOfService, 
												Sum(isNull(LengthService ,0)) as ''LengthService'', 
												Count(SizeOfService) as ''Total'', 
												Count(case when (smartgrowth=1) then 1 else null end) as SmartGrowth
			                                from 
												tblNJAWService
											LEFT JOIN
												tblNJAWSizeServ SS ON SS.RecID = tblNJAWService.SizeOfService
			                                LEFT JOIN 
												Towns on Towns.TownID = tblNJAWservice.town
			                                LEFT JOIN 
												OperatingCenters OC on OC.OperatingCenterID = tblNJAWService.OpCntr
			                                LEFT JOIN 
												ServiceCategories SC ON SC.ServiceCategoryID = tblNJAWService.CatofService
			                                where
												(charindex(''RENEW'', upper(isNull(SC.Description,''''))) > 0 
													OR 
													charindex(''INSTALL METER SET'', upper(isNull(SC.Description,''''))) > 0)
			                                and 
												DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
			                                and 
												devServD = isNull(@devdriv, devServD)
			                                group by 
												OC.OperatingCenterCode, Towns.town, Towns.TownID, SC.Description, SS.SizeServ
			                                order by 
												OC.OperatingCenterCode, Towns.town, SC.Description, SS.SizeServ
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
		                                                                    select 
												                                OperatingCenterCode as OpCntr, 
												                                Towns.town, 
												                                SC.Description as CatOfService, 
												                                isNull(SS.SizeServ,0) as SizeOfService, 
												                                LengthService as ''LengthService'', 
												                                ServNum, 
												                                case when (isnull(smartgrowth,0)=1) then ''Yes'' end as [SmartGrowth]
			                                                                from 
												                                tblNJAWService S
			                                                                LEFT JOIN 
												                                Towns on Towns.TownID = S.town
			                                                                LEFT JOIN 
												                                OperatingCenters OC on OC.OperatingCenterID = S.OpCntr
			                                                                LEFT JOIN 
												                                ServiceCategories SC on SC.ServiceCategoryID = S.CatOfService
			                                                                LEFT JOIN 
												                                tblNJAWSizeServ SS ON SS.RecID = S.SizeOfService
			                                                                where 
												                                (charindex(''INSTALLATION'', SC.Description) > 0 OR charindex('' NEW'', SC.Description) > 0)
				                                                            and 
												                                oc.OperatingCenterID = @opCntr
				                                                            and 
												                                DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
				                                                            and 
												                                devServD = isNull(@devdriv, devServD)
			                                                                order by 
												                                OpCntr, Towns.town, SC.Description, SS.SizeServ	
	                                                                    END
                                                                    ELSE
	                                                                    BEGIN
		                                                                    select 
												                                OperatingCenterCode as OpCntr, 
												                                Towns.town, 
												                                SC.Description as CatOfService, 
												                                isNull(SS.SizeServ,0) as SizeOfService, 
												                                LengthService as ''LengthService'', 
												                                ServNum, 
												                                case when (isnull(smartgrowth,0)=1) then ''Yes'' end as [SmartGrowth] 
			                                                                from 
												                                tblNJAWService S
		                                                                    LEFT JOIN 
												                                Towns on Towns.TownID = S.town
		                                                                    LEFT JOIN 
												                                OperatingCenters OC on OC.OperatingCenterID = S.OpCntr
		                                                                    LEFT JOIN 
												                                ServiceCategories SC on SC.ServiceCategoryID = S.CatOfService
		                                                                    LEFT JOIN 
												                                tblNJAWSizeServ SS ON SS.RecID = S.SizeOfService
			                                                                where 
												                                (charindex(''INSTALLATION'', SC.Description) > 0 OR charindex('' NEW'', SC.Description) > 0)
				                                                            and 
												                                DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
				                                                            and 
												                                devServD = isNull(@devdriv, devServD)
			                                                                order by 
			                                                                    OpCntr, Towns.town, SC.Description, SS.SizeServ
	                                                                    END
                                                                    ' 
END;

";

            public const string ROLLBACK_AGREEMENT =
                                    "UPDATE tblNJAWService SET Agreement = 'YES' where Agreement = '1';" +
                                    "UPDATE tblNJAWService SET Agreement = 'NO'  where Agreement = '0';",
                                ROLLBACK_BSDM_PERMIT =
                                    "UPDATE tblNJAWService SET BSDWPermit = 'YES' where BSDWPermit = '1';" +
                                    "UPDATE tblNJAWService SET BSDWPermit = 'NO'  where BSDWPermit = '0';",
                                ROLLBACK_DEV_SERV_D =
                                    "UPDATE tblNJAWService SET DevServD = 'YES' where DevServD = '1';" +
                                    "UPDATE tblNJAWService SET DevServD = 'NO'  where DevServD = '0';",
                                ROLLBACK_METER_SET_REQUIRED =
                                    "UPDATE tblNJAWService SET MeterSetReq = 'YES' where MeterSetReq = '1';" +
                                    "UPDATE tblNJAWService SET MeterSetReq = 'NO'  where MeterSetReq = '0';",
                                ROLLBACK_SMART_GROWTH =
                                    "UPDATE tblNJAWService SET SmartGrowth = 'YES' where SmartGrowth = '1';" +
                                    "UPDATE tblNJAWService SET SmartGrowth = 'NO'  where SmartGrowth = '0';";

            public const string REMOVE_CONSTRAINTS =
                "IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_Agreement]') AND type = 'D')" +
                "   ALTER TABLE [tblNJAWService] DROP CONSTRAINT [DF_tblNJAWService_Agreement];" +
                "IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_BSDWPermit]') AND type = 'D')" +
                "   ALTER TABLE [tblNJAWService] DROP CONSTRAINT [DF_tblNJAWService_BSDWPermit];" +
                "IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_DevServD]') AND type = 'D')" +
                "   ALTER TABLE [tblNJAWService] DROP CONSTRAINT [DF_tblNJAWService_DevServD];" +
                "IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_MeterSetReq]') AND type = 'D')" +
                "   ALTER TABLE [tblNJAWService] DROP CONSTRAINT [DF_tblNJAWService_MeterSetReq];" +
                "IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_SmartGrowth]') AND type = 'D')" +
                "   ALTER TABLE [tblNJAWService] DROP CONSTRAINT [DF_tblNJAWService_SmartGrowth];" +
                "if  EXISTS (SELECT * FROM sys.stats WHERE name = N'STAT_tblNJAWService_1933965966_56_48_47_18' AND object_id = object_id(N'[tblNJAWService]'))" +
                "   DROP STATISTICS [dbo].[tblNJAWService].[STAT_tblNJAWService_1933965966_56_48_47_18];" +
                "IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'IDX_CatofService_OpCntr_RecID')" +
                "   DROP INDEX [IDX_CatofService_OpCntr_RecID] ON [tblNJAWService] WITH ( ONLINE = OFF );";

            public const string RESTORE_CONSTRAINTS =
                "IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_Agreement]') AND type = 'D') " +
                "   ALTER TABLE [tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_Agreement]  DEFAULT ('') FOR [Agreement];" +
                "IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_BSDWPermit]') AND type = 'D')" +
                "   ALTER TABLE [tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_BSDWPermit]  DEFAULT ('') FOR [BSDWPermit];" +
                "IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_DevServD]') AND type = 'D')" +
                "   ALTER TABLE [tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_DevServD]  DEFAULT ('') FOR [DevServD];" +
                "IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_MeterSetReq]') AND type = 'D')" +
                "   ALTER TABLE [tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_MeterSetReq]  DEFAULT ('') FOR [MeterSetReq];" +
                "IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tblNJAWService_SmartGrowth]') AND type = 'D')" +
                "   ALTER TABLE [tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_SmartGrowth]  DEFAULT ('') FOR [SmartGrowth];" +
                "IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tblNJAWService]') AND name = N'IDX_CatofService_OpCntr_RecID')" +
                "   CREATE NONCLUSTERED INDEX [IDX_CatofService_OpCntr_RecID] ON [tblNJAWService] " +
                "   ([CatofService] ASC,[OpCntr] ASC,[RecID] ASC,[SmartGrowth] ASC) " +
                "   WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)";

            public const string RESTORE_STORED_PROCEDURES = @"
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
	                                    CASE WHEN isNull(S.SmartGrowth,'''') = ''1'' THEN ''X'' ELSE '''' END as ''SmartGrowth'',
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
END;
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RptServicesRenewed]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
                                    /****** Object:  StoredProcedure [dbo].[RptServicesRenewed]    Script Date: 05/31/2011 11:16:45 ******/
                                    ALTER PROCEDURE [RptServicesRenewed] (@startDate datetime, @endDate dateTime, @opCntr varchar(4), @devdriv varchar(10))
                                    AS
                                    IF (Len(@opCntr) > 0)
	                                    BEGIN
		                                    select 
												OC.OperatingCenterCode as OpCntr,
												Towns.town, 
												Towns.TownID as RecID, 
												SC.Description as CatOfService, 
												isNull(SS.SizeServ,'''') as SizeOfService, 
												Sum(isNull(LengthService ,0)) as ''LengthService'', 
												Count(SizeOfService) as ''Total'', 
												Count(case when (smartgrowth=''Yes'') then 1 else null end) as SmartGrowth
			                                from 
												tblNJAWService
											LEFT JOIN
												tblNJAWSizeServ SS ON SS.RecID = tblNJAWService.SizeOfService
			                                LEFT JOIN 
												Towns on Towns.TownID = tblNJAWservice.town
			                                LEFT JOIN 
												OperatingCenters OC on OC.OperatingCenterID = tblNJAWService.OpCntr
			                                LEFT JOIN 
												ServiceCategories SC ON SC.ServiceCategoryID = tblNJAWService.CatofService
		                                    where 
			                                    (charindex(''RENEW'', upper(isNull(SC.Description,''''))) > 0 
			                                    OR 
			                                    charindex(''INSTALL METER SET'', upper(isNull(SC.Description,''''))) > 0)
		                                    and 
												OperatingCenterID = @opCntr
		                                    and 
												DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
		                                    and 
												devServD = isNull(@devdriv, devServD)
			                                group by 
												OC.OperatingCenterCode, Towns.town, Towns.TownID, SC.Description, SS.SizeServ
			                                order by 
												OC.OperatingCenterCode, Towns.town, SC.Description, SS.SizeServ	
	                                    END
                                    ELSE
	                                    BEGIN
		                                    select 
												OC.OperatingCenterCode as OpCntr, 
												Towns.town, 
												Towns.TownID as RecID, 
												SC.Description as CatOfService, 
												isNull(SS.SizeServ,'''') as SizeOfService, 
												Sum(isNull(LengthService ,0)) as ''LengthService'', 
												Count(SizeOfService) as ''Total'', 
												Count(case when (smartgrowth=''Yes'') then 1 else null end) as SmartGrowth
			                                from 
												tblNJAWService
											LEFT JOIN
												tblNJAWSizeServ SS ON SS.RecID = tblNJAWService.SizeOfService
			                                LEFT JOIN 
												Towns on Towns.TownID = tblNJAWservice.town
			                                LEFT JOIN 
												OperatingCenters OC on OC.OperatingCenterID = tblNJAWService.OpCntr
			                                LEFT JOIN 
												ServiceCategories SC ON SC.ServiceCategoryID = tblNJAWService.CatofService
			                                where
												(charindex(''RENEW'', upper(isNull(SC.Description,''''))) > 0 
													OR 
													charindex(''INSTALL METER SET'', upper(isNull(SC.Description,''''))) > 0)
			                                and 
												DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
			                                and 
												devServD = isNull(@devdriv, devServD)
			                                group by 
												OC.OperatingCenterCode, Towns.town, Towns.TownID, SC.Description, SS.SizeServ
			                                order by 
												OC.OperatingCenterCode, Towns.town, SC.Description, SS.SizeServ
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
		                                                                    select 
												                                OperatingCenterCode as OpCntr, 
												                                Towns.town, 
												                                SC.Description as CatOfService, 
												                                isNull(SS.SizeServ,0) as SizeOfService, 
												                                LengthService as ''LengthService'', 
												                                ServNum, 
												                                case when (isnull(smartgrowth,'''')=''YES'') then ''Yes'' end as [SmartGrowth]
			                                                                from 
												                                tblNJAWService S
			                                                                LEFT JOIN 
												                                Towns on Towns.TownID = S.town
			                                                                LEFT JOIN 
												                                OperatingCenters OC on OC.OperatingCenterID = S.OpCntr
			                                                                LEFT JOIN 
												                                ServiceCategories SC on SC.ServiceCategoryID = S.CatOfService
			                                                                LEFT JOIN 
												                                tblNJAWSizeServ SS ON SS.RecID = S.SizeOfService
			                                                                where 
												                                (charindex(''INSTALLATION'', SC.Description) > 0 OR charindex('' NEW'', SC.Description) > 0)
				                                                            and 
												                                oc.OperatingCenterID = @opCntr
				                                                            and 
												                                DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
				                                                            and 
												                                devServD = isNull(@devdriv, devServD)
			                                                                order by 
												                                OpCntr, Towns.town, SC.Description, SS.SizeServ	
	                                                                    END
                                                                    ELSE
	                                                                    BEGIN
		                                                                    select 
												                                OperatingCenterCode as OpCntr, 
												                                Towns.town, 
												                                SC.Description as CatOfService, 
												                                isNull(SS.SizeServ,0) as SizeOfService, 
												                                LengthService as ''LengthService'', 
												                                ServNum, 
												                                case when (isnull(smartgrowth,'''')=''YES'') then ''Yes'' end as [SmartGrowth] 
			                                                                from 
												                                tblNJAWService S
		                                                                    LEFT JOIN 
												                                Towns on Towns.TownID = S.town
		                                                                    LEFT JOIN 
												                                OperatingCenters OC on OC.OperatingCenterID = S.OpCntr
		                                                                    LEFT JOIN 
												                                ServiceCategories SC on SC.ServiceCategoryID = S.CatOfService
		                                                                    LEFT JOIN 
												                                tblNJAWSizeServ SS ON SS.RecID = S.SizeOfService
			                                                                where 
												                                (charindex(''INSTALLATION'', SC.Description) > 0 OR charindex('' NEW'', SC.Description) > 0)
				                                                            and 
												                                DateInstalled >= @startDate and DateInstalled < DateAdd(D, 1, @endDate)
				                                                            and 
												                                devServD = isNull(@devdriv, devServD)
			                                                                order by 
			                                                                    OpCntr, Towns.town, SC.Description, SS.SizeServ
	                                                                    END
                                                                    ' 
END;";
        }

        public struct Tables
        {
            public const string SERVICES = "tblNJAWService";
        }

        public struct Columns
        {
            public const string AGREEMENT = "Agreement",
                                BSDM_PERMIT = "BSDWPermit",
                                DEV_SERV_D = "DevServD",
                                METER_SET_REQUIREMENT = "MeterSetReq",
                                SMART_GROWTH = "SmartGrowth";
        }

        public struct StringLengths
        {
            public const int AGREEMENT = 3,
                             BSDM_PERMIT = 3,
                             DEV_SERV_D = 3,
                             METER_SET_REQUIREMENT = 3,
                             SMART_GROWTH = 3;
        }

        #endregion

        public override void Up()
        {
            Execute.Sql(Sql.REMOVE_CONSTRAINTS);

            Execute.Sql(Sql.UPDATE_AGREEMENT);
            Execute.Sql(Sql.UPDATE_BSDM_PERMIT);
            Execute.Sql(Sql.UPDATE_DEV_SERV_D);
            Execute.Sql(Sql.UPDATE_METER_SET_REQUIRED);
            Execute.Sql(Sql.UPDATE_SMART_GROWTH);

            Alter.Table(Tables.SERVICES).AlterColumn(Columns.AGREEMENT).AsBoolean().Nullable();
            Alter.Table(Tables.SERVICES).AlterColumn(Columns.BSDM_PERMIT).AsBoolean().Nullable();
            Alter.Table(Tables.SERVICES).AlterColumn(Columns.DEV_SERV_D).AsBoolean().Nullable();
            Alter.Table(Tables.SERVICES).AlterColumn(Columns.METER_SET_REQUIREMENT).AsBoolean().Nullable();
            Alter.Table(Tables.SERVICES).AlterColumn(Columns.SMART_GROWTH).AsBoolean().Nullable();

            Execute.Sql(Sql.UPDATE_STORED_PROCEDURES);
        }

        public override void Down()
        {
            Alter.Table(Tables.SERVICES).AlterColumn(Columns.SMART_GROWTH).AsAnsiString(StringLengths.SMART_GROWTH)
                 .Nullable();
            Alter.Table(Tables.SERVICES).AlterColumn(Columns.METER_SET_REQUIREMENT)
                 .AsAnsiString(StringLengths.METER_SET_REQUIREMENT).Nullable();
            Alter.Table(Tables.SERVICES).AlterColumn(Columns.DEV_SERV_D).AsAnsiString(StringLengths.DEV_SERV_D)
                 .Nullable();
            Alter.Table(Tables.SERVICES).AlterColumn(Columns.BSDM_PERMIT).AsAnsiString(StringLengths.BSDM_PERMIT)
                 .Nullable();
            Alter.Table(Tables.SERVICES).AlterColumn(Columns.AGREEMENT).AsAnsiString(StringLengths.AGREEMENT)
                 .Nullable();

            Execute.Sql(Sql.ROLLBACK_SMART_GROWTH);
            Execute.Sql(Sql.ROLLBACK_METER_SET_REQUIRED);
            Execute.Sql(Sql.ROLLBACK_DEV_SERV_D);
            Execute.Sql(Sql.ROLLBACK_BSDM_PERMIT);
            Execute.Sql(Sql.ROLLBACK_AGREEMENT);

            Execute.Sql(Sql.RESTORE_CONSTRAINTS);
            Execute.Sql(Sql.RESTORE_STORED_PROCEDURES);
        }
    }
}
