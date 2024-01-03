using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200127123246663), Tags("Production")]
    public class CreateViewsForDSICForMC1960 : Migration
    {
        public const string CREATE_HYDRANTS_VIEW_SQL_SERVER_BEGIN =
            @"CREATE VIEW dbo.HydrantsDSIC AS
";

        public const string CREATE_HYDRANTS_VIEW_SQLITE_BEGIN =
            @"CREATE VIEW HydrantsDSIC AS
";

        public const string CREATE_HYDRANTS_VIEW = @"WITH LatestWorkOrders (WorkOrderId, HydrantId) AS
(SELECT max(WorkOrderId), HydrantId FROM WorkOrders WHERE HydrantId IS NOT NULL AND WorkDescriptionId = 30 GROUP BY HydrantId)
,LatestAccountNumbers (AccountCharged, HydrantId) AS
(SELECT wo.AccountCharged, lwo.HydrantId FROM WorkOrders wo INNER JOIN LatestWorkOrders lwo ON wo.WorkOrderID = lwo.WorkOrderId)
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
    FireDistrict fd
ON
	h.FireDistrictId = fd.FireDistrictId
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

        public const string CREATE_HYDRANTS_VIEW_SQL_SERVER_END = @"
    COALESCE(wo.AccountCharged, h.WorkOrderNumber) LIKE 'R__-__[ABCFH]_.__-P-____';";

        public const string CREATE_HYDRANTS_VIEW_SQLITE_END = @"
    (COALESCE(wo.AccountCharged, h.WorkOrderNumber) LIKE 'R__-__A_.__-P-____'
OR
    COALESCE(wo.AccountCharged, h.WorkOrderNumber) LIKE 'R__-__B_.__-P-____'
OR
    COALESCE(wo.AccountCharged, h.WorkOrderNumber) LIKE 'R__-__C_.__-P-____'
OR
    COALESCE(wo.AccountCharged, h.WorkOrderNumber) LIKE 'R__-__F_.__-P-____'
OR
    COALESCE(wo.AccountCharged, h.WorkOrderNumber) LIKE 'R__-__H_.__-P-____');";

        public const string DROP_HYDRANTS_VIEW = "DROP VIEW dbo.HydrantsDSIC";

        public const string CREATE_SERVICES_VIEW_SQL_SERVER_BEGIN =
            @"CREATE VIEW dbo.ServicesDSIC AS
";

        public const string CREATE_SERVICES_VIEW_SQLITE_BEGIN =
            @"CREATE VIEW ServicesDSIC AS
";

        public const string CREATE_SERVICES_VIEW = @"WITH LatestWorkOrders (WorkOrderId, ServiceId) AS
(SELECT max(WorkOrderId), ServiceId FROM WorkOrders WHERE ServiceId IS NOT NULL AND WorkDescriptionId = 59 GROUP BY ServiceId)
,LatestAccountNumbers (AccountCharged, ServiceId) AS
(SELECT wo.AccountCharged, lwo.ServiceId FROM WorkOrders wo INNER JOIN LatestWorkOrders lwo ON wo.WorkOrderID = lwo.WorkOrderId)
SELECT
    s.Id as Id,
    s.ServiceNumber as ServiceNumber,
    s.StreetNumber as StreetNumber,
    s.DateInstalled as DateInstalled,
    COALESCE(wo.AccountCharged, s.TaskNumber1) as TaskNumber1,
    s.OperatingCenterId as OperatingCenterId,
    oc.OperatingCenterCode + ' - ' + oc.OperatingCenterName as OperatingCenter,
    t.Town as Town,
    st.FullStName as Street,
    c.Latitude as Latitude,
    c.Longitude as Longitude
FROM
    Services s
LEFT OUTER JOIN
    OperatingCenters oc
ON
    oc.OperatingCenterID = s.OperatingCenterId
LEFT OUTER JOIN
    Towns t
ON
    t.TownID = s.TownId
LEFT OUTER JOIN
    Streets st
ON
    st.StreetID = s.StreetId
LEFT OUTER JOIN
    Coordinates c
ON
    c.CoordinateID = s.CoordinateId
LEFT OUTER JOIN
    LatestAccountNumbers wo
ON
	wo.ServiceId = s.Id
WHERE
    oc.IsContractedOperations <> 1
AND
    s.DateInstalled IS NOT NULL
AND";

        public const string CREATE_SERVICES_VIEW_SQL_SERVER_END = @"
    COALESCE(wo.AccountCharged, s.TaskNumber1) LIKE 'R__-__[ABCFH]_.__-P-____';";

        public const string CREATE_SERVICES_VIEW_SQLITE_END = @"
    (COALESCE(wo.AccountCharged, s.TaskNumber1) LIKE 'R__-__A_.__-P-____'
OR
    COALESCE(wo.AccountCharged, s.TaskNumber1) LIKE 'R__-__B_.__-P-____'
OR
    COALESCE(wo.AccountCharged, s.TaskNumber1) LIKE 'R__-__C_.__-P-____'
OR
    COALESCE(wo.AccountCharged, s.TaskNumber1) LIKE 'R__-__F_.__-P-____'
OR
    COALESCE(wo.AccountCharged, s.TaskNumber1) LIKE 'R__-__H_.__-P-____');";

        public const string DROP_SERVICES_VIEW = "DROP VIEW dbo.ServicesDSIC";

        public override void Up()
        {
            Execute.Sql(CREATE_HYDRANTS_VIEW_SQL_SERVER_BEGIN + CREATE_HYDRANTS_VIEW +
                        CREATE_HYDRANTS_VIEW_SQL_SERVER_END);
            Execute.Sql(CREATE_SERVICES_VIEW_SQL_SERVER_BEGIN + CREATE_SERVICES_VIEW +
                        CREATE_SERVICES_VIEW_SQL_SERVER_END);
        }

        public override void Down()
        {
            Execute.Sql(DROP_HYDRANTS_VIEW);
            Execute.Sql(DROP_SERVICES_VIEW);
        }
    }
}
