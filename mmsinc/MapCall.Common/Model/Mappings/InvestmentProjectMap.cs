using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class InvestmentProjectMap : ClassMap<InvestmentProject>
    {
        public InvestmentProjectMap()
        {
            Id(x => x.Id);

            Map(x => x.ProjectNumber).Length(InvestmentProject.StringLengths.PROJECT_NUMBER).Nullable();
            Map(x => x.PPWorkOrder).Length(InvestmentProject.StringLengths.PP_WORKORDER).Nullable();
            Map(x => x.ProjectDescription).CustomSqlType("text").Nullable();
            Map(x => x.ProjectObstacles).CustomSqlType("text").Nullable();
            Map(x => x.ProjectRisks).CustomSqlType("text").Nullable();
            Map(x => x.ProjectApproach).CustomSqlType("text").Nullable();
            Map(x => x.ProjectDurationMonths).Nullable();
            Map(x => x.EstimatedProjectCost, "EstProjectCost")
               .Nullable().Length(18).Scale(0); // Why is this a decimal pretending to be an integer? -Ross 10/15/2015
            Map(x => x.FinalProjectCost).Nullable().Length(18).Scale(0); // same "what" as above
            Map(x => x.ContractedInspector).Nullable().Length(InvestmentProject.StringLengths.CONTRACTED_INSPECTOR);
            Map(x => x.StreetName).Nullable().Length(InvestmentProject.StringLengths.STREET_NAME);
            Map(x => x.CreatedBy).Nullable().Length(InvestmentProject.StringLengths.CREATED_BY);
            Map(x => x.CIMDate).Nullable();
            Map(x => x.ProjectFlagged).Nullable();
            Map(x => x.CurrentYearActive).Nullable();
            Map(x => x.BulkSale).Nullable();
            Map(x => x.RateCase).Nullable();
            Map(x => x.MISDates).Nullable();
            Map(x => x.COE).Nullable();
            Map(x => x.Geography).Nullable();
            Map(x => x.ForecastedInServiceDate).Nullable();
            Map(x => x.ControlDate).Nullable();
            Map(x => x.PPDate).Nullable();
            Map(x => x.PPScore).Nullable();
            Map(x => x.InServiceDate).Nullable();
            Map(x => x.CPSReferenceYear).Nullable();
            Map(x => x.CPSPriorityNumber).Nullable().Length(InvestmentProject.StringLengths.CPS_PRIORITY_NUMBER);
            Map(x => x.DurationLandAcquisitionInMonths).Nullable();
            Map(x => x.DurationPermitDesignInMonths).Nullable();
            Map(x => x.DurationConstructionInMonths).Nullable();
            Map(x => x.TargetStartDate).Nullable();
            Map(x => x.TargetEndDate).Nullable();

            References(x => x.OperatingCenter).Nullable();
            References(x => x.BusinessUnit, "BU").Nullable();
            References(x => x.AssetOwner, "AssetOwner").Nullable();
            References(x => x.ProjectManager, "ProjectManager").Nullable();
            References(x => x.ConstructionManager, "ConstructionManager").Nullable();
            References(x => x.CompanyInspector, "CompanyInspector").Nullable();
            References(x => x.EngineeringContractor, "EngineeringContractor").Nullable();
            References(x => x.ConstructionContractor, "ConstructionContractor").Nullable();
            References(x => x.PublicWaterSupply, "PWSID").Nullable();
            References(x => x.Facility).Nullable();
            References(x => x.Town, "Town").Nullable();
            References(x => x.Coordinate).Nullable();
            References(x => x.Phase, "Phase").Nullable();
            References(x => x.ProjectCategory, "Category").Nullable();
            References(x => x.AssetCategory, "AssetCategory").Nullable();
            References(x => x.ApprovalStatus, "ApprovalStatus").Nullable();
            References(x => x.ProjectStatus, "ProjectStatus").Nullable();

            HasMany(x => x.InvestmentProjectDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.InvestmentProjectNotes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
