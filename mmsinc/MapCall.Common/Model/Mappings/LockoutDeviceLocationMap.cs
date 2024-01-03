using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class LockoutDeviceLocationMap : ClassMap<LockoutDeviceLocation>
    {
        public LockoutDeviceLocationMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Description).Not.Nullable().Unique();
            Map(x => x.IsActive).Not.Nullable().Default("true");
        }
    }
}
