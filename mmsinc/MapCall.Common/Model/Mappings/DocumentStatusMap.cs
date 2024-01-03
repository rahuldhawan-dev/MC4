using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class DocumentStatusMap : ClassMap<DocumentStatus>
    {
        public DocumentStatusMap()
        {
            Table("DocumentStatuses");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Description).Not.Nullable().Length(255);
        }
    }
}
