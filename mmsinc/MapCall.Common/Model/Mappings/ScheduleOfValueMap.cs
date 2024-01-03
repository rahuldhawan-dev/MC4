using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ScheduleOfValueMap : ClassMap<ScheduleOfValue>
    {
        public ScheduleOfValueMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.ScheduleOfValueCategory).Nullable();
            References(x => x.UnitOfMeasure).Nullable();

            Map(x => x.Description).Not.Nullable();
            Map(x => x.LaborUnitCost).Nullable();
            Map(x => x.LaborUnitOvertimeCost).Nullable();
            Map(x => x.MaterialCost).Nullable();
            Map(x => x.MiscCost).Nullable();
        }
    }
}
