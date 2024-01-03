using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class TrainingContactHoursProgramCoordinatorMap : ClassMap<TrainingContactHoursProgramCoordinator>
    {
        public TrainingContactHoursProgramCoordinatorMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.ProgramCoordinator).Not.Nullable();
        }
    }
}
