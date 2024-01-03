using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221025131731591), Tags("Production")]
    public class MC4311AddingUpdatedByColumnToServicesTable : Migration
    {
        public override void Up()
        {
            Alter.Table("Services").AddForeignKeyColumn("LastUpdatedById", "tblPermissions", "RecId");
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
                          EntityName = 'Service'
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
                          EntityName = 'Service')
                  UPDATE Services SET LastUpdatedById = MostRecentUpdate.UserId FROM MostRecentUpdate WHERE Services.Id = MostRecentUpdate.EntityId;");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Services", "LastUpdatedById", "tblPermissions", "RecId");
        }
    }
}

