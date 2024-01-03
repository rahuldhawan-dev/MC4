using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

// ReSharper disable StringLiteralTypo

namespace MapCall.Common.Model.Migrations._2021
{
    /// <summary>
    /// 
    /// Why do we have this new migration?
    ///
    /// Lims was done in 3 phases over the course of 3 months.
    /// Phase 1: MC-3503
    /// Phase 2: MC-3502
    /// Phase 3: MC-2770
    ///
    /// Each phase built upon the previous. Towards the end of the third phase, when the world was about
    /// to become a ghost town and nothing was stirring not even a mouse - we decided to create a release
    /// that contained a nullable integer field on SampleSites named LimsSequenceNumber in MC-4067. This
    /// was originally created in MC-3502.
    ///
    /// If the migrations for MC-3502 and MC-3503 stayed in the place they were with their original respective dates,
    /// this would be turn into a breaking migration when they and MC-2770 eventually were released. Therefore,
    /// we decided to delete those two migrations and move their contents into a new migration (this one), as well
    /// as remove the create column for LimsSequenceNumber as it's not longer needed (because it was done as part of MC-4067).
    /// 
    /// </summary>
    /// <seealso cref="FluentMigrator.Migration" />
    [Migration(20211216101448579), Tags("Production")]
    public class MC2770LimsApiIntegration : Migration
    {
        public override void Up()
        {
            // Things from MC-3503 which was folded into MC-2770

            this.CreateLookupTableWithValues("SampleSiteProfileAnalysisTypes", "CHEM", "BACT");

            Create.Table("SampleSiteProfiles")
                  .WithIdentityColumn()
                  .WithColumn("Number").AsInt32().NotNullable()
                  .WithForeignKeyColumn("SampleSiteProfileAnalysisTypeId", "SampleSiteProfileAnalysisTypes", nullable: false)
                  .WithForeignKeyColumn("PublicWaterSupplyId", "PublicWaterSupplies", nullable: false);

            // Things from MC-3502 which was built upon MC-3503 then folded into MC-2770

            Rename.Column("ResourceInterviewsPersonel").OnTable("SampleSites").To("ResourceInterviewsPersonnel");
            Rename.Column("ResourceInterviewsContactors").OnTable("SampleSites").To("ResourceInterviewsContractors");

            Create.Column("LimsFacilityId").OnTable("SampleSites").AsAnsiString(30).Nullable();
            Create.Column("LimsSiteId").OnTable("SampleSites").AsAnsiString(20).Nullable();
            Create.Column("LimsPrimaryStationCode").OnTable("SampleSites").AsAnsiString(30).Nullable();

            Alter.Table("SampleSites").AddForeignKeyColumn("SampleSiteProfileId", "SampleSiteProfiles");

            Execute.Sql(@"update SampleSites 
                             set CommonSiteName = left(CommonSiteName, 20)
                               , LocationNameDescription = left(LocationNameDescription, 40);");

            Delete.ForeignKeyColumn("SampleSites", "HorizonLaboratoryInformationManagementSystemProfileId", "HorizonLaboratoryInformationManagementSystemProfiles");
            Delete.Table("HorizonLaboratoryInformationManagementSystemProfiles");
        }

        public override void Down()
        {
            // Things from MC-3502 which was built upon MC-3503 then folded into MC-2770
            
            Rename.Column("ResourceInterviewsPersonnel").OnTable("SampleSites").To("ResourceInterviewsPersonel");
            Rename.Column("ResourceInterviewsContractors").OnTable("SampleSites").To("ResourceInterviewsContactors");

            Delete.Column("LimsFacilityId").FromTable("SampleSites");
            Delete.Column("LimsSiteId").FromTable("SampleSites");
            Delete.Column("LimsPrimaryStationCode").FromTable("SampleSites");
            Delete.ForeignKeyColumn("SampleSites", "SampleSiteProfileId", "SampleSiteProfiles");

            this.CreateLookupTableWithValues("HorizonLaboratoryInformationManagementSystemProfiles");
            Alter.Table("SampleSites")
                 .AddForeignKeyColumn("HorizonLaboratoryInformationManagementSystemProfileId", "HorizonLaboratoryInformationManagementSystemProfiles")
                 .Nullable();

            // Things from MC-3503 which was folded into MC-2770

            Delete.Table("SampleSiteProfiles");
            Delete.Table("SampleSiteProfileAnalysisTypes");
        }
    }
}

