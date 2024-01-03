using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220221094558877), Tags("Production")]
    public class MC4132UpdateLeadCopperTierClassificationDescriptions : Migration
    {
        public override void Up()
        {
            // 1. Preferred Contact Method Types

            this.CreateLookupTableWithValues("SampleSiteCustomerContactMethods",
                "Email",
                "Mail",
                "Phone",
                "Text Message");

            Alter.Table("SampleSites")
                 .AddForeignKeyColumn("CustomerContactMethodId", "SampleSiteCustomerContactMethods");

            // 2. Validation Method Types

            this.CreateLookupTableWithValues("SampleSiteLeadCopperValidationMethods",
                100,
                "Visual confirmation of lead pipe on utility or customer side",
                "Lead swab test on customer plumbing and three (3) non-consecutive joints",
                "Document of building construction after 1986 (Tier 3 only!)", 
                "Pending");

            Alter.Table("SampleSites")
                 .AddForeignKeyColumn("LeadCopperValidationMethodId", "SampleSiteLeadCopperValidationMethods");

            // 3. Rework Lead Copper Tier Classifications

            Alter.Table("LeadCopperTierClassifications").AlterColumn("Description").AsAnsiString(255).NotNullable();

            Execute.Sql("update LeadCopperTierClassifications set Description = 'Tier 1- Single Family Residences with Lead Pipe or Lead Service Lines' where Id = 1");
            Execute.Sql("update LeadCopperTierClassifications set Description = 'Tier 2- Building & Multifamily Residences with Copper Pipes & Lead Solder installed after 1982' where Id = 2");
            Execute.Sql("update LeadCopperTierClassifications set Description = 'Tier 3- Single Family Residences with Copper Pipe & Lead Solder installed before 1983' where Id = 3");
            Execute.Sql("update LeadCopperTierClassifications set Tier = 'Other' where Id = 4");
            Execute.Sql("update LeadCopperTierClassifications set Description = 'Tier 1- Single Family Residences with Copper Pipes & Lead Solder installed after 1982' where Id = 5");
            Execute.Sql("update LeadCopperTierClassifications set Description = 'Tier 2- Building & Multifamily Residences with Lead Pipe or Lead Service Lines' where Id = 6");

            Delete.Column("Tier").FromTable("LeadCopperTierClassifications");
            Rename.Table("LeadCopperTierClassifications").To("SampleSiteLeadCopperTierClassifications");

            // 4. Other things

            Rename.Table("LeadCopperTierSampleCategories").To("SampleSiteLeadCopperTierSampleCategories");
            Alter.Table("SampleSites").AddColumn("LeadCopperTierThreeExplanation").AsAnsiString(255).Nullable();
        }

        public override void Down()
        {
            // 4. Other things

            Delete.Column("LeadCopperTierThreeExplanation").FromTable("SampleSites");
            Rename.Table("SampleSiteLeadCopperTierSampleCategories").To("LeadCopperTierSampleCategories");

            // 3. Rework Lead Copper Tier Classifications

            Rename.Table("SampleSiteLeadCopperTierClassifications").To("LeadCopperTierClassifications");
            Alter.Table("LeadCopperTierClassifications").AddColumn("Tier").AsAnsiString(12).Nullable();

            Execute.Sql("update LeadCopperTierClassifications set Tier = 'Tier 1', Description = 'Lead Service' where Id = 1");
            Execute.Sql("update LeadCopperTierClassifications set Tier = 'Tier 2', Description = 'Building & Multifamily Residences with Copper Pipes & Lead Solder installed after 1982' where Id = 2");
            Execute.Sql("update LeadCopperTierClassifications set Tier = 'Tier 3', Description = 'Single Family Structures that contain Copper Pipe with Lead Solder installed before 1983' where Id = 3");
            Execute.Sql("update LeadCopperTierClassifications set Tier = 'Tier 3-Other' where Id = 4");
            Execute.Sql("update LeadCopperTierClassifications set Tier = 'Tier 1', Description = 'Single Family with Copper & Lead Solder installed after 1982' where Id = 5");
            Execute.Sql("update LeadCopperTierClassifications set Tier = 'Tier 2', Description = 'Building & Multifamily Residences containing Lead Pipe or Service Lines' where Id = 6");

            Alter.Table("LeadCopperTierClassifications").AlterColumn("Description").AsAnsiString(100).NotNullable();

            // 2. Validation Method Types

            Delete.ForeignKeyColumn("SampleSites", "LeadCopperValidationMethodId", "SampleSiteLeadCopperValidationMethods");
            Delete.Table("SampleSiteLeadCopperValidationMethods");

            // 3. Preferred Contact Method Types

            Delete.ForeignKeyColumn("SampleSites", "CustomerContactMethodId", "SampleSiteCustomerContactMethods");
            Delete.Table("SampleSiteCustomerContactMethods");
        }
    }
}

