using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ScadaTagName : IEntityLookup
    {
        #region Private Members

        private ScadaTagNameDisplayItem _display;

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual string Description { get; set; }

        [StringLength(25)]
        public virtual string Units { get; set; }

        public virtual string Display => (_display ?? (_display = new ScadaTagNameDisplayItem {
            TagName = TagName,
            Description = Description
        })).Display;

        public virtual Equipment Equipment { get; set; }

        [Required]
        [StringLength(30)]
        public virtual string TagName { get; set; }

        public virtual bool Inactive { get; set; }

        #endregion

        #region Public Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return Display;
        }

        #endregion
    }

    [Serializable]
    public class ScadaTagNameDisplayItem : DisplayItem<ScadaTagName>
    {
        public string TagName { get; set; }
        public string Description { get; set; }

        public override string Display =>
            TagName + (string.IsNullOrWhiteSpace(Description) ? string.Empty : $" - {Description}");
    }
}
