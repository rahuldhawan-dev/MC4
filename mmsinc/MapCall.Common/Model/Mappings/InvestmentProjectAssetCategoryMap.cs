using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class InvestmentProjectAssetCategoryMap : EntityLookupMap<InvestmentProjectAssetCategory>
    {
        public InvestmentProjectAssetCategoryMap()
        {
            Table("InvestmentProjectAssetCategories");
        }
    }
}
