using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20191014104051261), Tags("Production")]
    public class MC1725FixAssetsDueInspection : Migration
    {
        #region Constants

        public struct Hydrants
        {
            public const string DROP_HYDRANTS_DUE_INSPECTION_VIEW = "DROP VIEW [HydrantsDueInspection]";

            public const string CREATE_HYDRANTS_DUE_INSPECTION_VIEW = @"CREATE VIEW [HydrantsDueInspection] AS
SELECT hyd.Id,
CASE
-- NOT ACTIVE
WHEN (hyd.AssetStatusId not in(1,13,14))
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
            WHEN (YEAR(GETDATE()) - (SELECT MAX(YEAR(hi.DateInspected)) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < hyd.InspectionFrequency
            THEN 0
            ELSE 1
        END
        -- Month
        WHEN 3
        THEN CASE
            WHEN DATEDIFF(mm, GETDATE(), (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < hyd.InspectionFrequency
            THEN 0
            ELSE 1
        END
        -- Week
        WHEN 2
        THEN CASE
            WHEN DATEDIFF(WW, GETDATE(), (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < hyd.InspectionFrequency
            THEN 0
            ELSE 1
        END
        -- Day
        WHEN 1
        THEN CASE
            WHEN DATEDIFF(D, GETDATE(), (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < hyd.InspectionFrequency
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
    WHEN hyd.Zone = ((ABS(oc.ZoneStartYear - YEAR(GETDATE())) % oc.HydrantInspectionFrequency) + 1)
    AND ((SELECT COUNT(1) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id) = 0
        OR (SELECT MAX(YEAR(hi.DateInspected)) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id) < YEAR(GETDATE()))
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
        WHEN (YEAR(GETDATE()) - (SELECT MAX(YEAR(hi.DateInspected)) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < oc.HydrantInspectionFrequency
        THEN 0
        ELSE 1
    END
    -- Month
    WHEN 3
    THEN CASE
        WHEN DATEDIFF(mm, GETDATE(), (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < oc.HydrantInspectionFrequency
        THEN 0
        ELSE 1
    END
    -- Week
    WHEN 2
    THEN CASE
        WHEN DATEDIFF(WW, GETDATE(), (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < oc.HydrantInspectionFrequency
        THEN 0
        ELSE 1
    END
    -- Day
    WHEN 1
    THEN CASE
        WHEN DATEDIFF(D, GETDATE(), (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < oc.HydrantInspectionFrequency
        THEN 0
        ELSE 1
    END
END

END as RequiresInspection
FROM Hydrants hyd
INNER JOIN OperatingCenters oc
ON hyd.OperatingCenterId = oc.OperatingCenterID";

            public const string ROLLBACK_HYDRANT_DUE_INSPECTION_VIEW = @"CREATE VIEW [HydrantsDueInspection] AS
SELECT hyd.Id,
CASE
-- NOT this_.ACTIVE
WHEN (hyd.AssetStatusId <> 1)
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
            WHEN (YEAR(GETDATE()) - (SELECT MAX(YEAR(hi.DateInspected)) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < hyd.InspectionFrequency
            THEN 0
            ELSE 1
        END
        -- Month
        WHEN 3
        THEN CASE
            WHEN DATEDIFF(mm, GETDATE(), (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < hyd.InspectionFrequency
            THEN 0
            ELSE 1
        END
        -- Week
        WHEN 2
        THEN CASE
            WHEN DATEDIFF(WW, GETDATE(), (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < hyd.InspectionFrequency
            THEN 0
            ELSE 1
        END
        -- Day
        WHEN 1
        THEN CASE
            WHEN DATEDIFF(D, GETDATE(), (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < hyd.InspectionFrequency
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
    WHEN hyd.Zone = ((ABS(oc.ZoneStartYear - YEAR(GETDATE())) % oc.HydrantInspectionFrequency) + 1)
    AND ((SELECT COUNT(1) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id) = 0
        OR (SELECT MAX(YEAR(hi.DateInspected)) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id) < YEAR(GETDATE()))
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
        WHEN (YEAR(GETDATE()) - (SELECT MAX(YEAR(hi.DateInspected)) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < oc.HydrantInspectionFrequency
        THEN 0
        ELSE 1
    END
    -- Month
    WHEN 3
    THEN CASE
        WHEN DATEDIFF(mm, GETDATE(), (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < oc.HydrantInspectionFrequency
        THEN 0
        ELSE 1
    END
    -- Week
    WHEN 2
    THEN CASE
        WHEN DATEDIFF(WW, GETDATE(), (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < oc.HydrantInspectionFrequency
        THEN 0
        ELSE 1
    END
    -- Day
    WHEN 1
    THEN CASE
        WHEN DATEDIFF(D, GETDATE(), (SELECT MAX(hi.DateInspected) FROM HydrantInspections hi WHERE hi.HydrantId = hyd.Id)) < oc.HydrantInspectionFrequency
        THEN 0
        ELSE 1
    END
END

END as RequiresInspection
FROM Hydrants hyd
INNER JOIN OperatingCenters oc
ON hyd.OperatingCenterId = oc.OperatingCenterID";
        }

        public struct Valves
        {
            public const string DROP_VALVES_DUE_INSPECTION_VIEW = "DROP VIEW [ValvesDueInspection]";

            public const string CREATE_VALVES_DUE_INSPECTION_VIEW = @"CREATE VIEW [ValvesDueInspection] AS
SELECT val.Id,
    CASE
	/*not active*/
	WHEN (val.AssetStatusId not in (1, 13, 14)) THEN 0 
	WHEN ((val.ValveBillingId <> 3 and val.ValveBillingId is not null) AND (val.InspectionFrequency is null or val.InspectionFrequencyUnitId is null)) THEN 0 
	WHEN (val.ValveControlsId = 3) THEN 0
	WHEN val.BPUKPI = 1 THEN 0
	/*val.VALVE IS LESS THAN 2 val.inches AND IS val.A val.CONTROLS val.A val.BLOW OFF*/
    WHEN (vs.Size < 2.0 AND val.ValveControlsId = 2) THEN 0
    /*ZONE IS NOT val.REQUIRED FOR val.INSPECTION AND NO val.INSPECTION val.FREQUENCY IS SET*/
    WHEN (oc.UsesValveInspectionFrequency = 0 AND val.ValveZoneId in (1,2,3,4) AND val.ValveZoneId <> ((ABS(2011-YEAR(GETDATE()))%4)+1))
    THEN 0
    /* ZONE IS NOT val.REQUIRED FOR val.INSPECTION AND NO val.INSPECTION val.FREQUENCY IS SET*/
    WHEN (oc.UsesValveInspectionFrequency = 0 AND val.ValveZoneId in (5,6) AND val.ValveZoneId <> ((ABS(2011-YEAR(GETDATE()))%2)+5))
	THEN 0
    /*IF THE INSPECTION FREQUENCY IS SET AND ANNUAL, WE GO DEEPER DOWN THE RABBIT HOLE - SOUTH ORANGE VILLAGE INTRODUCED THIS */
    WHEN (oc.UsesValveInspectionFrequency = 1 AND val.InspectionFrequency is not null AND NullIf(val.InspectionFrequency,0) is not null AND val.InspectionFrequencyUnitId = 4 AND (val.ValveZoneId % nullif(val.InspectionFrequency, 0)) <> ((ABS(2011-YEAR(GETDATE()))%nullif(val.InspectionFrequency, 0))))
    THEN 0
    /*val.ALREADY val.INSPECTED FOR val.THE YEAR*/
    WHEN vi.Id IS NULL
        THEN 1
    ELSE 0
	END as RequiresInspection
FROM Valves val
INNER JOIN OperatingCenters oc
ON val.OperatingCenterId = oc.OperatingCenterId
LEFT OUTER JOIN ValveSizes vs
ON val.ValveSizeId = vs.Id
LEFT OUTER JOIN ValveInspections vi
ON vi.ValveID = val.Id AND YEAR(vi.DateInspected) = YEAR(GETDATE()) AND vi.Operated = 1
LEFT OUTER JOIN ValveInspections vi_newer
ON vi_newer.ValveID = val.Id AND YEAR(vi_newer.DateInspected) = YEAR(GETDATE()) AND vi_newer.Operated = 1 AND vi_newer.DateInspected > vi.DateInspected
WHERE vi_newer.ValveID IS NULL
";

            public const string ROLLBACK_VALVES_DUE_INSPECTION_VIEW = @"CREATE VIEW [ValvesDueInspection] AS
SELECT val.Id,
    CASE
	/*not active*/
	WHEN (val.AssetStatusId <> 1) THEN 0 
	WHEN ((val.ValveBillingId <> 3 and val.ValveBillingId is not null) AND (val.InspectionFrequency is null or val.InspectionFrequencyUnitId is null)) THEN 0 
	WHEN (val.ValveControlsId = 3) THEN 0
	WHEN val.BPUKPI = 1 THEN 0
	/*val.VALVE IS LESS THAN 2 val.inches AND IS val.A val.CONTROLS val.A val.BLOW OFF*/
    WHEN (vs.Size < 2.0 AND val.ValveControlsId = 2) THEN 0
    /*ZONE IS NOT val.REQUIRED FOR val.INSPECTION AND NO val.INSPECTION val.FREQUENCY IS SET*/
    WHEN (oc.UsesValveInspectionFrequency = 0 AND val.ValveZoneId in (1,2,3,4) AND val.ValveZoneId <> ((ABS(2011-YEAR(GETDATE()))%4)+1))
    THEN 0
    /* ZONE IS NOT val.REQUIRED FOR val.INSPECTION AND NO val.INSPECTION val.FREQUENCY IS SET*/
    WHEN (oc.UsesValveInspectionFrequency = 0 AND val.ValveZoneId in (5,6) AND val.ValveZoneId <> ((ABS(2011-YEAR(GETDATE()))%2)+5))
	THEN 0
    /*IF THE INSPECTION FREQUENCY IS SET AND ANNUAL, WE GO DEEPER DOWN THE RABBIT HOLE - SOUTH ORANGE VILLAGE INTRODUCED THIS */
    WHEN (oc.UsesValveInspectionFrequency = 1 AND val.InspectionFrequency is not null AND NullIf(val.InspectionFrequency,0) is not null AND val.InspectionFrequencyUnitId = 4 AND (val.ValveZoneId % nullif(val.InspectionFrequency, 0)) <> ((ABS(2011-YEAR(GETDATE()))%nullif(val.InspectionFrequency, 0))))
    THEN 0
    /*val.ALREADY val.INSPECTED FOR val.THE YEAR*/
    WHEN vi.Id IS NULL
        THEN 1
    ELSE 0
	END as RequiresInspection
FROM Valves val
INNER JOIN OperatingCenters oc
ON val.OperatingCenterId = oc.OperatingCenterId
LEFT OUTER JOIN ValveSizes vs
ON val.ValveSizeId = vs.Id
LEFT OUTER JOIN ValveInspections vi
ON vi.ValveID = val.Id AND YEAR(vi.DateInspected) = YEAR(GETDATE()) AND vi.Operated = 1
LEFT OUTER JOIN ValveInspections vi_newer
ON vi_newer.ValveID = val.Id AND YEAR(vi_newer.DateInspected) = YEAR(GETDATE()) AND vi_newer.Operated = 1 AND vi_newer.DateInspected > vi.DateInspected
WHERE vi_newer.ValveID IS NULL
";
        }

        #endregion

        public override void Up()
        {
            Execute.Sql(Hydrants.DROP_HYDRANTS_DUE_INSPECTION_VIEW);
            Execute.Sql(Hydrants.CREATE_HYDRANTS_DUE_INSPECTION_VIEW);

            Execute.Sql(Valves.DROP_VALVES_DUE_INSPECTION_VIEW);
            Execute.Sql(Valves.CREATE_VALVES_DUE_INSPECTION_VIEW);
        }

        public override void Down()
        {
            Execute.Sql(Valves.DROP_VALVES_DUE_INSPECTION_VIEW);
            Execute.Sql(Valves.ROLLBACK_VALVES_DUE_INSPECTION_VIEW);

            Execute.Sql(Hydrants.DROP_HYDRANTS_DUE_INSPECTION_VIEW);
            Execute.Sql(Hydrants.ROLLBACK_HYDRANT_DUE_INSPECTION_VIEW);
        }
    }
}
