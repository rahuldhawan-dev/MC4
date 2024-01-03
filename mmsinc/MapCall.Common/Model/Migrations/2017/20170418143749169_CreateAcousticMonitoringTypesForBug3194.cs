using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170418143749169), Tags("Production")]
    public class CreateAcousticMonitoringTypesForBug3194 : Migration
    {
        public const string TABLE_NAME = "AcousticMonitoringTypes";

        public override void Up()
        {
            Alter.Column("Description").OnTable("WorkOrderRequesters").AsString(25);
            Update.Table("WorkOrderRequesters").Set(new {Description = "Acoustic Monitoring"})
                  .Where(new {Description = "Echologics"});

            Create.LookupTable(TABLE_NAME);

            Insert.IntoTable(TABLE_NAME).Rows(
                new {Description = "EchoShore DX"},
                new {Description = "EchoShore cell"},
                new {Description = "EchoShore TX"},
                new {Description = "EchoShore M"},
                new {Description = "MLog"},
                new {Description = "Permalog Plus"},
                new {Description = "PermaNet Plus"}
            );

            Alter.Table("WorkOrders").AddForeignKeyColumn("AcousticMonitoringTypeId", TABLE_NAME);
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("WorkOrders", "AcousticMonitoringTypeId", TABLE_NAME);
            Delete.Table(TABLE_NAME);
        }
    }
}
