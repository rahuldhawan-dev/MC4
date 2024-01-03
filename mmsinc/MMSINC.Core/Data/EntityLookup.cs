using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MMSINC.Data
{
    [Serializable]
    public class ReadOnlyEntityLookup : IEntityLookup
    {
        #region Constants

        public struct StringLengths
        {
            public const int DESCRIPTION = 50;
        }

        #endregion

        #region Properties

        // TODO: This needs to be a protected setter.
        public virtual int Id { get; set; }

        [Required, StringLength(StringLengths.DESCRIPTION)]
        public virtual string Description { get; set; }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    [Serializable]
    public class EntityLookup : ReadOnlyEntityLookup { }

    public interface
        IEntityLookup : IEntity, IValidatableObject // TODO: Does this need IValidatableObject still? -Ross 9/29/2015
    {
        string Description { get; }
    }
}
