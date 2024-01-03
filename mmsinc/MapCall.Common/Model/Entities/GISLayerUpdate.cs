using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class GISLayerUpdate : IEntityWithCreationTracking<User>, IValidatableObject
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual User CreatedBy { get; set; }

        [Required, DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        // This is NOT our "UpdatedAt"
        public virtual DateTime Updated { get; set; }

        [Required]
        public virtual DateTime CreatedAt { get; set; }

        [Required]
        public virtual bool IsActive { get; set; }

        [Required, StringLength(GISLayerUpdateMap.MAP_ID_LENGTH), DisplayName("Map ID")]
        public virtual string MapId { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
