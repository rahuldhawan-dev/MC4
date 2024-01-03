using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ServiceSize : IValidatableObject, IEntityLookup
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual bool Hydrant { get; set; }
        public virtual bool Lateral { get; set; }
        public virtual bool Main { get; set; }
        public virtual bool Meter { get; set; }
        public virtual int? SortOrder { get; set; }
        public virtual bool Service { get; set; }
        public virtual string ServiceSizeDescription { get; set; }
        public virtual decimal Size { get; set; }
        public virtual string SAPCode { get; set; }

        public virtual string Description => ServiceSizeDescription;

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return ServiceSizeDescription;
        }

        #endregion
    }
}
