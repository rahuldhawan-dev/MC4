using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class InvestmentProjectCategoryMap : EntityLookupMap<InvestmentProjectCategory>
    {
        public InvestmentProjectCategoryMap()
        {
            Table("InvestmentProjectCategories");
        }
    }
}
