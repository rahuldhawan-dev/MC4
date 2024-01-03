using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201020103533743), Tags("Production")]
    public class AddLastUpdatedFieldsToSewerManholesValvesForMC2535 : Migration
    {
        public override void Up()
        {
            Alter.Table("Valves").AddForeignKeyColumn("LastUpdatedById", "tblPermissions", "RecId");
            Alter.Table("SewerOpenings")
                 .AddForeignKeyColumn("LastUpdatedById", "tblPermissions", "RecId")
                 .AddColumn("LastUpdated").AsDateTime().Nullable();

            Execute.Sql("update AuditLogEntries SET EntityName = 'SewerOpening' WHERE EntityName = 'SewerManhole'");
            Execute.Sql(@"with MostRecentUpdateDate (EntityId, TimeStamp)
AS
    (SELECT
        EntityId, MAX(Timestamp)
    FROM
        AuditLogEntries
    WHERE
        AuditEntryType IN ('Update', 'Insert')
    AND
        EntityName = 'SewerOpening'
    GROUP BY
        EntityId),
MostRecentUpdate (EntityId, UserId, LastUpdated)
AS
    (SELECT DISTINCT
        ale.EntityId, ale.UserId, ale.[Timestamp]
    FROM
        AuditLogEntries ale
    INNER JOIN
        MostRecentUpdateDate up
    ON
        up.EntityId = ale.EntityId
    AND
        up.TimeStamp = ale.TimeStamp
    WHERE
        AuditEntryType IN ('Update', 'Insert')
    AND
        EntityName = 'SewerOpening')
UPDATE SewerOpenings SET LastUpdatedById = MostRecentUpdate.UserId, LastUpdated = MostRecentUpdate.LastUpdated FROM MostRecentUpdate WHERE SewerOpenings.Id = MostRecentUpdate.EntityId;
");
            Execute.Sql(@"with MostRecentUpdateDate (EntityId, TimeStamp)
AS
    (SELECT
        EntityId, MAX(Timestamp)
    FROM
        AuditLogEntries
    WHERE
        AuditEntryType IN ('Update', 'Insert')
    AND
        EntityName = 'Valve'
    GROUP BY
        EntityId),
MostRecentUpdate (EntityId, UserId)
AS
    (SELECT DISTINCT
        ale.EntityId, ale.UserId
    FROM
        AuditLogEntries ale
    INNER JOIN
        MostRecentUpdateDate up
    ON
        up.EntityId = ale.EntityId
    AND
        up.TimeStamp = ale.TimeStamp
    WHERE
        AuditEntryType IN ('Update', 'Insert')
    AND
        EntityName = 'Valve')
UPDATE Valves SET LastUpdatedById = MostRecentUpdate.UserId FROM MostRecentUpdate WHERE Valves.Id = MostRecentUpdate.EntityId;");
            const string urlFormat =
                "https://utility.arcgis.com/usrsvcs/servers/efbd2ffeadff48cfb3ef76a3da02668b/rest/services/National/AW_National_OneMap_StagingEdit_SDE/FeatureServer/{0}";
            Alter.Table("AssetTypes").AddColumn("OneMapFeatureSource").AsAnsiString(255).Nullable();
            var valueTable = new[] {
                (UrlId: 3, AssetTypeId: 2),
                (UrlId: 4, AssetTypeId: 1),
                (UrlId: 13, AssetTypeId: 5)
            };
            foreach (var tup in valueTable)
            {
                Update
                   .Table("AssetTypes")
                   .Set(new {OneMapFeatureSource = string.Format(urlFormat, tup.UrlId)})
                   .Where(new {AssetTypeID = tup.AssetTypeId});
            }
        }

        public override void Down()
        {
            Execute.Sql("update AuditLogEntries SET EntityName = 'SewerManhole' WHERE EntityName = 'SewerOpening'");

            Delete.ForeignKeyColumn("Valves", "LastUpdatedById", "tblPermissions", "RecId");
            Delete.ForeignKeyColumn("SewerOpenings", "LastUpdatedById", "tblPermissions", "RecId");
            Delete.Column("LastUpdated").FromTable("SewerOpenings");
        }
    }
}
