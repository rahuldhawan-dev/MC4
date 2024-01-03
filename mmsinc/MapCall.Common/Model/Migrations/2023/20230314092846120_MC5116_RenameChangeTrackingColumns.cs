using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20230314092846120), Tags("Production")]
    public class MC5116_RenameChangeTrackingColumns : AutoReversingMigration
    {
        private const string CreatedAt = nameof(CreatedAt);
        private const string UpdatedAt = nameof(UpdatedAt);

        private const string CreatedById = nameof(CreatedById);
        private const string UpdatedById = nameof(UpdatedById);
        
        public override void Up()
        {
            Rename.Column("DateAdded").OnTable("AbsenceNotifications").To(CreatedAt);
            Rename.Column("DateAdded").OnTable("ActionItems").To(CreatedAt);
            Rename.Column("Created").OnTable("AllocationPermits").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("AssetInvestmentCategories").To(CreatedAt);
            Rename.Column("DateAdded").OnTable("AsBuiltImages").To(CreatedAt);
            Rename.Column("DateAdded").OnTable("BAPPTeamIdeas").To(CreatedAt);
            Rename.Column("DateAdded").OnTable("BlowOffInspections").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("Contacts").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("Contractors").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("ContractorAgreements").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("ContractorInsurance").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("CutoffSawQuestionnaires").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("Document").To(CreatedAt);
            Rename.Column("DateCreated").OnTable("tblFacilities").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("GeneralLiabilityClaims").To(CreatedAt);
            Rename.Column("DateAdded").OnTable("HydrantInspections").To(CreatedAt);
            Rename.Column("DateAdded").OnTable("Hydrants").To(CreatedAt);
            Rename.Column("DateCreated").OnTable("HydrantsOutOfService").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("Incidents").To(CreatedAt);
            Rename.Column("DateAdded").OnTable("Interconnections").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("JobSiteCheckLists").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("JobSiteCheckListComments").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("JobSiteCheckListCrewMembers").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("MarkoutDamages").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("Meters").To(CreatedAt);
            Rename.Column("DateCreated").OnTable("MeterChangeOutContracts").To(CreatedAt);
            Rename.Column("Date_Added").OnTable("Note").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("tblPWSID_Customer_Data").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("RecurringProjects").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("RecurringProjectTypes").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("RedTagPermits").To(CreatedAt);
            Rename.Column("DateAdded").OnTable("Regulations").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("Requisitions").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("Services").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("SewerMainCleanings").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("SewerOpenings").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("SewerOpeningConnections").To(CreatedAt);
            Rename.Column("DateAdded").OnTable("SewerOpeningInspections").To(CreatedAt);
            Rename.Column("DateAdded").OnTable("TapImages").To(CreatedAt);
            Rename.Column("DateAdded").OnTable("ValveInspections").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("Vehicles").To(CreatedAt);
            Rename.Column("DateAdded").OnTable("Valves").To(CreatedAt);
            Rename.Column("DateAdded").OnTable("ValveImages").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("WaterSamples").To(CreatedAt);
            Rename.Column("CreatedOn").OnTable("WorkOrders").To(CreatedAt);
            
            Rename.Column("LastUpdated").OnTable("AsBuiltImages").To(UpdatedAt);
            Rename.Column("ModifiedOn").OnTable("Document").To(UpdatedAt);
            Rename.Column("LastUpdated").OnTable("Equipment").To(UpdatedAt);
            Rename.Column("LastUpdated").OnTable("tblFacilities").To(UpdatedAt);
            Rename.Column("LastUpdated").OnTable("Hydrants").To(UpdatedAt);
            Rename.Column("DateUpdated").OnTable("PublicWaterSupplies").To(UpdatedAt);
            Rename.Column("Date_Updated").OnTable("tblPWSID_Customer_Data").To(UpdatedAt);
            Rename.Column("DateUpdated").OnTable("PublicWaterSupplyFirmCapacities").To(UpdatedAt);
            Rename.Column("LastUpdated").OnTable("Services").To(UpdatedAt);
            Rename.Column("LastUpdated").OnTable("SewerOpenings").To(UpdatedAt);
            Rename.Column("LastUpdated").OnTable("SystemDeliveryEntries").To(UpdatedAt);
            Rename.Column("LastUpdated").OnTable("Valves").To(UpdatedAt);
            
            Rename.Column("EnteredById").OnTable("AwiaCompliances").To(CreatedById);
            
            Rename.Column("LastUpdatedById").OnTable("AsBuiltImages").To(UpdatedById);
            Rename.Column("ModifiedById").OnTable("Document").To(UpdatedById);
            Rename.Column("LastUpdatedById").OnTable("Hydrants").To(UpdatedById);
            Rename.Column("LastUpdatedById").OnTable("Services").To(UpdatedById);
            Rename.Column("LastUpdatedById").OnTable("SewerOpenings").To(UpdatedById);
            Rename.Column("LastUpdatedById").OnTable("SystemDeliveryEntries").To(UpdatedById);
            Rename.Column("LastUpdatedById").OnTable("Valves").To(UpdatedById);
        }
    }
}

