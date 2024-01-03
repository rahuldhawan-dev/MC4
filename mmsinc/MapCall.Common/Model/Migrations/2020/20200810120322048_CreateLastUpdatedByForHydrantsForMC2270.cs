using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200810120322048), Tags("Production")]
    public class CreateLastUpdatedByForHydrantsForMC2270 : Migration
    {
        public override void Up()
        {
            Alter.Table("Hydrants").AddForeignKeyColumn("LastUpdatedById", "tblPermissions", "RecId");

            Execute.Sql(@"
with MostRecentUpdateDate (EntityId, TimeStamp)
AS
    (SELECT
        EntityId, MAX(Timestamp)
    FROM
        AuditLogEntries
    WHERE
        AuditEntryType IN ('Update', 'Insert')
    AND
        EntityName = 'Hydrant'
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
        EntityName = 'Hydrant')
UPDATE Hydrants SET LastUpdatedById = MostRecentUpdate.UserId FROM MostRecentUpdate WHERE Hydrants.Id = MostRecentUpdate.EntityId;");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Hydrants", "LastUpdatedById", "tblPermissions", "RecId");
        }
    }
}

