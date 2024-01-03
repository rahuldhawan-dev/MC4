using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201113082453658), Tags("Production")]
    public class CreateSPROCForReportForMC2396 : Migration
    {
        public override void Up()
        {
            // clean up existing sproc
            Execute.Sql(@"
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[rptWQComplaintsRollUp] (@Year int, @OpCode Varchar(5))
AS

--DECLARE @year int = 2019
--DECLARE @OpCode varchar(5) = 'NJ7'

WITH cteMonths([Month]) AS
(
    SELECT 1
    UNION ALL
    SELECT [Month] + 1 
    FROM cteMonths
    WHERE [Month] < 12
)
select
    OpCode,
    PWSID,
    0 as RecordId,
    complaintType,
    complaintTypeText,
    [1] as [Jan],
    [2] as [Feb],
    [3] as [Mar],
    [4] as [Apr],
    [5] as [May],
    [6] as [Jun],
    [7] as [Jul],
    [8] as [Aug],
    [9] as [Sep],
    [10] as [Oct],
    [11] as [Nov],
    [12] as [Dec]
FROM
(
    SELECT
        oc.OperatingCenterCode as [OpCode],
        pp.PWSID,
        ct.Id as [complaintType],
        ct.Description as [complaintTypeText],
        months.[Month],
        case when (isNull(wqc.id, 0) = 0) then 0  else 1 end as [CountMe]
    FROM
        OperatingCenters as oc
    cross join 
        [cteMonths] as months
    cross join
        WaterQualityComplaintTypes as ct
    INNER join
        OperatingCentersPublicWaterSupplies ocpws ON oc.OperatingCenterID = ocpws.OperatingCenterId
    LEFT join
        PublicWaterSupplies pp ON pp.Id = ocpws.PublicWaterSupplyId
    LEFT JOIN
        WaterQualityComplaints wqc
            ON isNull(wqc.OperatingCenterId, 0) = oc.OperatingCenterID 
            AND isNull(wqc.ComplaintTypeId, 0) = ct.Id 
            AND Month(isNull(wqc.DateComplaintReceived,0)) = months.[Month]
            AND Year(isNull(wqc.DateComplaintReceived, 0)) = @Year
            AND isNull(wqc.PWSID, 0) = pp.Id
    WHERE
        oc.OperatingCenterCode = @OpCode
) source
PIVOT
(
    sum([CountMe])
    FOR [Month]
    IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
) as pvtMonth
order by 2, 3
GO");

            Execute.Sql(@"
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[rptWQComplaintsByStateForYear] (@Year int, @State Varchar(2))
AS

--DECLARE @year int = 2019;
--DECLARE @State varchar(2) = 'NJ';

select
    OperatingCenterCode,
    ComplaintType,
    [1] as [Jan],
    [2] as [Feb],
    [3] as [Mar],
    [4] as [Apr],
    [5] as [May],
    [6] as [Jun],
    [7] as [Jul],
    [8] as [Aug],
    [9] as [Sep],
    [10] as [Oct],
    [11] as [Nov],
    [12] as [Dec]
FROM
(
    SELECT
        oc.OperatingCenterCode,
        ct.Description as [ComplaintType],
        Month(wqc.DateComplaintReceived) as TMonth,
        1 as [count]
    FROM
        WaterQualityComplaints wqc
    INNER join
        OperatingCenters oc ON oc.OperatingCenterID = wqc.OperatingCenterId
    INNER JOIN
        States s on s.stateID = oc.StateId
    INNER join
        WaterQualityComplaintTypes ct ON ct.Id = wqc.ComplaintTypeId
    WHERE
        s.Abbreviation = @State
    AND
        Year(wqc.DateComplaintReceived) = @Year
) source
PIVOT
(
    count([count])
    FOR TMonth
    IN ([1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12])
) as pvtMonth
order by 1, 2
GO");
        }

        public override void Down()
        {
            // replace existing sproc
            Execute.Sql(@"

ALTER PROCEDURE [dbo].[rptWQComplaintsRollUp] (@Year int, @OpCode Varchar(5))
AS

--DECLARE @year int
--SET @Year = 2007
--DECLARE @OpCode varchar(5)
--SET @OpCode = 'NJ4'

Declare @PWSID Table(OpCode varchar(10), PWSID varchar(9), RecordID int)
Declare @ComplaintType Table(complaintType int, complaintTypeText varChar(50))

insert into @PWSID (OpCode, PWSID, RecordID)
--select distinct oc.OperatingCenterCode as OpCode, pp.PWSID, pp.Id as RecordId from PublicWaterSupplies PP join OperatingCenters oc on PP.OperatingCenterID = oc.OperatingCenterID where oc.OperatingCenterCode = @OpCode
select distinct oc.OperatingCenterCode as OpCode, pp.PWSID, pp.Id as RecordId 
from PublicWaterSupplies PP 
join OperatingCentersPublicWaterSupplies ocpws on ocpws.PublicWaterSupplyId = PP.Id 
join OperatingCenters oc on ocpws.OperatingCenterID = oc.OperatingCenterID 
where oc.OperatingCenterCode = @OpCode
insert into @ComplaintType select Id, Description from WaterQualityComplaintTypes
select 
		*, 
		(
				Select Count(*) from WaterQualityComplaints 
				where 
						Year(DateComplaintReceived) = @Year
				AND
						Month(DateComplaintReceived) = 1
				AND
						WaterQualityComplaints.PWSID = #PWSID.RecordId
				AND
						WaterQualityComplaints.ComplaintTypeId = #ComplaintType.ComplaintType
		) as Jan,
		(
				Select Count(*) from WaterQualityComplaints 
				where 
						Year(DateComplaintReceived) = @Year
				AND
						Month(DateComplaintReceived) = 2
				AND
						WaterQualityComplaints.PWSID = #PWSID.RecordId
				AND
						WaterQualityComplaints.ComplaintTypeId = #ComplaintType.ComplaintType
		) as Feb,
		(
				Select Count(*) from WaterQualityComplaints 
				where 
						Year(DateComplaintReceived) = @Year
				AND
						Month(DateComplaintReceived) = 3
				AND
						WaterQualityComplaints.PWSID = #PWSID.RecordId
				AND
						WaterQualityComplaints.ComplaintTypeId = #ComplaintType.ComplaintType
		) as Mar,
		(
				Select Count(*) from WaterQualityComplaints 
				where 
						Year(DateComplaintReceived) = @Year
				AND
						Month(DateComplaintReceived) = 4
				AND
						WaterQualityComplaints.PWSID = #PWSID.RecordId
				AND
						WaterQualityComplaints.ComplaintTypeId = #ComplaintType.ComplaintType
		) as Apr,
		(
				Select Count(*) from WaterQualityComplaints 
				where 
						Year(DateComplaintReceived) = @Year
				AND
						Month(DateComplaintReceived) = 5
				AND
						WaterQualityComplaints.PWSID = #PWSID.RecordId
				AND
						WaterQualityComplaints.ComplaintTypeId = #ComplaintType.ComplaintType
		) as May,
		(
				Select Count(*) from WaterQualityComplaints 
				where 
						Year(DateComplaintReceived) = @Year
				AND
						Month(DateComplaintReceived) = 6
				AND
						WaterQualityComplaints.PWSID = #PWSID.RecordId
				AND
						WaterQualityComplaints.ComplaintTypeId = #ComplaintType.ComplaintType
		) as Jun,
		(
				Select Count(*) from WaterQualityComplaints 
				where 
						Year(DateComplaintReceived) = @Year
				AND
						Month(DateComplaintReceived) = 7
				AND
						WaterQualityComplaints.PWSID = #PWSID.RecordId
				AND
						WaterQualityComplaints.ComplaintTypeId = #ComplaintType.ComplaintType
		) as Jul,
		(
				Select Count(*) from WaterQualityComplaints 
				where 
						Year(DateComplaintReceived) = @Year
				AND
						Month(DateComplaintReceived) = 8
				AND
						WaterQualityComplaints.PWSID = #PWSID.RecordId
				AND
						WaterQualityComplaints.ComplaintTypeId = #ComplaintType.ComplaintType
		) as Aug,
		(
				Select Count(*) from WaterQualityComplaints 
				where 
						Year(DateComplaintReceived) = @Year
				AND
						Month(DateComplaintReceived) = 9
				AND
						WaterQualityComplaints.PWSID = #PWSID.RecordId
				AND
						WaterQualityComplaints.ComplaintTypeId = #ComplaintType.ComplaintType
		) as Sep,
		(
				Select Count(*) from WaterQualityComplaints 
				where 
						Year(DateComplaintReceived) = @Year
				AND
						Month(DateComplaintReceived) = 10
				AND
						WaterQualityComplaints.PWSID = #PWSID.RecordId
				AND
						WaterQualityComplaints.ComplaintTypeId = #ComplaintType.ComplaintType
		) as Oct,
		(
				Select Count(*) from WaterQualityComplaints 
				where 
						Year(DateComplaintReceived) = @Year
				AND
						Month(DateComplaintReceived) = 11
				AND
						WaterQualityComplaints.PWSID = #PWSID.RecordId
				AND
						WaterQualityComplaints.ComplaintTypeId = #ComplaintType.ComplaintType
		) as Nov,
		(
				Select Count(*) from WaterQualityComplaints 
				where 
						Year(DateComplaintReceived) = @Year
				AND
						Month(DateComplaintReceived) = 12
				AND
						WaterQualityComplaints.PWSID = #PWSID.RecordId
				AND
						WaterQualityComplaints.ComplaintTypeId = #ComplaintType.ComplaintType
		) as Dec

from 
		@PWSID as #PWSID, 
		@ComplaintType as #ComplaintType
order by 1,2,5
GO");

            Execute.Sql("DROP Procedure [rptWQComplaintsByStateForYear]");
        }
    }
}
