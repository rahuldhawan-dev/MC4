using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class IncidentNurseRecommendationTypeMap : EntityLookupMap<IncidentNurseRecommendationType>
    {
        protected override int DescriptionLength
        {
            get { return 100; }
        }
    }
}
