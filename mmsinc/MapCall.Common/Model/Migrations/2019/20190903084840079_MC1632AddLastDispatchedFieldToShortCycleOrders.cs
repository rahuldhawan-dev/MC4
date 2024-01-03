using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190903084840079), Tags("Production")]
    public class MC1632AddLastDispatchedFieldToShortCycleOrders : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders").AddColumn("LastDispatched").AsDateTime().Nullable();
            Execute.Sql(
                "update ShortCycleWorkOrders set LastDispatched = (SELECT TOP 1 ale.[TimeStamp] FROM AuditLogEntries ale WHERE ale.EntityId = ShortCycleWorkOrders.Id AND ale.entityName = 'ShortCycleWorkOrder' AND ale.AuditEntryType = 'Update' AND ale.FieldName <> 'ActiveMQStatus' ORDER BY ale.[Timestamp] desc)");
        }

        public override void Down()
        {
            Delete.Column("LastDispatched").FromTable("ShortCycleWorkOrders");
        }
    }
}
