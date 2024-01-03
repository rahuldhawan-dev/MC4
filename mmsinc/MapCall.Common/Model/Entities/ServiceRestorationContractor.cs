using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ServiceRestorationContractor : IEntityLookup
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual string Contractor { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual bool FinalRestoration { get; set; }
        public virtual bool PartialRestoration { get; set; }
        public virtual bool HasRestorations { get; set; }

        public virtual string Description => Contractor;

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
}
