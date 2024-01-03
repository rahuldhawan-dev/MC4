using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EventTypeMap : ClassMap<EventType>
    {
        #region Constants

        public const string TABLE_NAME = "EventTypes";

        #endregion

        #region Constructors

        public EventTypeMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "EventTypeID");
            Map(x => x.Description).Not.Nullable();
            Map(x => x.CreatedBy).Nullable();
        }

        #endregion
    }
}
