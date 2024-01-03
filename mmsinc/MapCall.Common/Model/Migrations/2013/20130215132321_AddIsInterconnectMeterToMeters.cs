using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130215132321), Tags("Production")]
    public class AddIsInterconnectMeterToMeters : Migration
    {
        public override void Up()
        {
            Alter.Table("Meters").AddColumn("IsInterconnectMeter").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("IsInterconnectMeter").FromTable("Meters");
        }
    }
}
