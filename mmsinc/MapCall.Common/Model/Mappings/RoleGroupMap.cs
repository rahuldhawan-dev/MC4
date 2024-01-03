using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class RoleGroupMap : ClassMap<RoleGroup>
    {
        public RoleGroupMap()
        {
            Id(x => x.Id);

            Map(x => x.Name)
               .Not.Nullable()
               .Unique()
               .Length(RoleGroup.StringLengths.NAME);

            HasMany(x => x.Roles)
               .KeyColumn("RoleGroupId")
               .Inverse().Cascade.AllDeleteOrphan();

            HasManyToMany(x => x.Users)
               .Table("RoleGroupsUsers")
               .ParentKeyColumn("RoleGroupId")
               .ChildKeyColumn("UserId");
        }
    }
}
