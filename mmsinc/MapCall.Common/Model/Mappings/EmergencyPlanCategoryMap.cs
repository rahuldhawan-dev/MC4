using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EmergencyPlanCategoryMap : EntityLookupMap<EmergencyPlanCategory>
    {
        public EmergencyPlanCategoryMap()
        {
            Table("EmergencyPlanCategories");
        }
    }
}
