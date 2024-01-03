using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SewerMainInspectionTypeMap : EntityLookupMap<SewerMainInspectionType>
    {
        public SewerMainInspectionTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            ReadOnly();
        }
    }
}
