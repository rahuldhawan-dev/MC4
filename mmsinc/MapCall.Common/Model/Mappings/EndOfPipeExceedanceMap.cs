using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EndOfPipeExceedanceMap : ClassMap<EndOfPipeExceedance>
    {
        public EndOfPipeExceedanceMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.EventDate).Not.Nullable();
            Map(x => x.ConsentOrder).Not.Nullable();
            Map(x => x.NewAcquisition).Not.Nullable();
            Map(x => x.BriefDescription).Not.Nullable();
            Map(x => x.EndOfPipeExceedanceRootCauseOtherReason).Nullable();
            Map(x => x.EndOfPipeExceedanceTypeOtherReason).Nullable();

            References(x => x.State).Not.Nullable();
            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.WasteWaterSystem).Not.Nullable();
            References(x => x.Facility).Nullable();
            References(x => x.EndOfPipeExceedanceType).Not.Nullable();
            References(x => x.EndOfPipeExceedanceRootCause).Not.Nullable();
            References(x => x.LimitationType).Not.Nullable();

            HasMany(x => x.Documents)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.ActionItems)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}

