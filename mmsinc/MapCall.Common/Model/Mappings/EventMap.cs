using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EventMap : ClassMap<Event>
    {
        #region Constants

        public const string TABLE_NAME = "Events";

        #endregion

        #region Constructors

        public EventMap()
        {
            //Main Form
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.OperatingCenter, "OperatingCenterId").Nullable();
            References(x => x.EventCategory, "EventCategoryId").Nullable();
            References(x => x.EventSubcategory, "EventSubcategoryId").Nullable();
            Map(x => x.EventSummary).Length(Event.StringLengths.EVENT_SUMMARY).Nullable();
            Map(x => x.IsActive).Nullable();
            Map(x => x.RootCause).Length(Event.StringLengths.ROOT_CAUSE).Nullable();
            Map(x => x.ResponseActions).Length(Event.StringLengths.RESPONSE_ACTIONS).Nullable();
            Map(x => x.EstimatedDurationHours).Nullable();
            Map(x => x.NumberCustomersImpacted).Nullable();
            Map(x => x.StartDate).Nullable();
            Map(x => x.EndDate).Nullable();
            Map(x => x.Owners).Length(Event.StringLengths.OWNERS).Nullable();
            References(x => x.Coordinate, "CoordinateID").Nullable();

            //Notes Docs 
            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").Inverse().Cascade.None();
        }

        #endregion
    }
}
