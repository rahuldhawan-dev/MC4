using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151102145148797), Tags("Production")]
    public class ConvertProjectsIPRelatedLookupsBug2636 : Migration
    {
        public override void Up()
        {
            // Convert lookups
            this.ExtractLookupTableLookup("ProjectsIP", "Category", "InvestmentProjectCategories", 50, "Category",
                deleteSafely: true);
            this.ExtractLookupTableLookup("ProjectsIP", "AssetCategory", "InvestmentProjectAssetCategories", 50,
                "AssetCategory", deleteSafely: true);
            this.ExtractLookupTableLookup("ProjectsIP", "Phase", "InvestmentProjectPhases", 50, "Phase",
                deleteSafely: true);
            this.ExtractLookupTableLookup("ProjectsIP", "ApprovalStatus", "InvestmentProjectApprovalStatuses", 50,
                "ApprovalStatus", deleteSafely: true);
            this.ExtractLookupTableLookup("ProjectsIP", "ProjectStatus", "InvestmentProjectStatuses", 50,
                "ProjectStatus", deleteSafely: true);

            // Rename AFTER the lookup extraction because otherwise that blows up
            Rename.Table("ProjectsIP").To("InvestmentProjects");

            // Switch foreign keys on here because it's using the old tblContractors table which has no values.
            Delete.ForeignKey("FK_ProjectsIP_tblContractors_ConstructionContractor").OnTable("InvestmentProjects");
            Delete.ForeignKey("FK_ProjectsIP_tblContractors_EngineeringContractor").OnTable("InvestmentProjects");

            Alter.Table("InvestmentProjects")
                 .AlterColumn("ProjectDurationMonths").AsInt32().Nullable()
                 .AlterColumn("CIMDate").AsDateTime().Nullable()
                 .AlterColumn("ForecastedInServiceDate").AsDateTime().Nullable()
                 .AlterColumn("ControlDate").AsDateTime().Nullable()
                 .AlterColumn("PPDate").AsDateTime().Nullable()
                 .AlterColumn("PPScore").AsInt32().Nullable()
                 .AlterColumn("InServiceDate").AsDateTime().Nullable()
                 .AlterColumn("ConstructionContractor").AsInt32().Nullable()
                 .ForeignKey("FK_InvestmentProjects_Contractors_ConstructionContractor", "Contractors", "ContractorID")
                 .AlterColumn("EngineeringContractor").AsInt32().Nullable()
                 .ForeignKey("FK_InvestmentProjects_Contractors_EngineeringContractor", "Contractors", "ContractorID")
                 .AddColumn("CPSReferenceYear").AsInt32().Nullable()
                 .AddColumn("CPSPriorityNumber").AsString(50).Nullable()
                 .AddColumn("DurationLandAcquisitionInMonths").AsInt32().Nullable()
                 .AddColumn("DurationPermitDesignInMonths").AsInt32().Nullable()
                 .AddColumn("DurationConstructionInMonths").AsInt32().Nullable()
                 .AddColumn("TargetStartDate").AsDateTime().Nullable()
                 .AddColumn("TargetEndDate").AsDateTime().Nullable();

            Delete.ForeignKey("FK_ProjectsIP_Lookup_ProjectCategory").OnTable("InvestmentProjects");
            Delete.Column("ProjectCategory").FromTable("InvestmentProjects");

            Rename.Column("ProjectID").OnTable("InvestmentProjects").To("Id");
            Rename.Column("OpCode").OnTable("InvestmentProjects").To("OperatingCenterId");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_InvestmentProjects_Contractors_ConstructionContractor").OnTable("InvestmentProjects");
            Delete.ForeignKey("FK_InvestmentProjects_Contractors_EngineeringContractor").OnTable("InvestmentProjects");

            Rename.Column("Id").OnTable("InvestmentProjects").To("ProjectId");
            Rename.Column("OperatingCenterId").OnTable("InvestmentProjects").To("OpCode");

            Alter.Table("InvestmentProjects")
                 .AddColumn("ProjectCategory").AsInt32().Nullable()
                 .ForeignKey("FK_ProjectsIP_Lookup_ProjectCategory", "Lookup", "LookupID")
                 .AlterColumn("ConstructionContractor").AsInt32().Nullable()
                 .ForeignKey("FK_ProjectsIP_tblContractors_ConstructionContractor", "tblContractors", "ContractorID")
                 .AlterColumn("EngineeringContractor").AsInt32().Nullable()
                 .ForeignKey("FK_ProjectsIP_tblContractors_EngineeringContractor", "tblContractors", "ContractorID");

            Delete.Column("CPSReferenceYear").FromTable("InvestmentProjects");
            Delete.Column("CPSPriorityNumber").FromTable("InvestmentProjects");
            Delete.Column("DurationLandAcquisitionInMonths").FromTable("InvestmentProjects");
            Delete.Column("DurationPermitDesignInMonths").FromTable("InvestmentProjects");
            Delete.Column("DurationConstructionInMonths").FromTable("InvestmentProjects");
            Delete.Column("TargetStartDate").FromTable("InvestmentProjects");
            Delete.Column("TargetEndDate").FromTable("InvestmentProjects");
            // No need to convert columns back to smalldate/smallint.

            // Rename BEFORE the lookup replacement or else the replace blows up
            Rename.Table("InvestmentProjects").To("ProjectsIP");

            // this.ReplaceLookupTableLookup("ProjectsIP", "ProjectCategory", "InvestmentProjectProjectCategories", 50, "ProjectCategory");
            this.ReplaceLookupTableLookup("ProjectsIP", "ApprovalStatus", "InvestmentProjectApprovalStatuses", 50,
                "ApprovalStatus");
            this.ReplaceLookupTableLookup("ProjectsIP", "ProjectStatus", "InvestmentProjectStatuses", 50,
                "ProjectStatus");
            this.ReplaceLookupTableLookup("ProjectsIP", "Phase", "InvestmentProjectPhases", 50, "Phase");
            this.ReplaceLookupTableLookup("ProjectsIP", "AssetCategory", "InvestmentProjectAssetCategories", 50,
                "AssetCategory");
            this.ReplaceLookupTableLookup("ProjectsIP", "Category", "InvestmentProjectCategories", 50, "Category");
        }
    }
}
