using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class FacilityProcessStepTriggerMap : ClassMap<FacilityProcessStepTrigger>
    {
        public FacilityProcessStepTriggerMap()
        {
            Id(x => x.Id);

            Map(x => x.Description)
               .Length(FacilityProcessStepTrigger.MAX_DESCRIPTION_LENGTH)
               .Not.Nullable()
               .Unique();

            Map(x => x.Sequence).Not.Nullable();

            References(x => x.Alarm, "FacilityProcessStepAlarmId").Not.Nullable();
            References(x => x.FacilityProcessStep).Not.Nullable();
            References(x => x.TriggerLevel, "FacilityProcessStepTriggerLevelId").Not.Nullable();
            References(x => x.TriggerType, "FacilityProcessStepTriggerTypeId").Not.Nullable();

            HasMany(x => x.Actions).KeyColumn("FacilityProcessStepTriggerId").Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
