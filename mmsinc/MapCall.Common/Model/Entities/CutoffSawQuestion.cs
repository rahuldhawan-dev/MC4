using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class CutoffSawQuestion : IEntity, IValidatableObject
    {
        #region Properties

        #region Table Column Properties

        public virtual int Id { get; set; }
        public virtual string Question { get; set; }
        public virtual int SortOrder { get; set; }
        public virtual bool IsActive { get; set; }

        #endregion

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return Question;
        }

        #endregion
    }
}
