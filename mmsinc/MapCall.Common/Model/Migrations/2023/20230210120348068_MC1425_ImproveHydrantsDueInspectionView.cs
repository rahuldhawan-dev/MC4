using FluentMigrator;
using MapCall.Common.Model.Migrations._2019;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230210120348068), Tags("Production")]
    public class MC1425_ImproveHydrantsDueInspectionView : Migration
    {
        private const string OLD_CREATE_VIEW =
            MC1725FixAssetsDueInspection.Hydrants.CREATE_HYDRANTS_DUE_INSPECTION_VIEW;

        private const string OLD_DROP_VIEW =
            MC1725FixAssetsDueInspection.Hydrants.DROP_HYDRANTS_DUE_INSPECTION_VIEW;

        public const string CREATE_VIEW = @"
CREATE VIEW [HydrantsDueInspectionView] AS
WITH SelectedAssetStatuses AS (
    SELECT AssetStatusID
      FROM AssetStatuses
     WHERE Description IN ('ACTIVE', 'REQUEST CANCELLATION', 'REQUEST RETIREMENT')
), SelectedHydrantInspections AS (
        SELECT
            inspection.Id,
            inspection.HydrantId,
            inspection.DateInspected
          FROM HydrantInspections inspection
    INNER JOIN Hydrants hydrant ON inspection.HydrantId = hydrant.Id
    INNER JOIN SelectedAssetStatuses status ON status.AssetStatusID = hydrant.AssetStatusId
         WHERE hydrant.IsNonBPUKPI <> 1
), MostRecentHydrantInspections AS (
        SELECT
            recent.Id,
            recent.HydrantId,
            recent.DateInspected
          FROM SelectedHydrantInspections recent
     LEFT JOIN SelectedHydrantInspections moreRecent
            ON recent.HydrantId = moreRecent.HydrantId
           AND moreRecent.DateInspected > recent.DateInspected
         WHERE moreRecent.Id IS NULL
)
       SELECT DISTINCT
           hydrant.Id,
           CASE
               WHEN NOT EXISTS
                   (SELECT 1
                      FROM SelectedAssetStatuses status
                     WHERE status.AssetStatusID = hydrant.AssetStatusId) OR
                   hydrant.IsNonBPUKPI = 1
               THEN 0  


               WHEN hydrant.InspectionFrequency IS NOT NULL AND
                    hydrant.InspectionFrequencyUnitId IS NOT NULL
               THEN CASE
                   -- no inspections yet so one is required regardless of schedule
                   WHEN inspection.Id IS NULL
                   THEN 1
                   ELSE CASE hydrant.InspectionFrequencyUnitId
                       -- Year
                       WHEN 4
                       THEN IIF(
                           (YEAR(GETDATE()) - YEAR(inspection.DateInspected)) < hydrant.InspectionFrequency,
                           0,
                           1)
                       -- Month
                       WHEN 3
                       THEN IIF(
                           DATEDIFF(mm, GETDATE(), inspection.DateInspected) < hydrant.InspectionFrequency,
                           0,
                           1)
                       -- Week
                       WHEN 2
                       THEN IIF(
                           DATEDIFF(WW, GETDATE(), inspection.DateInspected) < hydrant.InspectionFrequency,
                           0,
                           1)
                       -- Day
                       WHEN 1
                       THEN IIF(
                           DATEDIFF(D, GETDATE(), inspection.DateInspected) < hydrant.InspectionFrequency,
                           0,
                           1)
                   END
               END

               -- NOT PUBLIC OR PRIVATE
               WHEN HydrantBillingId NOT IN (2, 4) AND HydrantBillingId IS NOT NULL
               THEN 0

               -- IN THIS YEAR'S ZONE
               WHEN operatingCenter.ZoneStartYear IS NOT NULL AND hydrant.Zone IS NOT NULL
               THEN IIF(
                   hydrant.Zone =
                       ((ABS(operatingCenter.ZoneStartYear - YEAR(GETDATE())) %
                           operatingCenter.HydrantInspectionFrequency) + 1) AND
                       (inspection.Id IS NULL OR YEAR(inspection.DateInspected) < YEAR(GETDATE())),
                   1,
                   0)
               
               -- HAS NO INSPECTIONS
               WHEN inspection.Id IS NULL
               THEN 1

               ELSE CASE operatingCenter.HydrantInspectionFrequencyUnitId
                   -- Year
                   WHEN 4
                   THEN IIF(
                       (YEAR(GETDATE()) - YEAR(inspection.DateInspected)) <
                           operatingCenter.HydrantInspectionFrequency,
                       0,
                       1)
                   -- Month
                   WHEN 3
                   THEN IIF(
                       DATEDIFF(mm, GETDATE(), inspection.DateInspected) <
                           operatingCenter.HydrantInspectionFrequency,
                       0,
                       1)
                   -- Week
                   WHEN 2
                   THEN IIF(
                       DATEDIFF(WW, GETDATE(), inspection.DateInspected) <
                           operatingCenter.HydrantInspectionFrequency,
                       0,
                       1)
                   -- Day
                   WHEN 1
                   THEN IIF(
                       DATEDIFF(D, GETDATE(), inspection.DateInspected) <
                           operatingCenter.HydrantInspectionFrequency,
                       0,
                       1)
                   END

               END AS RequiresInspection
         FROM Hydrants hydrant
   INNER JOIN OperatingCenters operatingCenter
           ON operatingCenter.OperatingCenterID = hydrant.OperatingCenterId
    LEFT JOIN MostRecentHydrantInspections inspection
           ON inspection.HydrantId = hydrant.Id";

        public const string DROP_VIEW = "DROP VIEW [HydrantsDueInspectionView]";

        public override void Up()
        {
            Execute.Sql(OLD_DROP_VIEW);
            Execute.Sql(CREATE_VIEW);
        }

        public override void Down()
        {
            Execute.Sql(DROP_VIEW);
            Execute.Sql(OLD_CREATE_VIEW);
        }
    }
}

