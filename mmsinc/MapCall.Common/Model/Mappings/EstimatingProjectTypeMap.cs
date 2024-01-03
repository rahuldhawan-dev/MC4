using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EstimatingProjectTypeMap : ClassMap<EstimatingProjectType>
    {
        public EstimatingProjectTypeMap()
        {
            Table("EstimatingProjectTypes");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Description).Not.Nullable().Length(25);
        }
    }
}
