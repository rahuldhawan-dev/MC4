using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EmployeeAssignmentMap : ClassMap<EmployeeAssignment>
    {
        public EmployeeAssignmentMap()
        {
            Id(x => x.Id);

            Map(x => x.AssignedOn).Not.Nullable();
            Map(x => x.AssignedFor).Not.Nullable();
            Map(x => x.DateStarted).Nullable();
            Map(x => x.DateEnded).Nullable();
            Map(x => x.HoursWorked).Not.Nullable();

            Map(x => x.OrderIsOpen)
               .Not.Nullable()
               .Formula(
                    "(SELECT CASE WHEN pwo.DateCompleted IS NULL AND pwo.DateCancelled IS NULL THEN 1 ELSE 0 END FROM ProductionWorkOrders pwo WHERE pwo.Id = ProductionWorkOrderId)");

            References(x => x.AssignedTo).Not.Nullable();
            References(x => x.AssignedBy).Nullable();
            References(x => x.ProductionWorkOrder).Not.Nullable();

            HasManyToMany(x => x.Employees)
               .Table("EmployeeAssignmentsEmployees")
               .ParentKeyColumn("EmployeeAssignmentId")
               .ChildKeyColumn("EmployeeId");
        }
    }
}
