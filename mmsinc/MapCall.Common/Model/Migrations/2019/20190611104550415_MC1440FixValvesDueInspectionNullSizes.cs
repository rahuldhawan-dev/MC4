using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190611104550415), Tags("Production")]
    public class MC1440FixValvesDueInspectionNullSizes : Migration
    {
        public struct Sql
        {
            // This is needed for the migration to fix the existing view.
            public const string FIX_VALVE_VIEW_SQL_FORMAT = @"
ALTER VIEW ValvesDueInspection AS
SELECT val.Id," + MC1218CreateValvesDueInspectionView.Sql.REQUIRES_INSPECTION_SQL_FORMAT +
                                                            @" as RequiresInspection
FROM Valves val
INNER JOIN OperatingCenters oc
ON val.OperatingCenterId = oc.OperatingCenterId
LEFT OUTER JOIN ValveSizes vs
ON val.ValveSizeId = vs.Id
LEFT OUTER JOIN ValveInspections vi
ON vi.ValveID = val.Id AND YEAR(vi.DateInspected) = YEAR({0}) AND vi.Operated = 1
LEFT OUTER JOIN ValveInspections vi_newer
ON vi_newer.ValveID = val.Id AND YEAR(vi_newer.DateInspected) = YEAR({0}) AND vi_newer.Operated = 1 AND vi_newer.DateInspected > vi.DateInspected
WHERE vi_newer.ValveID IS NULL";

            // This needs to be slightly different from the above FIX_VALVE const because this
            // constant is used in the ValvesDueInspectionMap, which needs CREATE instead of ALTER.
            public const string CREATE_VALVE_VIEW_SQL_FORMAT = @"
CREATE VIEW ValvesDueInspection AS
SELECT val.Id," + MC1218CreateValvesDueInspectionView.Sql.REQUIRES_INSPECTION_SQL_FORMAT +
                                                               @" as RequiresInspection
FROM Valves val
INNER JOIN OperatingCenters oc
ON val.OperatingCenterId = oc.OperatingCenterId
LEFT OUTER JOIN ValveSizes vs
ON val.ValveSizeId = vs.Id
LEFT OUTER JOIN ValveInspections vi
ON vi.ValveID = val.Id AND YEAR(vi.DateInspected) = YEAR({0}) AND vi.Operated = 1
LEFT OUTER JOIN ValveInspections vi_newer
ON vi_newer.ValveID = val.Id AND YEAR(vi_newer.DateInspected) = YEAR({0}) AND vi_newer.Operated = 1 AND vi_newer.DateInspected > vi.DateInspected
WHERE vi_newer.ValveID IS NULL";

            // This needs to exist to rollback to the previous MC1218 version, but we can't use the MC1218
            // const cause this has to be an ALTER rather than CREATE.
            public const string ROLLBACK_VALVE_VIEW_SQL_FORMAT = @"
ALTER VIEW ValvesDueInspection AS
SELECT val.Id," + MC1218CreateValvesDueInspectionView.Sql.REQUIRES_INSPECTION_SQL_FORMAT +
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
        }

        public override void Up()
        {
            Execute.Sql(string.Format(Sql.FIX_VALVE_VIEW_SQL_FORMAT, "GETDATE()"));
        }

        public override void Down()
        {
            Execute.Sql(string.Format(Sql.ROLLBACK_VALVE_VIEW_SQL_FORMAT, "GETDATE()"));
        }
    }
}
