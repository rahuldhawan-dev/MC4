using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140617161015766), Tags("Production")]
    public class AddEquipmentIDToWorkOrdersForBug1948 : Migration
    {
        public const string TABLE_NAME = "WorkOrders",
                            COLUMN_NAME = "EquipmentID",
                            FOREIGN_TABLE_NAME = "Equipment",
                            FOREIGN_KEY = "FK_WorkOrders_Equipment_EquipmentID";

        public override void Up()
        {
            Alter.Table(TABLE_NAME)
                 .AddColumn(COLUMN_NAME)
                 .AsInt32()
                 .ForeignKey(FOREIGN_KEY, FOREIGN_TABLE_NAME, "EquipmentID")
                 .Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKey(FOREIGN_KEY).OnTable(TABLE_NAME);
            Delete.Column(COLUMN_NAME).FromTable(TABLE_NAME);
        }
    }
}
