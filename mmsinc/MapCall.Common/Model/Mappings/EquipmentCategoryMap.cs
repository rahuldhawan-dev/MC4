using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EquipmentCategoryMap : ClassMap<EquipmentCategory>
    {
        #region Constants

        public const string TABLE_NAME = "EquipmentCategories";

        #endregion

        #region Constructors

        public EquipmentCategoryMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "EquipmentCategoryID");

            Map(x => x.Description).Not.Nullable();

            HasMany(x => x.EquipmentPurposes).KeyColumn("CategoryID");
        }

        #endregion
    }
}
