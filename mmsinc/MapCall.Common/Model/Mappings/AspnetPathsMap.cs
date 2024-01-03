using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AspnetPathsMap : ClassMap<AspnetPaths>
    {
        public AspnetPathsMap()
        {
            Table("aspnet_Paths");

            LazyLoad();
            ReadOnly();

            Id(x => x.PathId).GeneratedBy.Assigned().Column("PathId");

            References(x => x.AspnetApplications).Column("ApplicationId").Not.Nullable();

            Map(x => x.Path).Column("Path").Not.Nullable();
            Map(x => x.LoweredPath).Column("LoweredPath").Not.Nullable();
        }
    }
}
