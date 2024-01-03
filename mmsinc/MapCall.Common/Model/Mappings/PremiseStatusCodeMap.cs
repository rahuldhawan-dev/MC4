using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PremiseStatusCodeMap : ClassMap<PremiseStatusCode>
    {
        public PremiseStatusCodeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Description).Not.Nullable();
        }
    }
}
