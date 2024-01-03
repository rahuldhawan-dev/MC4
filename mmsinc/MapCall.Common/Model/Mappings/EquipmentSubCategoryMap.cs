using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EquipmentSubCategoryMap : ClassMap<EquipmentSubCategory>
    {
        #region Constants

        public const string TABLE_NAME = "EquipmentSubCategories";

        #endregion

        #region Constructors

        public EquipmentSubCategoryMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "EquipmentSubCategoryID");

            Map(x => x.Description).Not.Nullable();

            HasMany(x => x.EquipmentPurposes).KeyColumn("SubCategoryID");
        }

        #endregion
    }
}
