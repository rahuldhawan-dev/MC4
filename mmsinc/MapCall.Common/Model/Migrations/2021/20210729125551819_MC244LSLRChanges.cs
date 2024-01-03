using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210729125551819), Tags("Production")]
    public class MC244LSLRChanges : Migration
    {
        public override void Up()
        {
            // The limit on Description was set to 20 for some inexplicable reason.
            Alter.Column("Description").OnTable("CustomerSideSLReplacementOfferStatuses").AsString(50).NotNullable();

            // Add new statuses
            Insert.IntoTable("CustomerSideSLReplacementOfferStatuses").Row(new { Description = "On List For Future LSLR" });
            Insert.IntoTable("CustomerSideSLReplacementOfferStatuses").Row(new { Description = "Initiated Customer Contact" });
            Insert.IntoTable("CustomerSideSLReplacementOfferStatuses").Row(new { Description = "Test Pit Completed" });
            Insert.IntoTable("CustomerSideSLReplacementOfferStatuses").Row(new { Description = "Offered Agreement" });
            Insert.IntoTable("CustomerSideSLReplacementOfferStatuses").Row(new { Description = "Unsafe Condition" });
            Insert.IntoTable("CustomerSideSLReplacementOfferStatuses").Row(new { Description = "Abandoned Premise" });
            Insert.IntoTable("CustomerSideSLReplacementOfferStatuses").Row(new { Description = "No Owner Found" });
            Insert.IntoTable("CustomerSideSLReplacementOfferStatuses").Row(new { Description = "Not Lead – Test Pit Confirmed" });

            Update.Table("CustomerSideSLReplacementOfferStatuses").Set(new { Description = "No Response" }).Where(new { Description = "No" });

            this.CreateLookupTableWithValues("ServiceRegroundingPremiseTypes", "Not Needed", "Customer Completed", "Contractor Completed");
            this.CreateLookupTableWithValues("ServiceOfferedAgreementTypes", "Pending", "Signed", "Declined");

            Create.Column("ServiceOfferedAgreementTypeId").OnTable("Services").AsInt32().Nullable()
                  .ForeignKey("FK_Services_ServiceOfferedAgreementTypes_ServiceOfferedAgreementTypeId", "ServiceOfferedAgreementTypes", "Id");
            Create.Column("OfferedAgreementDate").OnTable("Services").AsDateTime().Nullable();
            Create.Column("ServiceRegroundingPremiseTypeId").OnTable("Services")
                  .AsInt32().Nullable().ForeignKey("FK_Services_ServiceRegroundingPremiseTypes_ServiceRegroundingPremiseTypeId", "ServiceRegroundingPremiseTypes", "Id");

            // All stuff related to ServicePremiseContacts
            this.CreateLookupTableWithValues("ServicePremiseContactMethods", "In Person", "Spoke Via Phone",
                "Voice Message Left", "Doorhanger/Flyer Provided", "Meeting Invitation",
                "Lead Fact Sheet provided in this contact", "Email Sent", "Letter Sent", "Referred to another party", "Code Red",
                "Other");

            this.CreateLookupTableWithValues("ServicePremiseContactTypes", "Renter", "Owner", "PropertyManager");

            Create.Table("ServicePremiseContacts")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                  .WithColumn("ServiceId").AsInt32().NotNullable()
                  .ForeignKey("FK_ServicePremiseContacts_Services_ServiceId", "Services", "Id")
                  .WithColumn("ServicePremiseContactMethodId").AsInt32().NotNullable()
                  .ForeignKey("FK_Services_ServicePremiseContactMethods_ServicePremiseContactMethodId", "ServicePremiseContactMethods", "Id")
                  .WithColumn("NotifiedCustomerServiceCenter").AsBoolean().NotNullable()
                  .WithColumn("CertifiedLetterSent").AsBoolean().NotNullable()
                  .WithColumn("ContactDate").AsDateTime().NotNullable()
                  .WithColumn("ContactInformation").AsCustom("ntext").Nullable()
                  .WithColumn("CommunicationResults").AsCustom("ntext").Nullable()
                  .WithColumn("ServicePremiseContactTypeId").AsInt32().NotNullable()
                  .ForeignKey("FK_ServicePremiseContacts_ServicePremiseContactTypes_ServicePremiseContactTypeId", "ServicePremiseContactTypes", "Id")
                  .WithColumn("CreatedByUserId").AsInt32().NotNullable()
                  .ForeignKey("FK_ServicePremiseContacts_tblPermissions_CreatedByUserId", "tblPermissions", "RecId");

            // All stuff related to ServiceFlush
            this.CreateLookupTableWithValues("ServiceFlushFlushTypes", "Standard - Inside Premise", "Other");
            this.CreateLookupTableWithValues("ServiceFlushSampleTypes", "First Flush", "Overnight Stagnant", "Resample");
            this.CreateLookupTableWithValues("ServiceFlushSampleStatuses", "Taken", "Error – Resampled", "Results Received");
            this.CreateLookupTableWithValues("ServiceFlushSampleTakenByTypes", "Employee", "Contractor/Plumber", "Customer");
            this.CreateLookupTableWithValues("ServiceFlushPremiseContactMethods", "In Person", "Spoke Via Phone", "Voice Message Left", "Letter Sent", "Code Red");
            this.CreateLookupTableWithValues("ServiceFlushReplacementTypes", "Company", "Customer", "Both");

            Create.Table("ServiceFlushes")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("ServiceId").AsInt32().NotNullable()
                  .ForeignKey("FK_ServiceFlushes_Services_ServiceId", "Services", "Id")
                  .WithColumn("ServiceFlushFlushTypeId").AsInt32().NotNullable()
                  .ForeignKey("FK_ServiceFlushes_ServiceFlushFlushTypes_ServiceFlushFlushTypeId", "ServiceFlushFlushTypes", "Id")
                  .WithColumn("ServiceFlushSampleTypeId").AsInt32().NotNullable()
                  .ForeignKey("FK_ServiceFlushes_ServiceFlushSampleTypes_ServiceFlushSampleTypeId", "ServiceFlushSampleTypes", "Id")
                  .WithColumn("ServiceFlushSampleStatusId").AsInt32().NotNullable()
                  .ForeignKey("FK_ServiceFlushes_ServiceFlushSampleStatuses_ServiceFlushSampleStatusId", "ServiceFlushSampleStatuses", "Id")
                  .WithColumn("ServiceFlushSampleTakenByTypeId").AsInt32().NotNullable()
                  .ForeignKey("FK_ServiceFlushes_ServiceFlushSampleTakenByTypes_ServiceFlushSampleTakenByTypeId", "ServiceFlushSampleTakenByTypes", "Id")
                  .WithColumn("ServiceFlushPremiseContactMethodId").AsInt32().Nullable()
                  .ForeignKey("FK_ServiceFlushes_ServiceFlushPremiseContactMethods_ServiceFlushPremiseContactMethodId", "ServiceFlushPremiseContactMethods", "Id")
                  .WithColumn("ServiceFlushReplacementTypeId").AsInt32().Nullable()
                  .ForeignKey("FK_ServiceFlushes_ServiceFlushReplacementTypes_ServiceFlushReplacementTypeId", "ServiceFlushReplacementTypes", "Id")
                  .WithColumn("SampleDate").AsDateTime().NotNullable()
                  .WithColumn("SampleResultPassed").AsBoolean().Nullable()
                  .WithColumn("PremiseContactDate").AsDateTime().Nullable()
                  .WithColumn("NotifiedCustomerServiceCenter").AsBoolean().Nullable()
                  .WithColumn("HasSentNotification").AsBoolean().NotNullable()
                  .WithColumn("FlushingNotes").AsCustom("ntext").Nullable()
                  .WithColumn("CreatedByUserId").AsInt32().NotNullable()
                  .ForeignKey("FK_ServiceFlushes_tblPermissions_CreatedByUserId", "tblPermissions", "RecId");

            this.CreateLookupTableWithValues("ServiceSubfloorConditions", "Finished Basement", "Unfinished Basement", "Crawl Space", "Slab");
            this.CreateLookupTableWithValues("ServiceTerminationPoints", "Inside Shutoff", "Other");

            Alter.Table("Services")
                 .AddColumn("YearOfHomeConstruction").AsInt32().Nullable()
                 .AddColumn("InactiveDate").AsDateTime().Nullable()
                 .AddColumn("ServiceTerminationPointId").AsInt32().Nullable()
                 .ForeignKey("FK_Services_ServiceTerminationPoints_ServiceTerminationPointId", "ServiceTerminationPoints", "Id")
                 .AddColumn("OtherPoint").AsCustom("ntext").Nullable()
                 .AddColumn("ServiceSubfloorConditionId").AsInt32().Nullable()
                 .ForeignKey("FK_Services_ServiceSubfloorConditions_ServiceSubfloorConditionId", "ServiceSubfloorConditions", "Id")
                 .AddColumn("ProjectManagerUserId").AsInt32().Nullable()
                 .ForeignKey("FK_Services_tblPermissions_ProjectManagerUserId", "tblPermissions", "RecId");

            //Adds New Notification Purpose for Flushing Results not received after 2 weeks email notification
            Insert.IntoTable("NotificationPurposes").Row(new { ModuleID = 73, Purpose = "Flushing Results Not Received" });
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_Services_tblPermissions_ProjectManagerUserId").OnTable("Services");
            Delete.ForeignKey("FK_Services_ServiceSubfloorConditions_ServiceSubfloorConditionId").OnTable("Services");
            Delete.ForeignKey("FK_Services_ServiceTerminationPoints_ServiceTerminationPointId").OnTable("Services");
            Delete.Column("ServiceTerminationPointId").FromTable("Services");
            Delete.Column("OtherPoint").FromTable("Services");
            Delete.Column("ServiceSubfloorConditionId").FromTable("Services");
            Delete.Column("ProjectManagerUserId").FromTable("Services");
            Delete.Column("InactiveDate").FromTable("Services");
            Delete.Column("YearOfHomeConstruction").FromTable("Services");

            Delete.Table("ServiceTerminationPoints");
            Delete.Table("ServiceSubfloorConditions");
            Delete.Table("ServiceFlushes");

            Delete.Table("ServiceFlushPremiseContactMethods");
            Delete.Table("ServiceFlushSampleTakenByTypes");
            Delete.Table("ServiceFlushSampleStatuses");
            Delete.Table("ServiceFlushSampleTypes");
            Delete.Table("ServiceFlushFlushTypes");
            Delete.Table("ServiceFlushReplacementTypes");

            Delete.Table("ServicePremiseContacts");
            Delete.ForeignKey("FK_Services_ServiceRegroundingPremiseTypes_ServiceRegroundingPremiseTypeId").OnTable("Services");

            Delete.Table("ServicePremiseContactTypes");

            Delete.ForeignKey("FK_Services_ServiceOfferedAgreementTypes_ServiceOfferedAgreementTypeId").OnTable("Services");

            Delete.Column("ServiceOfferedAgreementTypeId").FromTable("Services");
            Delete.Column("OfferedAgreementDate").FromTable("Services");
            Delete.Column("ServiceRegroundingPremiseTypeId").FromTable("Services");

            Delete.Table("ServicePremiseContactMethods");
            Delete.Table("ServiceRegroundingPremiseTypes");
            Delete.Table("ServiceOfferedAgreementTypes");

            Update.Table("CustomerSideSLReplacementOfferStatuses").Set(new { Description = "No" }).Where(new { Description = "No Response" });

            Delete.FromTable("CustomerSideSLReplacementOfferStatuses").Row(new { Description = "On List For Future LSLR" });
            Delete.FromTable("CustomerSideSLReplacementOfferStatuses").Row(new { Description = "Initiated Customer Contact" });
            Delete.FromTable("CustomerSideSLReplacementOfferStatuses").Row(new { Description = "Test Pit Completed" });
            Delete.FromTable("CustomerSideSLReplacementOfferStatuses").Row(new { Description = "Offered Agreement" });
            Delete.FromTable("CustomerSideSLReplacementOfferStatuses").Row(new { Description = "Unsafe Condition" });
            Delete.FromTable("CustomerSideSLReplacementOfferStatuses").Row(new { Description = "Abandoned Premise" });
            Delete.FromTable("CustomerSideSLReplacementOfferStatuses").Row(new { Description = "No Owner Found" });
            Delete.FromTable("CustomerSideSLReplacementOfferStatuses").Row(new { Description = "Not Lead – Test Pit Confirmed" });

            Delete.FromTable("NotificationPurposes").Row(new { Purpose = "Flushing Results Not Received" });
        }
    }
}

