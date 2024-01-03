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
    public class Union : IThingWithDocuments, IThingWithNotes, IEntityLookup
    {
        #region Constants

        public struct StringLengths
        {
            public const int BARGAINING_UNIT = 255, ICON = 50;
        }

        #endregion

        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }

        [Required, StringLength(StringLengths.BARGAINING_UNIT)]
        public virtual string BargainingUnit { get; set; }

        [StringLength(StringLengths.ICON)]
        public virtual string Icon { get; set; }

        #endregion

        #region References

        public virtual IList<Document<Union>> Documents { get; set; }
        public virtual IList<Note<Union>> Notes { get; set; }

        #endregion

        #region Logical Properties

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        [DoesNotExport]
        public virtual string TableName => UnionMap.TABLE_NAME;

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        [Display(Name = "Documents")]
        public virtual int DocumentCount => Documents.Count;

        [Display(Name = "Notes")]
        public virtual int NoteCount => Notes.Count;

        [DoesNotExport]
        public virtual string Description => BargainingUnit;

        #endregion

        #endregion

        #region Constructors

        public Union()
        {
            Documents = new List<Document<Union>>();
            Notes = new List<Note<Union>>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}
