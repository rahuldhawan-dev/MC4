using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class
        EnvironmentalPermitRequirementTrackingFrequencyMap : EntityLookupMap<
            EnvironmentalPermitRequirementTrackingFrequency>
    {
        public EnvironmentalPermitRequirementTrackingFrequencyMap()
        {
            Table("EnvironmentalPermitRequirementTrackingFrequencies");
        }
    }
}
