using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class StreetOpeningPermit : IEntity, IValidatableObject
    {
        #region Constants

        public struct DisplayNames
        {
            public const string STREET_OPENING_PERMIT_NUMBER = "Permit #",
                                PERMIT_ID = "Permit Id",
                                IS_PAID_FOR = "Paid";
        }

        public struct StringLengths
        {
            public const int STREET_OPENING_PERMIT_NUMBER = 25;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }

        [View(DisplayNames.STREET_OPENING_PERMIT_NUMBER)]
        public virtual string StreetOpeningPermitNumber { get; set; }

        [Required,
         DisplayFormat(ApplyFormatInEditMode = true,
             DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime DateRequested { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true,
            DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? DateIssued { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true,
            DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? ExpirationDate { get; set; }

        public virtual string Notes { get; set; }

        /// <summary>
        /// The external id used by the Permits API to reference this permit.
        /// </summary>
        [View(DisplayNames.PERMIT_ID)]
        public virtual int? PermitId { get; set; }
        public virtual bool? HasMetDrawingRequirement { get; set; }

        [View(DisplayNames.IS_PAID_FOR)]
        public virtual bool? IsPaidFor { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
