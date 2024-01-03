using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class RoadwayImprovementNotification
        : IEntityWithCreationTracking<User>,
            IValidatableObject,
            IThingWithNotes,
            IThingWithDocuments,
            IThingWithCoordinate
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Town Town { get; set; }

        [DisplayName("Entity")]
        public virtual RoadwayImprovementNotificationEntity RoadwayImprovementNotificationEntity { get; set; }

        public virtual string Description { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime ExpectedProjectStartDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime DateReceived { get; set; }

        public virtual Coordinate Coordinate { get; set; }

        [DisplayName("Status")]
        public virtual RoadwayImprovementNotificationStatus RoadwayImprovementNotificationStatus { get; set; }

        [DisplayName("Precon Action Taken")]
        public virtual RoadwayImprovementNotificationPreconAction RoadwayImprovementNotificationPreconAction
        {
            get;
            set;
        }

        public virtual DateTime? PreconMeetingDate { get; set; }
        public virtual MapIcon Icon => Coordinate.Icon;
        public virtual IList<RoadwayImprovementNotificationStreet> RoadwayImprovementNotificationStreets { get; set; }

        public virtual User CreatedBy { get; set; }
        public virtual DateTime CreatedAt { get; set; }

        #region Logical Properties

        #region Documents

        public virtual IList<RoadwayImprovementNotificationDocument> RoadwayImprovementNotificationDocuments
        {
            get;
            set;
        }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return RoadwayImprovementNotificationDocuments.Map(epd => (IDocumentLink)epd); }
        }

        public virtual IList<Document> Documents
        {
            get { return RoadwayImprovementNotificationDocuments.Map(epd => epd.Document); }
        }

        #endregion

        #region Notes

        public virtual IList<RoadwayImprovementNotificationNote> RoadwayImprovementNotificationNotes { get; set; }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return RoadwayImprovementNotificationNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<Note> Notes
        {
            get { return RoadwayImprovementNotificationNotes.Map(n => n.Note); }
        }

        #endregion

        [DoesNotExport]
        public virtual string TableName => nameof(RoadwayImprovementNotification) + "s";

        #endregion

        #endregion

        #region Constructors

        public RoadwayImprovementNotification()
        {
            RoadwayImprovementNotificationStreets = new List<RoadwayImprovementNotificationStreet>();
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    [Serializable]
    public class RoadwayImprovementNotificationStatus : EntityLookup { }

    [Serializable]
    public class RoadwayImprovementNotificationEntity : EntityLookup { }
}
