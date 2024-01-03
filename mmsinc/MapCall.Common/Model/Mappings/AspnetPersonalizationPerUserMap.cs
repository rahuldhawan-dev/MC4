using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AspnetPersonalizationPerUserMap : ClassMap<AspnetPersonalizationPerUser>
    {
        public AspnetPersonalizationPerUserMap()
        {
            Table("aspnet_PersonalizationPerUser");

            LazyLoad();
            ReadOnly();

            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");

            References(x => x.AspnetPaths).Column("PathId").Nullable();
            References(x => x.AspnetUsers).Column("UserId").Nullable();

            Map(x => x.PageSettings).Column("PageSettings").Not.Nullable();
            Map(x => x.LastUpdatedDate).Not.Nullable();
        }
    }
}
