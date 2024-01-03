using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MapIconOffset : IEntityLookup
    {
        #region Properties

        public virtual int Id { get; set; }

        [Required]
        public virtual string Description { get; set; }

        public virtual IList<MapIcon> MapIcons { get; set; } = new List<MapIcon>();

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
