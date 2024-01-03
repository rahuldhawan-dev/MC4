using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using MMSINC.Data;
using NHibernate.Classic;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PipeDataLookupType : EntityLookup
    {
        #region Properties

        public virtual IList<PipeDataLookupValue> PipeDataLookupValues { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
