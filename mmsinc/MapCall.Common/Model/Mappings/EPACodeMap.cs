using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EPACodeMap : ClassMap<EPACode>
    {
        public EPACodeMap()
        {
            Table("EPACodes");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Description).Not.Nullable().Length(500);
        }
    }
}
