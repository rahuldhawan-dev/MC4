using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SewerOverflowMap : ClassMap<SewerOverflow>
    {
        public SewerOverflowMap()
        {
            Id(x => x.Id, "SewerOverflowID");

            // TODO: Every single column is marked as nullable, but not all of them should be.
            // This includes in the database.

            Map(x => x.IncidentDate).Nullable();
            Map(x => x.TalkedTo).Nullable();
            Map(x => x.StreetNumber).Nullable();
            Map(x => x.GallonsOverflowedEstimated).Nullable();
            Map(x => x.GallonsInContainedDrains).Nullable();
            Map(x => x.GallonsFlowedIntoBodyOfWater).Nullable();
            Map(x => x.EnforcingAgencyCaseNumber).Nullable();
            Map(x => x.CallReceived).Nullable();
            Map(x => x.CrewArrivedOnSite).Nullable();
            Map(x => x.SewageContained).Nullable();
            Map(x => x.StoppageCleared).Nullable();
            Map(x => x.WorkCompleted).Nullable();
            Map(x => x.LocationOfStoppage).Nullable();
            Map(x => x.TruckNumber).Nullable();
            Map(x => x.OverflowCustomers).Nullable();
            Map(x => x.CreatedAt, "CreatedOn");
            Map(x => x.IsSystemNewlyAcquired).Not.Nullable().Default("false");
            Map(x => x.IsSystemUnderConsentOrder).Not.Nullable().Default("false");
            Map(x => x.SewageRecoveredGallons).Nullable();
            Map(x => x.DischargeLocationOther).Nullable();

            // OperatingCenter is a required field and is set on every record. 
            //  But it's marked nullable in the database.
            References(x => x.OperatingCenter).Nullable();
            References(x => x.Town).Nullable();
            References(x => x.WasteWaterSystem).Nullable();
            References(x => x.Street).Nullable();
            References(x => x.CrossStreet, "CrossStreet").Nullable();
            References(x => x.Coordinate).Nullable();
            References(x => x.BodyOfWater).Nullable();
            References(x => x.SewerClearingMethod).Nullable();
            References(x => x.AreaCleanedUpTo).Nullable();
            References(x => x.ZoneType).Nullable();
            References(x => x.CreatedBy).Nullable();
            References(x => x.WorkOrder).Nullable();
            References(x => x.WeatherType, "DischargeWeatherRelatedTypeId").Nullable();
            References(x => x.DischargeLocation, "SewerOverflowDischargeLocationId").Nullable();
            References(x => x.OverflowType, "SewerOverflowTypeId").Nullable();
            References(x => x.OverflowCause, "SewerOverflowCauseId").Nullable();

            HasMany(x => x.SewerOverflowNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.SewerOverflowDocuments).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
