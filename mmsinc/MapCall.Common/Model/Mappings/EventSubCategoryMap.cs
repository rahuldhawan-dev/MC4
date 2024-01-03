using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EventSubcategoryMap : ClassMap<EventSubcategory>
    {
        #region Constants

        public const string TABLE_NAME = "EventSubCategories";

        #endregion

        #region Constructors

        public EventSubcategoryMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id);

            Map(x => x.Description).Not.Nullable();
        }

        #endregion
    }
}
