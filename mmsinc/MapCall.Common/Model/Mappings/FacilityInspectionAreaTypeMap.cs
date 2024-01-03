using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class FacilityInspectionAreaTypeMap : EntityLookupMap<FacilityInspectionAreaType>
    {
        public FacilityInspectionAreaTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
