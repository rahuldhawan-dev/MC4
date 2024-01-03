using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160513135538876), Tags("Production")]
    public class CreateWQSamplePlansTableForBug2918 : Migration
    {
        public struct TableNames
        {
            public const string PLAN_TYPES = "SamplePlanTypes",
                                PLANS = "SamplePlans";
        }

        public override void Up()
        {
            Create.Table(TableNames.PLAN_TYPES)
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(50);

            Execute.Sql($"INSERT INTO {TableNames.PLAN_TYPES} (Description) VALUES ('Lead Copper');");
            Execute.Sql($"INSERT INTO {TableNames.PLAN_TYPES} (Description) VALUES ('Stage II DBP');");
            Execute.Sql($"INSERT INTO {TableNames.PLAN_TYPES} (Description) VALUES ('Coliform');");

            Create.Table(TableNames.PLANS)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("PublicWaterSupplyId", "PublicWaterSupplies", "Id", false)
                  .WithColumn("CWS").AsBoolean().NotNullable()
                  .WithColumn("NTNC").AsBoolean().NotNullable()
                  .WithForeignKeyColumn("ContactPersonId", "tblEmployees", "tblEmployeeId", false)
                  .WithColumn("MonitoringPeriodFrom").AsDateTime().NotNullable()
                  .WithColumn("MonitoringPeriodTo").AsDateTime().NotNullable()
                  .WithColumn("Standard").AsBoolean().NotNullable()
                  .WithColumn("Reduced").AsBoolean().NotNullable()
                  .WithColumn("MinimumSamplesRequired").AsInt32().NotNullable()
                  .WithColumn("NameOfCertifiedLaboratory").AsString(50).NotNullable()

                   // 13a
                  .WithColumn("SameAsPreviousPeriod").AsBoolean().NotNullable()
                  .WithColumn("AllSamplesTier1").AsBoolean().NotNullable()
                  .WithColumn("Tier2Sites").AsBoolean().NotNullable()
                  .WithColumn("Tier3Sites").AsBoolean().NotNullable()
                  .WithColumn("Tier1SitesVerified").AsBoolean().NotNullable()
                  .WithColumn("LeadServiceLines").AsBoolean().NotNullable()
                  .WithColumn("LeadLinesVerified").AsBoolean().NotNullable()
                  .WithColumn("FiftyPercent").AsBoolean().NotNullable()
                  .WithColumn("Comments").AsCustom("text").Nullable();
        }

        public override void Down()
        {
            Delete.Table(TableNames.PLANS);
            Delete.Table(TableNames.PLAN_TYPES);
        }
    }
}
