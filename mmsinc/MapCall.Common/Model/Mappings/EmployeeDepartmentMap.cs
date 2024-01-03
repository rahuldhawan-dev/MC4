using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EmployeeDepartmentMap : ClassMap<EmployeeDepartment>
    {
        public EmployeeDepartmentMap()
        {
            Id(x => x.Id);

            Map(x => x.Description).Not.Nullable().Unique();
        }
    }
}
