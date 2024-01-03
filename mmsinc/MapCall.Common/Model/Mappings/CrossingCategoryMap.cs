using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class CrossingCategoryMap : ClassMap<CrossingCategory>
    {
        #region Constants

        public const string TABLE_NAME = "CrossingCategories";

        #endregion

        #region Constructors

        public CrossingCategoryMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "CrossingCategoryID");
            Map(x => x.Description).Not.Nullable();
        }

        #endregion
    }
}
