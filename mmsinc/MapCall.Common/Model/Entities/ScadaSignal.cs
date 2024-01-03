using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ScadaSignal : IEntity
    {
        #region Table Properties

        public virtual int Id { get; set; }
        public virtual string TagName { get; set; }
        public virtual string Description { get; set; }
        public virtual string EngineeringUnits { get; set; }
        public virtual string TagId { get; set; }

        public virtual ScadaTagName ScadaTagName { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return TagName;
        }

        #endregion
    }
}
