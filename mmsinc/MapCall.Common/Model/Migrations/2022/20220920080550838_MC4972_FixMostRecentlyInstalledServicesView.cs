﻿using FluentMigrator;
using OstensiblyOnLegion =
    MapCall.Common.Model.Migrations._2022.MC4778_AddMostRecentEditorToMostRecentlyInstalledServicesView;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220920080550838), Tags("Production")]
    public class MC4972_FixMostRecentlyInstalledServicesView : Migration
    {
        public const string VIEW_NAME = OstensiblyOnLegion.VIEW_NAME;

        public const string OLD_VIEW_SQL = OstensiblyOnLegion.NEW_VIEW_SQL;

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
), MostRecentServices AS (
    SELECT
        recent.PremiseId,
        recent.Id,
        recent.DateInstalled,
        recent.LastUpdated,
        recent.MeterSettingSizeId,
        recent.ServiceMaterialId,
        recent.ServiceSizeId,
        recent.CustomerSideMaterialId,
        recent.CustomerSideSizeId,
        recent.CoordinateId
    FROM SelectedServices recent
    LEFT JOIN SelectedServices moreRecent
          ON moreRecent.PremiseId = recent.PremiseId
          AND
             -- most recent DateInstalled
             (moreRecent.DateInstalled > recent.DateInstalled OR
              -- matching DateInstalled, most recent LastUpdated
               (moreRecent.DateInstalled = recent.DateInstalled AND
                moreRecent.LastUpdated > recent.LastUpdated) OR
              -- matching DateInstalled, one has null LastUpdated other doesn't so we take the non-null
               (moreRecent.DateInstalled = recent.DateInstalled AND
                recent.LastUpdated IS NULL AND
                moreRecent.LastUpdated IS NOT NULL) OR
              -- matching DateInstalled, both have null LastUpdated so take the highest Id
               (moreRecent.DateInstalled = recent.DateInstalled AND
                moreRecent.LastUpdated IS NULL AND
                recent.LastUpdated IS NULL AND
                moreRecent.Id > recent.Id) OR
              -- matching DateInstalled, matching LastUpdated, take the highest Id
               (moreRecent.DateInstalled = recent.DateInstalled AND
                moreRecent.LastUpdated = recent.LastUpdated AND
                moreRecent.Id > recent.Id))
    WHERE moreRecent.Id IS NULL
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
    NULL as [ServiceMaterialSetById],
    NULL as [CustomerMaterialSetById],
    COALESCE(p.CoordinateId, s.CoordinateId) as [CoordinateId]
FROM MostRecentServices s
INNER JOIN Premises p ON p.Id = s.PremiseId";

        public override void Up()
        {
            Execute.Sql($"ALTER VIEW [{VIEW_NAME}] AS{NEW_VIEW_SQL}");
        }

        public override void Down()
        {
            Execute.Sql($"ALTER VIEW [{VIEW_NAME}] AS{OLD_VIEW_SQL}");
        }
    }
}

