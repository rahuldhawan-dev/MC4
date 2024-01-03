using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AspnetUsersMap : ClassMap<AspnetUsers>
    {
        public AspnetUsersMap()
        {
            Table("aspnet_Users");

            LazyLoad();
            ReadOnly();

            Id(x => x.UserId).GeneratedBy.Assigned().Column("UserId");

            References(x => x.AspnetApplications).Column("ApplicationId").Not.Nullable();

            Map(x => x.UserName).Column("UserName").Not.Nullable();
            Map(x => x.LoweredUserName).Column("LoweredUserName").Not.Nullable();
            Map(x => x.MobileAlias).Column("MobileAlias").Nullable();
            Map(x => x.IsAnonymous).Column("IsAnonymous").Not.Nullable();
            Map(x => x.LastActivityDate).Column("LastActivityDate").Not.Nullable();
        }
    }
}
