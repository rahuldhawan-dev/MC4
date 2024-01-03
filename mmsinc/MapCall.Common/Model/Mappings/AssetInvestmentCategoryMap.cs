using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AssetInvestmentCategoryMap : ClassMap<AssetInvestmentCategory>
    {
        #region Constants

        public const string TABLE_NAME = "AssetInvestmentCategories";

        #endregion

        #region Constructors

        public AssetInvestmentCategoryMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "AssetInvestmentCategoryID");
            Map(x => x.Description).Not.Nullable();
            Map(x => x.CreatedBy).Not.Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            HasMany(x => x.RecurringProjects).KeyColumn("SecondaryAssetInvestmentCategoryID");
        }

        #endregion
    }
}
