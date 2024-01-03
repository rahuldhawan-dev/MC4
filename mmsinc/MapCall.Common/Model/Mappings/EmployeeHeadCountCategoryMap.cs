using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EmployeeHeadCountCategoryMap : EntityLookupMap<EmployeeHeadCountCategory>
    {
        public EmployeeHeadCountCategoryMap()
        {
            Table("EmployeeHeadCountCategories");
        }
    }
}
