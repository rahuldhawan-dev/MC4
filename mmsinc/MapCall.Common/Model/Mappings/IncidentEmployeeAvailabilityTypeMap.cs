using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class IncidentEmployeeAvailabilityTypeMap : EntityLookupMap<IncidentEmployeeAvailabilityType>
    {
        public IncidentEmployeeAvailabilityTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
