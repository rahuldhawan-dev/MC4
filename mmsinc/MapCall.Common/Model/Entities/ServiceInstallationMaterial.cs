using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    public class ServiceInstallationMaterial : IEntityLookup
    {
        #region Constants

        public struct StringLengths
        {
            public const int DESCRIPTION = 50, PART_SIZE = 10, PART_QUANTITY = 10;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual ServiceCategory ServiceCategory { get; set; }
        public virtual ServiceSize ServiceSize { get; set; }
        public virtual int SortOrder { get; set; }

        [StringLength(StringLengths.DESCRIPTION)]
        public virtual string Description { get; set; }

        [StringLength(StringLengths.PART_SIZE)]
        public virtual string PartSize { get; set; }

        [StringLength(StringLengths.PART_QUANTITY)]
        public virtual string PartQuantity { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    public class ServiceServiceInstallationMaterial
    {
        public virtual Service Service { get; set; }
        public virtual ServiceInstallationMaterial ServiceInstallationMaterial { get; set; }

        #region Exposed Methods

        public override bool Equals(object obj)
        {
            var other = obj as ServiceServiceInstallationMaterial;
            if (ReferenceEquals(null, this)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Service == other.Service && ServiceInstallationMaterial == other.ServiceInstallationMaterial;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
