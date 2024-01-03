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
    public class EventDocument : IEntity, IThingWithNotes, IThingWithDocuments
    {
        #region Consts

        public struct StringLengths
        {
            public const int DESCRIPTION = 32000;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Facility Facility { get; set; }
        public virtual EventType EventType { get; set; }
        public virtual string Description { get; set; }

        #endregion

        #region Documents

        [DoesNotExport]
        public virtual string TableName => EventDocumentMap.TABLE_NAME;

        public virtual IList<Document<EventDocument>> Documents { get; set; }
        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        #endregion

        #region Notes

        public virtual IList<Note<EventDocument>> Notes { get; set; }
        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        #endregion
    }
}
