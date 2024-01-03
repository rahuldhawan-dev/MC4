﻿using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190605120700157), Tags("Production")]
    public class MC1384AddDistinctToValvesDueInspectionView : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"ALTER VIEW [dbo].[ValvesDueInspection] AS
SELECT DISTINCT val.Id,
    CASE
	/*not active*/
	WHEN (val.ValveStatusId <> 1) THEN 0 
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
INNER JOIN ValveSizes vs
ON val.ValveSizeId = vs.Id
LEFT OUTER JOIN ValveInspections vi
ON vi.ValveID = val.Id AND YEAR(vi.DateInspected) = YEAR(GETDATE()) AND vi.Operated = 1
LEFT OUTER JOIN ValveInspections vi_newer
ON vi_newer.ValveID = val.Id AND YEAR(vi_newer.DateInspected) = YEAR(GETDATE()) AND vi_newer.Operated = 1 AND vi_newer.DateInspected > vi.DateInspected
WHERE vi_newer.ValveID IS NULL
");
        }

        public override void Down()
        {
            Execute.Sql(@"ALTER VIEW [dbo].[ValvesDueInspection] AS
SELECT val.Id,
    CASE
	/*not active*/
	WHEN (val.ValveStatusId <> 1) THEN 0 
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
INNER JOIN ValveSizes vs
ON val.ValveSizeId = vs.Id
LEFT OUTER JOIN ValveInspections vi
ON vi.ValveID = val.Id AND YEAR(vi.DateInspected) = YEAR(GETDATE()) AND vi.Operated = 1
LEFT OUTER JOIN ValveInspections vi_newer
ON vi_newer.ValveID = val.Id AND YEAR(vi_newer.DateInspected) = YEAR(GETDATE()) AND vi_newer.Operated = 1 AND vi_newer.DateInspected > vi.DateInspected
WHERE vi_newer.ValveID IS NULL
");
        }
    }
}
