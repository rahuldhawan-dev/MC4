using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130718104117), Tags("Production")]
    public class UpdateServiceRelatedStoredProcedures : Migration
    {
        // RptServicesRenewed----
        // RptServicesInstalledDetail
        // RptServicesRenewedRetirementsDetail---

        #region Constants

        public struct Sql
        {
            public const string ROLLBACK_STORED_PROCEDURES = @"
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
												    isNull(SizeOfService,0) as SizeOfService, 
												    Sum(isNull(LengthService ,0)) as ''LengthService'', 
												    Count(SizeOfService) as ''Total'', 
												    Count(case when (smartgrowth=''Yes'') then 1 else null end) as SmartGrowth
			                                    from 
												    tblNJAWService
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
												    OC.OperatingCenterCode, Towns.town, Towns.TownID, SC.Description, SizeofService
			                                    order by 
												    OC.OperatingCenterCode, Towns.town, SC.Description, SizeofService	
	                                        END
                                        ELSE
	                                        BEGIN
		                                        select 
												    OC.OperatingCenterCode as OpCntr, 
												    Towns.town, 
												    Towns.TownID as RecID, 
												    SC.Description as CatOfService, 
												    isNull(SizeOfService,0) as SizeOfService, 
												    Sum(isNull(LengthService ,0)) as ''LengthService'', 
												    Count(SizeOfService) as ''Total'', 
												    Count(case when (smartgrowth=''Yes'') then 1 else null end) as SmartGrowth
			                                    from 
												    tblNJAWService
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
												    OC.OperatingCenterCode, Towns.town, Towns.TownID, SC.Description, SizeofService
			                                    order by 
												    OC.OperatingCenterCode, Towns.town, SC.Description, SizeofService
	                                        END' 
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
                                    END",
                                UPDATE_STORED_PROCEDURES =
                                    @"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RptServicesRenewed]') AND type in (N'P', N'PC'))
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
										                                tblNJAWSizeServ SS on SS.RecID = tblNJAWService.SizeOfService
                                                                    left join
                                                                        ServiceMaterials SM on SM.ServiceMaterialID = tblNJAWService.PrevServiceMatl
                                                                    where 
                                                                    --year(OrigInstDate) > 1900
                                                                    --and 
                                                                    DateInstalled >= @startDate and DateInstalled <= @endDate
                                                                    and 
                                                                    SC.Description = @CatOfService
                                                                    and 
                                                                    SS.SizeServ = @SizeOfService
                                                                    and 
                                                                    Town = @Town' 
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
                                END
";
        }

        #endregion

        public override void Up()
        {
            Execute.Sql(Sql.UPDATE_STORED_PROCEDURES);
        }

        public override void Down()
        {
            Execute.Sql(Sql.ROLLBACK_STORED_PROCEDURES);
        }
    }
}
