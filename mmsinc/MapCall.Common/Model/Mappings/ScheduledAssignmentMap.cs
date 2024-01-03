using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace Permits.Data.Models.Mappings
{
    public class ScheduledAssignmentMap : ClassMap<ScheduledAssignment>
    {
        public ScheduledAssignmentMap()
        {
            Table("MaintenancePlanScheduledAssignments");
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.AssignedFor).Not.Nullable();
            Map(x => x.ScheduledDate).Not.Nullable();

            References(x => x.MaintenancePlan).Not.Nullable();
            References(x => x.AssignedTo).Not.Nullable();
            References(x => x.CreatedBy).Nullable();
        }
    }
}

