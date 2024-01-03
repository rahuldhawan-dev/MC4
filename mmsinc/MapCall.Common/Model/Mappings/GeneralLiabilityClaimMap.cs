using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class GeneralLiabilityClaimMap : ClassMap<GeneralLiabilityClaim>
    {
        public GeneralLiabilityClaimMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.LiabilityType).Not.Nullable();
            References(x => x.GeneralLiabilityClaimType).Nullable();
            References(x => x.CrashType).Nullable();
            References(x => x.Coordinate).Not.Nullable();
            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.CompanyContact).Not.Nullable();
            References(x => x.ClaimsRepresentative).Not.Nullable();

            Map(x => x.ClaimNumber).Nullable();
            Map(x => x.MeterBox);
            Map(x => x.CurbValveBox);
            Map(x => x.Excavation);
            Map(x => x.Barricades);
            Map(x => x.Vehicle);
            Map(x => x.WaterMeter);
            Map(x => x.FireHydrant);
            Map(x => x.Backhoe);
            Map(x => x.WaterQuality);
            Map(x => x.WaterPressure);
            Map(x => x.WaterMain);
            Map(x => x.ServiceLine);
            Map(x => x.Description).Not.Nullable();
            Map(x => x.Name);
            Map(x => x.PhoneNumber);
            Map(x => x.Address);
            Map(x => x.Email);
            Map(x => x.DriverName);
            Map(x => x.DriverPhone);
            Map(x => x.PhhContacted).Column("PHHContacted");
            Map(x => x.OtherDriver);
            Map(x => x.OtherDriverPhone);
            Map(x => x.OtherDriverAddress);
            Map(x => x.LocationOfIncident);
            Map(x => x.IncidentDateTime).Not.Nullable();
            Map(x => x.VehicleYear);
            Map(x => x.VehicleMake);
            Map(x => x.VehicleVin).Column("VehicleVIN");
            Map(x => x.LicenseNumber);
            Map(x => x.PoliceCalled);
            Map(x => x.PoliceDepartment);
            Map(x => x.PoliceCaseNumber);
            Map(x => x.WitnessStatement);
            Map(x => x.Witness);
            Map(x => x.WitnessPhone);
            Map(x => x.AnyInjuries);
            Map(x => x.ReportedBy).Not.Nullable();
            Map(x => x.ReportedByPhone);
            Map(x => x.IncidentNotificationDate).Not.Nullable();
            Map(x => x.IncidentReportedDate).Nullable();
            Map(x => x.CompletedDate).Nullable();
            Map(x => x.CreatedBy).Not.Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.SAPWorkOrderId).Nullable();
            Map(x => x.Why1).Nullable();
            Map(x => x.Why2).Nullable();
            Map(x => x.Why3).Nullable();
            Map(x => x.Why4).Nullable();
            Map(x => x.Why5).Nullable();
            Map(x => x.DateSubmitted).Nullable();
            Map(x => x.OtherTypeOfCrash).Nullable();
            Map(x => x.FiveWhysCompleted).Not.Nullable();
            HasMany(x => x.GeneralLiabilityClaimNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.GeneralLiabilityClaimDocuments).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.ActionItems).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
