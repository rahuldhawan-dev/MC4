using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EventDocumentMap : ClassMap<EventDocument>
    {
        #region Constants

        public const string TABLE_NAME = "EventDocuments";

        #endregion

        #region Constructors

        public EventDocumentMap()
        {
            //Main Form
            Table(TABLE_NAME);
            Id(x => x.Id, "EventDocumentID").GeneratedBy.Identity();
            References(x => x.OperatingCenter, "OperatingCenterID").Nullable();
            References(x => x.Facility, "FacilityID").Nullable();
            References(x => x.EventType, "EventTypeID").Not.Nullable();
            Map(x => x.Description).Length(EventDocument.StringLengths.DESCRIPTION).Nullable();

            //Notes Docs 
            HasMany(x => x.Documents).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.Notes).KeyColumn("LinkedId").Inverse().Cascade.None();
        }

        #endregion
    }
}
