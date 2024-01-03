using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class CovidOutcomeCategoryMap : EntityLookupMap<CovidOutcomeCategory>
    {
        public const string TABLE_NAME = "CovidOutcomeCategories";

        public CovidOutcomeCategoryMap()
        {
            Table(TABLE_NAME);
        }
    }
}
