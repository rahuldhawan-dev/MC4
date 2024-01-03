using FluentMigrator;
using Humanizer;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20230314092846121), Tags("Production")]
    public class MC5116_AdjustFormulaBasedChangeTrackingColumns : Migration
    {
        private void PullCreatedAtFromAuditLog(
            string entityName,
            string tableName = null,
            string idColumn = "Id")
        {
            tableName = tableName ?? entityName.Pluralize();

            Create.Column("CreatedAt").OnTable(tableName).AsDateTime().Nullable();
            
            this.UpdateColumnFromAuditLog(entityName, "CreatedAt", "Timestamp", "Create", tableName, idColumn);
        }

        private void PullUpdatedAtFromAuditLog(
            string entityName,
            string tableName = null,
            string idColumn = "Id")
        {
            tableName = tableName ?? entityName.Pluralize();

            Create.Column("UpdatedAt").OnTable(tableName).AsDateTime().Nullable();
            
            this.UpdateColumnFromAuditLog(entityName, "UpdatedAt", "Timestamp", "Update", tableName, idColumn);
        }

        private void PullCreatedByFromAuditLog(
            string entityName,
            string tableName = null,
            string idColumn = "Id")
        {
            tableName = tableName ?? entityName.Pluralize();

            Create
               .Column("CreatedById")
               .OnTable(tableName)
               .AsInt32()
               .ForeignKey($"FK_{tableName}_tblPermissions_CreatedById", "tblPermissions", "RecId")
               .Nullable();
            
            this.UpdateColumnFromAuditLog(entityName, "CreatedById", "UserId", "Create", tableName, idColumn);
            
            Execute.Sql($@"
UPDATE t
SET t.CreatedById = u.RecId
FROM {tableName} t
INNER JOIN tblPermissions u
ON u.UserName = 'mcadmin'
WHERE t.CreatedById IS NULL");

            Alter
               .Column("CreatedById")
               .OnTable(tableName)
               .AsInt32()
               .NotNullable();
        }

        private void PullUpdatedByFromAuditLog(
            string entityName,
            string tableName = null,
            string idColumn = "Id")
        {
            tableName = tableName ?? entityName.Pluralize();

            Create
               .Column("UpdatedById")
               .OnTable(tableName)
               .AsInt32()
               .ForeignKey($"FK_{tableName}_tblPermissions_UpdatedById", "tblPermissions", "RecId")
               .Nullable();
            
            this.UpdateColumnFromAuditLog(
                entityName,
                "UpdatedById",
                "UserId",
                "Update",
                tableName,
                idColumn);
            
            Execute.Sql($@"
UPDATE t
SET t.UpdatedById = u.RecId
FROM {tableName} t
INNER JOIN tblPermissions u
ON u.UserName = 'mcadmin'
WHERE t.UpdatedById IS NULL");

            Alter
               .Column("UpdatedById")
               .OnTable(tableName)
               .AsInt32()
               .NotNullable();
        }
        
        public override void Up()
        {
            PullCreatedAtFromAuditLog("RoadwayImprovementNotification");
            
            PullCreatedByFromAuditLog("RoadwayImprovementNotification");

            PullUpdatedAtFromAuditLog("DocumentLink", "DocumentLink", "DocumentLinkId");
            PullUpdatedAtFromAuditLog("Incident");
            
            PullUpdatedByFromAuditLog("DocumentLink", "DocumentLink", "DocumentLinkId");
        }

        public override void Down()
        {
            Delete.Column("CreatedAt").FromTable("RoadwayImprovementNotifications");

            Delete.ForeignKeyColumn(
                "RoadwayImprovementNotifications",
                "CreatedById",
                "tblPermissions",
                "RecId");

            Delete.Column("UpdatedAt").FromTable("DocumentLink");
            Delete.Column("UpdatedAt").FromTable("Incidents");
            
            Delete.ForeignKeyColumn("DocumentLink", "UpdatedById", "tblPermissions", "RecId");
        }
    }
}

