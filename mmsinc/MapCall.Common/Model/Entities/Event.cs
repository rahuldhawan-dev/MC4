using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Event : IEntity, IThingWithNotes, IThingWithDocuments
    {
        #region Consts

        public struct StringLengths
        {
            public const int EVENT_SUMMARY = 32000,
                             ROOT_CAUSE = 32000,
                             RESPONSE_ACTIONS = 32000,
                             OWNERS = 50;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual EventCategory EventCategory { get; set; }
        public virtual EventSubcategory EventSubcategory { get; set; }
        public virtual string EventSummary { get; set; }
        public virtual bool? IsActive { get; set; }
        public virtual string RootCause { get; set; }
        public virtual string ResponseActions { get; set; }
        public virtual int? EstimatedDurationHours { get; set; }
        public virtual int? NumberCustomersImpacted { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual string Owners { get; set; }
        public virtual Coordinate Coordinate { get; set; }

        #endregion

        #region Documents

        [DoesNotExport]
        public virtual string TableName => EventMap.TABLE_NAME;

        public virtual IList<Document<Event>> Documents { get; set; }
        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        #endregion

        #region Notes

        public virtual IList<Note<Event>> Notes { get; set; }
        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        #endregion
    }
}
