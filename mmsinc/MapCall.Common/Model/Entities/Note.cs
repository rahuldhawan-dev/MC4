using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Note : IEntityWithCreationTimeTracking, IValidatableObject
    {
        #region Properties

        public virtual int Id { get; set; }

        [Required, Display(Name = "Note")]
        public virtual string Text { get; set; }

        [Required]
        public virtual DateTime CreatedAt { get; set; }

        [Required]
        public virtual int LinkedId { get; set; }

        [Required]
        public virtual string CreatedBy { get; set; }

        [Required]
        public virtual DataType DataType { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    public interface IThingWithNotes
    {
        int Id { get; }

        IList<INoteLink> LinkedNotes { get; }

        // used so we can get a list of DataTypes
        // for creating notes
        string TableName { get; }
    }

    public interface INoteLink
    {
        #region Abstract Properties

        int Id { get; }
        Note Note { get; set; }
        DataType DataType { get; set; }
        int LinkedId { get; }

        #endregion
    }
}
