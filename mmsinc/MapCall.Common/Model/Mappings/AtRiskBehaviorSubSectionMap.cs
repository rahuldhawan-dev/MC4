using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class AtRiskBehaviorSubSectionMap : EntityLookupMap<AtRiskBehaviorSubSection>
    {
        public AtRiskBehaviorSubSectionMap()
        {
            Map(x => x.SubSectionNumber)
               .Not.Nullable();

            References(x => x.Section, "AtRiskBehaviorSectionId")
               .Not.Nullable();
        }
    }
}
