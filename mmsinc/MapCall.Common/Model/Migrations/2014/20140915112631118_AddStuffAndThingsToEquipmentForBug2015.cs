using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140915112631118), Tags("Production")]
    public class AddStuffAndThingsToEquipmentForBug2015 : Migration
    {
        public override void Up()
        {
            Alter.Table("Equipment").AddColumn("CreatedAt").AsDateTime().Nullable();
            Alter.Table("Equipment").AddColumn("AssetControlSignOffDate").AsDateTime().Nullable();

            Alter.Table("Equipment")
                 .AddColumn("RequestedById").AsInt32()
                 .ForeignKey("FK_Equipment_tblEmployee_RequestedById", "tblEmployee", "tblEmployeeID").Nullable();

            Alter.Table("Equipment")
                 .AddColumn("AssetControlSignOffById").AsInt32()
                 .ForeignKey("FK_Equipment_tblEmployee_AssetControlSignOffById", "tblEmployee", "tblEmployeeID")
                 .Nullable();

            Alter.Table("Equipment")
                 .AddColumn("IsReplacement").AsBoolean().WithDefaultValue(false).NotNullable();

            Alter.Table("Equipment")
                 .AddColumn("SAPEquipmentIdBeingReplaced").AsInt32().Nullable();

            Alter.Table("Equipment")
                 .AddColumn("CreatedById").AsInt32().Nullable().ForeignKey("FK_Equipment_tblPermissions_CreatedById",
                      "tblPermissions", "RecId");

            Execute.Sql(
                "INSERT INTO NotificationPurposes (ModuleID, Purpose) SELECT ModuleID, 'Equipment In Service' FROM NotificationPurposes WHERE Purpose = 'Equipment Record Created';");
        }

        public override void Down()
        {
            Delete.Column("CreatedAt").FromTable("Equipment");
            Delete.Column("AssetControlSignOffDate").FromTable("Equipment");

            Delete.ForeignKey("FK_Equipment_tblEmployee_RequestedById").OnTable("Equipment");
            Delete.Column("RequestedById").FromTable("Equipment");

            Delete.ForeignKey("FK_Equipment_tblEmployee_AssetControlSignOffById").OnTable("Equipment");
            Delete.Column("AssetControlSignOffById").FromTable("Equipment");

            Delete.Column("IsReplacement").FromTable("Equipment");

            Delete.Column("SAPEquipmentIdBeingReplaced").FromTable("Equipment");

            Delete.ForeignKey("FK_Equipment_tblPermissions_CreatedById").OnTable("Equipment");
            Delete.Column("CreatedById").FromTable("Equipment");

            Execute.Sql("DELETE FROM NotificationPurposes WHERE Purpose = 'Equipment In Service';");
        }
    }
}
