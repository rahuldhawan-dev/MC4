using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ActionItemType : IEntityLookup
    {
        #region Constants

        public struct Indices 
        {
            public const int NOT_LISTED = 11;
        }

        #endregion

        #region Properties
        public virtual int Id { get; set; }
        
        public virtual string Description { get; set; }
        
        public virtual DataType DataType { get; set; }

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
