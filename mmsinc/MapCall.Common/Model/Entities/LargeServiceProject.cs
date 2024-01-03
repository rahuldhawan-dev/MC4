using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class LargeServiceProject : IEntity, IValidatableObject, IThingWithCoordinate, IThingWithNotes,
        IThingWithDocuments
    {
        #region Constants

        public struct StringLengths
        {
            public const int WBS_NUMBER = 18,
                             PROJECT_TITLE = 255,
                             PROJECT_ADDRESS = 255,
                             CONTACT_NAME = 100,
                             CONTACT_EMAIL = 100,
                             CONTACT_PHONE = 20;
        }

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Town Town { get; set; }
        public virtual string WBSNumber { get; set; }
        public virtual string ProjectTitle { get; set; }
        public virtual string ProjectAddress { get; set; }
        public virtual AssetCategory AssetCategory { get; set; }
        public virtual AssetType AssetType { get; set; }
        public virtual PipeDiameter ProposedPipeDiameter { get; set; }
        public virtual string ContactName { get; set; }
        public virtual string ContactEmail { get; set; }
        public virtual string ContactPhone { get; set; }
        public virtual DateTime? InitialContactDate { get; set; }

        [DoesNotExport]
        public virtual MapIcon Icon => Coordinate.Icon;

        public virtual Coordinate Coordinate { get; set; }
        public virtual DateTime? InServiceDate { get; set; }

        /// <summary>
        /// This is a formula property.
        /// </summary>
        public virtual string CreatedBy { get; set; }

        #endregion

        #region Logical Properties

        #region Documents

        public virtual IList<LargeServiceProjectDocument> LargeServiceProjectDocuments { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return LargeServiceProjectDocuments.Map(epd => (IDocumentLink)epd); }
        }

        public virtual IList<Document> Documents
        {
            get { return LargeServiceProjectDocuments.Map(epd => epd.Document); }
        }

        #endregion

        #region Notes

        public virtual IList<LargeServiceProjectNote> LargeServiceProjectNotes { get; set; }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return LargeServiceProjectNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<Note> Notes
        {
            get { return LargeServiceProjectNotes.Map(n => n.Note); }
        }

        #endregion

        [DoesNotExport]
        public virtual string TableName => nameof(LargeServiceProject) + "s";

        public virtual decimal? Latitude => Coordinate?.Latitude;
        public virtual decimal? Longitude => Coordinate?.Longitude;

        #endregion

        #endregion

        #region Constructors

        public LargeServiceProject()
        {
            LargeServiceProjectDocuments = new List<LargeServiceProjectDocument>();
            LargeServiceProjectNotes = new List<LargeServiceProjectNote>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
