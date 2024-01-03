using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170822141615072), Tags("Production")]
    public class Bug3343 : Migration
    {
        public override void Up()
        {
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
");
        }

        public override void Down()
        {
            // There's no rollback as it would create an invalid sql statement.
        }
    }
}
