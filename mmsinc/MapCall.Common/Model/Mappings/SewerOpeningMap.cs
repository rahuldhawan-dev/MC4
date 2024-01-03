using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SewerOpeningMap : ClassMap<SewerOpening>
    {
        #region Constructors

        public SewerOpeningMap()
        {
            Id(x => x.Id);

            DynamicUpdate();

            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.Town).Not.Nullable();
            References(x => x.BodyOfWater, "BodyOfWaterID").Nullable();
            References(x => x.Street).Nullable();
            References(x => x.IntersectingStreet).Nullable();
            References(x => x.Coordinate).Nullable().Cascade.All();
            References(x => x.Status, "AssetStatusID").Nullable();
            References(x => x.SewerOpeningMaterial).Nullable();
            References(x => x.TownSection).Nullable();
            References(x => x.FunctionalLocation);
            References(x => x.CreatedBy, "CreatedBy").Nullable();
            References(x => x.WasteWaterSystem).Nullable();
            References(x => x.InspectionFrequencyUnit).Nullable();
            References(x => x.SewerOpeningType).Not.Nullable();
            References(x => x.UpdatedBy).Nullable();

            Map(x => x.InspectionFrequency).Nullable();
            Map(x => x.OpeningNumber)
               .Length(SewerOpening.StringLengths.OPENING_NUMBER)
               .Not.Nullable();
            Map(x => x.TaskNumber)
               .Length(SewerOpening.StringLengths.TASK_NUMBER)
               .Nullable();
            Map(x => x.DateInstalled).Nullable();
            Map(x => x.DateRetired).Nullable();
            Map(x => x.MapPage)
               .Length(SewerOpening.StringLengths.MAP_PAGE)
               .Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.UpdatedAt).Not.Nullable();
            Map(x => x.StreetNumber).Length(SewerOpening.StringLengths.STREET_NUMBER).Nullable();
            Map(x => x.OldNumber, "OldNumber")
               .Length(SewerOpening.StringLengths.OLD_NUMBER)
               .Nullable();
            Map(x => x.DistanceFromCrossStreet, "DistanceFromCrossStreet")
               .Length(SewerOpening.StringLengths.DISTANCE_FROM_CROSS_STREET)
               .Nullable();
            Map(x => x.IsEpoxyCoated).Nullable();
            Map(x => x.OpeningSuffix).Not.Nullable();
            Map(x => x.IsDoghouseOpening).Nullable();
            Map(x => x.SAPEquipmentId).Nullable();
            Map(x => x.SAPErrorCode).Nullable();
            Map(x => x.Route).Nullable();
            Map(x => x.Stop).Nullable();
            Map(x => x.OutfallNumber).Nullable();
            Map(x => x.LocationDescription).Nullable();
            Map(x => x.GeoEFunctionalLocation).Nullable();
            Map(x => x.DepthToInvert).Nullable();
            Map(x => x.RimElevation).Nullable();
            Map(x => x.Critical).Not.Nullable();
            Map(x => x.CriticalNotes).Nullable().Length(SewerOpening.StringLengths.CRITICAL_NOTES);

            HasMany(x => x.WorkOrders).KeyColumn("SewerOpeningID");
            HasMany(x => x.Notes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.UpstreamSewerOpeningConnections)
               .KeyColumn("DownstreamSewerOpeningID")
               .LazyLoad().Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.DownstreamSewerOpeningConnections)
               .KeyColumn("UpstreamSewerOpeningID")
               .LazyLoad().Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.SewerMainCleanings1)
               .KeyColumn("Opening1Id")
               .LazyLoad().Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.SewerMainCleanings2)
               .KeyColumn("Opening2Id")
               .LazyLoad().Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.SewerOpeningInspections)
               .KeyColumn("SewerOpeningId")
               .LazyLoad().Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.NpdesRegulatorInspections)
               .KeyColumn("SewerOpeningId")
               .LazyLoad().Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.SmartCoverAlerts)
               .KeyColumn("SewerOpeningId")
               .LazyLoad().Inverse().Cascade.AllDeleteOrphan();
        }

        #endregion
    }
}
