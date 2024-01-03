using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190618114054786), Tags("Production")]
    public class MC1152AddFieldToEnvironmentalPermits : Migration
    {
        public override void Up()
        {
            Alter.Table("EnvironmentalPermits").AddColumn("ReportingRequired").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("ReportingRequired").FromTable("EnvironmentalPermits");
        }
    }
}
