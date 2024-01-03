using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EnvironmentalPermitFee : IEntity
    {
        #region Structs

        public struct StringLengths
        {
            public const int PAYMENT_METHOD_PHONE = 20,
                             PAYMENT_METHOD_URL = 2000,
                             PAYMENT_ORGANIZATION_NAME = 200;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual EnvironmentalPermit EnvironmentalPermit { get; set; }

        [View("Fee Type")]
        public virtual EnvironmentalPermitFeeType EnvironmentalPermitFeeType { get; set; }

        [View(FormatStyle.Currency)]
        public virtual decimal Fee { get; set; }

        [View("Effective Date", FormatStyle.Date)]
        public virtual DateTime? PaymentEffectiveDate { get; set; }

        public virtual Employee PaymentOwner { get; set; }
        public virtual int? PaymentDueInterval { get; set; }
        public virtual RecurringFrequencyUnit PaymentDueFrequencyUnit { get; set; }

        [StringLength(StringLengths.PAYMENT_ORGANIZATION_NAME)]
        public virtual string PaymentOrganizationName { get; set; }

        [Multiline]
        public virtual string PaymentOrganizationContactInfo { get; set; }

        public virtual EnvironmentalPermitFeePaymentMethod PaymentMethod { get; set; }

        [Multiline]
        public virtual string PaymentMethodMailAddress { get; set; }

        [StringLength(StringLengths.PAYMENT_METHOD_PHONE)]
        public virtual string PaymentMethodPhone { get; set; }

        [StringLength(StringLengths.PAYMENT_METHOD_URL)]
        public virtual string PaymentMethodUrl { get; set; }

        #region Logical

        public virtual string PaymentFrequency
        {
            get
            {
                // Don't send poorly formatted strings if we're missing values.
                if (!PaymentDueInterval.HasValue || PaymentDueFrequencyUnit == null)
                {
                    return null;
                }

                return $"{PaymentDueInterval} {PaymentDueFrequencyUnit.Description}";
            }
        }

        #endregion

        #endregion
    }

    [Serializable]
    public class EnvironmentalPermitFeeType : EntityLookup { }

    [Serializable]
    public class EnvironmentalPermitFeePaymentMethod : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            // NOTE: If you add or change values to this, then you need to update EnvironmentalPermitFee/Form.js
            public const int MAIL = 1,
                             PHONE = 2,
                             URL = 3;
        }
    }
}
