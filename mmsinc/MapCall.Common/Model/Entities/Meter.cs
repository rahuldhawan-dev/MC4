using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Meter : IEntityWithCreationTimeTracking, IValidatableObject
    {
        #region Constants

        public struct StringLengths
        {
            #region Constants

            public const int SERIAL_NUMBER = 50,
                             ORCOM_EQUIPMENT_NUMBER = 50,
                             ERT_NUMBER_1_LOW_OR_ONLY = 50,
                             ERT_NUMBER_2_HIGH = 50,
                             CREATED_BY = 50;

            #endregion
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual IList<Interconnection> Interconnections { get; set; }
        public virtual MeterProfile Profile { get; set; }
        public virtual MeterStatus Status { get; set; }

        [StringLength(StringLengths.SERIAL_NUMBER)]
        public virtual string SerialNumber { get; set; }

        [StringLength(StringLengths.ORCOM_EQUIPMENT_NUMBER)]
        public virtual string OrcomEquipmentNumber { get; set; }

        [StringLength(StringLengths.ERT_NUMBER_1_LOW_OR_ONLY)]
        public virtual string ERTNumber1LowOrOnly { get; set; }

        [StringLength(StringLengths.ERT_NUMBER_2_HIGH)]
        public virtual string ERTNumber2High { get; set; }

        public virtual DateTime? DatePurchased { get; set; }
        public virtual DateTime CreatedAt { get; set; }

        [StringLength(StringLengths.CREATED_BY)]
        public virtual string CreatedBy { get; set; }

        public virtual int? PremiseID { get; set; }
        public virtual bool? IsInterconnectMeter { get; set; }

        public virtual string Description => SerialNumber;

        #endregion

        #region Constructors

        public Meter()
        {
            Interconnections = new List<Interconnection>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
