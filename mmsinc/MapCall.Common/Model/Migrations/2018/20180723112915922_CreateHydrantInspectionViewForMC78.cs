using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180723112915922), Tags("Production")]
    public class CreateHydrantInspectionViewForMC78 : Migration
    {
        public struct Sql
        {
            public const string CREATE_HYDRANT_REQUIRES_INSPECTION = @"
                CREATE FUNCTION [dbo].[HydrantRequiresInspection]
	                (	
		                @hydrantStatusId int, 
		                @hydrantBillingId int,
		                @inspFreqUnitId int,
		                @inspFreq int,
		                @lastInspectionDate datetime
	                )
                Returns BIT
                AS
                BEGIN
	                IF ('ACTIVE' <> (select Upper(hs.Description) from HydrantStatuses hs where hs.Id = @hydrantStatusId))									
		                RETURN 0 
	                IF ('PUBLIC' <> (select Upper(hb.Description) from HydrantBillings hb where hb.Id = @hydrantBillingId) AND (@inspFreq is null or @inspFreqUnitId is null))
		                RETURN 0 

	                -- Has not been inspected yet so yeah it requires inspection
	                IF (@lastInspectionDate is null)
		                RETURN 1 

	                declare @date datetime
	                set @date = getdate()
	                declare @inspFreqUnit varchar(10)
	                set @inspFreqUnit = (select rfu.Description from RecurringFrequencyUnits rfu where rfu.Id = @inspFreqUnitId) 

	                IF (@inspFreqUnit = 'Year')
			                IF (DateDiff(YY, @lastInspectionDate, @date) < @InspFreq)
				                RETURN 0
	                IF (@inspFreqUnit = 'Month')
			                IF (DateDiff(mm, @lastInspectionDate, @date) < @InspFreq)
				                RETURN 0
	                IF (@inspFreqUnit = 'Week')
			                IF (DateDiff(WW, @lastInspectionDate, @date) < @InspFreq)
				                RETURN 0
	                IF (@inspFreqUnit = 'Day')
			                IF (DateDiff(D, @lastInspectionDate, @date) < @InspFreq)
				                 RETURN 0 

	                RETURN 1
                END
";

            public const string DROP_HYDRANT_REQUIRES_INSPECTION = "DROP FUNCTION [dbo].[HydrantRequiresInspection]";

            public const string REQUIRES_INSPECTION_SQL_FORMAT = @"
CASE
-- NOT this_.ACTIVE
WHEN (hyd.HydrantStatusId <> 1)
THEN 0
WHEN hyd.IsNonBPUKPI = 1 
THEN 0

WHEN (hyd.InspectionFrequency IS NOT NULL AND hyd.InspectionFrequencyUnitId IS NOT NULL)
THEN CASE
    WHEN (SELECT COUNT(1) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id) = 0
    THEN 1
    ELSE CASE hyd.InspectionFrequencyUnitId
        -- Year
        WHEN 4
        THEN CASE
            WHEN (YEAR({0}) - (SELECT MAX(YEAR(hi.DateInspected)) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < hyd.InspectionFrequency
            THEN 0
            ELSE 1
        END
        -- Month
        WHEN 3
        THEN CASE
            WHEN DATEDIFF(mm, {0}, (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < hyd.InspectionFrequency
            THEN 0
            ELSE 1
        END
        -- Week
        WHEN 2
        THEN CASE
            WHEN DATEDIFF(WW, {0}, (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < hyd.InspectionFrequency
            THEN 0
            ELSE 1
        END
        -- Day
        WHEN 1
        THEN CASE
            WHEN DATEDIFF(D, {0}, (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < hyd.InspectionFrequency
            THEN 0
            ELSE 1
        END
    END
END

-- NOT PUBLIC OR PRIVATE
WHEN hyd.HydrantBillingId <> 2 AND hyd.HydrantBillingId <> 4 AND hyd.HydrantBillingId is not null
THEN 0

-- IN THIS YEAR'S ZONE
WHEN oc.ZoneStartYear IS NOT NULL
AND hyd.Zone IS NOT NULL
AND hyd.Zone = ((ABS(oc.ZoneStartYear - YEAR({0})) % oc.HydrantInspectionFrequency) + 1)
THEN CASE
    WHEN (SELECT COUNT(1) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id) = 0
    OR (SELECT MAX(YEAR(hi.DateInspected)) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id) < YEAR({0})
    THEN 1
    ELSE 0
END

-- HAS NO INSPECTIONS
WHEN (SELECT COUNT(1) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id) < 1
THEN 1

ELSE CASE oc.HydrantInspectionFrequencyUnitId
    -- Year
    WHEN 4
    THEN CASE
        WHEN (YEAR({0}) - (SELECT MAX(YEAR(hi.DateInspected)) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < oc.HydrantInspectionFrequency
        THEN 0
        ELSE 1
    END
    -- Month
    WHEN 3
    THEN CASE
        WHEN DATEDIFF(mm, {0}, (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < oc.HydrantInspectionFrequency
        THEN 0
        ELSE 1
    END
    -- Week
    WHEN 2
    THEN CASE
        WHEN DATEDIFF(WW, {0}, (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < oc.HydrantInspectionFrequency
        THEN 0
        ELSE 1
    END
    -- Day
    WHEN 1
    THEN CASE
        WHEN DATEDIFF(D, {0}, (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < oc.HydrantInspectionFrequency
        THEN 0
        ELSE 1
    END
END

END";

            public const string CREATE_HYDRANT_VIEW_SQL_FORMAT = @"
CREATE VIEW HydrantsDueInspection AS
SELECT hyd.Id," + REQUIRES_INSPECTION_SQL_FORMAT +
                                                                 @" as RequiresInspection
FROM Hydrants hyd
INNER JOIN OperatingCenters oc
ON hyd.OperatingCenterId = oc.OperatingCenterID";

            public const string DROP_HYDRANT_VIEW = "DROP VIEW HydrantsDueInspection";
        }

        public override void Up()
        {
            Execute.Sql(Sql.CREATE_HYDRANT_VIEW_SQL_FORMAT, "GetDate()");
            Execute.Sql(Sql.DROP_HYDRANT_REQUIRES_INSPECTION);
        }

        public override void Down()
        {
            Execute.Sql(Sql.DROP_HYDRANT_VIEW);
            Execute.Sql(Sql.CREATE_HYDRANT_REQUIRES_INSPECTION);
        }
    }
}
