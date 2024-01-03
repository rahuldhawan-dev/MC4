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
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SewerMainCleaning
        : IEntityWithCreationTracking<User>,
            IValidatableObject,
            IThingWithNotes,
            IThingWithDocuments,
            IThingWithOperatingCenter,
            ISAPInspection,
            IThingWithSyncing
    {
        #region Constants

        public struct StringLengths
        {
            public const int OPENING1_CATCHBASIN = 55,
                             OPENING2_CATCHBASIN = 55,
                             MAP_PAGE = 50,
                             OVERFLOW_OPENING_1 = 255,
                             OVERFLOW_OPENING_2 = 255,
                             CREATED_BY = 50;
        }

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }
        
        public virtual OperatingCenter OperatingCenter { get; set; }
        
        public virtual Town Town { get; set; }
        
        public virtual DateTime? Date { get; set; }

        public virtual DateTime? InspectedDate { get; set; }
        
        public virtual SewerMainInspectionType InspectionType { get; set; }

        public virtual SewerMainInspectionGrade InspectionGrade { get; set; }
        
        public virtual Street Street { get; set; }
        
        public virtual Street CrossStreet { get; set; }
        
        public virtual Street CrossStreet2 { get; set; }

        public virtual SewerOpening Opening1 { get; set; }

        public virtual SewerOpening Opening2 { get; set; }

        [View("Opening 2 is a Terminus")]
        public virtual bool? Opening2IsATerminus { get; set; }
        
        // TODO: This should *not* be a float. This should be a decimal in the db and here.
        // There's almost never a reason to use floats/doubles in business logic. 
        public virtual float? FootageOfMainInspected { get; set; }

        public virtual bool BlockageFound { get; set; }

        public virtual CauseOfBlockage CauseOfBlockage { get; set; }

        public virtual bool Overflow { get; set; }

        public virtual SewerOverflow SewerOverflow { get; set; }

        [DisplayName("Notes")]
        public virtual string TableNotes { get; set; }

        public virtual OpeningCondition Opening1Condition { get; set; }

        public virtual OpeningFrameAndCover Opening1FrameAndCover { get; set; }

        public virtual string Opening1Catchbasin { get; set; }

        public virtual string OverflowOpening1 { get; set; }

        public virtual OpeningCondition Opening2Condition { get; set; }

        public virtual OpeningFrameAndCover Opening2FrameAndCover { get; set; }

        public virtual string Opening2Catchbasin { get; set; }

        public virtual string OverflowOpening2 { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual MainCondition MainCondition { get; set; }

        [View("Gallons of Water Used (Unmetered)")]
        public virtual int? GallonsOfWaterUsed { get; set; }

        public virtual Hydrant HydrantUsed { get; set; }

        public virtual string SAPErrorCode { get; set; }

        public virtual string SAPNotificationNumber { get; set; }

        public virtual bool NeedsToSync { get; set; }

        public virtual DateTime? LastSyncedAt { get; set; }

        [DoesNotExport]
        public virtual CleaningSchedule CleaningSchedule { get; set; }

        [DoesNotExport]
        public virtual bool SendToSAP =>
            (
                (Opening1 != null && Opening1.OperatingCenter.SAPEnabled &&
                 !Opening1.OperatingCenter.IsContractedOperations)
                ||
                (Opening2 != null && Opening2.OperatingCenter.SAPEnabled &&
                 !Opening2.OperatingCenter.IsContractedOperations)
            )
            && string.IsNullOrEmpty(SAPNotificationNumber);

        #endregion

        #region Logical Properties

        #region Documents

        public virtual IList<SewerMainCleaningDocument> SewerMainCleaningDocuments { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return SewerMainCleaningDocuments.Map(epd => (IDocumentLink)epd); }
        }

        public virtual IList<Document> Documents
        {
            get { return SewerMainCleaningDocuments.Map(epd => epd.Document); }
        }

        #endregion

        #region Notes

        public virtual IList<SewerMainCleaningNote> SewerMainCleaningNotes { get; set; }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return SewerMainCleaningNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<Note> Notes
        {
            get { return SewerMainCleaningNotes.Map(n => n.Note); }
        }

        #endregion

        public virtual string TableName => nameof(SewerMainCleaning) + "s";

        #region Formula Fields

        public virtual int Month { get; set; }
        public virtual int Year { get; set; }

        #endregion

        #endregion

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    [Serializable]
    public class OpeningCondition : ReadOnlyEntityLookup
    {
        public virtual bool IsActive { get; set; }
    }

    [Serializable]
    public class OpeningFrameAndCover : EntityLookup { }

    [Serializable]
    public class CleaningSchedule : EntityLookup { }
}
