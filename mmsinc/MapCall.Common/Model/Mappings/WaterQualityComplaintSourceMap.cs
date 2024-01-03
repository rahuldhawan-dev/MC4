using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class WaterQualityComplaintSourceMap : ClassMap<WaterQualityComplaintSource>
    {
        public WaterQualityComplaintSourceMap()
        {
            Table("WaterQualityComplaintSources");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Description).Not.Nullable().Length(30);
        }
    }
}
