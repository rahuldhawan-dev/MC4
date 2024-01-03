using FluentMigrator;
using MapCall.Common.Model.Migrations._2018;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190513105114360), Tags("Production")]
    public class AddRequestRetirementHydrantsToRequiresInspectionViewForMC1248 : Migration
    {
        public struct Sql
        {
            public static readonly string REQUIRES_INSPECTION_SQL_FORMAT = $@"
CASE
-- NOT this_.ACTIVE
WHEN (hyd.HydrantStatusId NOT IN (1 /*active*/, 6/*request retirement*/))
THEN 0
WHEN hyd.IsNonBPUKPI = 1 
THEN 0

WHEN (hyd.InspectionFrequency IS NOT NULL AND hyd.InspectionFrequencyUnitId IS NOT NULL)
THEN CASE
    WHEN (SELECT COUNT(1) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id) < 1
    THEN 1
    ELSE CASE hyd.InspectionFrequencyUnitId
        -- Year
        WHEN 4
        THEN CASE
            WHEN (YEAR({{0}}) - (SELECT MAX(YEAR(hi.DateInspected)) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < hyd.InspectionFrequency
            THEN 0
            ELSE 1
        END
        -- Month
        WHEN 3
        THEN CASE
            WHEN DATEDIFF(mm, {{0}}, (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < hyd.InspectionFrequency
            THEN 0
            ELSE 1
        END
        -- Week
        WHEN 2
        THEN CASE
            WHEN DATEDIFF(WW, {{0}}, (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < hyd.InspectionFrequency
            THEN 0
            ELSE 1
        END
        -- Day
        WHEN 1
        THEN CASE
            WHEN DATEDIFF(D, {{0}}, (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < hyd.InspectionFrequency
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
THEN CASE
    WHEN hyd.Zone = ((ABS(oc.ZoneStartYear - YEAR({{0}})) % oc.HydrantInspectionFrequency) + 1)
    AND ((SELECT COUNT(1) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id) = 0
        OR (SELECT MAX(YEAR(hi.DateInspected)) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id) < YEAR({{0}}))
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
        WHEN (YEAR({{0}}) - (SELECT MAX(YEAR(hi.DateInspected)) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < oc.HydrantInspectionFrequency
        THEN 0
        ELSE 1
    END
    -- Month
    WHEN 3
    THEN CASE
        WHEN DATEDIFF(mm, {{0}}, (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < oc.HydrantInspectionFrequency
        THEN 0
        ELSE 1
    END
    -- Week
    WHEN 2
    THEN CASE
        WHEN DATEDIFF(WW, {{0}}, (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < oc.HydrantInspectionFrequency
        THEN 0
        ELSE 1
    END
    -- Day
    WHEN 1
    THEN CASE
        WHEN DATEDIFF(D, {{0}}, (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < oc.HydrantInspectionFrequency
        THEN 0
        ELSE 1
    END
END

END";

            public static readonly string CREATE_HYDRANT_VIEW_SQL_FORMAT = @"
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
            Execute.Sql(AlterHydrantsDueInspectionViewToExcludeHydrantsNeverPreviouslyInspectedForMC78
                       .Sql.DROP_HYDRANT_VIEW);
            Execute.Sql(Sql.CREATE_HYDRANT_VIEW_SQL_FORMAT, "GetDate()");
        }

        public override void Down()
        {
            Execute.Sql(AlterHydrantsDueInspectionViewToExcludeHydrantsNeverPreviouslyInspectedForMC78
                       .Sql.DROP_HYDRANT_VIEW);
            Execute.Sql(
                AlterHydrantsDueInspectionViewToExcludeHydrantsNeverPreviouslyInspectedForMC78
                   .Sql.CREATE_HYDRANT_VIEW_SQL_FORMAT, "GetDate()");
        }
    }
}
