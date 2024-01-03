using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EchoshoreSiteMap : ClassMap<EchoshoreSite>
    {
        public EchoshoreSiteMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.Town).Not.Nullable();
            References(x => x.OperatingCenter).Not.Nullable();

            Map(x => x.Description).Not.Nullable().Length(EchoshoreSite.StringLengths.DESCRIPTION);
        }
    }
}
