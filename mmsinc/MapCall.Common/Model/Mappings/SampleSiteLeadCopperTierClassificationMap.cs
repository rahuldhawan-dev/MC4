using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SampleSiteLeadCopperTierClassificationMap : EntityLookupMap<SampleSiteLeadCopperTierClassification>
    {
        public SampleSiteLeadCopperTierClassificationMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();

            HasManyToMany(x => x.States)
               .Table("SampleSiteLeadCopperTierClassificationsStates")
               .ParentKeyColumn("SampleSiteLeadCopperTierClassificationId")
               .ChildKeyColumn("StateId")
               .Cascade
               .All();
        }
    }
}
