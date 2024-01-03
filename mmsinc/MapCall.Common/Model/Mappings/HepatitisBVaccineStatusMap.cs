using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class HepatitisBVaccineStatusMap : ClassMap<HepatitisBVaccineStatus>
    {
        public HepatitisBVaccineStatusMap()
        {
            Table("HepatitisBVaccineStatuses");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Description).Not.Nullable().Length(50);
        }
    }
}
