using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class WorkCategoryMap : EntityLookupMap<WorkCategory>
    {
        protected override string IdName => "WorkCategoryID";

        public WorkCategoryMap()
        {
            Table("WorkCategories");
        }
    }
}
