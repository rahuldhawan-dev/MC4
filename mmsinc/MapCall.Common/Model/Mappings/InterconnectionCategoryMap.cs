using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class InterconnectionCategoryMap : ClassMap<InterconnectionCategory>
    {
        #region Constants

        public const string TABLE_NAME = "InterconnectionCategories";

        #endregion

        #region Constructors

        public InterconnectionCategoryMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "InterconnectionCategoryId").GeneratedBy.Identity();
            Map(x => x.Description).Not.Nullable().Length(InterconnectionCategory.StringLengths.DESCRIPTION);
            HasMany(x => x.Interconnections).KeyColumn("CategoryId");
        }

        #endregion
    }
}
