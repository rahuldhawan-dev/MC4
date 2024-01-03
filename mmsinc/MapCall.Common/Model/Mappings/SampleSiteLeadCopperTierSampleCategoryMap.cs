using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SampleSiteLeadCopperTierSampleCategoryMap : EntityLookupMap<SampleSiteLeadCopperTierSampleCategory>
    {
        protected override int DescriptionLength => 100;

        public SampleSiteLeadCopperTierSampleCategoryMap()
        {
            Map(x => x.DisplayValue).Length(SampleSiteLeadCopperTierSampleCategory.DISPLAY_VALUE_LENGTH).Not.Nullable();

            HasManyToMany(x => x.TierClassifications)
               .Table("SampleSiteLeadCopperTierClassificationsSampleSiteLeadCopperTierSampleCategories")
               .ParentKeyColumn("SampleSiteLeadCopperTierSampleCategoryId")
               .ChildKeyColumn("SampleSiteLeadCopperTierClassificationId")
               .Cascade
               .All();
        }
    }
}
