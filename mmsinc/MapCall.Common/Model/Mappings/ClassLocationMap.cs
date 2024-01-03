using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ClassLocationMap : ClassMap<ClassLocation>
    {
        public ClassLocationMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.OperatingCenter).Not.Nullable();

            Map(x => x.Name).Column("Description").Not.Nullable().Length(35);
        }
    }
}
