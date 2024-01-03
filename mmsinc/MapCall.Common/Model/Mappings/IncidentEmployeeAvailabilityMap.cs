using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class IncidentEmployeeAvailabilityMap : ClassMap<IncidentEmployeeAvailability>
    {
        public IncidentEmployeeAvailabilityMap()
        {
            Table("IncidentEmployeeAvailabilities");

            Id(x => x.Id);

            Map(x => x.EndDate).Nullable();
            Map(x => x.StartDate).Not.Nullable();

            References(x => x.Incident).Not.Nullable();
            References(x => x.EmployeeAvailabilityType, "IncidentEmployeeAvailabilityTypeId").Not.Nullable();
        }
    }
}
