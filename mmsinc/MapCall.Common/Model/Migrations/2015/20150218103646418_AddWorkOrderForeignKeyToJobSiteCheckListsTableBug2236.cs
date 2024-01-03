using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150218103646418), Tags("Production")]
    public class AddWorkOrderForeignKeyToJobSiteCheckListsTableBug2236 : Migration
    {
        private const string TABLE = "JobSiteCheckLists";

        public override void Up()
        {
            Create.ForeignKey("FK_JobSiteCheckLists_WorkOrders_MapCallWorkOrderId")
                  .FromTable(TABLE).ForeignColumn("MapCallWorkOrderId")
                  .ToTable("WorkOrders").PrimaryColumn("WorkOrderID");

            Rename.Column("WorkOrderId").OnTable(TABLE).To("SAPWorkOrderId");

            Alter.Column("SAPWorkOrderId").OnTable(TABLE)
                 .AsString(50).Nullable();
        }

        public override void Down()
        {
            Execute.Sql("UPDATE [JobSiteCheckLists] SET [SAPWorkOrderId] = 0 where SAPWorkOrderId is null");
            Alter.Column("SAPWorkOrderId").OnTable(TABLE)
                 .AsString(50).NotNullable();

            Rename.Column("SAPWorkOrderId").OnTable(TABLE).To("WorkOrderId");
            Delete.ForeignKey("FK_JobSiteCheckLists_WorkOrders_MapCallWorkOrderId")
                  .OnTable(TABLE);
        }
    }
}
