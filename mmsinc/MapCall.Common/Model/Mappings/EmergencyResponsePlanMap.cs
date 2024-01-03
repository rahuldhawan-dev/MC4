using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EmergencyResponsePlanMap : ClassMap<EmergencyResponsePlan>
    {
        public EmergencyResponsePlanMap()
        {
            Id(x => x.Id);

            Map(x => x.Title).Not.Nullable();
            Map(x => x.Description).Not.Nullable();

            References(x => x.State).Not.Nullable();
            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.Facility).Nullable();
            References(x => x.EmergencyPlanCategory, "PlanCategoryId").Nullable();
            References(x => x.ReviewFrequency).Nullable();

            HasMany(x => x.EmergencyResponsePlanDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.EmergencyResponsePlanNotes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Reviews)
               .KeyColumn("PlanId").Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
