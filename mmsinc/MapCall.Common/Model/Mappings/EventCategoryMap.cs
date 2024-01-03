using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EventCategoryMap : ClassMap<EventCategory>
    {
        #region Constants

        public const string TABLE_NAME = "EventCategories";

        #endregion

        #region Constructors

        public EventCategoryMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id);

            Map(x => x.Description).Not.Nullable();
        }

        #endregion
    }
}
