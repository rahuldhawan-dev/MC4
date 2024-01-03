using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AspnetPersonalizationAllUsersMap : ClassMap<AspnetPersonalizationAllUsers>
    {
        public AspnetPersonalizationAllUsersMap()
        {
            Table("aspnet_PersonalizationAllUsers");

            LazyLoad();
            ReadOnly();

            Id(x => x.PathId).GeneratedBy.Assigned().Column("PathId");

            References(x => x.AspnetPaths).Column("PathId").Not.Nullable().ReadOnly();

            Map(x => x.PageSettings).Column("PageSettings").Not.Nullable();
            Map(x => x.LastUpdatedDate).Not.Nullable();
        }
    }
}
