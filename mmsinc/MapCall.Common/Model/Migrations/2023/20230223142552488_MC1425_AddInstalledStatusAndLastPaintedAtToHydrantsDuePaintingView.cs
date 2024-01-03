using FluentMigrator;
using PreviousMigration = MapCall.Common.Model.Migrations._2023.MC1425_CreateHydrantsDuePaintingView;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230223142552488), Tags("Production")]
    public class MC1425_AddInstalledStatusAndLastPaintedAtToHydrantsDuePaintingView : Migration
    {
        public const string DROP_VIEW = PreviousMigration.DROP_VIEW;

        public const string CREATE_VIEW = @"
CREATE VIEW [HydrantsDuePaintingView] AS
WITH SelectedAssetStatuses AS (
    SELECT AssetStatusID
      FROM AssetStatuses
     WHERE Description IN ('ACTIVE', 'INSTALLED', 'REQUEST CANCELLATION', 'REQUEST RETIREMENT')
), SelectedHydrantPaintings AS (
        SELECT
            painting.Id,
            painting.HydrantId,
            painting.PaintedAt
          FROM HydrantPaintings painting
    INNER JOIN Hydrants hydrant ON painting.HydrantId = hydrant.Id
    INNER JOIN SelectedAssetStatuses status ON status.AssetStatusID = hydrant.AssetStatusId
         WHERE hydrant.IsNonBPUKPI <> 1
), MostRecentHydrantPaintings AS (
        SELECT
            recent.Id,
            recent.HydrantId,
            recent.PaintedAt
          FROM SelectedHydrantPaintings recent
     LEFT JOIN SelectedHydrantPaintings moreRecent
            ON recent.HydrantId = moreRecent.HydrantId
           AND moreRecent.PaintedAt > recent.PaintedAt
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


               WHEN hydrant.PaintingFrequency IS NOT NULL AND
                    hydrant.PaintingFrequencyUnitId IS NOT NULL
               THEN CASE
                   -- no paintings yet so one is required regardless of schedule
                   WHEN painting.Id IS NULL
                   THEN 1
                   ELSE CASE hydrant.PaintingFrequencyUnitId
                       -- Year
                       WHEN 4
                       THEN IIF(
                           (YEAR(GETDATE()) - YEAR(painting.PaintedAt)) < hydrant.PaintingFrequency,
                           0,
                           1)
                       -- Month
                       WHEN 3
                       THEN IIF(
                           DATEDIFF(mm, GETDATE(), painting.PaintedAt) < hydrant.PaintingFrequency,
                           0,
                           1)
                       -- Week
                       WHEN 2
                       THEN IIF(
                           DATEDIFF(WW, GETDATE(), painting.PaintedAt) < hydrant.PaintingFrequency,
                           0,
                           1)
                       -- Day
                       WHEN 1
                       THEN IIF(
                           DATEDIFF(D, GETDATE(), painting.PaintedAt) < hydrant.PaintingFrequency,
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
                           operatingCenter.HydrantPaintingFrequency) + 1) AND
                       (painting.Id IS NULL OR YEAR(painting.PaintedAt) < YEAR(GETDATE())),
                   1,
                   0)
               
               -- HAS NO PAINTINGS
               WHEN painting.Id IS NULL
               THEN 1

               ELSE CASE operatingCenter.HydrantPaintingFrequencyUnitId
                   -- Year
                   WHEN 4
                   THEN IIF(
                       (YEAR(GETDATE()) - YEAR(painting.PaintedAt)) <
                           operatingCenter.HydrantPaintingFrequency,
                       0,
                       1)
                   -- Month
                   WHEN 3
                   THEN IIF(
                       DATEDIFF(mm, GETDATE(), painting.PaintedAt) <
                           operatingCenter.HydrantPaintingFrequency,
                       0,
                       1)
                   -- Week
                   WHEN 2
                   THEN IIF(
                       DATEDIFF(WW, GETDATE(), painting.PaintedAt) <
                           operatingCenter.HydrantPaintingFrequency,
                       0,
                       1)
                   -- Day
                   WHEN 1
                   THEN IIF(
                       DATEDIFF(D, GETDATE(), painting.PaintedAt) <
                           operatingCenter.HydrantPaintingFrequency,
                       0,
                       1)
                   ELSE 0
                   END

               END AS RequiresPainting,
               painting.PaintedAt AS LastPaintedAt
         FROM Hydrants hydrant
   INNER JOIN OperatingCenters operatingCenter
           ON operatingCenter.OperatingCenterID = hydrant.OperatingCenterId
    LEFT JOIN MostRecentHydrantPaintings painting
           ON painting.HydrantId = hydrant.Id";

        public override void Up()
        {
            Execute.Sql(DROP_VIEW);
            Execute.Sql(CREATE_VIEW);
        }

        public override void Down()
        {
            Execute.Sql(DROP_VIEW);
            Execute.Sql(PreviousMigration.CREATE_VIEW);
        }
    }
}

