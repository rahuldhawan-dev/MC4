using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class StateRegion : IEntityLookup
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual string Region { get; set; }
        public virtual State State { get; set; }

        public virtual string Description => Region;

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
