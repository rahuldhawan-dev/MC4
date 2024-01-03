using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class HydrantOutOfService : IEntityWithCreationTimeTracking, IValidatableObject
    {
        #region Consts

        public struct StringLengths
        {
            public const int FIRE_CONTACT = 50,
                             FIRE_FAX = 20,
                             FIRE_PHONE = 20;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? BackInServiceDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime OutOfServiceDate { get; set; }

        public virtual DateTime CreatedAt { get; set; }
        public virtual string FireDepartmentContact { get; set; }
        public virtual string FireDepartmentFax { get; set; }
        public virtual string FireDepartmentPhone { get; set; }
        public virtual Hydrant Hydrant { get; set; }
        public virtual User BackInServiceByUser { get; set; }
        public virtual User OutOfServiceByUser { get; set; }

        #endregion

        #region Public Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
