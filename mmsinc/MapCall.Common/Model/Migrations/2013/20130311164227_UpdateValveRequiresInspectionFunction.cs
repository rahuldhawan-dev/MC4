using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130311164227), Tags("Production")]
    public class UpdateValveRequiresInspectionFunction : Migration
    {
        public struct Sql
        {
            public const string ORIGINAL_VIEW = @"ALTER VIEW 
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
	dbo.RequiresInspectionValveByZone(ValveSize, ValCtrl, ValveStatus, BPUKPI, BillInfo, (Select max(dateInspect) from tblNJAWValInspData where UPPER(isNull(Operated,'NO')) = 'YES' AND tblNJAWValInspData.ValNum = tblNJAWValves.ValNum and tblNJAWValInspData.OpCntr = tblNJAWValves.opCntr) , DateInst, GetDate(), ValveZone) = 1 
GROUP BY
	OpCntr, tblNJAWValves.town, case when (valvesize>=12.0) then '>=12' else '<12' end
GO
",
                                NEW_VIEW = @"
ALTER VIEW 
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
",
                                ORIGINAL_FUNCTION = @"
/****** Object:  UserDefinedFunction [dbo].[RequiresInspectionValveByZone]    Script Date: 03/12/2013 09:24:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RequiresInspectionValveByZone]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
ALTER FUNCTION	[dbo].[RequiresInspectionValveByZone] (
	@ValveSize float, 
	@ValCtrl varchar(25), 
	@ValveStatus varchar(10), 
	@bpuKPI varchar(2),
	@billinfo varchar(16),
	@lastInspectionDate datetime, 
	@DateInst datetime,
	@Date datetime,
	@ValveZone int
)
RETURNS INT	
AS

/* Rules buried in sql code */
/* TODO: Move to tested C# code */
/*
   | Small Zone | Large Zone | Year  |
   |          1 |          5 | 2011  |
   |          2 |          6 | 2012  |
   |          3 |          5 | 2013  |
   |          4 |          6 | 2014  | Repeat
   |		  7 |		     | Annual|
*/
BEGIN

Declare @seedYear int
Declare @ZoneRequired int
select @seedYear = 2011

if (@ValveZone <= 4) -- small
	begin
		if ((abs(@seedYear - year(@date)) % 4 ) + 1 = @ValveZone) 
			select @ZoneRequired = 1 
		else 
			select @ZoneRequired = 0
	end
if (@ValveZone > 4) -- large
	begin
		if ((abs(@seedYear - year(@date)) % 2) + 5 = @ValveZone)
			select @ZoneRequired = 1
		else
			select @ZoneRequired = 0
	end
if (@ValveZone = 7)
	begin
		select @ZoneRequired = 1
	end
	
	-- LETS CLEAN UP OUR VARIABLES FOR NULL SINCE WE''RE CHECKING FOR <>
	SELECT @ValveSize = isNull(@ValveSize, '''')
	SELECT @ValCtrl = isNull(@ValCtrl, '''')
	SELECT @ValveStatus = isNull(@ValveStatus, '''')
	SELECT @bpuKPI = isNull(@bpuKPI, '''')
	SELECT @lastInspectionDate = isNull(@lastInspectionDate, ''01/01/1900'')
	SELECT @DateInst = isNull(@DateInst, ''01/01/1900'')
	SELECT @billinfo = isNull(@billinfo, '''')
	DECLARE @Result INT
	
/*
	-- WE START OUT THINKING THE VALVE REQUIRES INSPECTION BELOW ARE THE ERROR CODES
		-1 - NOT ACTIVE
		-2 - IS BPUKPI
		-3 - IT''S A BLOW OFF WITH FLUSHING
		-4 - IT DIDN''T EXIST IN THE YEAR WE''RE CHECKING
		-5 - VALVE IS LESS THAN 2in AND IS A CNTRLS A BLOW OFF
		-6 - ZONE IS NOT REQUIRED FOR INSPECTION
		-7 - ALREADY INSPECTED FOR THE YEAR
		-10 - NOT PUBLIC
*/
	SELECT @Result = 1   

	-- IF ANY CONDITIONS ARE MET, THEN THE VALVE DOESN''T REQUIRE INSPECTION
	
/* -1 - NOT ACTIVE */ 
	IF (UPPER(@ValveStatus) <> ''ACTIVE'')									BEGIN RETURN -1 END

/* -10 - NOT PUBLIC */
	IF (UPPER(@billinfo) <> ''PUBLIC'')
		BEGIN IF(UPPER(@billinfo) <> '''')									BEGIN RETURN -10 END END

/* -2 - IS BPUKPI */
	IF (UPPER(@bpuKPI) <> '''')												BEGIN RETURN -2 END	

/* -3 - IT''S A BLOW OFF WITH FLUSHING */
	IF (UPPER(@ValCtrl) = ''BLOW OFF WITH FLUSHING'')							BEGIN RETURN -3 END

/* -4 - IT DIDN''T EXIST IN THE YEAR WE''RE CHECKING */
	IF (@DateInst > @Date)													BEGIN RETURN -4 END

/* -5 - VALVE IS LESS THAN 2in AND IS A CNTRLS A BLOW OFF */
	IF (@ValveSize < 2 AND UPPER(@ValCtrl) = ''BLOW OFF'')					BEGIN RETURN -5 END

/* -6 - ZONE IS NOT REQUIRED FOR INSPECTION */
	IF (@ZoneRequired = 0)													BEGIN RETURN -6 END

/* -7 - ALREADY INSPECTED FOR THE YEAR */ 
	IF (Year(@lastInspectionDate) >= Year(@date))							BEGIN RETURN -7 END
	
/*
	IF (Upper(@InspFreqUnit) = ''Y'')
		BEGIN
			--If the difference between the year of the last inspection date 
			--and the current date is greater than or = to the yearly frequency provided
			IF (@DateInst>@lastInspectionDate)
				BEGIN
					IF (DateDiff(YY, @DateInst, @Date) < Cast(@InspFreq as Int))
						BEGIN RETURN -6 END
				END
			ELSE
				BEGIN
					IF (DateDiff(YY, @lastInspectionDate, @Date) < Cast(@InspFreq as Int))
						BEGIN RETURN -7 END
				END
		END
	
*/
	Return @Result

END

' 
END

GO

",
                                NEW_FUNCTION = @"ALTER FUNCTION	[dbo].[RequiresInspectionValveByZone] (
	@ValveSize float, 
	@ValCtrl varchar(25), 
	@ValveStatus varchar(10), 
	@bpuKPI varchar(2),
	@billinfo varchar(16),
	@lastInspectionDate datetime, 
	@DateInst datetime,
	@Date datetime,
	@ValveZone int, 
	@Town int
)
RETURNS INT	
AS

/* Rules buried in sql code */
/* TODO: Move to tested C# code */
/*
   | Small Zone | Large Zone | Year  |
   |          1 |          5 | 2011  |
   |          2 |          6 | 2012  |
   |          3 |          5 | 2013  |
   |          4 |          6 | 2014  | Repeat
   |		  7 |		     | Annual|
*/
BEGIN

Declare @seedYear int
Declare @ZoneRequired int
select @seedYear = 2011

if (@ValveZone <= 4) -- small
	begin
		if ((abs(@seedYear - year(@date)) % 4 ) + 1 = @ValveZone) 
			select @ZoneRequired = 1 
		else 
			select @ZoneRequired = 0
	end
if (@ValveZone > 4) -- large
	begin
		if ((abs(@seedYear - year(@date)) % 2) + 5 = @ValveZone)
			select @ZoneRequired = 1
		else
			select @ZoneRequired = 0
	end
if (@ValveZone = 7)
	begin
		select @ZoneRequired = 1
	end
	
	-- LETS CLEAN UP OUR VARIABLES FOR NULL SINCE WE'RE CHECKING FOR <>
	SELECT @ValveSize = isNull(@ValveSize, '')
	SELECT @ValCtrl = isNull(@ValCtrl, '')
	SELECT @ValveStatus = isNull(@ValveStatus, '')
	SELECT @bpuKPI = isNull(@bpuKPI, '')
	SELECT @lastInspectionDate = isNull(@lastInspectionDate, '01/01/1900')
	SELECT @DateInst = isNull(@DateInst, '01/01/1900')
	SELECT @billinfo = isNull(@billinfo, '')
	DECLARE @Result INT
	
/*
	-- WE START OUT THINKING THE VALVE REQUIRES INSPECTION BELOW ARE THE ERROR CODES
		-1 - NOT ACTIVE
		-2 - IS BPUKPI
		-3 - IT'S A BLOW OFF WITH FLUSHING
		-4 - IT DIDN'T EXIST IN THE YEAR WE'RE CHECKING
		-5 - VALVE IS LESS THAN 2in AND IS A CNTRLS A BLOW OFF
		-6 - ZONE IS NOT REQUIRED FOR INSPECTION
		-7 - ALREADY INSPECTED FOR THE YEAR
		-10 - NOT PUBLIC
*/
	SELECT @Result = 1   

	-- IF ANY CONDITIONS ARE MET, THEN THE VALVE DOESN'T REQUIRE INSPECTION
	
/* -1 - NOT ACTIVE */ 
	IF (UPPER(@ValveStatus) <> 'ACTIVE')									BEGIN RETURN -1 END

/* -10 - NOT PUBLIC */
	IF (UPPER(@billinfo) <> 'PUBLIC')
		BEGIN IF(UPPER(@billinfo) <> '')									BEGIN RETURN -10 END END

/* -2 - IS BPUKPI */
	IF (UPPER(@bpuKPI) <> '')												BEGIN RETURN -2 END	

/* -3 - IT'S A BLOW OFF WITH FLUSHING */
	IF (UPPER(@ValCtrl) = 'BLOW OFF WITH FLUSHING')							BEGIN RETURN -3 END

/* -4 - IT DIDN'T EXIST IN THE YEAR WE'RE CHECKING */
	IF (@DateInst > @Date)													BEGIN RETURN -4 END

/* -5 - VALVE IS LESS THAN 2in AND IS A CNTRLS A BLOW OFF */
	IF (@ValveSize < 2 AND UPPER(@ValCtrl) = 'BLOW OFF')					BEGIN RETURN -5 END

/* 1 - SPECIAL SANDY RELIEF RULE */
	IF (@Town IN (192, 197, 193, 194, 196, 280) AND @lastInspectionDate < cast('03/12/2013' as DateTime))
																			BEGIN RETURN 1 END
/* -6 - ZONE IS NOT REQUIRED FOR INSPECTION */
	IF (@ZoneRequired = 0)													BEGIN RETURN -6 END

/* -7 - ALREADY INSPECTED FOR THE YEAR */ 
	IF (Year(@lastInspectionDate) >= Year(@date))							BEGIN RETURN -7 END

	Return @Result

END
";
        }

        public override void Up()
        {
            Execute.Sql(Sql.NEW_FUNCTION);
            Execute.Sql(Sql.NEW_VIEW);
        }

        public override void Down()
        {
            Execute.Sql(Sql.ORIGINAL_FUNCTION);
            Execute.Sql(Sql.ORIGINAL_VIEW);
            Execute.Sql(UpdateValveInspectionStoredProcedures.Sql.ORIGINAL_SP_1);
            Execute.Sql(UpdateValveInspectionStoredProcedures.Sql.ORIGINAL_SP_2);
            Execute.Sql(UpdateViewsForInspectionsByValveZone.Sql.ORIGINAL_VIEW);
        }
    }
}
