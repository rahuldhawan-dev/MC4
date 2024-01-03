using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ABCIndicatorMap : ClassMap<ABCIndicator>
    {
        public ABCIndicatorMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Description).Not.Nullable().Length(20);
        }
    }
}
