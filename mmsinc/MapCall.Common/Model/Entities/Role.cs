using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Role : IEntity, IValidatableObject
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual User User { get; set; }

        [Obsolete(
            "This should be retrieved from Module.Application. There's no reason to even store this in the Roles table.")]
        public virtual Application Application { get; set; }

        public virtual Module Module { get; set; }
        public virtual RoleAction Action { get; set; }

        /// <summary>
        /// If null, this Role represents *all* OperatingCenters.
        /// </summary>
        public virtual OperatingCenter OperatingCenter { get; set; }

        /// <summary>
        /// Returns true if this role applies to all OperatingCenters.
        /// </summary>
        public virtual bool IsValidForAnyOperatingCenter => (OperatingCenter == null);

        #endregion

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }
    }
}
