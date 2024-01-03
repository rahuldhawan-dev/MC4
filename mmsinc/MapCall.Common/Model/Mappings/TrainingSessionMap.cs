using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class TrainingSessionMap : ClassMap<TrainingSession>
    {
        public TrainingSessionMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            ;

            Map(x => x.StartDateTime).Not.Nullable();
            Map(x => x.EndDateTime).Not.Nullable();

            References(x => x.TrainingRecord).Not.Nullable();
        }
    }
}
