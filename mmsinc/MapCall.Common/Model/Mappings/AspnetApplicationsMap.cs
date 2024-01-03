using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AspnetApplicationsMap : ClassMap<AspnetApplications>
    {
        public AspnetApplicationsMap()
        {
            Table("aspnet_Applications");

            LazyLoad();
            ReadOnly();

            Id(x => x.ApplicationId).GeneratedBy.Assigned().Column("ApplicationId");

            Map(x => x.ApplicationName).Column("ApplicationName").Not.Nullable().Unique();
            Map(x => x.LoweredApplicationName).Column("LoweredApplicationName").Not.Nullable().Unique();
            Map(x => x.Description).Column("Description").Nullable();
        }
    }
}
