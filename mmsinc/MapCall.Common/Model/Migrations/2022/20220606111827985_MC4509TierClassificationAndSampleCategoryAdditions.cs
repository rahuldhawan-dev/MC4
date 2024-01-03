using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220606111827985), Tags("Production")]
    // ReSharper disable once InconsistentNaming
    public class MC4509TierClassificationAndSampleCategoryAdditions : Migration
    {
        public override void Up()
        {
            var existingClassificationIds = new[] { 1, 2, 3, 4, 5, 6 };
            var existingSampleCategoryIds = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            var stateIdsExceptIllinois = new[] {
                24, 23, 26, 25, 5, 27, 28, 29, 6, 22, 7, 8, 30, 9, 31, 10, 32, 34, 11, 33, 35, 36, 12, 37, 38, 43, 44, 39, 41, 1, 42, 40, 2, 45, 46, 47, 21, 3,
                48, 49, 50, 13, 51, 52, 14, 53, 20, 54, 15, 55
            };

            /* 1. We need to support classifications and states being m2m */
            Create.Table("SampleSiteLeadCopperTierClassificationsStates")
                  .WithForeignKeyColumn(
                       "SampleSiteLeadCopperTierClassificationId", 
                       "SampleSiteLeadCopperTierClassifications", 
                       "Id", 
                       false, 
                       "FK_SampleSiteLeadCopperTierClassificationsStates_SampleSiteLeadCopperTierClassifications_Id")
                  .WithForeignKeyColumn(
                       "StateId", 
                       "States", 
                       "StateId", 
                       false, 
                       "FK_SampleSiteLeadCopperTierClassificationsStates_States_StateId");

            /* 2. We also need to support classifications and sample categories being m2m */
            Create.Table("SampleSiteLeadCopperTierClassificationsSampleSiteLeadCopperTierSampleCategories")
                  .WithForeignKeyColumn(
                       "SampleSiteLeadCopperTierClassificationId", 
                       "SampleSiteLeadCopperTierClassifications", 
                       "Id", 
                       false,
                       "FK_ClassificationsSampleCategories_SampleSiteLeadCopperTierClassifications_Id")
                  .WithForeignKeyColumn(
                       "SampleSiteLeadCopperTierSampleCategoryId", 
                       "SampleSiteLeadCopperTierSampleCategories", 
                       "Id", 
                       false,
                       "FK_ClassificationsSampleCategories_SampleSiteLeadCopperTierSampleCategories_Id");

            /* 3. We have some new tiers */
            Execute.Sql(@"set identity_insert SampleSiteLeadCopperTierClassifications on;
                          insert into SampleSiteLeadCopperTierClassifications(Id, Description) VALUES (7, 'Tier 1');
                          insert into SampleSiteLeadCopperTierClassifications(Id, Description) VALUES (8, 'Tier 2');
                          insert into SampleSiteLeadCopperTierClassifications(Id, Description) VALUES (9, 'Tier 3');
                          set identity_insert SampleSiteLeadCopperTierClassifications off;");

            /* 4. And we have some new sample categories */
            Execute.Sql(@"set identity_insert SampleSiteLeadCopperTierSampleCategories on;
                          insert into SampleSiteLeadCopperTierSampleCategories(Id, DisplayValue, Description) VALUES (15, 'A', 'Single family, copper pipe with lead solder constructed after 1982 (but before June 1, 1986)');
                          insert into SampleSiteLeadCopperTierSampleCategories(Id, DisplayValue, Description) VALUES (16, 'B', 'Single family, lead pipes');
                          insert into SampleSiteLeadCopperTierSampleCategories(Id, DisplayValue, Description) VALUES (17, 'C', 'Single family, lead service lines');
                          insert into SampleSiteLeadCopperTierSampleCategories(Id, DisplayValue, Description) VALUES (18, 'D', 'Multifamily, copper pipe with lead solder constructed after 1982');
                          insert into SampleSiteLeadCopperTierSampleCategories(Id, DisplayValue, Description) VALUES (19, 'E', 'Multifamily, lead pipes');
                          insert into SampleSiteLeadCopperTierSampleCategories(Id, DisplayValue, Description) VALUES (20, 'F', 'Multifamily, lead service line');
                          insert into SampleSiteLeadCopperTierSampleCategories(Id, DisplayValue, Description) VALUES (21, 'J', 'Building, copper pipe with lead solder constructed after 1982');
                          insert into SampleSiteLeadCopperTierSampleCategories(Id, DisplayValue, Description) VALUES (22, 'K', 'Building, lead pipes');
                          insert into SampleSiteLeadCopperTierSampleCategories(Id, DisplayValue, Description) VALUES (23, 'L', 'Building, lead service line');
                          insert into SampleSiteLeadCopperTierSampleCategories(Id, DisplayValue, Description) VALUES (24, 'S', 'Single family, copper pipe with lead solder constructed before 1983');
                          insert into SampleSiteLeadCopperTierSampleCategories(Id, DisplayValue, Description) VALUES (25, 'R', 'Random location');
                          set identity_insert SampleSiteLeadCopperTierSampleCategories off;");

            /* 5. Three of these tier classifications are for illinois only */
            Execute.Sql("insert into SampleSiteLeadCopperTierClassificationsStates (StateId, SampleSiteLeadCopperTierClassificationId) values (4, 7)");
            Execute.Sql("insert into SampleSiteLeadCopperTierClassificationsStates (StateId, SampleSiteLeadCopperTierClassificationId) values (4, 8)");
            Execute.Sql("insert into SampleSiteLeadCopperTierClassificationsStates (StateId, SampleSiteLeadCopperTierClassificationId) values (4, 9)");

            /* 6. All the other tiers are for all the other states =/ */
            var tierMappingForAllNotIllinoisStates = new System.Text.StringBuilder(512);
            foreach (var classificationId in existingClassificationIds)
            {
                foreach (var stateId in stateIdsExceptIllinois)
                {
                    tierMappingForAllNotIllinoisStates.AppendFormat("insert into SampleSiteLeadCopperTierClassificationsStates (StateId, SampleSiteLeadCopperTierClassificationId) values ({0}, {1});", stateId, classificationId);
                }
            }

            Execute.Sql(tierMappingForAllNotIllinoisStates.ToString());

            /* 7. The newly added sample categories in step 4 are for specific tiers */

            /* 7a. Tier 1 (id 7) Sample Categories A through F */
            Execute.Sql("insert into SampleSiteLeadCopperTierClassificationsSampleSiteLeadCopperTierSampleCategories (SampleSiteLeadCopperTierClassificationId, SampleSiteLeadCopperTierSampleCategoryId) values (7, 15)");
            Execute.Sql("insert into SampleSiteLeadCopperTierClassificationsSampleSiteLeadCopperTierSampleCategories (SampleSiteLeadCopperTierClassificationId, SampleSiteLeadCopperTierSampleCategoryId) values (7, 16)");
            Execute.Sql("insert into SampleSiteLeadCopperTierClassificationsSampleSiteLeadCopperTierSampleCategories (SampleSiteLeadCopperTierClassificationId, SampleSiteLeadCopperTierSampleCategoryId) values (7, 17)");
            Execute.Sql("insert into SampleSiteLeadCopperTierClassificationsSampleSiteLeadCopperTierSampleCategories (SampleSiteLeadCopperTierClassificationId, SampleSiteLeadCopperTierSampleCategoryId) values (7, 18)");
            Execute.Sql("insert into SampleSiteLeadCopperTierClassificationsSampleSiteLeadCopperTierSampleCategories (SampleSiteLeadCopperTierClassificationId, SampleSiteLeadCopperTierSampleCategoryId) values (7, 19)");
            Execute.Sql("insert into SampleSiteLeadCopperTierClassificationsSampleSiteLeadCopperTierSampleCategories (SampleSiteLeadCopperTierClassificationId, SampleSiteLeadCopperTierSampleCategoryId) values (7, 20)");

            /* 7b. Tier 2 (id 8) Sample Categories J through L */
            Execute.Sql("insert into SampleSiteLeadCopperTierClassificationsSampleSiteLeadCopperTierSampleCategories (SampleSiteLeadCopperTierClassificationId, SampleSiteLeadCopperTierSampleCategoryId) values (8, 21)");
            Execute.Sql("insert into SampleSiteLeadCopperTierClassificationsSampleSiteLeadCopperTierSampleCategories (SampleSiteLeadCopperTierClassificationId, SampleSiteLeadCopperTierSampleCategoryId) values (8, 22)");
            Execute.Sql("insert into SampleSiteLeadCopperTierClassificationsSampleSiteLeadCopperTierSampleCategories (SampleSiteLeadCopperTierClassificationId, SampleSiteLeadCopperTierSampleCategoryId) values (8, 23)");

            /* 7c. Tier 3 (id 9) Sample Categories S and R */
            Execute.Sql("insert into SampleSiteLeadCopperTierClassificationsSampleSiteLeadCopperTierSampleCategories (SampleSiteLeadCopperTierClassificationId, SampleSiteLeadCopperTierSampleCategoryId) values (9, 24)");
            Execute.Sql("insert into SampleSiteLeadCopperTierClassificationsSampleSiteLeadCopperTierSampleCategories (SampleSiteLeadCopperTierClassificationId, SampleSiteLeadCopperTierSampleCategoryId) values (9, 25)");

            /* 7e. All the existing tiers (ids 1-6) prior to this migration are valid for all the existing sample categories (ids 1 through 14) prior to this migration */
            var tierMappingForAllOtherSampleCategories = new System.Text.StringBuilder(1024);

            foreach (var tierId in existingClassificationIds)
            {
                foreach (var sampleCategoryId in existingSampleCategoryIds)
                {
                    tierMappingForAllNotIllinoisStates.AppendFormat("insert into SampleSiteLeadCopperTierClassificationsSampleSiteLeadCopperTierSampleCategories (SampleSiteLeadCopperTierClassificationId, SampleSiteLeadCopperTierSampleCategoryId) values ({0}, {1});", tierId, sampleCategoryId);
                }
            }

            Execute.Sql(tierMappingForAllNotIllinoisStates.ToString());
        }

        public override void Down()
        {
            /* 1. We need to support classifications and states being m2m */
            Delete.Table("SampleSiteLeadCopperTierClassificationsStates");

            /* 2. We also need to support classifications and sample categories being m2m */
            Delete.Table("SampleSiteLeadCopperTierClassificationsSampleSiteLeadCopperTierSampleCategories");

            /* 3. We have some new tiers */
            Execute.Sql("update SampleSites set LeadCopperTierClassificationId = null where LeadCopperTierClassificationId in (7, 8, 9)");
            Execute.Sql("delete from SampleSiteLeadCopperTierClassifications where id in (7, 8, 9)");

            /* 4. And we have some new sample categories */
            Execute.Sql("update SampleSites set LeadCopperTierSampleCategoryId = null where LeadCopperTierSampleCategoryId in (15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25)");
            Execute.Sql("delete from SampleSiteLeadCopperTierSampleCategories where id in (15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25)");

            /* 5 through 7e - all DML and nothing needed since they're deleted in steps 1 and 2 */
        }
    }
}

