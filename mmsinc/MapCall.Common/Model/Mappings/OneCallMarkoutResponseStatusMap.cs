using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class OneCallMarkoutResponseStatusMap : ClassMap<OneCallMarkoutResponseStatus>
    {
        public OneCallMarkoutResponseStatusMap()
        {
            Table("OneCallMarkoutResponseStatuses");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Description).Not.Nullable();
        }
    }
}
