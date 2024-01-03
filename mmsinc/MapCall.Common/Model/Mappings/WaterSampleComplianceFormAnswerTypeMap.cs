using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class WaterSampleComplianceFormAnswerTypeMap : EntityLookupMap<WaterSampleComplianceFormAnswerType>
    {
        public WaterSampleComplianceFormAnswerTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
