using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230823134000001), Tags("Production")]
    public class MC6105_AddDeactivationColumnsToMaintenancePlan : Migration
    {
        public override void Up()
        {
            Alter.Table("MaintenancePlans").AddColumn("DeactivationReason").AsAnsiString(150).Nullable()
                 .AddColumn("DeactivationEmployee").AsInt32().Nullable()
                 .AddColumn("DeactivationDate").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("DeactivationReason").FromTable("MaintenancePlans");
            Delete.Column("DeactivationEmployee").FromTable("MaintenancePlans");
            Delete.Column("DeactivationDate").FromTable("MaintenancePlans");
        }
    }
}

