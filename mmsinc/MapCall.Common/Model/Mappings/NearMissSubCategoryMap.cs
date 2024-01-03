using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class NearMissSubCategoryMap : EntityLookupMap<NearMissSubCategory>
    {
        public const string TABLE_NAME = "NearMissSubCategories";

        public NearMissSubCategoryMap()
        {
            Table(TABLE_NAME);
            References(x => x.Category);
        }
    }
}
