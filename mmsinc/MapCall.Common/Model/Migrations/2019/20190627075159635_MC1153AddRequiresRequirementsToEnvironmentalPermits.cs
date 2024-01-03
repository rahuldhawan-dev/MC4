using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190627075159635), Tags("Production")]
    public class MC1153AddRequiresRequirementsToEnvironmentalPermits : Migration
    {
        public override void Up()
        {
            Alter.Table("EnvironmentalPermits").AddColumn("RequiresRequirements").AsBoolean().Nullable();
            Execute.Sql(
                "UPDATE EnvironmentalPermits SET RequiresRequirements = CASE WHEN (EXISTS (SELECT 1 from EnvironmentalPermitRequirements epr WHERE EnvironmentalPermitId = epr.PermitId)) THEN 1 ELSE 0 END");
            Alter.Column("RequiresRequirements").OnTable("EnvironmentalPermits").AsBoolean().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("RequiresRequirements").FromTable("EnvironmentalPermits");
        }
    }
}
