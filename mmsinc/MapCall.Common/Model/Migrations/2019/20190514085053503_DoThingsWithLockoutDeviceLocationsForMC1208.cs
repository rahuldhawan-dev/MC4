using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190514085053503), Tags("Production")]
    public class DoThingsWithLockoutDeviceLocationsForMC1208 : Migration
    {
        public const string TABLE_NAME = "LockoutDeviceLocations";

        public override void Up()
        {
            Alter.Table(TABLE_NAME).AddColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true);

            Update.Table(TABLE_NAME).Set(new {IsActive = false}).Where(new {Description = "Circuit Breaker"});

            Insert.IntoTable(TABLE_NAME).Rows(new {Description = "Electrical"}, new {Description = "Other"});

            Alter.Table("LockoutForms").AddColumn("IsolationPointDescription").AsAnsiString(25).Nullable();
        }

        public override void Down()
        {
            Delete.Column("IsActive").FromTable(TABLE_NAME);

            Delete.FromTable(TABLE_NAME).Rows(new {Description = "Electrical"}, new {Description = "Other"});

            Delete.Column("IsolationPointDescription").FromTable("LockoutForms");
        }
    }
}
