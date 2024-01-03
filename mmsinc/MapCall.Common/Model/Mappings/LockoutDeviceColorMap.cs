using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class LockoutDeviceColorMap : ClassMap<LockoutDeviceColor>
    {
        public LockoutDeviceColorMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Description).Not.Nullable().Unique();
        }
    }
}
