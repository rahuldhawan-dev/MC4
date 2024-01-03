using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PublicWaterSupplyCustomerData
        : IEntityWithCreationTimeTracking, IEntityWithUpdateTimeTracking, IValidatableObject
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual PublicWaterSupply PWSID { get; set; }
        public virtual DateTime CreatedAt { get; set; }

        [StringLength(50)]
        public virtual string CreatedBy { get; set; }

        public virtual int? NumberCustomers { get; set; }
        public virtual int? PopulationServed { get; set; }

        [StringLength(255)]
        public virtual string Notes { get; set; }

        public virtual DateTime UpdatedAt { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
