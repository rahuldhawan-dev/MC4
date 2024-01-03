using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ScadaSignalMap : ClassMap<ScadaSignal>
    {
        public ScadaSignalMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();

            Map(x => x.TagName).Not.Nullable();
            Map(x => x.TagId).Not.Nullable();
            Map(x => x.Description).Nullable();
            Map(x => x.EngineeringUnits).Nullable();

            HasOne(x => x.ScadaTagName);
        }
    }
}
