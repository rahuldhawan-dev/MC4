using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class WasteWaterSystemMap : ClassMap<WasteWaterSystem>
    {
        #region Constructors

        public WasteWaterSystemMap()
        {
            Id(x => x.Id);

            Map(x => x.ForceLength).Nullable();
            Map(x => x.GravityLength).Nullable();
            Map(x => x.IsCombinedSewerSystem).Nullable();
            Map(x => x.HasConsentOrder).Nullable();
            Map(x => x.NumberOfCustomers).Nullable();
            Map(x => x.NumberOfLiftStations).Nullable();
            Map(x => x.PermitNumber).Not.Nullable();
            Map(x => x.PeakFlowMGD).Nullable();
            Map(x => x.TreatmentDescription).Nullable();
            Map(x => x.WasteWaterSystemName).Not.Nullable();
            Map(x => x.DateOfOwnership).Nullable();
            Map(x => x.DateOfResponsibility).Nullable();
            Map(x => x.ConsentOrderStartDate).Nullable();
            Map(x => x.ConsentOrderEndDate).Nullable();
            Map(x => x.NewSystemInitialSafetyAssessmentCompleted).Nullable();
            Map(x => x.DateSafetyAssessmentActionItemsCompleted).Nullable();
            Map(x => x.NewSystemInitialWQEnvAssessmentCompleted).Nullable();
            Map(x => x.DateWQEnvAssessmentActionItemsCompleted).Nullable();
            Map(x => x.CurrentLicensedContractor).Nullable();

            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.BusinessUnit).Nullable();
            References(x => x.Status).Nullable();
            References(x => x.Ownership).Nullable();
            References(x => x.Type).Nullable();
            References(x => x.SubType).Nullable();
            References(x => x.LicensedOperatorStatus).Nullable();

            HasManyToMany(x => x.Towns)
               .Table(nameof(WasteWaterSystem) + "s" + nameof(Town) + "s")
               .ParentKeyColumn("WasteWaterSystemId")
               .ChildKeyColumn("TownId")
               .Cascade.All();

            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.EnvironmentalPermits).KeyColumn("WasteWaterSystemId").Inverse().Cascade.None();
            HasMany(x => x.WasteWaterSystemBasins).KeyColumn("WasteWaterSystemId").Inverse().Cascade.None();

            HasManyToMany(x => x.OperatorLicenses)
               .Table("OperatorLicensesWasteWaterSystems")
               .ParentKeyColumn("WasteWaterSystemId")
               .ChildKeyColumn("OperatorLicenseId");
            
            HasMany(x => x.PlanningPlantWasteWaterSystems)
               .KeyColumn("WasteWaterSystemId")
               .Cascade.All()
               .Inverse();
        }

        #endregion
    }
}
