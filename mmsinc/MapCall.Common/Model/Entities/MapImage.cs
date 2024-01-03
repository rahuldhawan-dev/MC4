using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MapImage : IEntity, IValidatableObject, IAssetImage
    {
        // NOTE: There aren't any string length props here because this data is not editable.

        #region Properties

        public virtual int Id { get; set; }

        public virtual string MapPage { get; set; }
        public virtual string Gradient { get; set; }
        public virtual string North { get; set; }
        public virtual string South { get; set; }
        public virtual string East { get; set; }
        public virtual string West { get; set; }
        public virtual string Directory { get; set; }
        public virtual string FileName { get; set; }
        public virtual Town Town { get; set; }
        public virtual string TownSection { get; set; }

        /// <summary>
        /// This isn't a DateTime in the database.
        /// </summary>
        public virtual string DateRevised { get; set; }

        public virtual byte[] ImageData =>
            throw new NotSupportedException("This is only needed for uploads and MapImages can not be uploaded.");

        #endregion

        #region Public Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
