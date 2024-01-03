using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class AtRiskBehaviorSectionMap : EntityLookupMap<AtRiskBehaviorSection>
    {
        public AtRiskBehaviorSectionMap()
        {
            Map(x => x.SectionNumber).Not.Nullable();

            HasMany(x => x.SubSections).KeyColumn("AtRiskBehaviorSectionId").Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
