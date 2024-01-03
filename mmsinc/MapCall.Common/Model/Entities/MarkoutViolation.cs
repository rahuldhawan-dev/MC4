using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.Utilities.Excel;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MarkoutViolation : IEntity, IThingWithNotes, IThingWithDocuments, IThingWithCoordinate
    {
        public struct StringLengths
        {
            public const int MARKOUT_VIOLATION_STATUS = 255,
                             VIOLATION = 255,
                             MARKOUT_REQUEST_NUMBER = 255,
                             OC_NUMBER = 50,
                             OPERATOR_OF_FACILITY = 255,
                             LOCATION = 255,
                             MARKOUT_PERFORMED_BY = 255,
                             ROOT_CAUSE = 255;
        }

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }

        [DoesNotExport]
        public virtual MapIcon Icon => Coordinate?.Icon;

        public virtual string MarkoutViolationStatus { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual string Violation { get; set; }

        [View(DisplayFormat = CommonStringFormats.DATE)]
        public virtual DateTime? DateOfViolationNotice { get; set; }

        public virtual string MarkoutRequestNumber { get; set; }

        [View(DisplayName = "Case Number")]
        public virtual string OCNumber { get; set; }

        public virtual string OperatorOfFacility { get; set; }
        public virtual string Location { get; set; }
        public virtual Town Town { get; set; }

        [View(DisplayFormat = CommonStringFormats.DATE)]
        public virtual DateTime? DateOfProbableViolation { get; set; }

        public virtual string MarkoutPerformedBy { get; set; }
        public virtual string RootCause { get; set; }
        public virtual bool? Contest { get; set; }

        [View(FormatStyle.Currency)]
        public virtual decimal? FineAmount { get; set; }

        public virtual WorkOrder WorkOrder { get; set; }

        public virtual Coordinate Coordinate { get; set; }

        #endregion

        #region Logical Properties

        [DoesNotExport]
        public virtual string TableName => nameof(MarkoutViolation) + "s";

        [DoesNotExport]
        public virtual IList<MarkoutViolationDocument> MarkoutViolationDocuments { get; set; }

        [DoesNotExport]
        public virtual IList<MarkoutViolationNote> MarkoutViolationNotes { get; set; }

        [DoesNotExport]
        public virtual IList<IDocumentLink> LinkedDocuments => MarkoutViolationDocuments.Map(e => (IDocumentLink)e);

        [DoesNotExport]
        public virtual IList<Document> Documents => MarkoutViolationDocuments.Map(e => e.Document);

        [DoesNotExport]
        public virtual IList<INoteLink> LinkedNotes => MarkoutViolationNotes.Map(e => (INoteLink)e);

        [DoesNotExport]
        public virtual IList<Note> Notes => MarkoutViolationNotes.Map(e => e.Note);

        #endregion

        #endregion
    }
}
