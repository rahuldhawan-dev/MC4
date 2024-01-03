using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class FacilityProcessStepTriggerActionMap : ClassMap<FacilityProcessStepTriggerAction>
    {
        public FacilityProcessStepTriggerActionMap()
        {
            Id(x => x.Id);

            Map(x => x.Action)
               .Length(FacilityProcessStepTriggerAction.MAX_ACTION_LENGTH)
               .Not.Nullable();
            Map(x => x.ActionResponse)
               .Length(FacilityProcessStepTriggerAction.MAX_ACTION_RESPONSE_LENGTH)
               .Not.Nullable();
            Map(x => x.Sequence).Not.Nullable();

            References(x => x.Trigger, "FacilityProcessStepTriggerId").Not.Nullable();
        }
    }
}
