using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160712134733084), Tags("Production")]
    public class FixedStoredProceduresForWQComplaintReportsForBug3039 : Migration
    {
        public override void Up()
        {
            #region rptWQComplaints

            Execute.Sql(@"ALTER PROCEDURE [dbo].[rptWQComplaints] (@Year int, @OpCode Varchar(5))
AS
--DECLARE @year int
--SET @Year = 2007
--DECLARE @OpCode varchar(5)
--SET @OpCode = 'NJ4'

Declare @Town Table(OpCode varchar(10), Town varchar(50), TownID int)
Declare @ComplaintType Table(complaintType int, complaintTypeText varChar(50))

insert into @Town (OpCode, Town, TownID)
Select distinct OperatingCenterCode, T.Town, T.TownID from Towns T join OperatingCentersTowns oct on oct.TownID = T.TownID inner join OperatingCenters oc on oct.OperatingCenterID = oc.OperatingCenterID where OperatingCenterCode = @OPCode  order by town
insert into @ComplaintType select Id, Description from WaterQualityComplaintTypes;
select 
		*, 
		(
				Select Count(*) from WaterQualityComplaints 
				where 
						Year(DateComplaintReceived) = @Year
				AND
						Month(DateComplaintReceived) = 1
				AND
						WaterQualityComplaints.TownId = #Town.TownID
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
						WaterQualityComplaints.TownId = #Town.TownID
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
						WaterQualityComplaints.TownId = #Town.TownID
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
						WaterQualityComplaints.TownId = #Town.TownID
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
						WaterQualityComplaints.TownId = #Town.TownID
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
						WaterQualityComplaints.TownId = #Town.TownID
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
						WaterQualityComplaints.TownId = #Town.TownID
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
						WaterQualityComplaints.TownId = #Town.TownID
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
						WaterQualityComplaints.TownId = #Town.TownID
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
						WaterQualityComplaints.TownId = #Town.TownID
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
						WaterQualityComplaints.TownId = #Town.TownID
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
						WaterQualityComplaints.TownId = #Town.TownID
				AND
						WaterQualityComplaints.ComplaintTypeId = #ComplaintType.ComplaintType
		) as Dec

from 
		@Town as #Town, 
		@ComplaintType as #ComplaintType
order by 1,2,5");

            #endregion

            #region rptWQComplaintsNoResponseTimesComplaint

            Execute.Sql(
                @"ALTER PROCEDURE [dbo].[rptWQComplaintsNoResponseTimesComplaint] (@OpCode varchar(5), @Year char(4)) AS

DECLARE @ComplaintTypeID int;
SET @ComplaintTypeID = 
	(SELECT Id FROM ORCOMOrderTypes WHERE Description = 'Complaint');

SELECT
	@OpCode AS [OpCode],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 1
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Jan],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 2
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Feb],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 3
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Mar],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 4
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Apr],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 5
		AND
			c.InitialLocalResponseDate IS NULL	) AS [May],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 6
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Jun],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 7
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Jul],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 8
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Aug],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 9
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Sep],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 10
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Oct],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 11
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Nov],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 12
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Dec]");

            #endregion

            #region rptWQComplaintsNoResponseTimesInquiry

            Execute.Sql(
                @"ALTER PROCEDURE [dbo].[rptWQComplaintsNoResponseTimesInquiry] (@OpCode varchar(5), @Year char(4)) AS

DECLARE @ComplaintTypeID int;
SET @ComplaintTypeID =
    (SELECT Id FROM ORCOMOrderTypes WHERE Description = 'Inquiry');

        SELECT
            @OpCode AS[OpCode],
	(	SELECT
            count(1)

        FROM
            WaterQualityComplaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOMOrderTypeId = @ComplaintTypeID
        AND

            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND

            datepart(month, c.DateComplaintReceived) = 1
		AND
            c.InitialLocalResponseDate IS NULL) AS[Jan],
	(	SELECT
            count(1)

        FROM
            WaterQualityComplaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOMOrderTypeId = @ComplaintTypeID
        AND

            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND

            datepart(month, c.DateComplaintReceived) = 2
		AND
            c.InitialLocalResponseDate IS NULL) AS[Feb],
	(	SELECT
            count(1)

        FROM
            WaterQualityComplaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOMOrderTypeId = @ComplaintTypeID
        AND

            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND

            datepart(month, c.DateComplaintReceived) = 3
		AND
            c.InitialLocalResponseDate IS NULL) AS[Mar],
	(	SELECT
            count(1)

        FROM
            WaterQualityComplaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOMOrderTypeId = @ComplaintTypeID
        AND

            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND

            datepart(month, c.DateComplaintReceived) = 4
		AND
            c.InitialLocalResponseDate IS NULL) AS[Apr],
	(	SELECT
            count(1)

        FROM
            WaterQualityComplaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOMOrderTypeId = @ComplaintTypeID
        AND

            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND

            datepart(month, c.DateComplaintReceived) = 5
		AND
            c.InitialLocalResponseDate IS NULL) AS[May],
	(	SELECT
            count(1)

        FROM
            WaterQualityComplaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOMOrderTypeId = @ComplaintTypeID
        AND

            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND

            datepart(month, c.DateComplaintReceived) = 6
		AND
            c.InitialLocalResponseDate IS NULL) AS[Jun],
	(	SELECT
            count(1)

        FROM
            WaterQualityComplaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOMOrderTypeId = @ComplaintTypeID
        AND

            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND

            datepart(month, c.DateComplaintReceived) = 7
		AND
            c.InitialLocalResponseDate IS NULL) AS[Jul],
	(	SELECT
            count(1)

        FROM
            WaterQualityComplaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOMOrderTypeId = @ComplaintTypeID
        AND

            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND

            datepart(month, c.DateComplaintReceived) = 8
		AND
            c.InitialLocalResponseDate IS NULL) AS[Aug],
	(	SELECT
            count(1)

        FROM
            WaterQualityComplaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOMOrderTypeId = @ComplaintTypeID
        AND

            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND

            datepart(month, c.DateComplaintReceived) = 9
		AND
            c.InitialLocalResponseDate IS NULL) AS[Sep],
	(	SELECT
            count(1)

        FROM
            WaterQualityComplaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOMOrderTypeId = @ComplaintTypeID
        AND

            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND

            datepart(month, c.DateComplaintReceived) = 10
		AND
            c.InitialLocalResponseDate IS NULL) AS[Oct],
	(	SELECT
            count(1)

        FROM
            WaterQualityComplaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOMOrderTypeId = @ComplaintTypeID
        AND

            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND

            datepart(month, c.DateComplaintReceived) = 11
		AND
            c.InitialLocalResponseDate IS NULL) AS[Nov],
	(	SELECT
            count(1)

        FROM
            WaterQualityComplaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOMOrderTypeId = @ComplaintTypeID
        AND

            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND

            datepart(month, c.DateComplaintReceived) = 12
		AND
            c.InitialLocalResponseDate IS NULL) AS[Dec]");

            #endregion

            #region rptWQComplaintsResponseTimeToComplaint

            Execute.Sql(
                @"ALTER PROCEDURE [dbo].[rptWQComplaintsResponseTimeToComplaint] (@OpCode varchar(5), @Year char(4)) AS
DECLARE @ComplaintTypeID int;
SET @ComplaintTypeID = (SELECT Id FROM ORCOMOrderTypes WHERE Description = 'Complaint');
DECLARE @Categories TABLE(
	CategoryName varchar(50) not null,
	MinID int not null,
	MaxID int not null
);
INSERT INTO @Categories
SELECT 'Aesthetics', min(Id), max(Id)
FROM WaterQualityComplaintTypes
WHERE Description LIKE 'Aesthetics%';
INSERT INTO @Categories
SELECT 'Information Inquiry', min(Id), max(Id)
FROM WaterQualityComplaintTypes
WHERE Description LIKE 'Information Inquiry%';
INSERT INTO @Categories
SELECT 'Medical', min(Id), max(Id)
FROM WaterQualityComplaintTypes
WHERE Description LIKE 'Medical%';
SELECT
	@OpCode AS [OpCode],
	cat.CategoryName,
	'< 2' AS Val,
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 1
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS [Jan],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 2
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS [Feb],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 3
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS [Mar],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 4
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS [Apr],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 5
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS [May],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 6
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS [Jun],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 7
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS [Jul],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 8
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS [Aug],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 9
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS [Sep],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 10
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS [Oct],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 11
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS [Nov],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 12
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS [Dec]
FROM
	@Categories AS cat
UNION
SELECT
	@OpCode AS [OpCode],
	cat.CategoryName,
	'< 4' AS Val,
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 1
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 4	) AS [Jan],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 2
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 4	) AS [Feb],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 3
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 4	) AS [Mar],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 4
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 4	) AS [Apr],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 5
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 4	) AS [May],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 6
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 4	) AS [Jun],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 7
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 4	) AS [Jul],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 8
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 4	) AS [Aug],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 9
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 4	) AS [Sep],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 10
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 4	) AS [Oct],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 11
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 4	) AS [Nov],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 12
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) < 4	) AS [Dec]
FROM
	@Categories AS cat
UNION
SELECT
	@OpCode AS [OpCode],
	cat.CategoryName,
	'> 4' AS Val,
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 1
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 4	) AS [Jan],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 2
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 4	) AS [Feb],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 3
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 4	) AS [Mar],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 4
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 4	) AS [Apr],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 5
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 4	) AS [May],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 6
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 4	) AS [Jun],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 7
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 4	) AS [Jul],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 8
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 4	) AS [Aug],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 9
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 4	) AS [Sep],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 10
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 4	) AS [Oct],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 11
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 4	) AS [Nov],
	(	SELECT
			count(1)
		FROM
			WaterQualityComplaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOMOrderTypeId = @ComplaintTypeID
		AND
			c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
		AND
			datepart(month, c.DateComplaintReceived) = 12
		AND
			datediff(hour, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 4	) AS [Dec]
FROM
	@Categories AS cat;");

            #endregion

            #region rptWQComplaintsResponseTimeToInquiry

            Execute.Sql(
                @"ALTER PROCEDURE [dbo].[rptWQComplaintsResponseTimeToInquiry] (@OpCode varchar(5), @Year char(4)) AS
DECLARE @InquiryTypeID int;
SET @InquiryTypeID = (SELECT Id FROM ORCOMOrderTypes WHERE Description = 'Inquiry');
        DECLARE @Categories TABLE(
            CategoryName varchar(50) not null,
	MinID int not null,
    MaxID int not null
);
INSERT INTO @Categories
SELECT 'Aesthetics', min(Id), max(Id)
FROM WaterQualityComplaintTypes
WHERE Description LIKE 'Aesthetics%';
INSERT INTO @Categories
SELECT 'Information Inquiry', min(Id), max(Id)
FROM WaterQualityComplaintTypes
WHERE Description LIKE 'Information Inquiry%';
INSERT INTO @Categories
SELECT 'Medical', min(Id), max(Id)
FROM WaterQualityComplaintTypes
WHERE Description LIKE 'Medical%';
SELECT
    @OpCode AS[OpCode],
	cat.CategoryName,
	'< 1' AS[Val],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 1
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 1	) AS[Jan],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 2
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 1	) AS[Feb],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 3
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 1	) AS[Mar],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 4
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 1	) AS[Apr],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 5
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 1	) AS[May],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 6
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 1	) AS[Jun],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 7
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 1	) AS[Jul],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 8
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 1	) AS[Aug],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 9
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 1	) AS[Sep],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 10
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 1	) AS[Oct],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 11
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 1	) AS[Nov],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 12
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 1	) AS[Dec]
FROM
    @Categories AS cat
UNION
SELECT
    @OpCode AS[OpCode],
	cat.CategoryName,
	'< 2' AS[Val],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 1
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS[Jan],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 2
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS[Feb],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 3
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS[Mar],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 4
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS[Apr],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 5
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS[May],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 6
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS[Jun],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 7
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS[Jul],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 8
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS[Aug],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 9
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS[Sep],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 10
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS[Oct],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 11
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS[Nov],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 12
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) < 2	) AS[Dec]
FROM
    @Categories AS cat
UNION
SELECT
    @OpCode AS[OpCode],
	cat.CategoryName,
	'> 2' AS[Val],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 1
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2	) AS[Jan],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 2
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2	) AS[Feb],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 3
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2	) AS[Mar],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 4
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2	) AS[Apr],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 5
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2	) AS[May],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 6
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2	) AS[Jun],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 7
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2	) AS[Jul],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 8
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2	) AS[Aug],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 9
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2	) AS[Sep],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 10
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2	) AS[Oct],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 11
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2	) AS[Nov],
	(	SELECT
            count(1)
        FROM
            WaterQualityComplaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOMOrderTypeId = @InquiryTypeID
        AND
            c.ComplaintTypeId BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.DateComplaintReceived) as char(4)) = @Year
        AND
            datepart(month, c.DateComplaintReceived) = 12
		AND
            datediff(day, c.DateComplaintReceived, c.InitialLocalResponseDate) >= 2	) AS[Dec]
FROM
    @Categories AS cat;");

            #endregion

            #region rptWQComplaintsRollUp

            Execute.Sql(@"ALTER PROCEDURE [dbo].[rptWQComplaintsRollUp] (@Year int, @OpCode Varchar(5))
AS

--DECLARE @year int
--SET @Year = 2007
--DECLARE @OpCode varchar(5)
--SET @OpCode = 'NJ4'

Declare @PWSID Table(OpCode varchar(10), PWSID varchar(9), RecordID int)
Declare @ComplaintType Table(complaintType int, complaintTypeText varChar(50))

insert into @PWSID (OpCode, PWSID, RecordID)
select distinct oc.OperatingCenterCode as OpCode, pp.PWSID, pp.Id as RecordId from PublicWaterSupplies PP join OperatingCenters oc on PP.OperatingCenterID = oc.OperatingCenterID where oc.OperatingCenterCode = @OpCode
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
order by 1,2,5");

            #endregion
        }

        public override void Down()
        {
            #region rptWQComplaints

            Execute.Sql(@"ALTER PROCEDURE [dbo].[rptWQComplaints] (@Year int, @OpCode Varchar(5))
AS
--DECLARE @year int
--SET @Year = 2007
--DECLARE @OpCode varchar(5)
--SET @OpCode = 'NJ4'

Declare @Town Table(OpCode varchar(10), Town varchar(50), TownID int)
Declare @ComplaintType Table(complaintType int, complaintTypeText varChar(50))

insert into @Town (OpCode, Town, TownID)
Select distinct OperatingCenterCode, T.Town, T.TownID from Towns T join OperatingCentersTowns oct on oct.TownID = T.TownID inner join OperatingCenters oc on oct.OperatingCenterID = oc.OperatingCenterID where OperatingCenterCode = @OPCode  order by town
insert into @ComplaintType select LookupID, LookupValue from lookup where LookupType = 'WQ_Complaint_Type'
select 
		*, 
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 1
				AND
						tblWQ_Complaints.Town = #Town.TownID
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Jan,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 2
				AND
						tblWQ_Complaints.Town = #Town.TownID
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Feb,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 3
				AND
						tblWQ_Complaints.Town = #Town.TownID
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Mar,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 4
				AND
						tblWQ_Complaints.Town = #Town.TownID
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Apr,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 5
				AND
						tblWQ_Complaints.Town = #Town.TownID
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as May,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 6
				AND
						tblWQ_Complaints.Town = #Town.TownID
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Jun,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 7
				AND
						tblWQ_Complaints.Town = #Town.TownID
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Jul,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 8
				AND
						tblWQ_Complaints.Town = #Town.TownID
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Aug,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 9
				AND
						tblWQ_Complaints.Town = #Town.TownID
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Sep,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 10
				AND
						tblWQ_Complaints.Town = #Town.TownID
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Oct,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 11
				AND
						tblWQ_Complaints.Town = #Town.TownID
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Nov,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 12
				AND
						tblWQ_Complaints.Town = #Town.TownID
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Dec

from 
		@Town as #Town, 
		@ComplaintType as #ComplaintType
order by 1,2,5");

            #endregion

            #region rptWQComplaintsNoResponseTimesComplaint

            Execute.Sql(
                @"ALTER PROCEDURE [dbo].[rptWQComplaintsNoResponseTimesComplaint] (@OpCode varchar(5), @Year char(4)) AS

DECLARE @ComplaintTypeID int;
SET @ComplaintTypeID = 
	(SELECT LookupID FROM Lookup WHERE TableName = 'tblWQ_Complaints' AND LookupType = 'ORCOM_OrderType' AND LookupValue = 'Complaint');

SELECT
	@OpCode AS [OpCode],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 1
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Jan],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 2
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Feb],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 3
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Mar],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 4
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Apr],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 5
		AND
			c.InitialLocalResponseDate IS NULL	) AS [May],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 6
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Jun],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 7
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Jul],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 8
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Aug],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 9
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Sep],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 10
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Oct],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 11
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Nov],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 12
		AND
			c.InitialLocalResponseDate IS NULL	) AS [Dec]");

            #endregion

            #region rptWQComplaintsNoResponseTimesInquiry

            Execute.Sql(
                @"ALTER PROCEDURE [dbo].[rptWQComplaintsNoResponseTimesInquiry] (@OpCode varchar(5), @Year char(4)) AS

DECLARE @ComplaintTypeID int;
SET @ComplaintTypeID =
    (SELECT LookupID FROM Lookup WHERE TableName = 'tblWQ_Complaints' AND LookupType = 'ORCOM_OrderType' AND LookupValue = 'Inquiry');

        SELECT
            @OpCode AS[OpCode],
	(	SELECT
            count(1)

        FROM
            tblWQ_Complaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOM_OrderType = @ComplaintTypeID
        AND

            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND

            datepart(month, c.Date_Complaint_Received) = 1
		AND
            c.InitialLocalResponseDate IS NULL) AS[Jan],
	(	SELECT
            count(1)

        FROM
            tblWQ_Complaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOM_OrderType = @ComplaintTypeID
        AND

            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND

            datepart(month, c.Date_Complaint_Received) = 2
		AND
            c.InitialLocalResponseDate IS NULL) AS[Feb],
	(	SELECT
            count(1)

        FROM
            tblWQ_Complaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOM_OrderType = @ComplaintTypeID
        AND

            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND

            datepart(month, c.Date_Complaint_Received) = 3
		AND
            c.InitialLocalResponseDate IS NULL) AS[Mar],
	(	SELECT
            count(1)

        FROM
            tblWQ_Complaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOM_OrderType = @ComplaintTypeID
        AND

            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND

            datepart(month, c.Date_Complaint_Received) = 4
		AND
            c.InitialLocalResponseDate IS NULL) AS[Apr],
	(	SELECT
            count(1)

        FROM
            tblWQ_Complaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOM_OrderType = @ComplaintTypeID
        AND

            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND

            datepart(month, c.Date_Complaint_Received) = 5
		AND
            c.InitialLocalResponseDate IS NULL) AS[May],
	(	SELECT
            count(1)

        FROM
            tblWQ_Complaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOM_OrderType = @ComplaintTypeID
        AND

            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND

            datepart(month, c.Date_Complaint_Received) = 6
		AND
            c.InitialLocalResponseDate IS NULL) AS[Jun],
	(	SELECT
            count(1)

        FROM
            tblWQ_Complaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOM_OrderType = @ComplaintTypeID
        AND

            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND

            datepart(month, c.Date_Complaint_Received) = 7
		AND
            c.InitialLocalResponseDate IS NULL) AS[Jul],
	(	SELECT
            count(1)

        FROM
            tblWQ_Complaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOM_OrderType = @ComplaintTypeID
        AND

            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND

            datepart(month, c.Date_Complaint_Received) = 8
		AND
            c.InitialLocalResponseDate IS NULL) AS[Aug],
	(	SELECT
            count(1)

        FROM
            tblWQ_Complaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOM_OrderType = @ComplaintTypeID
        AND

            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND

            datepart(month, c.Date_Complaint_Received) = 9
		AND
            c.InitialLocalResponseDate IS NULL) AS[Sep],
	(	SELECT
            count(1)

        FROM
            tblWQ_Complaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOM_OrderType = @ComplaintTypeID
        AND

            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND

            datepart(month, c.Date_Complaint_Received) = 10
		AND
            c.InitialLocalResponseDate IS NULL) AS[Oct],
	(	SELECT
            count(1)

        FROM
            tblWQ_Complaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOM_OrderType = @ComplaintTypeID
        AND

            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND

            datepart(month, c.Date_Complaint_Received) = 11
		AND
            c.InitialLocalResponseDate IS NULL) AS[Nov],
	(	SELECT
            count(1)

        FROM
            tblWQ_Complaints AS c

        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)

        AND
            c.ORCOM_OrderType = @ComplaintTypeID
        AND

            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND

            datepart(month, c.Date_Complaint_Received) = 12
		AND
            c.InitialLocalResponseDate IS NULL) AS[Dec]");

            #endregion

            #region rptWQComplaintsResponseTimeToComplaint

            Execute.Sql(
                @"ALTER PROCEDURE [dbo].[rptWQComplaintsResponseTimeToComplaint] (@OpCode varchar(5), @Year char(4)) AS
DECLARE @ComplaintTypeID int;
SET @ComplaintTypeID = (SELECT LookupID FROM Lookup WHERE TableName = 'tblWQ_Complaints' AND LookupType = 'ORCOM_OrderType' AND LookupValue = 'Complaint');
DECLARE @Categories TABLE(
	CategoryName varchar(50) not null,
	MinID int not null,
	MaxID int not null
);
INSERT INTO @Categories
SELECT 'Aesthetics', min(LookupID), max(LookupID)
FROM Lookup
WHERE LookupType = 'WQ_Complaint_Type'
AND LookupValue LIKE 'Aesthetics%';
INSERT INTO @Categories
SELECT 'Information Inquiry', min(LookupID), max(LookupID)
FROM Lookup
WHERE LookupType = 'WQ_Complaint_Type'
AND LookupValue LIKE 'Information Inquiry%';
INSERT INTO @Categories
SELECT 'Medical', min(LookupID), max(LookupID)
FROM Lookup
WHERE LookupType = 'WQ_Complaint_Type'
AND LookupValue LIKE 'Medical%';
SELECT
	@OpCode AS [OpCode],
	cat.CategoryName,
	'< 2' AS Val,
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 1
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS [Jan],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 2
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS [Feb],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 3
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS [Mar],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 4
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS [Apr],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 5
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS [May],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 6
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS [Jun],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 7
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS [Jul],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 8
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS [Aug],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 9
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS [Sep],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 10
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS [Oct],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 11
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS [Nov],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 12
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS [Dec]
FROM
	@Categories AS cat
UNION
SELECT
	@OpCode AS [OpCode],
	cat.CategoryName,
	'< 4' AS Val,
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 1
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 4	) AS [Jan],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 2
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 4	) AS [Feb],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 3
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 4	) AS [Mar],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 4
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 4	) AS [Apr],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 5
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 4	) AS [May],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 6
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 4	) AS [Jun],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 7
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 4	) AS [Jul],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 8
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 4	) AS [Aug],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 9
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 4	) AS [Sep],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 10
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 4	) AS [Oct],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 11
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 4	) AS [Nov],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 12
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 4	) AS [Dec]
FROM
	@Categories AS cat
UNION
SELECT
	@OpCode AS [OpCode],
	cat.CategoryName,
	'> 4' AS Val,
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 1
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 4	) AS [Jan],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 2
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 4	) AS [Feb],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 3
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 4	) AS [Mar],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 4
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 4	) AS [Apr],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 5
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 4	) AS [May],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 6
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 4	) AS [Jun],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 7
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 4	) AS [Jul],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 8
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 4	) AS [Aug],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 9
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 4	) AS [Sep],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 10
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 4	) AS [Oct],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 11
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 4	) AS [Nov],
	(	SELECT
			count(1)
		FROM
			tblWQ_Complaints AS c
		WHERE
			EXISTS (SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
		AND
			c.ORCOM_OrderType = @ComplaintTypeID
		AND
			c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
		AND
			cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
		AND
			datepart(month, c.Date_Complaint_Received) = 12
		AND
			datediff(hour, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 4	) AS [Dec]
FROM
	@Categories AS cat;");

            #endregion

            #region rptWQComplaintsResponseTimeToInquiry

            Execute.Sql(
                @"ALTER PROCEDURE [dbo].[rptWQComplaintsResponseTimeToInquiry] (@OpCode varchar(5), @Year char(4)) AS
DECLARE @InquiryTypeID int;
SET @InquiryTypeID = (SELECT LookupID FROM Lookup WHERE TableName = 'tblWQ_Complaints' AND LookupType = 'ORCOM_OrderType' AND LookupValue = 'Inquiry');
        DECLARE @Categories TABLE(
            CategoryName varchar(50) not null,
	MinID int not null,
    MaxID int not null
);
        INSERT INTO @Categories
        SELECT 'Aesthetics', min(LookupID), max(LookupID)
FROM Lookup
WHERE LookupType = 'WQ_Complaint_Type'
AND LookupValue LIKE 'Aesthetics%';
INSERT INTO @Categories
SELECT 'Information Inquiry', min(LookupID), max(LookupID)
FROM Lookup
WHERE LookupType = 'WQ_Complaint_Type'
AND LookupValue LIKE 'Information Inquiry%';
INSERT INTO @Categories
SELECT 'Medical', min(LookupID), max(LookupID)
FROM Lookup
WHERE LookupType = 'WQ_Complaint_Type'
AND LookupValue LIKE 'Medical%';
SELECT
    @OpCode AS[OpCode],
	cat.CategoryName,
	'< 1' AS[Val],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 1
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 1	) AS[Jan],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 2
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 1	) AS[Feb],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 3
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 1	) AS[Mar],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 4
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 1	) AS[Apr],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 5
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 1	) AS[May],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 6
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 1	) AS[Jun],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 7
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 1	) AS[Jul],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 8
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 1	) AS[Aug],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 9
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 1	) AS[Sep],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 10
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 1	) AS[Oct],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 11
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 1	) AS[Nov],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 12
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 1	) AS[Dec]
FROM
    @Categories AS cat
UNION
SELECT
    @OpCode AS[OpCode],
	cat.CategoryName,
	'< 2' AS[Val],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 1
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS[Jan],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 2
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS[Feb],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 3
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS[Mar],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 4
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS[Apr],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 5
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS[May],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 6
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS[Jun],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 7
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS[Jul],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 8
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS[Aug],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 9
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS[Sep],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 10
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS[Oct],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 11
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS[Nov],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 12
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 1
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) < 2	) AS[Dec]
FROM
    @Categories AS cat
UNION
SELECT
    @OpCode AS[OpCode],
	cat.CategoryName,
	'> 2' AS[Val],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 1
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2	) AS[Jan],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 2
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2	) AS[Feb],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 3
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2	) AS[Mar],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 4
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2	) AS[Apr],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 5
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2	) AS[May],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 6
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2	) AS[Jun],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 7
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2	) AS[Jul],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 8
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2	) AS[Aug],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 9
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2	) AS[Sep],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 10
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2	) AS[Oct],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 11
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2	) AS[Nov],
	(	SELECT
            count(1)
        FROM
            tblWQ_Complaints AS c
        WHERE
            EXISTS(SELECT 1 FROM PublicWaterSupplies PP Join OperatingCenters OC on Oc.OperatingCenterID = PP.OperatingCenterID WHERE oc.OperatingCenterCode = @OpCode AND PP.Id = c.PWSID)
        AND
            c.ORCOM_OrderType = @InquiryTypeID
        AND
            c.WQ_Complaint_Type BETWEEN cat.MinID AND cat.MaxID
        AND
            cast(datepart(year, c.Date_Complaint_Received) as char(4)) = @Year
        AND
            datepart(month, c.Date_Complaint_Received) = 12
		AND
            datediff(day, c.Date_Complaint_Received, c.InitialLocalResponseDate) >= 2	) AS[Dec]
FROM
    @Categories AS cat;");

            #endregion

            #region rptWQComplaintsRollUp

            Execute.Sql(@"ALTER PROCEDURE [dbo].[rptWQComplaintsRollUp] (@Year int, @OpCode Varchar(5))
AS

--DECLARE @year int
--SET @Year = 2007
--DECLARE @OpCode varchar(5)
--SET @OpCode = 'NJ4'

Declare @PWSID Table(OpCode varchar(10), PWSID varchar(9), RecordID int)
Declare @ComplaintType Table(complaintType int, complaintTypeText varChar(50))

insert into @PWSID (OpCode, PWSID, RecordID)
select distinct oc.OperatingCenterCode as OpCode, pp.PWSID, pp.Id as RecordId from PublicWaterSupplies PP join OperatingCenters oc on PP.OperatingCenterID = oc.OperatingCenterID where oc.OperatingCenterCode = @OpCode
insert into @ComplaintType select LookupID, LookupValue from lookup where LookupType = 'WQ_Complaint_Type'
select 
		*, 
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 1
				AND
						tblWQ_Complaints.PWSID = #PWSID.RecordId
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Jan,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 2
				AND
						tblWQ_Complaints.PWSID = #PWSID.RecordId
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Feb,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 3
				AND
						tblWQ_Complaints.PWSID = #PWSID.RecordId
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Mar,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 4
				AND
						tblWQ_Complaints.PWSID = #PWSID.RecordId
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Apr,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 5
				AND
						tblWQ_Complaints.PWSID = #PWSID.RecordId
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as May,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 6
				AND
						tblWQ_Complaints.PWSID = #PWSID.RecordId
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Jun,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 7
				AND
						tblWQ_Complaints.PWSID = #PWSID.RecordId
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Jul,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 8
				AND
						tblWQ_Complaints.PWSID = #PWSID.RecordId
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Aug,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 9
				AND
						tblWQ_Complaints.PWSID = #PWSID.RecordId
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Sep,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 10
				AND
						tblWQ_Complaints.PWSID = #PWSID.RecordId
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Oct,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 11
				AND
						tblWQ_Complaints.PWSID = #PWSID.RecordId
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Nov,
		(
				Select Count(*) from tblWQ_Complaints 
				where 
						Year(Date_Complaint_Received) = @Year
				AND
						Month(Date_Complaint_Received) = 12
				AND
						tblWQ_Complaints.PWSID = #PWSID.RecordId
				AND
						tblWQ_Complaints.WQ_Complaint_Type = #ComplaintType.ComplaintType
		) as Dec

from 
		@PWSID as #PWSID, 
		@ComplaintType as #ComplaintType
order by 1,2,5");

            #endregion
        }
    }
}
