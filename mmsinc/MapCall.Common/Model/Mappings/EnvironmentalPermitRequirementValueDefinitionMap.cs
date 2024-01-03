using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class
        EnvironmentalPermitRequirementValueDefinitionMap : EntityLookupMap<EnvironmentalPermitRequirementValueDefinition
        >
    {
        public EnvironmentalPermitRequirementValueDefinitionMap()
        {
            Table("EnvironmentalPermitRequirementValueDefinitions");
        }
    }
}
