using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class NearMissMap : ClassMap<NearMiss>
    {
        #region Constants

        public const string TABLE_NAME = "NearMisses";

        #endregion

        #region Constructors

        public NearMissMap()
        {
            Table(TABLE_NAME);
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.OperatingCenter).Nullable();
            References(x => x.Category).Nullable();
            References(x => x.Type).Nullable();
            References(x => x.SubCategory).Nullable();
            References(x => x.Town).Nullable();
            References(x => x.Facility).Nullable();
            References(x => x.Coordinate).Nullable();
            References(x => x.WorkOrderType).Nullable();
            References(x => x.WorkOrder).Nullable();
            References(x => x.SystemType).Nullable();
            References(x => x.WasteWaterSystem).Nullable();
            References(x => x.PublicWaterSupply).Nullable();
            References(x => x.ProductionWorkOrder).Nullable();
            References(x => x.LifeSavingRuleType).Nullable();
            References(x => x.StopWorkUsageType).Nullable();
            References(x => x.Employee).Nullable();
            References(x => x.ActionTakenType).Nullable();
            References(x => x.ReviewedBy, "ReviewedBy").Nullable();

            Map(x => x.IncidentNumber)
               .Nullable()
               .Length(NearMiss.StringLengths.INCIDENT_NUMBER);
            Map(x => x.WorkOrderNumber)
               .Nullable()
               .Length(NearMiss.StringLengths.WORK_ORDER_NUMBER);
            Map(x => x.LocationDetails)
               .Nullable()
               .Length(NearMiss.StringLengths.LOCATION_DETAILS);
            Map(x => x.ReportedBy)
               .Nullable()
               .Length(NearMiss.StringLengths.REPORTED_BY);
            Map(x => x.Severity)
               .Nullable()
               .Length(NearMiss.StringLengths.SEVERITY);
            Map(x => x.DateCompleted)
               .Nullable();
            Map(x => x.DescribeOther)
               .Nullable()
               .Length(NearMiss.StringLengths.DESCRIBE_OTHER);
            Map(x => x.Description).Not.Nullable().Length(int.MaxValue);
            Map(x => x.SeriousInjuryOrFatality).Nullable();
            Map(x => x.OccurredAt).Not.Nullable();
            Map(x => x.ReportedToRegulator).Nullable();
            Map(x => x.NotCompanyFacility).Nullable();
            Map(x => x.ReportAnonymously).Nullable();
            Map(x => x.RelatedToContractor).Nullable();
            Map(x => x.ActionTaken).Nullable().Length(NearMiss.StringLengths.ACTION_TAKEN);
            Map(x => x.ReviewedDate).Nullable();
            Map(x => x.ContractorCompany).Nullable();
            Map(x => x.SubmittedOnBehalfOfAnotherEmployee).Nullable();
            Map(x => x.StopWorkAuthorityPerformed).Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.CompletedCorrectiveActions).Nullable();
            Map(x => x.ShortCycleWorkOrderNumber).Nullable();

            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.ActionItems).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }

        #endregion
    }
}