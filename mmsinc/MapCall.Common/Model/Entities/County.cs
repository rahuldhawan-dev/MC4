using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class County : IEntity, IValidatableObject
    {
        #region Constants

        public struct StringLengths
        {
            public const int NAME = 50;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        [StringLength(StringLengths.NAME)]
        public virtual string Name { get; set; }

        public virtual bool? Enabled { get; set; }

        public virtual State State { get; set; }

        public virtual IList<Town> Towns { get; set; }

        #endregion

        #region Constructors

        public County()
        {
            Towns = new List<Town>();
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
