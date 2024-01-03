using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class RecurringProjectMainMap : ClassMap<RecurringProjectMain>
    {
        public RecurringProjectMainMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();

            References(x => x.RecurringProject).Not.Nullable();

            Map(x => x.Layer).Not.Nullable();
            Map(x => x.Guid).Not.Nullable();
            Map(x => x.TotalInfoMasterScore).Nullable();
            Map(x => x.Length).Not.Nullable();

            Map(x => x.DateInstalled).Nullable();
            Map(x => x.Diameter).Nullable();
            Map(x => x.Material).Nullable();
        }
    }
}
