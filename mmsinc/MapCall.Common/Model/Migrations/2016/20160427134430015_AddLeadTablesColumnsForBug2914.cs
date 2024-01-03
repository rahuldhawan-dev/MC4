using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160427134430015), Tags("Production")]
    public class AddLeadTablesColumnsForBug2914 : Migration
    {
        public struct TableNames
        {
            public const string SAMPLE_SITES = "tblWQSample_Sites";
        }

        public override void Up()
        {
            this.CreateLookupTableWithValues("LeadCopperTierClassifications", "Tier 1", "Tier 2", "Tier 3", "Other");
            this.CreateLookupTableWithValues("LeadCopperTierSampleCategories", "1", "2", "3", "4");
            // Is Alternate Site
            Alter.Table("Services")
                 .AddColumn("LeadAndCopperCommunicationProvided").AsBoolean().Nullable();

            Alter.Table(TableNames.SAMPLE_SITES)
                 .AddForeignKeyColumn("LeadCopperTierClassificationId", "LeadCopperTierClassifications").Nullable()
                 .AddForeignKeyColumn("LeadCopperTierSampleCategoryId", "LeadCopperTierSampleCategories").Nullable()
                 .AddColumn("IsAlternateSite").AsBoolean().Nullable()
                 .AddColumn("ResourceDistributionMaps").AsBoolean().Nullable()
                 .AddColumn("ResourceCapitalImprovement").AsBoolean().Nullable()
                 .AddColumn("ResourceUtilityRecords").AsBoolean().Nullable()
                 .AddColumn("ResourceSamplingResults").AsBoolean().Nullable()
                 .AddColumn("ResourceInterviewsPersonel").AsBoolean().Nullable()
                 .AddColumn("ResourceCommunitySurvey").AsBoolean().Nullable()
                 .AddColumn("ResourceCountyAppraisal").AsBoolean().Nullable()
                 .AddColumn("ResourceContacts").AsBoolean().Nullable()
                 .AddColumn("ResourceSurveyResults").AsBoolean().Nullable()
                 .AddColumn("ResourceInterviewsResidents").AsBoolean().Nullable()
                 .AddColumn("ResourceInterviewsContactors").AsBoolean().Nullable()
                 .AddColumn("ResourceLeadCheckSwabs").AsBoolean().Nullable()
                 .AddColumn("OtherSources").AsAnsiString(255).Nullable();
        }

        public override void Down()
        {
            this.DeleteForeignKeyColumn(TableNames.SAMPLE_SITES, "LeadCopperTierClassificationId",
                "LeadCopperTierClassifications");
            Delete.Table("LeadCopperTierClassifications");
            this.DeleteForeignKeyColumn(TableNames.SAMPLE_SITES, "LeadCopperTierSampleCategoryId",
                "LeadCopperTierSampleCategories");
            Delete.Table("LeadCopperTierSampleCategories");
            Delete.Column("IsAlternateSite").FromTable(TableNames.SAMPLE_SITES);
            Delete.Column("ResourceDistributionMaps").FromTable(TableNames.SAMPLE_SITES);
            Delete.Column("ResourceCapitalImprovement").FromTable(TableNames.SAMPLE_SITES);
            Delete.Column("ResourceUtilityRecords").FromTable(TableNames.SAMPLE_SITES);
            Delete.Column("ResourceSamplingResults").FromTable(TableNames.SAMPLE_SITES);
            Delete.Column("ResourceInterviewsPersonel").FromTable(TableNames.SAMPLE_SITES);
            Delete.Column("ResourceCommunitySurvey").FromTable(TableNames.SAMPLE_SITES);
            Delete.Column("ResourceCountyAppraisal").FromTable(TableNames.SAMPLE_SITES);
            Delete.Column("ResourceContacts").FromTable(TableNames.SAMPLE_SITES);
            Delete.Column("ResourceSurveyResults").FromTable(TableNames.SAMPLE_SITES);
            Delete.Column("ResourceInterviewsResidents").FromTable(TableNames.SAMPLE_SITES);
            Delete.Column("ResourceInterviewsContactors").FromTable(TableNames.SAMPLE_SITES);
            Delete.Column("ResourceLeadCheckSwabs").FromTable(TableNames.SAMPLE_SITES);
            Delete.Column("OtherSources").FromTable(TableNames.SAMPLE_SITES);

            Delete.Column("LeadAndCopperCommunicationProvided").FromTable("Services");
        }
    }
}
