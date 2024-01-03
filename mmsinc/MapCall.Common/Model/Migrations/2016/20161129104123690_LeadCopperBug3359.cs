using System;
using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161129104123690), Tags("Production")]
    public class LeadCopperBug3359 : Migration
    {
        public override void Up()
        {
            Alter.Column("Description").OnTable("LeadCopperTierSampleCategories").AsString(100).NotNullable();
            Alter.Table("LeadCopperTierSampleCategories").AddColumn("DisplayValue").AsString(5).Nullable();

            Action<string, string> doUpdate = (old, update) => {
                Update.Table("LeadCopperTierSampleCategories").Set(new {Description = update, DisplayValue = old})
                      .Where(new {Description = old});
            };

            doUpdate("i", "Single family residence with lead service line");
            doUpdate("ii", "Single family residence with lead solder copper piping constructed after 1982");
            doUpdate("iii", "Single family residence with lead plumbing after 1982");
            doUpdate("iv", "Multi-family residence with lead service line");
            doUpdate("v", "Multi-family residence with lead solder copper piping constructed after 1982");
            doUpdate("vi", "Multi-family residence with lead plumbing");
            doUpdate("vii", "Single family home with lead solder copper piping constructed before 1983");
            doUpdate("viii", "Single family home that does not meet Tier 1, 2, or 3 criteria");
            doUpdate("ix", "Multi-family home that does not meet Tier 1, 2, or 3 criteria");
            doUpdate("x", "Non-residential building with lead service line");
            doUpdate("xi", "Non-residential building with lead solder copper piping constructed after 1982");
            doUpdate("xii", "Non - residential building with lead plumbing");
            doUpdate("xiii", "Non-residential building with lead solder copper piping constructed before 1983");
            doUpdate("xiv", "Non-residential building that does not meet Tier 1, 2, or 3 criteria");

            Alter.Column("DisplayValue").OnTable("LeadCopperTierSampleCategories").AsString(5).NotNullable();

            this.CreateLookupTableWithValues("SampleSitePointOfUseTreatmentTypes", "None", "Entire Building",
                "Individual Tap(s)");

            Alter.Table("tblWQSample_Sites")
                 .AddColumn("SampleSitePointOfUseTreatmentTypeId").AsInt32().Nullable()
                 .ForeignKey(
                      "FK_tblWQSample_Sites_SampleSitePointOfUseTreatmentTypes_SampleSitePointOfUseTreatmentTypeId",
                      "SampleSitePointOfUseTreatmentTypes", "Id");
        }

        public override void Down()
        {
            Delete.ForeignKey(
                       "FK_tblWQSample_Sites_SampleSitePointOfUseTreatmentTypes_SampleSitePointOfUseTreatmentTypeId")
                  .OnTable("tblWQSample_Sites");

            Delete.Column("SampleSitePointOfUseTreatmentTypeId").FromTable("tblWQSample_Sites");
            Delete.Table("SampleSitePointOfUseTreatmentTypes");

            // Set data back to how it was
            Execute.Sql("UPDATE LeadCopperTierSampleCategories set Description = DisplayValue");

            Delete.Column("DisplayValue").FromTable("LeadCopperTierSampleCategories");
        }
    }
}
