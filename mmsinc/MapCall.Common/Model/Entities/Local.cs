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
    public class Local : IEntity, IValidatableObject, IThingWithNotes, IThingWithDocuments, IThingWithCoordinate,
        IThingWithOperatingCenter
    {
        #region Constants

        public struct StringLengths
        {
            public const int LOCAL = 255, DESCRIPTION = 255, SAP_UNION_DESCRIPTION = 255;
        }

        #endregion

        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }

        [Required, StringLength(StringLengths.LOCAL), Display(Name = "Local")]
        public virtual string Name { get; set; }

        [StringLength(StringLengths.DESCRIPTION)]
        public virtual string Description { get; set; }

        #endregion

        #region References

        [Display(Name = "Bargaining Unit"), Required]
        public virtual Union Union { get; set; }

        public virtual Coordinate Coordinate { get; set; }
        public virtual MapIcon Icon => Coordinate != null ? Coordinate.Icon : null;

        [Display(Name = "Op Code"), Required]
        public virtual OperatingCenter OperatingCenter { get; set; }

        public virtual Division Division { get; set; }
        public virtual State State { get; set; }

        public virtual bool IsActive { get; set; }
        public virtual string SAPUnionDescription { get; set; }

        public virtual IList<Document<Local>> Documents { get; set; }
        public virtual IList<Note<Local>> Notes { get; set; }

        #endregion

        #region Logical Properties

        public virtual decimal? Latitude => (Coordinate != null) ? Coordinate.Latitude : (decimal?)null;

        public virtual decimal? Longitude => (Coordinate != null) ? Coordinate.Longitude : (decimal?)null;

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        [Display(Name = "Documents")]
        public virtual int DocumentCount => Documents.Count;

        [Display(Name = "Notes")]
        public virtual int NoteCount => Notes.Count;

        [DoesNotExport]
        public virtual string TableName => LocalMap.TABLE_NAME;

        #endregion

        #endregion

        #region Constructors

        public Local()
        {
            Documents = new List<Document<Local>>();
            Notes = new List<Note<Local>>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
