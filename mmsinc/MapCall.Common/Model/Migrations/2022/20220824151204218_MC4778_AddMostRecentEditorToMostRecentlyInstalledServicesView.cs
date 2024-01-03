using FluentMigrator;
using OnTheLastEpisodeOfGoldenGirls =
    MapCall.Common.Model.Migrations._2022.MC4880_AddCoordinatesToMostRecentlyInstalledServicesView;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220824151204218), Tags("Production")]
    public class MC4778_AddMostRecentEditorToMostRecentlyInstalledServicesView : Migration
    {
        public const string VIEW_NAME = OnTheLastEpisodeOfGoldenGirls.VIEW_NAME;

        public const string OLD_VIEW_SQL = OnTheLastEpisodeOfGoldenGirls.NEW_VIEW_SQL;

        public const string NEW_VIEW_SQL = @"
WITH InsertUpdateServiceAuditLogEntries AS (
    SELECT
        UserId,
        AuditEntryType,
        EntityId,
        FieldName,
        Timestamp,
        ContractorUserId
    FROM AuditLogEntries
    WHERE EntityName = 'Service'
    AND AuditEntryType IN ('Insert', 'Update')
), ServiceMaterialAuditLogEntries AS (
    SELECT
        UserId,
        AuditEntryType,
        EntityId,
        Timestamp,
        ContractorUserId
    FROM InsertUpdateServiceAuditLogEntries
    WHERE FieldName = 'ServiceMaterial'
), MostRecentServiceMaterialAuditLogEntries AS (
    SELECT
        recent.UserId,
        recent.AuditEntryType,
        recent.EntityId,
        recent.Timestamp,
        recent.ContractorUserId
    FROM ServiceMaterialAuditLogEntries recent
    LEFT JOIN ServiceMaterialAuditLogEntries moreRecent
    ON recent.EntityId = moreRecent.EntityId
    AND recent.Timestamp > recent.Timestamp
    WHERE moreRecent.Timestamp IS NULL
), CustomerSideMaterialAuditLogEntries AS (
    SELECT
        UserId,
        AuditEntryType,
        EntityId,
        Timestamp,
        ContractorUserId
    FROM InsertUpdateServiceAuditLogEntries
    WHERE FieldName = 'CustomerSideMaterial'
), MostRecentCustomerSideMaterialAuditLogEntries AS (
    SELECT
        recent.UserId,
        recent.EntityId,
        recent.ContractorUserId
    FROM CustomerSideMaterialAuditLogEntries recent
    LEFT JOIN CustomerSideMaterialAuditLogEntries moreRecent
    ON recent.EntityId = moreRecent.EntityId
    AND recent.Timestamp > recent.Timestamp
    WHERE moreRecent.Timestamp IS NULL
), FilteredServiceCategories AS (
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
    serviceMaterialEntry.UserId as [ServiceMaterialSetById],
    customerMaterialEntry.UserId as [CustomerMaterialSetById],
    COALESCE(p.CoordinateId, s.CoordinateId) as [CoordinateId]
FROM MostRecentServices s
INNER JOIN Premises p ON p.Id = s.PremiseId
LEFT JOIN MostRecentServiceMaterialAuditLogEntries serviceMaterialEntry
    ON serviceMaterialEntry.EntityId = s.Id
LEFT JOIN MostRecentCustomerSideMaterialAuditLogEntries customerMaterialEntry
    ON customerMaterialEntry.EntityId = s.Id";

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

