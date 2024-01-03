using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class RecurringFrequencyUnitMap : ClassMap<RecurringFrequencyUnit>
    {
        public RecurringFrequencyUnitMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Description);
        }
    }
}
