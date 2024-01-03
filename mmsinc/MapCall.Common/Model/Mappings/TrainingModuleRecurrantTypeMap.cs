using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class TrainingModuleRecurrantTypeMap : ClassMap<TrainingModuleRecurrantType>
    {
        public TrainingModuleRecurrantTypeMap()
        {
            LazyLoad();

            Id(x => x.Id).Column("Id").GeneratedBy.Assigned();

            Map(x => x.Description).Not.Nullable().Length(50);
        }
    }
}
