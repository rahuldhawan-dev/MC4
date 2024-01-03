using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150716102513558), Tags("Production")]
    public class UpdateBacterialWaterSampleResultsForBug2491 : Migration
    {
        public struct TableNames
        {
            public const string BACTERIAL_WATER_SAMPLE_RESULTS = "BacterialWaterSamples",
                                BACTERIAL_SAMPLE_TYPES = "BacterialSampleTypes";
        }

        public struct StringLengths
        {
            public const int ADDRESS = 255, SAP_WORK_ORDER_ID = 50;
        }

        public struct Sql
        {
            public const string
                RESTORE_INDEXES_AND_SUCH =
                    "CREATE NONCLUSTERED INDEX [IX_tblWqSampleResultsBacti_rptSampleResultsPerYear] ON [dbo].[tblWQSampleResultsBacti](	[SampleSiteID] ASC,	[Sample_Date] ASC,	[Bacti_Sample_TYPE] ASC,	[Coliform_Confirm] ASC,	[Cl2_Total] ASC,	[Cl2_Free] ASC,	[pH] ASC,	[Value_Conductivity] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];" +
                    "ALTER TABLE [dbo].[tblWQSampleResultsBacti] ADD  CONSTRAINT [DF_tblWQSampleResultsBacti_Non_Sheen_Colony_Count]  DEFAULT ((1)) FOR [Non_Sheen_Colony_Count];" +
                    "ALTER TABLE [dbo].[tblWQSampleResultsBacti] ADD  CONSTRAINT [DF_tblWQSampleResultsBacti_Sheen_Colony_Count]  DEFAULT ((1)) FOR [Sheen_Colony_Count];",
                REMOVE_INDEXES_AND_SUCH =
                    "DROP INDEX [IX_tblWqSampleResultsBacti_rptSampleResultsPerYear] ON [dbo].[tblWQSampleResultsBacti];" +
                    "ALTER TABLE [dbo].[tblWQSampleResultsBacti] DROP CONSTRAINT [DF_tblWQSampleResultsBacti_Non_Sheen_Colony_Count];" +
                    "ALTER TABLE [dbo].[tblWQSampleResultsBacti] DROP CONSTRAINT [DF_tblWQSampleResultsBacti_Sheen_Colony_Count];",
                CLEANUP_DATA = "Update BacterialWaterSamples SET Ph = 73.76 where ph = 7376";

            #region sp

            public const string UPDATE_STORED_PROCEDURE = @"
ALTER PROCEDURE [dbo].[rptSampleResultsPerYear] (@Year char(4))
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
",
                                ROLLBACK_STORED_PROCEDURE =
                                    @"ALTER PROCEDURE [dbo].[rptSampleResultsPerYear] (@Year char(4))
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
DECLARE @tblTypes TABLE(TypeID int,
						TypeName varchar(25));

INSERT INTO @tblTypes (TypeID, TypeName)
SELECT DISTINCT
	LookupID,
	LookupValue
FROM
	Lookup
WHERE
	LookupType = 'Bacti_Sample_TYPE';

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
	t.TypeName AS Type,
	(	SELECT
			COUNT(1)
		FROM
			tblWQSampleResultsBacti AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Bacti_Sample_TYPE = t.TypeID
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 1
		AND
			DatePart(year, Sample_Date) = @Year	) AS Jan,
	(	SELECT
			COUNT(1)
		FROM
			tblWQSampleResultsBacti AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Bacti_Sample_TYPE = t.TypeID
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 2
		AND
			DatePart(year, Sample_Date) = @Year	) AS Feb,
	(	SELECT
			COUNT(1)
		FROM
			tblWQSampleResultsBacti AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Bacti_Sample_TYPE = t.TypeID
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 3
		AND
			DatePart(year, Sample_Date) = @Year	) AS Mar,
	(	SELECT
			COUNT(1)
		FROM
			tblWQSampleResultsBacti AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Bacti_Sample_TYPE = t.TypeID
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 4
		AND
			DatePart(year, Sample_Date) = @Year	) AS Apr,
	(	SELECT
			COUNT(1)
		FROM
			tblWQSampleResultsBacti AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Bacti_Sample_TYPE = t.TypeID
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 5
		AND
			DatePart(year, Sample_Date) = @Year	) AS May,
	(	SELECT
			COUNT(1)
		FROM
			tblWQSampleResultsBacti AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Bacti_Sample_TYPE = t.TypeID
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 6
		AND
			DatePart(year, Sample_Date) = @Year	) AS Jun,
	(	SELECT
			COUNT(1)
		FROM
			tblWQSampleResultsBacti AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Bacti_Sample_TYPE = t.TypeID
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 7
		AND
			DatePart(year, Sample_Date) = @Year	) AS Jul,
	(	SELECT
			COUNT(1)
		FROM
			tblWQSampleResultsBacti AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Bacti_Sample_TYPE = t.TypeID
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 8
		AND
			DatePart(year, Sample_Date) = @Year	) AS Aug,
	(	SELECT
			COUNT(1)
		FROM
			tblWQSampleResultsBacti AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Bacti_Sample_TYPE = t.TypeID
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 9
		AND
			DatePart(year, Sample_Date) = @Year	) AS Sep,
	(	SELECT
			COUNT(1)
		FROM
			tblWQSampleResultsBacti AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Bacti_Sample_TYPE = t.TypeID
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 10
		AND
			DatePart(year, Sample_Date) = @Year	) AS Oct,
	(	SELECT
			COUNT(1)
		FROM
			tblWQSampleResultsBacti AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Bacti_Sample_TYPE = t.TypeID
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 11
		AND
			DatePart(year, Sample_Date) = @Year	) AS Nov,
	(	SELECT
			COUNT(1)
		FROM
			tblWQSampleResultsBacti AS srb
		INNER JOIN
			tblWQSample_Sites AS ss
		ON
			srb.SampleSiteID = ss.SampleSiteID
		WHERE
			srb.Bacti_Sample_TYPE = t.TypeID
		AND
			ss.PWSID = p.PWSIDID
		AND
			DatePart(month, Sample_Date) = 12
		AND
			DatePart(year, Sample_Date) = @Year	) AS Dec
FROM
	@tblPWSID AS p
CROSS JOIN
	@tblTypes AS t

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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
			tblWQSampleResultsBacti AS srb
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
";

            #endregion
        }

        public override void Up()
        {
            #region Count Operators

            Execute.Sql(
                "UPDATE Lookup SET TableName = 'tblWQSampleResultsBacti' where LookupType in ('Non_Sheen_Colony_Count_Operator','Sheen_Colony_Count_Operator');");

            this.ExtractLookupTableLookup("tblWQSampleResultsBacti", "Non_Sheen_Colony_Count_Operator",
                "NonSheenColonyCountOperators", 50, "Non_Sheen_Colony_Count_Operator",
                deleteSafely: true);
            this.ExtractLookupTableLookup("tblWQSampleResultsBacti", "Sheen_Colony_Count_Operator",
                "SheenColonyCountOperators", 50, "Sheen_Colony_Count_Operator",
                deleteSafely: true);

            #endregion

            Execute.Sql(Sql.REMOVE_INDEXES_AND_SUCH);
            Rename.Table("tblWQSampleResultsBacti").To(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS);
            Execute.Sql(
                "update datatype set table_name = 'BacterialWaterSamples' where Data_Type = 'WQ Sample Results Bacti'");
            Rename.Column("Sample_Result_Id").OnTable(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS).To("Id");
            Execute.Sql(Sql.CLEANUP_DATA);

            #region Bacterial Sample Types

            this.CreateLookupTableWithValues(TableNames.BACTERIAL_SAMPLE_TYPES, "Compliance", "Process Control",
                "New Main", "Recheck", "System Repair");
            Alter.Table(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS)
                 .AddForeignKeyColumn("BacterialSampleTypeId", TableNames.BACTERIAL_SAMPLE_TYPES);
            Execute.Sql("UPDATE " + TableNames.BACTERIAL_WATER_SAMPLE_RESULTS +
                        " SET BacterialSampleTypeId = (SELECT Id from " + TableNames.BACTERIAL_SAMPLE_TYPES +
                        " WHERE Description = 'Compliance') " +
                        " WHERE Bacti_Sample_TYPE = 334;" +
                        "UPDATE " + TableNames.BACTERIAL_WATER_SAMPLE_RESULTS +
                        " SET BacterialSampleTypeId = (SELECT Id from " + TableNames.BACTERIAL_SAMPLE_TYPES +
                        " WHERE Description = 'Recheck') " +
                        " WHERE Bacti_Sample_TYPE = 333;");
            Delete.ForeignKey("FK_tblWQSampleResultsBacti_Lookup_Bacti_Sample_TYPE")
                  .OnTable(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS);
            Delete.Column("Bacti_Sample_TYPE").FromTable(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS);

            #endregion

            Alter.Table(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS)
                 .AddForeignKeyColumn("EstimatingProjectId", "EstimatingProjects");
            Alter.Table(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS)
                 .AddColumn("SAPWorkOrderId").AsAnsiString(StringLengths.SAP_WORK_ORDER_ID).Nullable();
            Alter.Table(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS)
                 .AddForeignKeyColumn("OriginalBacterialWaterSampleId", TableNames.BACTERIAL_WATER_SAMPLE_RESULTS);
            Alter.Table(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS)
                 .AddForeignKeyColumn("TownId", "Towns", "TownID");
            Alter.Table(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS)
                 .AddColumn("Address").AsAnsiString(StringLengths.ADDRESS).Nullable();
            Alter.Table(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS)
                 .AddForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateID");

            Alter.Column("Cl2_Free").OnTable(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS)
                 .AsDecimal(5, 3).Nullable();
            Alter.Column("Cl2_Total").OnTable(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS)
                 .AsDecimal(5, 3).Nullable();
            Alter.Column("Monochloramine").OnTable(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS)
                 .AsDecimal(5, 3).Nullable();
            Alter.Column("FreeAmmonia").OnTable(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS)
                 .AsDecimal(5, 3).Nullable();
            Alter.Column("ph").OnTable(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS)
                 .AsDecimal(5, 3).Nullable();
            Alter.Column("Temp_Celsius").OnTable(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS)
                 .AsDecimal(5, 1).Nullable();
            Alter.Column("Value_Fe").OnTable(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS)
                 .AsDecimal(6, 3).Nullable();
            Alter.Column("Value_Mn").OnTable(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS)
                 .AsDecimal(6, 3).Nullable();
            Alter.Column("Value_Turb").OnTable(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS)
                 .AsDecimal(5, 3).Nullable();
            Alter.Column("Value_Ortho").OnTable(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS)
                 .AsDecimal(6, 3).Nullable();
            Alter.Column("Value_Conductivity").OnTable(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS)
                 .AsDecimal(6, 2).Nullable();
            Alter.Column("Non_Sheen_Colony_Count").OnTable(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS)
                 .AsInt32().Nullable();
            Alter.Column("Sheen_Colony_Count").OnTable(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS)
                 .AsInt32().Nullable();

            Alter.Table(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS).AddColumn("ATP").AsDecimal(5, 3).Nullable();

            Execute.Sql("update BacterialWaterSamples Set SampleSiteID = null where SampleSiteID = 0;" +
                        "update BacterialWaterSamples Set Sample_ID = null where Sample_ID = 0;");

            Execute.Sql(Sql.UPDATE_STORED_PROCEDURE);

            Alter.Table("tblWQSample_Sites").AddColumn("CouponSite").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("CouponSite").FromTable("tblWQSample_Sites");

            #region Bacterial Sample Types

            Execute.Sql(
                "update datatype set table_name = 'tblWQSampleResultsBacti' where Data_Type = 'WQ Sample Results Bacti'");

            Alter.Table(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS)
                 .AddColumn("Bacti_Sample_TYPE").AsInt32()
                 .ForeignKey("FK_tblWQSampleResultsBacti_Lookup_Bacti_Sample_TYPE", "Lookup", "LookupID").Nullable();
            Execute.Sql("UPDATE " + TableNames.BACTERIAL_WATER_SAMPLE_RESULTS +
                        " SET Bacti_Sample_TYPE = 334 WHERE BacterialSampleTypeId = (Select Id from BacterialSampleTypes where Description = 'Compliance')");
            Execute.Sql("UPDATE " + TableNames.BACTERIAL_WATER_SAMPLE_RESULTS +
                        " SET Bacti_Sample_TYPE = 333 WHERE BacterialSampleTypeId = (Select Id from BacterialSampleTypes where Description = 'Recheck')");
            Delete.ForeignKeyColumn(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS, "BacterialSampleTypeId",
                TableNames.BACTERIAL_SAMPLE_TYPES);
            Delete.Table(TableNames.BACTERIAL_SAMPLE_TYPES);

            #endregion

            Delete.Column("ATP").FromTable(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS);
            Delete.ForeignKeyColumn(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS, "CoordinateId", "Coordinates",
                "CoordinateID");
            Delete.Column("Address").FromTable(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS);
            Delete.ForeignKeyColumn(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS, "TownId", "Towns", "TownID");
            Delete.ForeignKeyColumn(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS, "OriginalBacterialWaterSampleId",
                TableNames.BACTERIAL_WATER_SAMPLE_RESULTS);
            Delete.Column("SAPWorkOrderId")
                  .FromTable(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS);
            Delete.ForeignKeyColumn(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS, "EstimatingProjectId",
                "EstimatingProjects");

            Rename.Column("Id").OnTable(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS).To("Sample_Result_Id");
            Rename.Table(TableNames.BACTERIAL_WATER_SAMPLE_RESULTS).To("tblWQSampleResultsBacti");

            Execute.Sql(Sql.RESTORE_INDEXES_AND_SUCH);

            #region Count Operators

            this.ReplaceLookupTableLookup("tblWQSampleResultsBacti",
                "Non_Sheen_Colony_Count_Operator", "NonSheenColonyCountOperators",
                50, "Non_Sheen_Colony_Count_Operator");
            this.ReplaceLookupTableLookup("tblWQSampleResultsBacti",
                "Sheen_Colony_Count_Operator", "SheenColonyCountOperators",
                50, "Sheen_Colony_Count_Operator");

            #endregion

            Execute.Sql(Sql.ROLLBACK_STORED_PROCEDURE);
        }
    }
}
