using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130313093024), Tags("Production")]
    public class UpdateValveInspectionStoredProcedures : Migration
    {
        public struct Sql
        {
            public const string
                ORIGINAL_SP_1 = @"
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RptValveInspectionsReqOperated]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

/* [RptValveInspectionsReqOperated] */

ALTER PROCEDURE [dbo].[RptValveInspectionsReqOperated] @opCntr varchar(3), @year int
AS
--GRANT ALL ON [dbo].[RptValveInspectionsReqOperated] TO MCUser
--exec [RptValveInspectionsOperated] ''NJ7'', 2007
/*
Declare @year int
Declare @opCntr varchar(3)
Select @year = 2007
Select @opCntr = ''NJ7''
*/
DECLARE @tblTable TABLE(Size varchar(16), Operated varchar(3), Total int, MonthNum int, [Month] varchar(10), [Year] int)
DECLARE @tblValvesToBeInspected TABLE (Size varchar(16), ValNum varchar(15), opcntr varchar(10), recID int)
DECLARE @LowCount INT
DECLARE @HighCount INT

--PLACE ALL THE VALVES TO BE INSPECTED IN TEMPORARY TABLE
Insert Into @tblValvesToBeInspected
	SELECT 
		CASE WHEN (Cast(ValveSize as float) >= 12.0) THEN ''>= 12""'' ELSE ''<12""'' END, ValNum, OpCntr, RecID
		FROM tblnjawValves 
			where OpCntr = @opCntr
			AND 
				DBO.RequiresInspectionValveByZone( ValveSize, ValCtrl, ValveStatus, BPUKPI, BillInfo, 
				(Select max(dateInspect) from tblNJAWValInspData where UPPER(isNull(Operated,''NO'')) = ''YES'' AND tblNJAWValInspData.ValNum = tblNJAWValves.ValNum and tblNJAWValInspData.OpCntr = tblNJAWValves.opCntr and year(dateInspect) < @year)
				, DateInst, ''12/31/'' + Cast(@year as varchar(4)), ValveZone
				) = 1
			ORDER BY 1

Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  1),1, ''Jan'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  2),2, ''Feb'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  3),3, ''Mar'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  4),4, ''Apr'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  5),5, ''May'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  6),6, ''Jun'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  7),7, ''Jul'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  8),8, ''Aug'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  9),9, ''Sep'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  10),10, ''Oct'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  11),11, ''Nov'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  12),12, ''Dec'', @Year

Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  1),1, ''Jan'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  2),2, ''Feb'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  3),3, ''Mar'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  4),4, ''Apr'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  5),5, ''May'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  6),6, ''Jun'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  7),7, ''Jul'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  8),8, ''Aug'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  9),9, ''Sep'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  10),10, ''Oct'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  11),11, ''Nov'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  12),12, ''Dec'', @Year

Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 1),1, ''Jan'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 2),2, ''Feb'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 3),3, ''Mar'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 4),4, ''Apr'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 5),5, ''May'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 6),6, ''Jun'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 7),7, ''Jul'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 8),8, ''Aug'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 9),9, ''Sep'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 10),10, ''Oct'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 11),11, ''Nov'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 12),12, ''Dec'', @Year

Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 1),1, ''Jan'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 2),2, ''Feb'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 3),3, ''Mar'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 4),4, ''Apr'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 5),5, ''May'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 6),6, ''Jun'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 7),7, ''Jul'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 8),8, ''Aug'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 9),9, ''Sep'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 10),10, ''Oct'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 11),11, ''Nov'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 12),12, ''Dec'', @Year

SELECT * from @tblTable
' 
END
GO",
                ORIGINAL_SP_2 = @"
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RptValveInspections]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [dbo].[RptValveInspections] @opCntr varchar(3), @year int
AS
--Declare @year int
--Declare @opCntr varchar(3)
--Select @year = 2011
--Select @opCntr = ''EW1''

DECLARE @tblValvesToBeInspected TABLE (Size varchar(16), ValNum varchar(15), opcntr varchar(10), valveID int)
DECLARE @LowCount varchar(55)
DECLARE @HighCount varchar(55)
DECLARE @LowTotalValves varchar(55)
DECLARE @HighTotalValves varchar(55)

--PLACE ALL THE VALVES TO BE INSPECTED IN TEMPORARY TABLE
Insert Into @tblValvesToBeInspected
	SELECT 
		CASE WHEN (Cast(ValveSize as float) >= 12.0) THEN ''>= 12""'' ELSE ''<12""'' END, ValNum, OpCntr, RecID
		FROM tblnjawValves 
			where OpCntr = @opCntr
			AND 
				DBO.RequiresInspectionValveByZone( ValveSize, ValCtrl, ValveStatus, BPUKPI, BillInfo, 
				(Select max(dateInspect) from tblNJAWValInspData where UPPER(isNull(Operated,''NO'')) = ''YES'' AND tblNJAWValInspData.valveID = tblNJAWValves.recID and year(dateInspect) < @year)
				, DateInst, ''12/31/'' + Cast(@year as varchar(4)), ValveZone
				) = 1
			ORDER BY 1

Select @LowCount = (Select count(1) from @tblValvesToBeInspected where size = ''<12""'')
Select @HighCount = (Select count(1) from @tblValvesToBeInspected where size = ''>= 12""'')
Select @LowTotalValves = (Select count(1) from tblNJAWValves where opCntr = @opCntr AND Cast(IsNull(ValveSize,0) as float) < 12.0 and upper(isNull(ValveStatus,'''')) = ''ACTIVE'' AND isNull(Valctrl,'''') <> ''BLOW OFF WITH FLUSHING'' AND not (valctrl = ''BLOW OFF'' and cast(valvesize as float) < 2.0))
Select @HighTotalValves = (Select count(1) from tblNJAWValves where opCntr = @opCntr AND Cast(IsNull(ValveSize,0) as float) >= 12.0 and upper(isNull(ValveStatus,'''')) = ''ACTIVE''  AND isNull(Valctrl,'''') <> ''BLOW OFF WITH FLUSHING'')

DECLARE @table TABLE(Size varchar(16), valveID int, [month] int)

/* 
	we don''t want to count multiple inspections twice 
	store the latest in a temp table
*/
insert into @table
select 
	Distinct
	CASE WHEN (Cast(V.ValveSize as float) >= 12.0) THEN ''>= 12""'' ELSE ''<12""'' END, 
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
	UPPER(isNull(I.Operated,''NO'')) = ''YES'' 
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
	left(datename(month, cast([month] as varchar(2)) + ''/01/2000''),3) as [Month],
	@Year as [Year],
	CASE WHEN Size = ''>= 12""'' THEN @HighCount ELSE @LowCount END as reqinsp,
	CASE WHEN Size = ''>= 12""'' THEN @HighTotalValves ELSE @LowTotalValves END as totalValves
from 
	@table
group by
	Size, [month]
order by
	Size, [Month]
' 
END
GO",
                NEW_SP_1 = @"
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RptValveInspectionsReqOperated]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

/* [RptValveInspectionsReqOperated] */

ALTER PROCEDURE [dbo].[RptValveInspectionsReqOperated] @opCntr varchar(3), @year int
AS
--GRANT ALL ON [dbo].[RptValveInspectionsReqOperated] TO MCUser
--exec [RptValveInspectionsOperated] ''NJ7'', 2007
/*
Declare @year int
Declare @opCntr varchar(3)
Select @year = 2007
Select @opCntr = ''NJ7''
*/
DECLARE @tblTable TABLE(Size varchar(16), Operated varchar(3), Total int, MonthNum int, [Month] varchar(10), [Year] int)
DECLARE @tblValvesToBeInspected TABLE (Size varchar(16), ValNum varchar(15), opcntr varchar(10), recID int)
DECLARE @LowCount INT
DECLARE @HighCount INT

--PLACE ALL THE VALVES TO BE INSPECTED IN TEMPORARY TABLE
Insert Into @tblValvesToBeInspected
	SELECT 
		CASE WHEN (Cast(ValveSize as float) >= 12.0) THEN ''>= 12""'' ELSE ''<12""'' END, ValNum, OpCntr, RecID
		FROM tblnjawValves 
			where OpCntr = @opCntr
			AND 
				DBO.RequiresInspectionValveByZone( ValveSize, ValCtrl, ValveStatus, BPUKPI, BillInfo, 
				(Select max(dateInspect) from tblNJAWValInspData where UPPER(isNull(Operated,''NO'')) = ''YES'' AND tblNJAWValInspData.ValNum = tblNJAWValves.ValNum and tblNJAWValInspData.OpCntr = tblNJAWValves.opCntr and year(dateInspect) < @year)
				, DateInst, ''12/31/'' + Cast(@year as varchar(4)), ValveZone, tblNJAWValves.Town
				) = 1
			ORDER BY 1

Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  1),1, ''Jan'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  2),2, ''Feb'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  3),3, ''Mar'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  4),4, ''Apr'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  5),5, ''May'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  6),6, ''Jun'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  7),7, ''Jul'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  8),8, ''Aug'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  9),9, ''Sep'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  10),10, ''Oct'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  11),11, ''Nov'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  12),12, ''Dec'', @Year

Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  1),1, ''Jan'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  2),2, ''Feb'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  3),3, ''Mar'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  4),4, ''Apr'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  5),5, ''May'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  6),6, ''Jun'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  7),7, ''Jul'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  8),8, ''Aug'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  9),9, ''Sep'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  10),10, ''Oct'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  11),11, ''Nov'', @Year
Insert Into @tblTable 
	Select ''<12""'' as ''Size'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)< 12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) =  12),12, ''Dec'', @Year

Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 1),1, ''Jan'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 2),2, ''Feb'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 3),3, ''Mar'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 4),4, ''Apr'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 5),5, ''May'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 6),6, ''Jun'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 7),7, ''Jul'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 8),8, ''Aug'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 9),9, ''Sep'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 10),10, ''Oct'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 11),11, ''Nov'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''YES'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''YES'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 12),12, ''Dec'', @Year

Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 1),1, ''Jan'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 2),2, ''Feb'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 3),3, ''Mar'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 4),4, ''Apr'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 5),5, ''May'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 6),6, ''Jun'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 7),7, ''Jul'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 8),8, ''Aug'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 9),9, ''Sep'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 10),10, ''Oct'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 11),11, ''Nov'', @Year
Insert Into @tblTable 
	Select ''>= 12""'', ''NO'', (SELECT count(*) FROM tblNJAWValInspData LEFT JOIN tblNJAWValves on tblNJAWValves.ValNum = tblNJAWValInspData.ValNum and tblNJAWValves.opCntr = tblNJAWValInspData.opCntr WHERE tblNJAWValves.ValNum in (SELECT ValNum from @tblValvesToBeInspected) AND UPPER(isNull(Operated,''NO'')) = ''NO'' AND CAST([ValveSize] AS FLOAT)>=12.0 AND YEAR([DateInspect]) = @Year AND tblNJAWValves.opCntr = @opCntr AND Month(DateInspect) = 12),12, ''Dec'', @Year

SELECT * from @tblTable
' 
END
GO",
                NEW_SP_2 =
                    @"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RptValveInspections]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [dbo].[RptValveInspections] @opCntr varchar(3), @year int
AS
--Declare @year int
--Declare @opCntr varchar(3)
--Select @year = 2011
--Select @opCntr = ''EW1''

DECLARE @tblValvesToBeInspected TABLE (Size varchar(16), ValNum varchar(15), opcntr varchar(10), valveID int)
DECLARE @LowCount varchar(55)
DECLARE @HighCount varchar(55)
DECLARE @LowTotalValves varchar(55)
DECLARE @HighTotalValves varchar(55)

--PLACE ALL THE VALVES TO BE INSPECTED IN TEMPORARY TABLE
Insert Into @tblValvesToBeInspected
	SELECT 
		CASE WHEN (Cast(ValveSize as float) >= 12.0) THEN ''>= 12""'' ELSE ''<12""'' END, ValNum, OpCntr, RecID
		FROM tblnjawValves 
			where OpCntr = @opCntr
			AND 
				DBO.RequiresInspectionValveByZone( ValveSize, ValCtrl, ValveStatus, BPUKPI, BillInfo, 
				(Select max(dateInspect) from tblNJAWValInspData where UPPER(isNull(Operated,''NO'')) = ''YES'' AND tblNJAWValInspData.valveID = tblNJAWValves.recID and year(dateInspect) < @year)
				, DateInst, ''12/31/'' + Cast(@year as varchar(4)), ValveZone, tblNJAWValves.Town
				) = 1
			ORDER BY 1

Select @LowCount = (Select count(1) from @tblValvesToBeInspected where size = ''<12""'')
Select @HighCount = (Select count(1) from @tblValvesToBeInspected where size = ''>= 12""'')
Select @LowTotalValves = (Select count(1) from tblNJAWValves where opCntr = @opCntr AND Cast(IsNull(ValveSize,0) as float) < 12.0 and upper(isNull(ValveStatus,'''')) = ''ACTIVE'' AND isNull(Valctrl,'''') <> ''BLOW OFF WITH FLUSHING'' AND not (valctrl = ''BLOW OFF'' and cast(valvesize as float) < 2.0))
Select @HighTotalValves = (Select count(1) from tblNJAWValves where opCntr = @opCntr AND Cast(IsNull(ValveSize,0) as float) >= 12.0 and upper(isNull(ValveStatus,'''')) = ''ACTIVE''  AND isNull(Valctrl,'''') <> ''BLOW OFF WITH FLUSHING'')

DECLARE @table TABLE(Size varchar(16), valveID int, [month] int)

/* 
	we don''t want to count multiple inspections twice 
	store the latest in a temp table
*/
insert into @table
select 
	Distinct
	CASE WHEN (Cast(V.ValveSize as float) >= 12.0) THEN ''>= 12""'' ELSE ''<12""'' END, 
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
	UPPER(isNull(I.Operated,''NO'')) = ''YES'' 
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
	left(datename(month, cast([month] as varchar(2)) + ''/01/2000''),3) as [Month],
	@Year as [Year],
	CASE WHEN Size = ''>= 12""'' THEN @HighCount ELSE @LowCount END as reqinsp,
	CASE WHEN Size = ''>= 12""'' THEN @HighTotalValves ELSE @LowTotalValves END as totalValves
from 
	@table
group by
	Size, [month]
order by
	Size, [Month]
' 
END
GO";
        }

        public override void Up()
        {
            Execute.Sql(Sql.NEW_SP_1);
            Execute.Sql(Sql.NEW_SP_2);
        }

        public override void Down()
        {
            //Execute.Sql(Sql.ORIGINAL_SP_1);
            //Execute.Sql(Sql.ORIGINAL_SP_2);
        }
    }
}
