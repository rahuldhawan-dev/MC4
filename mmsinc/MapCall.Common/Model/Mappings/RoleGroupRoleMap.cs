using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class RoleGroupRoleMap : ClassMap<RoleGroupRole>
    {
        public RoleGroupRoleMap()
        {
            Id(x => x.Id);
            References(x => x.Action).Not.Nullable();
            References(x => x.Module).Not.Nullable();
            References(x => x.OperatingCenter).Nullable();
            References(x => x.RoleGroup).Not.Nullable();
        }
    }
}
