using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class CrewAssignmentMap : ClassMap<CrewAssignment>
    {
        #region Constructors

        public CrewAssignmentMap()
        {
            Id(x => x.Id, "CrewAssignmentID");

            References(x => x.Crew).Not.Nullable();
            References(x => x.WorkOrder).Not.Nullable();
            References(x => x.StartedBy).Nullable();

            Map(x => x.AssignedOn).Not.Nullable();
            Map(x => x.AssignedFor).Not.Nullable();
            Map(x => x.DateStarted);
            Map(x => x.DateEnded);
            Map(x => x.Priority).Not.Nullable();
            Map(x => x.EmployeesOnJob);
        }

        #endregion
    }
}
