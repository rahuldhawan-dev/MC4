using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class AllocationPermitWithdrawalNode : IEntity, IValidatableObject, IThingWithCoordinate
    {
        #region Properties

        public virtual int Id { get; set; }

        public virtual IList<AllocationPermit> AllocationGroupings { get; set; }

        public virtual AllocationCategory AllocationCategory { get; set; }
        public virtual Facility Facility { get; set; }
        public virtual Coordinate Coordinate { get; set; }

        public virtual MapIcon Icon => Coordinate != null ? Coordinate.Icon : null;

        [StringLength(25)]
        public virtual string WellPermitNumber { get; set; }

        public virtual string Description { get; set; }

        [DisplayName("Allowable GPM")]
        public virtual decimal? AllowableGpm { get; set; }

        [DisplayName("Allowable GPD")]
        public virtual decimal? AllowableGpd { get; set; }

        [DisplayName("Allowable MGM")]
        public virtual decimal? AllowableMgm { get; set; }

        [DisplayName("Capable GPM")]
        public virtual decimal? CapableGpm { get; set; }

        public virtual string WithdrawalConstraint { get; set; }
        public virtual bool? HasStandByPower { get; set; }
        public virtual decimal? CapacityUnderStandbyPower { get; set; }

        public virtual IList<Equipment> Equipment { get; set; }

        #endregion

        public AllocationPermitWithdrawalNode()
        {
            Equipment = new List<Equipment>();
        }

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
