using System;
using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20210112102904054), Tags("Production")]
    public class UpdateSampleSitesForMC2768 : Migration
    {
        public override void Up()
        {
            Execute.Sql("DROP INDEX [IX_tblWQSample_Sites_DataTextField] ON [dbo].[tblWQSample_Sites];");
            
            Alter.Table("tblWQSample_Sites").AddColumn("SampleLocationNotes").AsAnsiString(120).Nullable();
            Execute.Sql("UPDATE tblWQSample_Sites SET SampleLocationNotes = " +
                        "(SELECT Description FROM SampleLocations sl WHERE sl.Id = SampleLocationId)");
            Delete.ForeignKeyColumn("tblWQSample_Sites", "SampleLocationId", "SampleLocations");
            Delete.Table("SampleLocations");

            Rename.Table("tblWQSample_Sites").To("SampleSites");
            Execute.Sql("UPDATE DataType set Table_Name = 'SampleSites' where Table_Name = 'tblWQSample_Sites';");
            Rename.Column("SampleSiteID").OnTable("SampleSites").To("Id");
            Delete.Column("IsNewSite").FromTable("SampleSites");
            // add a note that we're changing status denied to pending
            Execute.Sql("INSERT INTO [Note](Note, Date_Added, DataLinkId, DataTypeId, CreatedBy) " +
                        "SELECT " +
                        "   'Changed status from Denied to Pending as Denied was removed.', GetDate(), Id, 71, 402 " +
                        "FROM " +
                        "   SampleSites " +
                        "WHERE " +
                        "   SiteStatusId = 4; ");
            Execute.Sql("INSERT INTO [Note](Note, Date_Added, DataLinkId, DataTypeId, CreatedBy) " +
                        "SELECT " +
                        "   Notes, GetDate(), Id, 71, 402 " +
                        "FROM " +
                        "   SampleSites " +
                        "WHERE " +
                        "   notes is not null; ");
            this.AddLookupValueWithId("SampleSiteStatuses", 5, "Pending");
            Execute.Sql("UPDATE SampleSites set SiteStatusId = 5 where SiteStatusId = 4;" +
                        "DELETE SampleSiteStatuses WHERE Id = 4;");
            this.CreateLookupTableWithValues("SampleSiteCollectionTypes", 
                "Raw", 
                "In Plant",
                "Entry Point",
                "Interconnect",
                "Distribution", "Wastewater");
            this.CreateLookupTableWithValues("SampleSiteLocationTypes", 
                "Primary",
                "Upstream",
                "Downstream",
                "Groundwater");

            Alter.Table("SampleSites")
                 .AddColumn("ValidatedAt").AsDateTime().Nullable()
                 .AddForeignKeyColumn("ValidatedById", "tblEmployee", "tblEmployeeID")
                 .AddColumn("IsComplianceSampleSite").AsBoolean().Nullable()
                 .AddColumn("IsProcessSampleSite").AsBoolean().Nullable()
                 .AddColumn("IsResearchSampleSite").AsBoolean().Nullable()
                 .AddForeignKeyColumn("CollectionTypeId", "SampleSiteCollectionTypes")
                 .AddForeignKeyColumn("LocationTypeId", "SampleSiteLocationTypes")
                 .AddColumn("CommonSiteName").AsAnsiString(20).Nullable()
                 .AddColumn("LocationNameDescription").AsAnsiString(40).Nullable()
                 .AddColumn("IsSentToLIMS").AsBoolean().NotNullable().WithDefaultValue(false)
                 .AddColumn("IsSentToSample1View").AsBoolean().NotNullable().WithDefaultValue(false)
                 .AddColumn("IsPermitRequired").AsBoolean().NotNullable().WithDefaultValue(false)
                 .AddColumn("IsLeadAndCopperSite").AsBoolean().NotNullable().WithDefaultValue(false)
                ;
            Rename.Column("NJDEPID").OnTable("SampleSites").To("AgencyID");
            Execute.Sql("UPDATE SampleSites SET CommonSiteName = LEFT(Sample_Site_Name, 20), LocationNameDescription=LEFT(Description,40);");
            Delete.Column("WQ_Sample_Site_ID").FromTable("SampleSites");
            Delete.Column("Sample_Site_Name").FromTable("SampleSites");
            Delete.Column("Description").FromTable("SampleSites");
            Delete.Column("CrossRef_SampleSiteID").FromTable("SampleSites");
            Delete.Column("Belleville_ID").FromTable("SampleSites");
            Delete.Column("Source_of_Supply_Site").FromTable("SampleSites");
            Delete.Column("POE_Site").FromTable("SampleSites");
            Delete.Column("Route").FromTable("SampleSites");
            Delete.Column("Route_Sequence").FromTable("SampleSites");
            Delete.Column("Notes").FromTable("SampleSites");

            this.CreateLookupTableWithValues("HorizonLaboratoryInformationManagementSystemProfiles");
            Alter.Table("SampleSites").AddForeignKeyColumn("HorizonLaboratoryInformationManagementSystemProfileId",
                "HorizonLaboratoryInformationManagementSystemProfiles").Nullable();
        }

        public override void Down()
        {
            this.CreateLookupTableWithValues("SampleLocations", "Kitchen Sink", "Bathroom Sink");
            Alter.Table("SampleSites")
                 .AddColumn("Notes").AsAnsiString(255).Nullable()
                 .AddColumn("Route_Sequence").AsAnsiString(50).Nullable()
                 .AddColumn("Route").AsAnsiString(50).Nullable()
                 .AddColumn("POE_Site").AsBoolean().Nullable()
                 .AddColumn("Source_of_Supply_Site").AsBoolean().Nullable()
                 .AddColumn("Belleville_ID").AsAnsiString(50).Nullable()
                 .AddColumn("CrossRef_SampleSiteID").AsAnsiString(50).Nullable()
                 .AddColumn("Description").AsAnsiString(255).Nullable()
                 .AddColumn("Sample_Site_Name").AsAnsiString(50).Nullable()
                 .AddColumn("WQ_Sample_Site_ID").AsAnsiString(255).Nullable()
                ;
            Rename.Column("AgencyId").OnTable("SampleSites").To("NJDEPID");
            Execute.Sql("UPDATE SampleSites SET Sample_Site_Name = CommonSiteName, Description = LocationNameDescription;");

            Delete.Column("IsLeadAndCopperSite").FromTable("SampleSites");
            Delete.Column("IsPermitRequired").FromTable("SampleSites");
            Delete.Column("IsSentToSample1View").FromTable("SampleSites");
            Delete.Column("IsSentToLIMS").FromTable("SampleSites");
            Delete.Column("LocationNameDescription").FromTable("SampleSites");
            Delete.Column("CommonSiteName").FromTable("SampleSites");
            Delete.ForeignKeyColumn("SampleSites", "LocationTypeId", "SampleSiteLocationTypes");
            Delete.ForeignKeyColumn("SampleSites", "CollectionTypeId", "SampleSiteCollectionTypes");
            Delete.Column("IsResearchSampleSite").FromTable("SampleSites");
            Delete.Column("IsProcessSampleSite").FromTable("SampleSites");
            Delete.Column("IsComplianceSampleSite").FromTable("SampleSites");
            Delete.ForeignKeyColumn("SampleSites", "ValidatedById", "tblEmployee", "tblEmployeeID");
            Delete.ForeignKeyColumn("SampleSites", "ValidatedById", "tblEmployee", "tblEmployeeID");
            Delete.Column("ValidatedAt").FromTable("SampleSites");
            Delete.ForeignKeyColumn("SampleSites", "HorizonLaboratoryInformationManagementSystemProfileId", "HorizonLaboratoryInformationManagementSystemProfiles");
            Delete.Table("HorizonLaboratoryInformationManagementSystemProfiles");
            Delete.Table("SampleSiteLocationTypes");
            Delete.Table("SampleSiteCollectionTypes");

            Execute.Sql("DELETE FROM Note where Note like 'Changed status from Denied to Pending as Denied was removed.' AnD DataTypeID = 71 AND CreatedBy = 402;");
            Execute.Sql("UPDATE SampleSites set SiteStatusId = null where SiteStatusId = 5");
            Execute.Sql("DELETE FROM SampleSiteStatuses where Id = 5");
            this.AddLookupValueWithId("SampleSiteStatuses", 4, "DENIED");
            Alter.Table("SampleSites")
                 .AddColumn("IsNewSite").AsBoolean().NotNullable().WithDefaultValue(false);
            Rename.Column("Id").OnTable("SampleSites").To("SampleSiteID");
            Rename.Table("SampleSites").To("tblWQSample_Sites");
            Alter.Table("tblWQSample_Sites").AddForeignKeyColumn("SampleLocationId", "SampleLocations");
            Execute.Sql("UPDATE tblWQSample_Sites SET SampleLocationId = (SELECT Id from SampleLocations WHERE Description = SampleLocationNotes)");
            Delete.Column("SampleLocationNotes").FromTable("tblWQSample_Sites");
            
            Execute.Sql("CREATE NONCLUSTERED INDEX [IX_tblWQSample_Sites_DataTextField] " +
                        "ON [dbo].[tblWQSample_Sites] ([CrossRef_SampleSiteID] ASC,[Town] ASC,[Description] ASC,[Sample_Site_Name] ASC)" +
                        "WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) " +
                        "ON [PRIMARY]");
        }
    }
}