using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220410005745198), Tags("Production")]
    public class MC3916AddingDeviceCategoryColumnToServiceInstallation : Migration
    {
        public override void Up()
        {
            Alter.Table("ServiceInstallations")
                 .AddColumn("MeterDeviceCategory").AsAnsiString(25).Nullable()
                 .AddColumn("Register1DeviceCategory").AsAnsiString(25).Nullable()
                 .AddColumn("Register2DeviceCategory").AsAnsiString(25).Nullable();
        }

        public override void Down()
        {
            Delete.Column("MeterDeviceCategory").FromTable("ServiceInstallations");
            Delete.Column("Register1DeviceCategory").FromTable("ServiceInstallations");
            Delete.Column("Register2DeviceCategory").FromTable("ServiceInstallations");
        }
    }
}

