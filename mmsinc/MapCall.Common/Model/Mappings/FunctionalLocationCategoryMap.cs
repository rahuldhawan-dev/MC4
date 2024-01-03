using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class FunctionalLocationCategoryMap : EntityLookupMap<FunctionalLocationCategory>
    {
        public const string TABLE_NAME = "FunctionalLocationCategories";

        public FunctionalLocationCategoryMap()
        {
            Table(TABLE_NAME);
            Map(x => x.SAPCode);
        }
    }
}
