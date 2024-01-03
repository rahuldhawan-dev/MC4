using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AspnetRolesMap : ClassMap<AspnetRoles>
    {
        public AspnetRolesMap()
        {
            Table("aspnet_Roles");

            LazyLoad();
            ReadOnly();

            Id(x => x.RoleId).GeneratedBy.Assigned().Column("RoleId");

            References(x => x.AspnetApplications).Column("ApplicationId").Not.Nullable();

            Map(x => x.RoleName).Column("RoleName").Not.Nullable();
            Map(x => x.LoweredRoleName).Column("LoweredRoleName").Not.Nullable();
            Map(x => x.Description).Column("Description").Nullable();
        }
    }
}
