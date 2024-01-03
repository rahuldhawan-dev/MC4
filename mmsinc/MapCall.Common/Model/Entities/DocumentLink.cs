using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class DocumentLink : IValidatableObject, IDocumentLink
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual int LinkedId { get; set; }

        public virtual Document Document { get; set; }
        public virtual DataType DataType { get; set; }
        public virtual DocumentType DocumentType { get; set; }
        public virtual DocumentStatus DocumentStatus { get; set; }
        public virtual int? ReviewFrequency { get; set; }
        public virtual RecurringFrequencyUnit ReviewFrequencyUnit { get; set; }
        [DisplayName("Review Frequency")]
        public virtual string ReviewFrequencyDisplay
        {
            get
            {
                if (ReviewFrequencyUnit == null)
                {
                    return null;
                }

                return ReviewFrequency + " " + ReviewFrequencyUnit;
            }
        }

        #region Formulas

        public virtual DateTime UpdatedAt { get; set; }
        public virtual User UpdatedBy { get; set; }
        public virtual DateTime? NextReviewDate { get; set; }

        #endregion

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    public interface IDocumentLink : IEntityWithUpdateTracking<User>
    {
        #region Abstract Properties

        int Id { get; }
        Document Document { get; set; }
        DocumentType DocumentType { get; set; }
        DataType DataType { get; set; }
        int LinkedId { get; }
        DateTime UpdatedAt { get; set; }
        User UpdatedBy { get; set; }

        #endregion
    }
}
