using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Street : IValidatableObject, IEntityLookup
    {
        #region Constants

        public struct StringLengths
        {
            public const int FULL_ST_NAME = 50,
                             NAME = 30;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        /// <summary>
        /// NOTE: THIS IS NOT A LOGICAL PROPERTY
        /// </summary>
        [StringLength(StringLengths.FULL_ST_NAME)]
        public virtual string FullStName { get; set; }

        public virtual Town Town { get; set; }
        public virtual string Name { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual StreetPrefix Prefix { get; set; }
        public virtual StreetSuffix Suffix { get; set; }

        public virtual string Description => FullStName;

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return FullStName;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
