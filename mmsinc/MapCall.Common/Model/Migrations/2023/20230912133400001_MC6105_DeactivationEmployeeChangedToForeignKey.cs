using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230912133400001), Tags("Production")]
    public class MC6105_DeactivationEmployeeChangedToForeignKey : Migration
    {
        public override void Up()
        {
            Delete.Column("DeactivationEmployee").FromTable("MaintenancePlans");

            Alter.Table("MaintenancePlans").AddColumn("DeactivationEmployeeId").AsInt32().Nullable()
                 .ForeignKey("FK_MaintenancePlans_tblEmployee_DeactivationEmployeeId", "tblEmployee", "tblEmployeeID").Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_MaintenancePlans_tblEmployee_DeactivationEmployeeId").OnTable("MaintenancePlans");
            Delete.Column("DeactivationEmployeeId").FromTable("MaintenancePlans");

            Alter.Table("MaintenancePlans").AddColumn("DeactivationEmployee").AsInt32().Nullable();
        }
    }
}
