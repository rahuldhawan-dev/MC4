using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PipeDiameter : IEntity, IValidatableObject
    {
        #region Properties

        public virtual int Id { get; set; }

        [Required]
        public virtual decimal Diameter { get; set; }

        public virtual IList<RecurringProject> RecurringProjects { get; set; }

        #endregion

        #region Constructors

        public PipeDiameter()
        {
            RecurringProjects = new List<RecurringProject>();
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Diameter.ToString();
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
