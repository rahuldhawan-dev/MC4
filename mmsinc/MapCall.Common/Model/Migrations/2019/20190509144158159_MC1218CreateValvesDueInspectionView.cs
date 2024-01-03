using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190509144158159), Tags("Production")]
    public class MC1218CreateValvesDueInspectionView : Migration
    {
        public struct Sql
        {
            public const string REQUIRES_INSPECTION_SQL_FORMAT = @"
    CASE
	/*not active*/
	WHEN (val.ValveStatusId <> 1) THEN 0 
	WHEN ((val.ValveBillingId <> 3 and val.ValveBillingId is not null) AND (val.InspectionFrequency is null or val.InspectionFrequencyUnitId is null)) THEN 0 
	WHEN (val.ValveControlsId = 3) THEN 0
	WHEN val.BPUKPI = 1 THEN 0
	/*val.VALVE IS LESS THAN 2 val.inches AND IS val.A val.CONTROLS val.A val.BLOW OFF*/
    WHEN (vs.Size < 2.0 AND val.ValveControlsId = 2) THEN 0
    /*ZONE IS NOT val.REQUIRED FOR val.INSPECTION AND NO val.INSPECTION val.FREQUENCY IS SET*/
    WHEN (oc.UsesValveInspectionFrequency = 0 AND val.ValveZoneId in (1,2,3,4) AND val.ValveZoneId <> ((ABS(2011-YEAR({0}))%4)+1))
    THEN 0
    /* ZONE IS NOT val.REQUIRED FOR val.INSPECTION AND NO val.INSPECTION val.FREQUENCY IS SET*/
    WHEN (oc.UsesValveInspectionFrequency = 0 AND val.ValveZoneId in (5,6) AND val.ValveZoneId <> ((ABS(2011-YEAR({0}))%2)+5))
	THEN 0
    /*IF THE INSPECTION FREQUENCY IS SET AND ANNUAL, WE GO DEEPER DOWN THE RABBIT HOLE - SOUTH ORANGE VILLAGE INTRODUCED THIS */
    WHEN (oc.UsesValveInspectionFrequency = 1 AND val.InspectionFrequency is not null AND NullIf(val.InspectionFrequency,0) is not null AND val.InspectionFrequencyUnitId = 4 AND (val.ValveZoneId % nullif(val.InspectionFrequency, 0)) <> ((ABS(2011-YEAR({0}))%nullif(val.InspectionFrequency, 0))))
    THEN 0
    /*val.ALREADY val.INSPECTED FOR val.THE YEAR*/
    WHEN vi.Id IS NULL
        THEN 1
    ELSE 0
	END";

            public const string CREATE_VALVE_VIEW_SQL_FORMAT = @"
CREATE VIEW ValvesDueInspection AS
SELECT val.Id," + REQUIRES_INSPECTION_SQL_FORMAT +
                                                               @" as RequiresInspection
FROM Valves val
INNER JOIN OperatingCenters oc
ON val.OperatingCenterId = oc.OperatingCenterId
INNER JOIN ValveSizes vs
ON val.ValveSizeId = vs.Id
LEFT OUTER JOIN ValveInspections vi
ON vi.ValveID = val.Id AND YEAR(vi.DateInspected) = YEAR({0}) AND vi.Operated = 1
LEFT OUTER JOIN ValveInspections vi_newer
ON vi_newer.ValveID = val.Id AND YEAR(vi_newer.DateInspected) = YEAR({0}) AND vi_newer.Operated = 1 AND vi_newer.DateInspected > vi.DateInspected
WHERE vi_newer.ValveID IS NULL";

            public const string DROP_VALVE_VIEW = "DROP VIEW ValvesDueInspection";
        }

        public override void Up()
        {
            Execute.Sql(string.Format(Sql.CREATE_VALVE_VIEW_SQL_FORMAT, "GETDATE()"));
        }

        public override void Down()
        {
            Execute.Sql(Sql.DROP_VALVE_VIEW);
        }
    }
}
