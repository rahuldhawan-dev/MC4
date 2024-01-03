using FluentMigrator;
using PreviouslyOnBlossom =
    MapCall.Common.Model.Migrations._2022.MC4687_CreateMostRecentlyInstalledServicesView;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220817074802739), Tags("Production")]
    public class MC4880_AddCoordinatesToMostRecentlyInstalledServicesView : Migration
    {
        public const string VIEW_NAME = PreviouslyOnBlossom.VIEW_NAME;

        public const string OLD_VIEW_SQL = PreviouslyOnBlossom.VIEW_SQL;

        public const string NEW_VIEW_SQL = @"
WITH FilteredServiceCategories AS (
    SELECT ServiceCategoryID
    FROM ServiceCategories
    WHERE Description IN (
        'Fire Retire Service Only',
        'Sewer Retire Service Only',
        'Water Retire Meter Set Only',
        'Water Retire Service Only'
    )
), SelectedServices AS (
    SELECT
        s.PremiseId,
        s.Id,
        s.DateInstalled,
        s.LastUpdated,
        s.MeterSettingSizeId,
        s.ServiceMaterialId,
        s.ServiceSizeId,
        s.CustomerSideMaterialId,
        s.CustomerSideSizeId,
        s.CoordinateId
    FROM Services s
    WHERE s.PremiseId IS NOT NULL
    AND s.DateInstalled IS NOT NULL
    AND s.ServiceCategoryId IS NOT NULL
    AND NOT EXISTS (SELECT 1
                FROM FilteredServiceCategories f
                WHERE f.ServiceCategoryID = s.ServiceCategoryId)
)
SELECT
    s.PremiseId,
    s.Id AS ServiceId,
    s.DateInstalled,
    s.LastUpdated,
    s.MeterSettingSizeId,
    s.ServiceMaterialId,
    s.ServiceSizeId,
    s.CustomerSideMaterialId,
    s.CustomerSideSizeId,
    COALESCE(p.CoordinateId, s.CoordinateId) as [CoordinateId]
FROM SelectedServices s
INNER JOIN Premises p ON p.Id = s.PremiseId
LEFT JOIN SelectedServices uns
    ON uns.PremiseId = s.PremiseId
    AND
       -- most recent DateInstalled
       (uns.DateInstalled > s.DateInstalled OR
        -- matching DateInstalled, most recent LastUpdated
         (uns.DateInstalled = s.DateInstalled AND
          uns.LastUpdated > s.LastUpdated) OR
        -- matching DateInstalled, one has null LastUpdated other doesn't so we take the non-null
         (uns.DateInstalled = s.DateInstalled AND
          s.LastUpdated IS NULL AND
          uns.LastUpdated IS NOT NULL) OR
        -- matching DateInstalled, both have null LastUpdated so take the highest Id
         (uns.DateInstalled = s.DateInstalled AND
          uns.LastUpdated IS NULL AND
          s.LastUpdated IS NULL AND
          uns.Id > s.Id) OR
        -- matching DateInstalled, matching LastUpdated, take the highest Id
         (uns.DateInstalled = s.DateInstalled AND
          uns.LastUpdated = s.LastUpdated AND
          uns.Id > s.Id))
WHERE uns.Id IS NULL";

        public override void Up()
        {
            Execute.Sql($@"ALTER VIEW [{VIEW_NAME}] AS{NEW_VIEW_SQL}");
        }

        public override void Down()
        {
            Execute.Sql($@"ALTER VIEW [{VIEW_NAME}] AS{OLD_VIEW_SQL}");
        }
    }
}

