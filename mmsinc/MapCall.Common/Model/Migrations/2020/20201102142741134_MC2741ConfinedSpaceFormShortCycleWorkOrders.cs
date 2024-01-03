using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201102142741134), Tags("Production")]
    public class MC2741ConfinedSpaceFormShortCycleWorkOrders : Migration
    {
        public override void Up()
        {
            Alter.Table("ConfinedSpaceForms")
                 .AddForeignKeyColumn("CreatedByUserId", "tblPermissions", "RecId").Nullable()
                 .AddColumn("CreatedAt").AsDateTime().Nullable()
                 .AddForeignKeyColumn("ShortCycleWorkOrderId", "ShortCycleWorkOrders").Nullable()
                 .AlterColumn("ProductionWorkOrderId").AsInt32().Nullable();

            // Populate the CreatedByUserId field from AuditLogEntries
            // This column should be not nullable, but it's impossible to do for local dev without having access
            // to the full AuditLogEntries table.
            Execute.Sql(@"update ConfinedSpaceForms 
set CreatedByUserId = (select top 1 UserId from AuditLogEntries where EntityName = 'ConfinedSpaceForm' and AuditEntryType = 'Insert' and EntityId = ConfinedSpaceForms.Id)");
            Execute.Sql(@"update ConfinedSpaceForms 
set CreatedAt = (select top 1 Timestamp from AuditLogEntries where EntityName = 'ConfinedSpaceForm' and AuditEntryType = 'Insert' and EntityId = ConfinedSpaceForms.Id)");
        }

        public override void Down()
        {
            // NOTE: Can not change ProductionWorkOrderId back to not-nullable.
            Delete.Column("CreatedAt").FromTable("ConfinedSpaceForms");
            Delete.ForeignKeyColumn("ConfinedSpaceForms", "CreatedByUserId", "tblPermissions", "RecId");
            Delete.ForeignKeyColumn("ConfinedSpaceForms", "ShortCycleWorkOrderId", "ShortCycleWorkOrders");
        }
    }
}
