using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AspnetUsersInRolesMap : ClassMap<AspnetUsersInRoles>
    {
        public AspnetUsersInRolesMap()
        {
            Table("aspnet_UsersInRoles");

            LazyLoad();
            ReadOnly();

            CompositeId().KeyProperty(x => x.UserId, "UserId")
                         .KeyProperty(x => x.RoleId, "RoleId");

            References(x => x.AspnetUsers).Column("UserId").Not.Nullable().ReadOnly();
            References(x => x.AspnetRoles).Column("RoleId").Not.Nullable().ReadOnly();
        }
    }
}
