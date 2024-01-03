using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150909133523867), Tags("Production")]
    public class UpdateStoredProcedureForBug2597 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"ALTER PROCEDURE [dbo].[rptSampleResultsPerYear] (@Year char(4))
AS
DECLARE @tblPWSID TABLE(PWSID varchar(10),
						PWSIDID int not null,
						Req_Jan int not null,
						Req_Feb int not null,
						Req_Mar int not null,
						Req_Apr int not null,
						Req_May int not null,
						Req_Jun int not null,
						Req_Jul int not null,
						Req_Aug int not null,
						Req_Sep int not null,
						Req_Oct int not null,
						Req_Nov int not null,
						Req_Dec int not null);

INSERT INTO @tblPWSID (
	PWSID,
	PWSIDID,
	Req_Jan,
	Req_Feb,
	Req_Mar,
	Req_Apr,
	Req_May,
	Req_Jun,
	Req_Jul,
	Req_Aug,
	Req_Sep,
	Req_Oct,
	Req_Nov,
	Req_Dec
)
SELECT DISTINCT
	PWSID,
	RecordID,
	IsNull(Jan_Required_Bacti_Samples, 0),
	IsNull(Feb_Required_Bacti_Samples, 0),
	IsNull(Mar_Required_Bacti_Samples, 0),
	IsNull(Apr_Required_Bacti_Samples, 0),
	IsNull(May_Required_Bacti_Samples, 0),
	IsNull(Jun_Required_Bacti_Samples, 0),
	IsNull(Jul_Required_Bacti_Samples, 0),
	IsNull(Aug_Required_Bacti_Samples, 0),
	IsNull(Sep_Required_Bacti_Samples, 0),
	IsNull(Oct_Required_Bacti_Samples, 0),
	IsNull(Nov_Required_Bacti_Samples, 0),
	IsNull(Dec_Required_Bacti_Samples, 0)
FROM
	tblPWSID
WHERE
	PWSID IS NOT NULL
AND
	LTrim(RTrim(PWSID)) <> ''
AND
	(SELECT Count(1) FROM tblWQSample_Sites SS where SS.PWSID = PWSID and SS.SiteStatusId = 1) > 0
ORDER BY
	PWSID;

----------------------------------------------------------------------------------------------------
-----------------------------------------REGULAR-TYPES----------------------------------------------
----------------------------------------------------------------------------------------------------
SELECT
	p.PWSID,
	p.Req_Jan,
	p.Req_Feb,
	p.Req_Mar,
	p.Req_Apr,
	p.Req_May,
	p.Req_Jun,
	p.Req_Jul,
	p.Req_Aug,
	p.Req_Sep,
	p.Req_Oct,
	p.Req_Nov,
	p.Req_Dec,
	t.Description AS Type,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS Jan,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS Feb,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS Mar,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS Apr,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS May,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS Jun,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS Jul,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS Aug,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS Sep,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS Oct,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS Nov,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS Dec
FROM
	@tblPWSID AS p
CROSS JOIN
	BacterialSampleTypes AS t

UNION

----------------------------------------------------------------------------------------------------
-----------------------------------------COLIFORM-CONFIRMED-----------------------------------------
----------------------------------------------------------------------------------------------------
SELECT
	p.PWSID,
	p.Req_Jan,
	p.Req_Feb,
	p.Req_Mar,
	p.Req_Apr,
	p.Req_May,
	p.Req_Jun,
	p.Req_Jul,
	p.Req_Aug,
	p.Req_Sep,
	p.Req_Oct,
	p.Req_Nov,
	p.Req_Dec,
	'Coliform Confirmed' AS [Type],
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS Jan,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS Feb,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS Mar,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS Apr,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS May,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS Jun,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS Jul,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS Aug,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS Sep,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS Oct,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS Nov,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS Dec
FROM
	@tblPWSID AS p

UNION

----------------------------------------------------------------------------------------------------
-------------------------------------------AVERAGES-------------------------------------------------
----------------------------------------------------------------------------------------------------

-- AVERAGES:
-- Total as Req_ on left
-- Free on right
SELECT
	p.PWSID,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jan],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Feb],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Mar],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Apr],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_May],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jun],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jul],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Aug],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Sep],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Oct],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Nov],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Dec],
	'Averages',
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS Jan,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS Feb,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS Mar,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS Apr,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS May,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS Jun,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS Jul,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS Aug,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS Sep,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS Oct,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS Nov,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS Dec
FROM
	@tblPWSID AS p

UNION

----------------------------------------------------------------------------------------------------
------------------------------------------CL2-MAX---------------------------------------------------
----------------------------------------------------------------------------------------------------
-- CL2Max:
-- Total as Req_ on left
-- Free on right
SELECT
	p.PWSID,
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jan],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Feb],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Mar],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Apr],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_May],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jun],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jul],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Aug],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Sep],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Oct],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Nov],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Dec],
	'CL2MAX' AS [Type],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jan],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Feb],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Mar],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Apr],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS [May],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jun],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jul],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Aug],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Sep],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Oct],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Nov],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Dec]
FROM
	@tblPWSID AS p

UNION

----------------------------------------------------------------------------------------------------
------------------------------------------CL2-MIN---------------------------------------------------
----------------------------------------------------------------------------------------------------
-- CL2Min:
-- Total as Req_ on left
-- Free on right
SELECT
	p.[PWSID],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jan],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Feb],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Mar],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Apr],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_May],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jun],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jul],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Aug],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Sep],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Oct],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Nov],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Dec],
		'CL2MIN' AS [Type],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jan],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Feb],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Mar],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Apr],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS [May],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jun],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jul],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Aug],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Sep],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Oct],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Nov],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Dec]
FROM
	@tblPWSID AS p

UNION

----------------------------------------------------------------------------------------------------
----------------------------------------------pH----------------------------------------------------
----------------------------------------------------------------------------------------------------
-- pH:
-- Min as Req_ on left
-- Max on right
SELECT
	p.[PWSID],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jan],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Feb],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Mar],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Apr],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_May],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jun],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jul],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Aug],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Sep],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Oct],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Nov],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Dec],
	'pH' AS [Type],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jan],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Feb],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Mar],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Apr],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS [May],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jun],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jul],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Aug],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Sep],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Oct],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Nov],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Dec]
FROM
	@tblPWSID AS p

UNION

----------------------------------------------------------------------------------------------------
-----------------------------------------CONDUCTIVITY-----------------------------------------------
----------------------------------------------------------------------------------------------------
-- Value_Conductivity:
-- Min as Req_ on left
-- Max on right
SELECT
	p.[PWSID],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jan],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Feb],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Mar],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Apr],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_May],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jun],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jul],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Aug],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Sep],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Oct],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Nov],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Dec],
	'Conductivity' AS [Type],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jan],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Feb],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Mar],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Apr],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS [May],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jun],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jul],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Aug],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Sep],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Oct],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Nov],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID AND ss.SiteStatusId = 1
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Dec]
FROM
	@tblPWSID AS p

ORDER BY
	PWSID;");
        }

        public override void Down()
        {
            Execute.Sql(@"ALTER PROCEDURE [dbo].[rptSampleResultsPerYear] (@Year char(4))
AS
DECLARE @tblPWSID TABLE(PWSID varchar(10),
						PWSIDID int not null,
						Req_Jan int not null,
						Req_Feb int not null,
						Req_Mar int not null,
						Req_Apr int not null,
						Req_May int not null,
						Req_Jun int not null,
						Req_Jul int not null,
						Req_Aug int not null,
						Req_Sep int not null,
						Req_Oct int not null,
						Req_Nov int not null,
						Req_Dec int not null);

INSERT INTO @tblPWSID (
	PWSID,
	PWSIDID,
	Req_Jan,
	Req_Feb,
	Req_Mar,
	Req_Apr,
	Req_May,
	Req_Jun,
	Req_Jul,
	Req_Aug,
	Req_Sep,
	Req_Oct,
	Req_Nov,
	Req_Dec
)
SELECT DISTINCT
	PWSID,
	RecordID,
	IsNull(Jan_Required_Bacti_Samples, 0),
	IsNull(Feb_Required_Bacti_Samples, 0),
	IsNull(Mar_Required_Bacti_Samples, 0),
	IsNull(Apr_Required_Bacti_Samples, 0),
	IsNull(May_Required_Bacti_Samples, 0),
	IsNull(Jun_Required_Bacti_Samples, 0),
	IsNull(Jul_Required_Bacti_Samples, 0),
	IsNull(Aug_Required_Bacti_Samples, 0),
	IsNull(Sep_Required_Bacti_Samples, 0),
	IsNull(Oct_Required_Bacti_Samples, 0),
	IsNull(Nov_Required_Bacti_Samples, 0),
	IsNull(Dec_Required_Bacti_Samples, 0)
FROM
	tblPWSID
WHERE
	PWSID IS NOT NULL
AND
	LTrim(RTrim(PWSID)) <> ''
ORDER BY
	PWSID;

----------------------------------------------------------------------------------------------------
-----------------------------------------REGULAR-TYPES----------------------------------------------
----------------------------------------------------------------------------------------------------
SELECT
	p.PWSID,
	p.Req_Jan,
	p.Req_Feb,
	p.Req_Mar,
	p.Req_Apr,
	p.Req_May,
	p.Req_Jun,
	p.Req_Jul,
	p.Req_Aug,
	p.Req_Sep,
	p.Req_Oct,
	p.Req_Nov,
	p.Req_Dec,
	t.Description AS Type,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS Jan,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS Feb,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS Mar,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS Apr,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS May,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS Jun,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS Jul,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS Aug,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS Sep,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS Oct,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS Nov,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.BacterialSampleTypeId = t.Id
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS Dec
FROM
	@tblPWSID AS p
CROSS JOIN
	BacterialSampleTypes AS t

UNION

----------------------------------------------------------------------------------------------------
-----------------------------------------COLIFORM-CONFIRMED-----------------------------------------
----------------------------------------------------------------------------------------------------
SELECT
	p.PWSID,
	p.Req_Jan,
	p.Req_Feb,
	p.Req_Mar,
	p.Req_Apr,
	p.Req_May,
	p.Req_Jun,
	p.Req_Jul,
	p.Req_Aug,
	p.Req_Sep,
	p.Req_Oct,
	p.Req_Nov,
	p.Req_Dec,
	'Coliform Confirmed' AS [Type],
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS Jan,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS Feb,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS Mar,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS Apr,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS May,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS Jun,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS Jul,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS Aug,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS Sep,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS Oct,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS Nov,
	(	SELECT
			COUNT(1)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Coliform_Confirm = 1
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS Dec
FROM
	@tblPWSID AS p

UNION

----------------------------------------------------------------------------------------------------
-------------------------------------------AVERAGES-------------------------------------------------
----------------------------------------------------------------------------------------------------

-- AVERAGES:
-- Total as Req_ on left
-- Free on right
SELECT
	p.PWSID,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jan],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Feb],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Mar],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Apr],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_May],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jun],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jul],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Aug],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Sep],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Oct],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Nov],
	(	SELECT
			IsNull(AVG(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Dec],
	'Averages',
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS Jan,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS Feb,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS Mar,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS Apr,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS May,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS Jun,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS Jul,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS Aug,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS Sep,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS Oct,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS Nov,
	(	SELECT
			IsNull(AVG(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS Dec
FROM
	@tblPWSID AS p

UNION

----------------------------------------------------------------------------------------------------
------------------------------------------CL2-MAX---------------------------------------------------
----------------------------------------------------------------------------------------------------
-- CL2Max:
-- Total as Req_ on left
-- Free on right
SELECT
	p.PWSID,
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jan],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Feb],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Mar],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Apr],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_May],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jun],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jul],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Aug],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Sep],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Oct],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Nov],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Dec],
	'CL2MAX' AS [Type],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jan],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Feb],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Mar],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Apr],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS [May],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jun],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jul],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Aug],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Sep],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Oct],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Nov],
	(	SELECT
			IsNull(MAX(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Dec]
FROM
	@tblPWSID AS p

UNION

----------------------------------------------------------------------------------------------------
------------------------------------------CL2-MIN---------------------------------------------------
----------------------------------------------------------------------------------------------------
-- CL2Min:
-- Total as Req_ on left
-- Free on right
SELECT
	p.[PWSID],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jan],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Feb],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Mar],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Apr],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_May],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jun],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jul],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Aug],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Sep],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Oct],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Nov],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Total]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Dec],
		'CL2MIN' AS [Type],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jan],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Feb],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Mar],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Apr],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS [May],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jun],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jul],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Aug],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Sep],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Oct],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Nov],
	(	SELECT
			IsNull(MIN(srb.[Cl2_Free]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Dec]
FROM
	@tblPWSID AS p

UNION

----------------------------------------------------------------------------------------------------
----------------------------------------------pH----------------------------------------------------
----------------------------------------------------------------------------------------------------
-- pH:
-- Min as Req_ on left
-- Max on right
SELECT
	p.[PWSID],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jan],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Feb],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Mar],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Apr],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_May],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jun],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jul],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Aug],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Sep],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Oct],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Nov],
	(	SELECT
			IsNull(MIN(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Dec],
	'pH' AS [Type],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jan],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Feb],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Mar],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Apr],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS [May],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jun],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jul],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Aug],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Sep],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Oct],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Nov],
	(	SELECT
			IsNull(MAX(srb.[pH]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Dec]
FROM
	@tblPWSID AS p

UNION

----------------------------------------------------------------------------------------------------
-----------------------------------------CONDUCTIVITY-----------------------------------------------
----------------------------------------------------------------------------------------------------
-- Value_Conductivity:
-- Min as Req_ on left
-- Max on right
SELECT
	p.[PWSID],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jan],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Feb],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Mar],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Apr],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_May],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jun],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Jul],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Aug],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Sep],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Oct],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Nov],
	(	SELECT
			IsNull(MIN(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Req_Dec],
	'Conductivity' AS [Type],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jan],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Feb],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Mar],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Apr],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS [May],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jun],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Jul],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Aug],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Sep],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Oct],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Nov],
	(	SELECT
			IsNull(MAX(srb.[Value_Conductivity]), 0)
		FROM
			BacterialWaterSamples AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS [Dec]
FROM
	@tblPWSID AS p

ORDER BY
	PWSID;
");
        }
    }
}
