using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EnvironmentalNonComplianceEventSubTypeMap : ClassMap<EnvironmentalNonComplianceEventSubType>
    {
        public EnvironmentalNonComplianceEventSubTypeMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.EnvironmentalNonComplianceEventType).Nullable();

            Map(x => x.Description).Not.Nullable();
        }
    }
}
