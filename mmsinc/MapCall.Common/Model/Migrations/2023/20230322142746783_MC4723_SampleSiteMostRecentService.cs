using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Data;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230322142746783), Tags("Production")]
    // ReSharper disable once InconsistentNaming
    public class MC4723_SampleSiteMostRecentService : Migration
    {
        public override void Up()
        {
            /* 1. Rename "Invalidation" to "Inactivation" and add a new lookup value */

            Rename.Table("ReasonsForSampleSiteInvalidation")
                  .To("SampleSiteInactivationReasons");
            
            Rename.Column("ReasonForInvalidationId")
                  .OnTable("SampleSites")
                  .To("SampleSiteInactivationReasonId");

            this.AddLookupValueWithId("SampleSiteInactivationReasons", 8, "New Service Details");

            /* 2. Add direct relationship between Sample Site and Domestic Water Premises */

            Alter.Table("SampleSites").AddForeignKeyColumn("PremiseId", "Premises");

            Execute.Sql(@"
                update SampleSites
                   set PremiseId = p.Id
                  from SampleSites ss
                  join Premises p 
                    on ss.PremiseNumber = p.PremiseNumber
                 where ss.PremiseNumber is not null
                   and p.ServiceUtilityTypeId = 5
                   and p.OperatingCenterId = ss.OperatingCenterId
                ");

            Delete.Column("PremiseNumber").FromTable("SampleSites");

            /* 3. Remove the direct relationship between Sample Site and Service */

            if (Schema.Table("SampleSites").Constraint("FK_tblWQSample_Sites_Services_ServiceId").Exists())
            {
                // The never been run path
                Delete.ForeignKey("FK_tblWQSample_Sites_Services_ServiceId").OnTable("SampleSites");
                Delete.Column("ServiceId").FromTable("SampleSites");
            }
            else
            {
                // the re-runnable local machine path
                Delete.ForeignKeyColumn("SampleSites", "ServiceId", "Services");
            }

            /* 4. remove notifications */

            ClassExtensions.MigrationExtensions.RemoveNotificationPurpose(this, "Water Quality", "General", "Sample Site Service Changed");
            ClassExtensions.MigrationExtensions.RemoveNotificationPurpose(this, "Water Quality", "General", "Sample Site Added To Service");
        }

        public override void Down()
        {
            /* 4. remove notifications */

            this.AddNotificationType("Water Quality", "General", "Sample Site Added To Service");
            this.AddNotificationType("Water Quality", "General", "Sample Site Service Changed");

            /* 3. Remove the direct relationship between Sample Site and Service */

            Alter.Table("SampleSites").AddForeignKeyColumn("ServiceId", "Services");

            Execute.Sql(@"
                update SampleSites
                   set ServiceId = mrs.ServiceId
                  from MostRecentlyInstalledServicesView mrs
                  join Premises p 
                    on mrs.PremiseId = p.Id
                 where SampleSites.PremiseId is not null
                   and SampleSites.PremiseId = p.Id
                   and SampleSites.OperatingCenterId = p.OperatingCenterId
                ");

            /* 2. Add direct relationship between Sample Site and Domestic Water Premises  */

            Create.Column("PremiseNumber").OnTable("SampleSites").AsAnsiString(20).Nullable();

            Execute.Sql(@"
                update SampleSites
                   set PremiseNumber = p.PremiseNumber
                  from SampleSites ss
                  join Premises p 
                    on ss.PremiseId = p.Id
                   and p.ServiceUtilityTypeId = 5
                ");

            this.DeleteForeignKeyColumn("SampleSites", "PremiseId", "Premises");

            /* 1. Rename "Invalidation" to "Inactivation" and add a new lookup value */

            Execute.Sql("update SampleSites set SampleSiteInactivationReasonId = 7 where SampleSiteInactivationReasonId = 8");

            this.DeleteLookupEntities("SampleSiteInactivationReasons", new[] { 8 });

            Rename.Column("SampleSiteInactivationReasonId")
                  .OnTable("SampleSites")
                  .To("ReasonForInvalidationId");

            Rename.Table("SampleSiteInactivationReasons").To("ReasonsForSampleSiteInvalidation");
        }
    }
}

