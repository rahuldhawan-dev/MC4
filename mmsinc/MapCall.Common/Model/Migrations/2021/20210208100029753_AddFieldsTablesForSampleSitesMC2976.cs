using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210208100029753), Tags("Production")]
    public class AddFieldsTablesForSampleSitesMC2976 : Migration
    {
        public const string SAMPLE_SITES_TABLE_NAME = "SampleSites";

        public override void Up()
        {
            this.CreateLookupTableWithValues("SampleSiteSubCollectionTypes", "Discharge", "Influent In Plant", "Effluent In Plant", "Sampling Station", "Flushing Outlet");
            Rename.Column("Sampling_Instructions").OnTable(SAMPLE_SITES_TABLE_NAME).To("SamplingInstructions");
            Alter.Table(SAMPLE_SITES_TABLE_NAME)
                 .AddColumn("SafetyConcerns").AsAnsiString(300).Nullable()
                 .AddForeignKeyColumn("SubCollectionTypeId", "SampleSiteSubCollectionTypes");
            Alter.Column("SamplingInstructions").OnTable(SAMPLE_SITES_TABLE_NAME).AsAnsiString(300).Nullable();
            Alter.Column("ZipCode").OnTable(SAMPLE_SITES_TABLE_NAME).AsAnsiString(10).Nullable();
            Rename.Column("CouponSite").OnTable(SAMPLE_SITES_TABLE_NAME).To("UnregulatedContaminantMonitoringRuleSite");
        }

        public override void Down()
        {
            Rename.Column("UnregulatedContaminantMonitoringRuleSite").OnTable(SAMPLE_SITES_TABLE_NAME).To("CouponSite");
            Delete.Column("SafetyConcerns").FromTable(SAMPLE_SITES_TABLE_NAME);
            Delete.ForeignKeyColumn(SAMPLE_SITES_TABLE_NAME, "SubCollectionTypeId", "SampleSiteSubCollectionTypes");
            Alter.Column("SamplingInstructions").OnTable(SAMPLE_SITES_TABLE_NAME).AsAnsiString(50).Nullable();
            Alter.Column("ZipCode").OnTable(SAMPLE_SITES_TABLE_NAME).AsAnsiString(5).Nullable(); 
            Rename.Column("SamplingInstructions").OnTable(SAMPLE_SITES_TABLE_NAME).To("Sampling_Instructions");
            Delete.Table("SampleSiteSubCollectionTypes");
        }
    }
}