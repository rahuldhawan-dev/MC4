using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class BodyOfWater : IEntityLookup
    {
        #region Constants

        public struct StringLengths
        {
            public const int DESCRIPTION = 50,
                             CRITICAL_NOTES = 255;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description => Name;

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual string CriticalNotes { get; set; }

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
