using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class OneCallMarkoutResponseTechniqueMap : ClassMap<OneCallMarkoutResponseTechnique>
    {
        public OneCallMarkoutResponseTechniqueMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Description).Not.Nullable();
        }
    }
}
