using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class LockoutDevicesMap : ClassMap<LockoutDevice>
    {
        public LockoutDevicesMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.Person).Not.Nullable();
            References(x => x.LockoutDeviceColor).Column("ColorId").Nullable();

            Map(x => x.SerialNumber);
            Map(x => x.Description).Not.Nullable();
        }
    }
}
