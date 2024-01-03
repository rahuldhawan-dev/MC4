using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AspnetProfileMap : ClassMap<AspnetProfile>
    {
        public AspnetProfileMap()
        {
            Table("aspnet_Profile");

            LazyLoad();
            ReadOnly();

            Id(x => x.UserId).GeneratedBy.Assigned().Column("UserId");

            References(x => x.AspnetUsers).Column("UserId").Not.Nullable().ReadOnly();

            Map(x => x.PropertyNames).Column("PropertyNames").Not.Nullable();
            Map(x => x.PropertyValuesString).Column("PropertyValuesString").Not.Nullable();
            Map(x => x.PropertyValuesBinary).Column("PropertyValuesBinary").Not.Nullable();
            Map(x => x.LastUpdatedDate).Not.Nullable();
        }
    }
}
