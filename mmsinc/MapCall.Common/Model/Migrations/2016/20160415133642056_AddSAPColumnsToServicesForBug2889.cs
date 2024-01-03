using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160415133642056), Tags("Production")]
    public class AddSAPColumnsToServicesForBug2889 : Migration
    {
        public override void Up()
        {
            Alter.Table("Services")
                 .AddColumn("SAPNotificationNumber").AsInt64().Nullable()
                 .AddColumn("SAPWorkOrderNumber").AsInt64().Nullable();
        }

        public override void Down()
        {
            Delete.Column("SAPNotificationNumber").FromTable("Services");
            Delete.Column("SAPWorkOrderNumber").FromTable("Services");
        }
    }
}
