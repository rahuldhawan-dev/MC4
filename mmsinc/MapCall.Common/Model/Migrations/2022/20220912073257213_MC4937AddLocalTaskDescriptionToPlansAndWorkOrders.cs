using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220912073257213), Tags("Production")]
    public class MC4937AddLocalTaskDescriptionToPlansAndWorkOrders : Migration
    {
        public override void Up()
        {
            Alter.Table("MaintenancePlans").AddColumn("LocalTaskDescription").AsAnsiString(75).Nullable();
            Alter.Table("ProductionWorkOrders").AddColumn("LocalTaskDescription").AsAnsiString(75).Nullable();
        }

        public override void Down()
        {
            Delete.Column("LocalTaskDescription").FromTable("MaintenancePlans");
            Delete.Column("LocalTaskDescription").FromTable("ProductionWorkOrders");
        }
    }
}

