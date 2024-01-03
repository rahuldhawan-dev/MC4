using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class InsuranceScoreQuartileMap : EntityLookupMap<InsuranceScoreQuartile>
    {
        public InsuranceScoreQuartileMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
