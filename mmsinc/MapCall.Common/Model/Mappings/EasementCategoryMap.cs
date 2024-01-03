using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EasementCategoryMap : EntityLookupMap<EasementCategory>
    {
        public const string TABLE_NAME = "EasementCategories";

        public EasementCategoryMap()
        {
            Table(TABLE_NAME);
        }
    }
}
