using System;
using System.Collections.Generic;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class WaterQualityComplaintCustomerSatisfactionMap : ClassMap<WaterQualityComplaintCustomerSatisfaction>
    {
        public WaterQualityComplaintCustomerSatisfactionMap()
        {
            Table("WaterQualityComplaintCustomerSatisfactions");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Description).Not.Nullable().Length(25);
        }
    }
}
