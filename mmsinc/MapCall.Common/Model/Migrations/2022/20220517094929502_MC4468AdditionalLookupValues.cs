using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220517094929502), Tags("Production")]
    // ReSharper disable once InconsistentNaming
    public class MC4468AdditionalLookupValues : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"set identity_insert SampleSiteLeadCopperValidationMethods on;
                  insert into SampleSiteLeadCopperValidationMethods(Id, Description) VALUES (5, 'Customer Survey Results');
                  insert into SampleSiteLeadCopperValidationMethods(Id, Description) VALUES (6, 'Historic Documentation');
                  set identity_insert SampleSiteLeadCopperValidationMethods off;");

            Execute.Sql(@"set identity_insert SampleSiteStatuses on;
                  insert into SampleSiteStatuses(Id, Description) VALUES (6, 'Archived - Duplicate Site');
                  set identity_insert SampleSiteStatuses off;");
        }

        public override void Down()
        {
            // Set 'Archived - Duplicate Site' to 'Inactive'
            Execute.Sql("update SampleSites set SiteStatusId = 2 where SiteStatusId = 6");
            Execute.Sql("delete from SampleSiteStatuses where id = 6");

            // There is no equivalent for this one, so set to null
            Execute.Sql("update SampleSites set LeadCopperValidationMethodId = null where LeadCopperValidationMethodId in (5, 6)");
            Execute.Sql("delete from SampleSiteLeadCopperValidationMethods where id in (5, 6)");
        }
    }
}

