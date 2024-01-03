using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160812093825482), Tags("Production")]
    public class AddColumnsForBugs3107and3098 : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("SampleLocations", "Kitchen Sink", "Bathroom Sink");
            Alter.Table("tblWQSample_Sites")
                 .AddColumn("PreviousMonitoringPeriod").AsBoolean().Nullable()
                 .AddForeignKeyColumn("SampleLocationId", "SampleLocations");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("tblWQSample_Sites", "SampleLocationId", "SampleLocations");
            Delete.Column("PreviousMonitoringPeriod").FromTable("tblWQSample_Sites");
            Delete.Table("SampleLocations");
        }
    }
}
