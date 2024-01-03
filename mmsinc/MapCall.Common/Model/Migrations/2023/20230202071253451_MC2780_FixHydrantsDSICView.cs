using FluentMigrator;
using LastViewMigration =
    MapCall.Common.Model.Migrations._2020.CreateViewsForDSICForMC1960;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230202071253451), Tags("Production")]
    public class MC2780_FixHydrantsDSICView : Migration
    {
        public const string VIEW_NAME = "HydrantsDSICView";

        public const string OLD_VIEW_SQL = LastViewMigration.CREATE_HYDRANTS_VIEW;

        public const string NEW_VIEW_SQL = @"
WITH LatestWorkOrders (WorkOrderId, HydrantId) AS (
    SELECT max(WorkOrderId), HydrantId
    FROM WorkOrders
    WHERE HydrantId IS NOT NULL
    AND WorkDescriptionId = 30
    GROUP BY HydrantId
), LatestAccountNumbers (AccountCharged, HydrantId) AS (
    SELECT wo.AccountCharged, lwo.HydrantId
    FROM WorkOrders wo
    INNER JOIN LatestWorkOrders lwo ON wo.WorkOrderID = lwo.WorkOrderId
)
SELECT
    h.Id as Id,
	h.StreetNumber as StreetNumber,
	h.HydrantNumber as HydrantNumber,
	h.SAPEquipmentId as SAPEquipmentId,
	h.DateInstalled as DateInstalled,
	COALESCE(wo.AccountCharged, h.WorkOrderNumber) as WBSNumber,
	fd.PremiseNumber as PremiseNumber,
    h.OperatingCenterId as OperatingCenterId,
    oc.OperatingCenterCode + ' - ' + oc.OperatingCenterName as OperatingCenter,
	t.Town as Town,
    s.FullStName as Street,
    c.Latitude as Latitude,
    c.Longitude as Longitude
FROM
	Hydrants h
LEFT OUTER JOIN
    OperatingCenters oc
ON
    oc.OperatingCenterID = h.OperatingCenterId
LEFT OUTER JOIN
    Towns t
ON
    t.TownID = h.Town
LEFT OUTER JOIN
    Streets s
ON
    s.StreetID = h.StreetId
LEFT OUTER JOIN
    FireDistricts fd
ON
	h.FireDistrictId = fd.Id
LEFT OUTER JOIN
    Coordinates c
ON
    c.CoordinateID = h.CoordinateId
LEFT OUTER JOIN
    LatestAccountNumbers wo
ON
	wo.HydrantId = h.Id
WHERE
    oc.IsContractedOperations <> 1
AND
    h.DateInstalled IS NOT NULL
AND";

        public const string NEW_VIEW_SQL_SERVER_END = LastViewMigration.CREATE_HYDRANTS_VIEW_SQL_SERVER_END;

        public const string NEW_VIEW_SQLITE_END = LastViewMigration.CREATE_HYDRANTS_VIEW_SQLITE_END;

        public override void Up()
        {
            Execute.Sql("DROP VIEW HydrantsDSIC");
            Execute.Sql($"CREATE VIEW [{VIEW_NAME}] AS{NEW_VIEW_SQL}{NEW_VIEW_SQL_SERVER_END}");
        }

        public override void Down()
        {
            Execute.Sql($"DROP VIEW [{VIEW_NAME}]");
            Execute.Sql($"CREATE VIEW [HydrantsDSIC] AS{OLD_VIEW_SQL}{NEW_VIEW_SQL_SERVER_END}");
        }
    }
}

