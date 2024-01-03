using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200403105019944), Tags("Production")]
    public class AddHealthDepartmentMigrationToCOVIDThingsForMC2151 : Migration
    {
        public override void Up()
        {
            Alter.Table("CovidIssues").AddColumn("HealthDepartmentNotification").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("HealthDepartmentNotification").FromTable("CovidIssues");
        }
    }
}
